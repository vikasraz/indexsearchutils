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

using NUnit.Framework;

using Directory = Lucene.Net.Store.Directory;
using RAMDirectory = Lucene.Net.Store.RAMDirectory;

namespace Lucene.Net.Util
{
	
	/// <summary> <code>TestBitVector</code> tests the <code>BitVector</code>, obviously.
	/// 
	/// 
	/// </summary>
	/// <version>  $Id: TestBitVector.java 583534 2007-10-10 16:46:35Z mikemccand $
	/// </version>
	[TestFixture]
	public class TestBitVector : LuceneTestCase
	{
		
		/// <summary> Test the default constructor on BitVectors of various sizes.</summary>
		/// <throws>  Exception </throws>
		[Test]
		public virtual void  TestConstructSize()
		{
			DoTestConstructOfSize(8);
			DoTestConstructOfSize(20);
			DoTestConstructOfSize(100);
			DoTestConstructOfSize(1000);
		}
		
		private void  DoTestConstructOfSize(int n)
		{
			BitVector bv = new BitVector(n);
			Assert.AreEqual(n, bv.Size());
		}
		
		/// <summary> Test the get() and set() methods on BitVectors of various sizes.</summary>
		/// <throws>  Exception </throws>
		[Test]
		public virtual void  TestGetSet()
		{
			DoTestGetSetVectorOfSize(8);
			DoTestGetSetVectorOfSize(20);
			DoTestGetSetVectorOfSize(100);
			DoTestGetSetVectorOfSize(1000);
		}
		
		private void  DoTestGetSetVectorOfSize(int n)
		{
			BitVector bv = new BitVector(n);
			for (int i = 0; i < bv.Size(); i++)
			{
				// ensure a set bit can be git'
				Assert.IsFalse(bv.Get(i));
				bv.Set(i);
				Assert.IsTrue(bv.Get(i));
			}
		}
		
		/// <summary> Test the clear() method on BitVectors of various sizes.</summary>
		/// <throws>  Exception </throws>
		[Test]
		public virtual void  TestClear()
		{
			DoTestClearVectorOfSize(8);
			DoTestClearVectorOfSize(20);
			DoTestClearVectorOfSize(100);
			DoTestClearVectorOfSize(1000);
		}
		
		private void  DoTestClearVectorOfSize(int n)
		{
			BitVector bv = new BitVector(n);
			for (int i = 0; i < bv.Size(); i++)
			{
				// ensure a set bit is cleared
				Assert.IsFalse(bv.Get(i));
				bv.Set(i);
				Assert.IsTrue(bv.Get(i));
				bv.Clear(i);
				Assert.IsFalse(bv.Get(i));
			}
		}
		
		/// <summary> Test the count() method on BitVectors of various sizes.</summary>
		/// <throws>  Exception </throws>
		[Test]
		public virtual void  TestCount()
		{
			DoTestCountVectorOfSize(8);
			DoTestCountVectorOfSize(20);
			DoTestCountVectorOfSize(100);
			DoTestCountVectorOfSize(1000);
		}
		
		private void  DoTestCountVectorOfSize(int n)
		{
			BitVector bv = new BitVector(n);
			// test count when incrementally setting bits
			for (int i = 0; i < bv.Size(); i++)
			{
				Assert.IsFalse(bv.Get(i));
				Assert.AreEqual(i, bv.Count());
				bv.Set(i);
				Assert.IsTrue(bv.Get(i));
				Assert.AreEqual(i + 1, bv.Count());
			}
			
			bv = new BitVector(n);
			// test count when setting then clearing bits
			for (int i = 0; i < bv.Size(); i++)
			{
				Assert.IsFalse(bv.Get(i));
				Assert.AreEqual(0, bv.Count());
				bv.Set(i);
				Assert.IsTrue(bv.Get(i));
				Assert.AreEqual(1, bv.Count());
				bv.Clear(i);
				Assert.IsFalse(bv.Get(i));
				Assert.AreEqual(0, bv.Count());
			}
		}
		
		/// <summary> Test writing and construction to/from Directory.</summary>
		/// <throws>  Exception </throws>
		[Test]
		public virtual void  TestWriteRead()
		{
			DoTestWriteRead(8);
			DoTestWriteRead(20);
			DoTestWriteRead(100);
			DoTestWriteRead(1000);
		}
		
		private void  DoTestWriteRead(int n)
		{
			Directory d = new RAMDirectory();
			
			BitVector bv = new BitVector(n);
			// test count when incrementally setting bits
			for (int i = 0; i < bv.Size(); i++)
			{
				Assert.IsFalse(bv.Get(i));
				Assert.AreEqual(i, bv.Count());
				bv.Set(i);
				Assert.IsTrue(bv.Get(i));
				Assert.AreEqual(i + 1, bv.Count());
				bv.Write(d, "TESTBV");
				BitVector compare = new BitVector(d, "TESTBV");
				// compare bit vectors with bits set incrementally
				Assert.IsTrue(DoCompare(bv, compare));
			}
		}
		
		/// <summary> Test r/w when size/count cause switching between bit-set and d-gaps file formats.  </summary>
		/// <throws>  Exception </throws>
		[Test]
		public virtual void  TestDgaps()
		{
			DoTestDgaps(1, 0, 1);
			DoTestDgaps(10, 0, 1);
			DoTestDgaps(100, 0, 1);
			DoTestDgaps(1000, 4, 7);
			DoTestDgaps(10000, 40, 43);
			DoTestDgaps(100000, 415, 418);
			DoTestDgaps(1000000, 3123, 3126);
		}
		
		private void  DoTestDgaps(int size, int count1, int count2)
		{
			Directory d = new RAMDirectory();
			BitVector bv = new BitVector(size);
			for (int i = 0; i < count1; i++)
			{
				bv.Set(i);
				Assert.AreEqual(i + 1, bv.Count());
			}
			bv.Write(d, "TESTBV");
			// gradually increase number of set bits
			for (int i = count1; i < count2; i++)
			{
				BitVector bv2 = new BitVector(d, "TESTBV");
				Assert.IsTrue(DoCompare(bv, bv2));
				bv = bv2;
				bv.Set(i);
				Assert.AreEqual(i + 1, bv.Count());
				bv.Write(d, "TESTBV");
			}
			// now start decreasing number of set bits
			for (int i = count2 - 1; i >= count1; i--)
			{
				BitVector bv2 = new BitVector(d, "TESTBV");
				Assert.IsTrue(DoCompare(bv, bv2));
				bv = bv2;
				bv.Clear(i);
				Assert.AreEqual(i, bv.Count());
				bv.Write(d, "TESTBV");
			}
		}

		/// <summary> Compare two BitVectors.
		/// This should really be an equals method on the BitVector itself.
		/// </summary>
		/// <param name="bv">One bit vector
		/// </param>
		/// <param name="compare">The second to compare
		/// </param>
		private bool DoCompare(BitVector bv, BitVector compare)
		{
			bool equal = true;
			for (int i = 0; i < bv.Size(); i++)
			{
				// bits must be equal
				if (bv.Get(i) != compare.Get(i))
				{
					equal = false;
					break;
				}
			}
			return equal;
		}
	}
}