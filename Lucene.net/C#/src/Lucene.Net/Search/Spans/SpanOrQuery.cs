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
using PriorityQueue = Lucene.Net.Util.PriorityQueue;
using ToStringUtils = Lucene.Net.Util.ToStringUtils;
using Query = Lucene.Net.Search.Query;

namespace Lucene.Net.Search.Spans
{
	
	/// <summary>Matches the union of its clauses.</summary>
	[Serializable]
	public class SpanOrQuery : SpanQuery
	{
		private class AnonymousClassSpans : Spans
		{
			public AnonymousClassSpans(Lucene.Net.Index.IndexReader reader, SpanOrQuery enclosingInstance)
			{
				InitBlock(reader, enclosingInstance);
			}
			private void  InitBlock(Lucene.Net.Index.IndexReader reader, SpanOrQuery enclosingInstance)
			{
				this.reader = reader;
				this.enclosingInstance = enclosingInstance;
			}
			private Lucene.Net.Index.IndexReader reader;
			private SpanOrQuery enclosingInstance;
			public SpanOrQuery Enclosing_Instance
			{
				get
				{
					return enclosingInstance;
				}
				
			}
			private SpanQueue queue = null;
			
			private bool InitSpanQueue(int target)
			{
				queue = new SpanQueue(enclosingInstance, Enclosing_Instance.clauses.Count);
				System.Collections.IEnumerator i = Enclosing_Instance.clauses.GetEnumerator();
				while (i.MoveNext())
				{
					Spans spans = ((SpanQuery) i.Current).GetSpans(reader);
					if (((target == - 1) && spans.Next()) || ((target != - 1) && spans.SkipTo(target)))
					{
						queue.Put(spans);
					}
				}
				return queue.Size() != 0;
			}
			
			public virtual bool Next()
			{
				if (queue == null)
				{
					return InitSpanQueue(- 1);
				}
				
				if (queue.Size() == 0)
				{
					// all done
					return false;
				}
				
				if (Top().Next())
				{
					// move to next
					queue.AdjustTop();
					return true;
				}
				
				queue.Pop(); // exhausted a clause
				return queue.Size() != 0;
			}
			
			private Spans Top()
			{
				return (Spans) queue.Top();
			}
			
			public virtual bool SkipTo(int target)
			{
				if (queue == null)
				{
					return InitSpanQueue(target);
				}
				
				while (queue.Size() != 0 && Top().Doc() < target)
				{
					if (Top().SkipTo(target))
					{
						queue.AdjustTop();
					}
					else
					{
						queue.Pop();
					}
				}
				
				return queue.Size() != 0;
			}
			
			public virtual int Doc()
			{
				return Top().Doc();
			}
			public virtual int Start()
			{
				return Top().Start();
			}
			public virtual int End()
			{
				return Top().End();
			}
			
			public override System.String ToString()
			{
				return "spans(" + Enclosing_Instance + ")@" + ((queue == null) ? "START" : (queue.Size() > 0 ? (Doc() + ":" + Start() + "-" + End()) : "END"));
			}
		}
		private System.Collections.ArrayList clauses;
		private System.String field;
		
		/// <summary>Construct a SpanOrQuery merging the provided clauses. </summary>
		public SpanOrQuery(SpanQuery[] clauses)
		{
			
			// copy clauses array into an ArrayList
			this.clauses = new System.Collections.ArrayList(clauses.Length);
			for (int i = 0; i < clauses.Length; i++)
			{
				SpanQuery clause = clauses[i];
				if (i == 0)
				{
					// check field
					field = clause.GetField();
				}
				else if (!clause.GetField().Equals(field))
				{
					throw new System.ArgumentException("Clauses must have same field.");
				}
				this.clauses.Add(clause);
			}
		}
		
		/// <summary>Return the clauses whose spans are matched. </summary>
		public virtual SpanQuery[] GetClauses()
		{
			return (SpanQuery[]) clauses.ToArray(typeof(SpanQuery[]));
		}
		
		public override System.String GetField()
		{
			return field;
		}
		
		/// <summary>Returns a collection of all terms matched by this query.</summary>
		/// <deprecated> use extractTerms instead
		/// </deprecated>
		/// <seealso cref="#ExtractTerms(Set)">
		/// </seealso>
		public override System.Collections.ICollection GetTerms()
		{
			System.Collections.ArrayList terms = new System.Collections.ArrayList();
			System.Collections.IEnumerator i = clauses.GetEnumerator();
			while (i.MoveNext())
			{
				SpanQuery clause = (SpanQuery) i.Current;
				terms.AddRange(clause.GetTerms());
			}
			return terms;
		}
		
		public override void  ExtractTerms(System.Collections.Hashtable terms)
		{
			System.Collections.IEnumerator i = clauses.GetEnumerator();
			while (i.MoveNext())
			{
				SpanQuery clause = (SpanQuery) i.Current;
				clause.ExtractTerms(terms);
			}
		}
		
		public override Query Rewrite(IndexReader reader)
		{
			SpanOrQuery clone = null;
			for (int i = 0; i < clauses.Count; i++)
			{
				SpanQuery c = (SpanQuery) clauses[i];
				SpanQuery query = (SpanQuery) c.Rewrite(reader);
				if (query != c)
				{
					// clause rewrote: must clone
					if (clone == null)
						clone = (SpanOrQuery) this.Clone();
					clone.clauses[i] = query;
				}
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
		
		public override System.String ToString(System.String field)
		{
			System.Text.StringBuilder buffer = new System.Text.StringBuilder();
			buffer.Append("spanOr([");
			System.Collections.IEnumerator i = clauses.GetEnumerator();
			while (i.MoveNext())
			{
				SpanQuery clause = (SpanQuery) i.Current;
				buffer.Append(clause.ToString(field));
				buffer.Append(", ");
			}
            if(clauses.Count>0) buffer.Length -= 2;
			buffer.Append("])");
			buffer.Append(ToStringUtils.Boost(GetBoost()));
			return buffer.ToString();
		}
		
		public  override bool Equals(System.Object o)
		{
			if (this == o)
				return true;
			if (o == null || GetType() != o.GetType())
				return false;
			
			SpanOrQuery that = (SpanOrQuery) o;
			
			if (!clauses.Equals(that.clauses))
				return false;
			if (!field.Equals(that.field))
				return false;
			
			return GetBoost() == that.GetBoost();
		}
		
		public override int GetHashCode()
		{
			int h = clauses.GetHashCode();
			h ^= ((h << 10) | (h >> 23));
			h ^= System.Convert.ToInt32(GetBoost());
			return h;
		}
		
		private class SpanQueue : PriorityQueue
		{
			private void  InitBlock(SpanOrQuery enclosingInstance)
			{
				this.enclosingInstance = enclosingInstance;
			}
			private SpanOrQuery enclosingInstance;
			public SpanOrQuery Enclosing_Instance
			{
				get
				{
					return enclosingInstance;
				}
				
			}
			public SpanQueue(SpanOrQuery enclosingInstance, int size)
			{
				InitBlock(enclosingInstance);
				Initialize(size);
			}
			
			public override bool LessThan(System.Object o1, System.Object o2)
			{
				Spans spans1 = (Spans) o1;
				Spans spans2 = (Spans) o2;
				if (spans1.Doc() == spans2.Doc())
				{
					if (spans1.Start() == spans2.Start())
					{
						return spans1.End() < spans2.End();
					}
					else
					{
						return spans1.Start() < spans2.Start();
					}
				}
				else
				{
					return spans1.Doc() < spans2.Doc();
				}
			}
		}
		
		
		public override Spans GetSpans(IndexReader reader)
		{
			if (clauses.Count == 1)
				// optimize 1-clause case
				return ((SpanQuery) clauses[0]).GetSpans(reader);
			
			return new AnonymousClassSpans(reader, this);
		}
	}
}
