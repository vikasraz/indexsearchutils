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

namespace Lucene.Net.Documents
{
	/// <summary> 
	/// 
	/// 
	/// </summary>
	[Serializable]
	public abstract class AbstractField : Fieldable
	{
		
		protected internal System.String name = "body";
		protected internal bool storeTermVector = false;
		protected internal bool storeOffsetWithTermVector = false;
		protected internal bool storePositionWithTermVector = false;
		protected internal bool omitNorms = false;
		protected internal bool isStored = false;
		protected internal bool isIndexed = true;
		protected internal bool isTokenized = true;
		protected internal bool isBinary = false;
		protected internal bool isCompressed = false;
		protected internal bool lazy = false;
		protected internal float boost = 1.0f;
		// the one and only data object for all different kind of field values
		protected internal System.Object fieldsData = null;
		
		protected internal AbstractField()
		{
		}
		
		protected internal AbstractField(System.String name, Field.Store store, Field.Index index, Field.TermVector termVector)
		{
			if (name == null)
				throw new System.NullReferenceException("name cannot be null");
			this.name = String.Intern(name); // field names are interned
			
			if (store == Field.Store.YES)
			{
				this.isStored = true;
				this.isCompressed = false;
			}
			else if (store == Field.Store.COMPRESS)
			{
				this.isStored = true;
				this.isCompressed = true;
			}
			else if (store == Field.Store.NO)
			{
				this.isStored = false;
				this.isCompressed = false;
			}
			else
			{
				throw new System.ArgumentException("unknown store parameter " + store);
			}
			
			if (index == Field.Index.NO)
			{
				this.isIndexed = false;
				this.isTokenized = false;
			}
			else if (index == Field.Index.TOKENIZED)
			{
				this.isIndexed = true;
				this.isTokenized = true;
			}
			else if (index == Field.Index.UN_TOKENIZED)
			{
				this.isIndexed = true;
				this.isTokenized = false;
			}
			else if (index == Field.Index.NO_NORMS)
			{
				this.isIndexed = true;
				this.isTokenized = false;
				this.omitNorms = true;
			}
			else
			{
				throw new System.ArgumentException("unknown index parameter " + index);
			}
			
			this.isBinary = false;
			
			SetStoreTermVector(termVector);
		}
		
		/// <summary>Sets the boost factor hits on this field.  This value will be
		/// multiplied into the score of all hits on this this field of this
		/// document.
		/// 
		/// <p>The boost is multiplied by {@link Lucene.Net.Documents.Document#GetBoost()} of the document
		/// containing this field.  If a document has multiple fields with the same
		/// name, all such values are multiplied together.  This product is then
		/// multipled by the value {@link Lucene.Net.Search.Similarity#LengthNorm(String,int)}, and
		/// rounded by {@link Lucene.Net.Search.Similarity#EncodeNorm(float)} before it is stored in the
		/// index.  One should attempt to ensure that this product does not overflow
		/// the range of that encoding.
		/// 
		/// </summary>
		/// <seealso cref="Lucene.Net.Documents.Document#SetBoost(float)">
		/// </seealso>
		/// <seealso cref="int)">
		/// </seealso>
		/// <seealso cref="Lucene.Net.Search.Similarity#EncodeNorm(float)">
		/// </seealso>
		public virtual void  SetBoost(float boost)
		{
			this.boost = boost;
		}
		
		/// <summary>Returns the boost factor for hits for this field.
		/// 
		/// <p>The default value is 1.0.
		/// 
		/// <p>Note: this value is not stored directly with the document in the index.
		/// Documents returned from {@link Lucene.Net.Index.IndexReader#Document(int)} and
		/// {@link Lucene.Net.Search.Hits#Doc(int)} may thus not have the same value present as when
		/// this field was indexed.
		/// 
		/// </summary>
		/// <seealso cref="#SetBoost(float)">
		/// </seealso>
		public virtual float GetBoost()
		{
			return boost;
		}
		
		/// <summary>Returns the name of the field as an interned string.
		/// For example "date", "title", "body", ...
		/// </summary>
		public virtual System.String Name()
		{
			return name;
		}
		
		protected internal virtual void  SetStoreTermVector(Field.TermVector termVector)
		{
			if (termVector == Field.TermVector.NO)
			{
				this.storeTermVector = false;
				this.storePositionWithTermVector = false;
				this.storeOffsetWithTermVector = false;
			}
			else if (termVector == Field.TermVector.YES)
			{
				this.storeTermVector = true;
				this.storePositionWithTermVector = false;
				this.storeOffsetWithTermVector = false;
			}
			else if (termVector == Field.TermVector.WITH_POSITIONS)
			{
				this.storeTermVector = true;
				this.storePositionWithTermVector = true;
				this.storeOffsetWithTermVector = false;
			}
			else if (termVector == Field.TermVector.WITH_OFFSETS)
			{
				this.storeTermVector = true;
				this.storePositionWithTermVector = false;
				this.storeOffsetWithTermVector = true;
			}
			else if (termVector == Field.TermVector.WITH_POSITIONS_OFFSETS)
			{
				this.storeTermVector = true;
				this.storePositionWithTermVector = true;
				this.storeOffsetWithTermVector = true;
			}
			else
			{
				throw new System.ArgumentException("unknown termVector parameter " + termVector);
			}
		}
		
		/// <summary>True iff the value of the field is to be stored in the index for return
		/// with search hits.  It is an error for this to be true if a field is
		/// Reader-valued. 
		/// </summary>
		public bool IsStored()
		{
			return isStored;
		}
		
		/// <summary>True iff the value of the field is to be indexed, so that it may be
		/// searched on. 
		/// </summary>
		public bool IsIndexed()
		{
			return isIndexed;
		}
		
		/// <summary>True iff the value of the field should be tokenized as text prior to
		/// indexing.  Un-tokenized fields are indexed as a single word and may not be
		/// Reader-valued. 
		/// </summary>
		public bool IsTokenized()
		{
			return isTokenized;
		}
		
		/// <summary>True if the value of the field is stored and compressed within the index </summary>
		public bool IsCompressed()
		{
			return isCompressed;
		}
		
		/// <summary>True iff the term or terms used to index this field are stored as a term
		/// vector, available from {@link Lucene.Net.Index.IndexReader#GetTermFreqVector(int,String)}.
		/// These methods do not provide access to the original content of the field,
		/// only to terms used to index it. If the original content must be
		/// preserved, use the <code>stored</code> attribute instead.
		/// 
		/// </summary>
		/// <seealso cref="String)">
		/// </seealso>
		public bool IsTermVectorStored()
		{
			return storeTermVector;
		}
		
		/// <summary> True iff terms are stored as term vector together with their offsets 
		/// (start and end positon in source text).
		/// </summary>
		public virtual bool IsStoreOffsetWithTermVector()
		{
			return storeOffsetWithTermVector;
		}
		
		/// <summary> True iff terms are stored as term vector together with their token positions.</summary>
		public virtual bool IsStorePositionWithTermVector()
		{
			return storePositionWithTermVector;
		}
		
		/// <summary>True iff the value of the filed is stored as binary </summary>
		public bool IsBinary()
		{
			return isBinary;
		}
		
		/// <summary>True if norms are omitted for this indexed field </summary>
		public virtual bool GetOmitNorms()
		{
			return omitNorms;
		}
		
		/// <summary>Expert:
		/// 
		/// If set, omit normalization factors associated with this indexed field.
		/// This effectively disables indexing boosts and length normalization for this field.
		/// </summary>
		public virtual void  SetOmitNorms(bool omitNorms)
		{
			this.omitNorms = omitNorms;
		}
		
		public virtual bool IsLazy()
		{
			return lazy;
		}
		
		/// <summary>Prints a Field for human consumption. </summary>
		public override System.String ToString()
		{
			System.Text.StringBuilder result = new System.Text.StringBuilder();
			if (isStored)
			{
				result.Append("stored");
				if (isCompressed)
					result.Append("/compressed");
				else
					result.Append("/uncompressed");
			}
			if (isIndexed)
			{
				if (result.Length > 0)
					result.Append(",");
				result.Append("indexed");
			}
			if (isTokenized)
			{
				if (result.Length > 0)
					result.Append(",");
				result.Append("tokenized");
			}
			if (storeTermVector)
			{
				if (result.Length > 0)
					result.Append(",");
				result.Append("termVector");
			}
			if (storeOffsetWithTermVector)
			{
				if (result.Length > 0)
					result.Append(",");
				result.Append("termVectorOffsets");
			}
			if (storePositionWithTermVector)
			{
				if (result.Length > 0)
					result.Append(",");
				result.Append("termVectorPosition");
			}
			if (isBinary)
			{
				if (result.Length > 0)
					result.Append(",");
				result.Append("binary");
			}
			if (omitNorms)
			{
				result.Append(",omitNorms");
			}
			if (lazy)
			{
				result.Append(",lazy");
			}
			result.Append('<');
			result.Append(name);
			result.Append(':');
			
			if (fieldsData != null && lazy == false)
			{
				result.Append(fieldsData);
			}
			
			result.Append('>');
			return result.ToString();
		}
		public abstract byte[] BinaryValue();
		public abstract System.IO.TextReader ReaderValue();
		public abstract System.String StringValue();
		public abstract Lucene.Net.Analysis.TokenStream TokenStreamValue();
	}
}