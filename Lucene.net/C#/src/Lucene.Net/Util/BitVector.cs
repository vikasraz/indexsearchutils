/*
 * Licensed to the Apache Software Foundation (ASF) under one or more
 * contributor license agreements.  See the NOTICE file distributed with
 * this work for additional information regarding copyright ownership.
 * The ASF licenses this file to You under the Apache License, Version 2.0
 * (the "License"); you may not use this file except in compliance with
 * the License.  You may obtain a copy of the License at
 * 
 * http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System;

using Directory = Lucene.Net.Store.Directory;
using IndexInput = Lucene.Net.Store.IndexInput;
using IndexOutput = Lucene.Net.Store.IndexOutput;

namespace Lucene.Net.Util
{
	
	/// <summary>Optimized implementation of a vector of bits.  This is more-or-less like
	/// java.util.BitSet, but also includes the following:
	/// <ul>
	/// <li>a count() method, which efficiently computes the number of one bits;</li>
	/// <li>optimized read from and write to disk;</li>
	/// <li>inlinable get() method;</li>
	/// <li>store and load, as bit set or d-gaps, depending on sparseness;</li> 
	/// </ul>
	/// </summary>
	/// <version>  $Id: BitVector.java 564236 2007-08-09 15:21:19Z gsingers $
	/// </version>
	public sealed class BitVector
	{
		
		private byte[] bits;
		private int size;
		private int count = - 1;
		
		/// <summary>Constructs a vector capable of holding <code>n</code> bits. </summary>
		public BitVector(int n)
		{
			size = n;
			bits = new byte[(size >> 3) + 1];
		}
		
		/// <summary>Sets the value of <code>bit</code> to one. </summary>
		public void  Set(int bit)
		{
			if (bit >= size)
			{
				throw new System.IndexOutOfRangeException("Index of bound " + bit);
			}
			bits[bit >> 3] |= (byte) (1 << (bit & 7));
			count = - 1;
		}
		
		/// <summary>Sets the value of <code>bit</code> to zero. </summary>
		public void  Clear(int bit)
		{
			if (bit >= size)
			{
				throw new System.IndexOutOfRangeException("Index of bound " + bit);
			}
			bits[bit >> 3] &= (byte) (~ (1 << (bit & 7)));
			count = - 1;
		}
		
		/// <summary>Returns <code>true</code> if <code>bit</code> is one and
		/// <code>false</code> if it is zero. 
		/// </summary>
		public bool Get(int bit)
		{
			if (bit >= size)
			{
				throw new System.IndexOutOfRangeException("Index of bound " + bit);
			}
			return (bits[bit >> 3] & (1 << (bit & 7))) != 0;
		}
		
		/// <summary>Returns the number of bits in this vector.  This is also one greater than
		/// the number of the largest valid bit number. 
		/// </summary>
		public int Size()
		{
			return size;
		}
		
		/// <summary>Returns the total number of one bits in this vector.  This is efficiently
		/// computed and cached, so that, if the vector is not changed, no
		/// recomputation is done for repeated calls. 
		/// </summary>
		public int Count()
		{
			// if the vector has been modified
			if (count == - 1)
			{
				int c = 0;
				int end = bits.Length;
				for (int i = 0; i < end; i++)
					c += BYTE_COUNTS[bits[i] & 0xFF]; // sum bits per byte
				count = c;
			}
			return count;
		}
		
		private static readonly byte[] BYTE_COUNTS = new byte[]{0, 1, 1, 2, 1, 2, 2, 3, 1, 2, 2, 3, 2, 3, 3, 4, 1, 2, 2, 3, 2, 3, 3, 4, 2, 3, 3, 4, 3, 4, 4, 5, 1, 2, 2, 3, 2, 3, 3, 4, 2, 3, 3, 4, 3, 4, 4, 5, 2, 3, 3, 4, 3, 4, 4, 5, 3, 4, 4, 5, 4, 5, 5, 6, 1, 2, 2, 3, 2, 3, 3, 4, 2, 3, 3, 4, 3, 4, 4, 5, 2, 3, 3, 4, 3, 4, 4, 5, 3, 4, 4, 5, 4, 5, 5, 6, 2, 3, 3, 4, 3, 4, 4, 5, 3, 4, 4, 5, 4, 5, 5, 6, 3, 4, 4, 5, 4, 5, 5, 6, 4, 5, 5, 6, 5, 6, 6, 7, 1, 2, 2, 3, 2, 3, 3, 4, 2, 3, 3, 4, 3, 4, 4, 5, 2, 3, 3, 4, 3, 4, 4, 5, 3, 4, 4, 5, 4, 5, 5, 6, 2, 3, 3, 4, 3, 4, 4, 5, 3, 4, 4, 5, 4, 5, 5, 6, 3, 4, 4, 5, 4, 5, 5, 6, 4, 5, 5, 6, 5, 6, 6, 7, 2, 3, 3, 4, 3, 4, 4, 5, 3, 4, 4, 5, 4, 5, 5, 6, 3, 4, 4, 5, 4, 5, 5, 6, 4, 5, 5, 6, 5, 6, 6, 7, 3, 4, 4, 5, 4, 5, 5, 6, 4, 5, 5, 6, 5, 6, 6, 7, 4, 5, 5, 6, 5, 6, 6, 7, 5, 6, 6, 7, 6, 7, 7, 8};
		
		
		/// <summary>Writes this vector to the file <code>name</code> in Directory
		/// <code>d</code>, in a format that can be read by the constructor {@link
		/// #BitVector(Directory, String)}.  
		/// </summary>
		public void  Write(Directory d, System.String name)
		{
			IndexOutput output = d.CreateOutput(name);
			try
			{
				if (IsSparse())
				{
					WriteDgaps(output); // sparse bit-set more efficiently saved as d-gaps.
				}
				else
				{
					WriteBits(output);
				}
			}
			finally
			{
				output.Close();
			}
		}
		
		/// <summary>Write as a bit set </summary>
		private void  WriteBits(IndexOutput output)
		{
			output.WriteInt(Size()); // write size
			output.WriteInt(Count()); // write count
			output.WriteBytes(bits, bits.Length);
		}
		
		/// <summary>Write as a d-gaps list </summary>
		private void  WriteDgaps(IndexOutput output)
		{
			output.WriteInt(- 1); // mark using d-gaps                         
			output.WriteInt(Size()); // write size
			output.WriteInt(Count()); // write count
			int last = 0;
			int n = Count();
			int m = bits.Length;
			for (int i = 0; i < m && n > 0; i++)
			{
				if (bits[i] != 0)
				{
					output.WriteVInt(i - last);
					output.WriteByte(bits[i]);
					last = i;
					n -= BYTE_COUNTS[bits[i] & 0xFF];
				}
			}
		}
		
		/// <summary>Indicates if the bit vector is sparse and should be saved as a d-gaps list, or dense, and should be saved as a bit set. </summary>
		private bool IsSparse()
		{
			// note: order of comparisons below set to favor smaller values (no binary range search.)
			// note: adding 4 because we start with ((int) -1) to indicate d-gaps format.
			// note: we write the d-gap for the byte number, and the byte (bits[i]) itself, therefore
			//       multiplying count by (8+8) or (8+16) or (8+24) etc.:
			//       - first 8 for writing bits[i] (1 byte vs. 1 bit), and 
			//       - second part for writing the byte-number d-gap as vint. 
			// note: factor is for read/write of byte-arrays being faster than vints.  
			int factor = 10;
			if (bits.Length < (1 << 7))
				return factor * (4 + (8 + 8) * Count()) < Size();
			if (bits.Length < (1 << 14))
				return factor * (4 + (8 + 16) * Count()) < Size();
			if (bits.Length < (1 << 21))
				return factor * (4 + (8 + 24) * Count()) < Size();
			if (bits.Length < (1 << 28))
				return factor * (4 + (8 + 32) * Count()) < Size();
			return factor * (4 + (8 + 40) * Count()) < Size();
		}
		
		/// <summary>Constructs a bit vector from the file <code>name</code> in Directory
		/// <code>d</code>, as written by the {@link #write} method.
		/// </summary>
		public BitVector(Directory d, System.String name)
		{
			IndexInput input = d.OpenInput(name);
			try
			{
				size = input.ReadInt(); // read size
				if (size == - 1)
				{
					ReadDgaps(input);
				}
				else
				{
					ReadBits(input);
				}
			}
			finally
			{
				input.Close();
			}
		}
		
		/// <summary>Read as a bit set </summary>
		private void  ReadBits(IndexInput input)
		{
			count = input.ReadInt(); // read count
			bits = new byte[(size >> 3) + 1]; // allocate bits
			input.ReadBytes(bits, 0, bits.Length);
		}
		
		/// <summary>read as a d-gaps list </summary>
		private void  ReadDgaps(IndexInput input)
		{
			size = input.ReadInt(); // (re)read size
			count = input.ReadInt(); // read count
			bits = new byte[(size >> 3) + 1]; // allocate bits
			int last = 0;
			int n = Count();
			while (n > 0)
			{
				last += input.ReadVInt();
				bits[last] = input.ReadByte();
				n -= BYTE_COUNTS[bits[last] & 0xFF];
			}
		}
	}
}