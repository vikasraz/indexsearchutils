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

using IndexReader = Lucene.Net.Index.IndexReader;
using Query = Lucene.Net.Search.Query;
using ToStringUtils = Lucene.Net.Util.ToStringUtils;

namespace Lucene.Net.Search.Spans
{
	
	/// <summary>Matches spans near the beginning of a field. </summary>
	[Serializable]
	public class SpanFirstQuery : SpanQuery
	{
		private class AnonymousClassSpans : Spans
		{
			public AnonymousClassSpans(Lucene.Net.Index.IndexReader reader, SpanFirstQuery enclosingInstance)
			{
				InitBlock(reader, enclosingInstance);
			}
			private void  InitBlock(Lucene.Net.Index.IndexReader reader, SpanFirstQuery enclosingInstance)
			{
				this.reader = reader;
				this.enclosingInstance = enclosingInstance;
				spans = Enclosing_Instance.match.GetSpans(reader);
			}
			private Lucene.Net.Index.IndexReader reader;
			private SpanFirstQuery enclosingInstance;
			public SpanFirstQuery Enclosing_Instance
			{
				get
				{
					return enclosingInstance;
				}
				
			}
			private Spans spans;
			
			public virtual bool Next()
			{
				while (spans.Next())
				{
					// scan to next match
					if (End() <= Enclosing_Instance.end)
						return true;
				}
				return false;
			}
			
			public virtual bool SkipTo(int target)
			{
				if (!spans.SkipTo(target))
					return false;
				
				if (spans.End() <= Enclosing_Instance.end)
				// there is a match
					return true;
				
				return Next(); // scan to next match
			}
			
			public virtual int Doc()
			{
				return spans.Doc();
			}
			public virtual int Start()
			{
				return spans.Start();
			}
			public virtual int End()
			{
				return spans.End();
			}
			
			public override System.String ToString()
			{
				return "spans(" + Enclosing_Instance.ToString() + ")";
			}
		}
		private SpanQuery match;
		private int end;
		
		/// <summary>Construct a SpanFirstQuery matching spans in <code>match</code> whose end
		/// position is less than or equal to <code>end</code>. 
		/// </summary>
		public SpanFirstQuery(SpanQuery match, int end)
		{
			this.match = match;
			this.end = end;
		}
		
		/// <summary>Return the SpanQuery whose matches are filtered. </summary>
		public virtual SpanQuery GetMatch()
		{
			return match;
		}
		
		/// <summary>Return the maximum end position permitted in a match. </summary>
		public virtual int GetEnd()
		{
			return end;
		}
		
		public override System.String GetField()
		{
			return match.GetField();
		}
		
		/// <summary>Returns a collection of all terms matched by this query.</summary>
		/// <deprecated> use extractTerms instead
		/// </deprecated>
		/// <seealso cref="#ExtractTerms(Set)">
		/// </seealso>
		public override System.Collections.ICollection GetTerms()
		{
			return match.GetTerms();
		}
		
		public override System.String ToString(System.String field)
		{
			System.Text.StringBuilder buffer = new System.Text.StringBuilder();
			buffer.Append("spanFirst(");
			buffer.Append(match.ToString(field));
			buffer.Append(", ");
			buffer.Append(end);
			buffer.Append(")");
			buffer.Append(ToStringUtils.Boost(GetBoost()));
			return buffer.ToString();
		}
		
		public override void  ExtractTerms(System.Collections.Hashtable terms)
		{
			match.ExtractTerms(terms);
		}
		
		public override Spans GetSpans(IndexReader reader)
		{
			return new AnonymousClassSpans(reader, this);
		}
		
		public override Query Rewrite(IndexReader reader)
		{
			SpanFirstQuery clone = null;
			
			SpanQuery rewritten = (SpanQuery) match.Rewrite(reader);
			if (rewritten != match)
			{
				clone = (SpanFirstQuery) this.Clone();
				clone.match = rewritten;
			}
			
			if (clone != null)
			{
				return clone; // some clauses rewrote
			}
			else
			{
				return this; // no clauses rewrote
			}
		}
		
		public  override bool Equals(System.Object o)
		{
			if (this == o)
				return true;
			if (!(o is SpanFirstQuery))
				return false;
			
			SpanFirstQuery other = (SpanFirstQuery) o;
			return this.end == other.end && this.match.Equals(other.match) && this.GetBoost() == other.GetBoost();
		}
		
		public override int GetHashCode()
		{
			int h = match.GetHashCode();
			h ^= ((h << 8) | ((int) (((uint) h) >> 25))); // reversible
			h ^= System.Convert.ToInt32(GetBoost()) ^ end;
			return h;
		}
	}
}