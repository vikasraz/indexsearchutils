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

using CorruptIndexException = Lucene.Net.Index.CorruptIndexException;
using Hits = Lucene.Net.Search.Hits;
using IndexSearcher = Lucene.Net.Search.IndexSearcher;
using Query = Lucene.Net.Search.Query;
using QueryUtils = Lucene.Net.Search.QueryUtils;
using ScoreDoc = Lucene.Net.Search.ScoreDoc;
using TopDocs = Lucene.Net.Search.TopDocs;

namespace Lucene.Net.Search.Function
{
	
	/// <summary> Test search based on OrdFieldSource and ReverseOrdFieldSource.
	/// <p>
	/// Tests here create an index with a few documents, each having
	/// an indexed "id" field.
	/// The ord values of this field are later used for scoring.
	/// <p>
	/// The order tests use Hits to verify that docs are ordered as expected.
	/// <p>
	/// The exact score tests use TopDocs top to verify the exact score.  
	/// </summary>
	[TestFixture]
	public class TestOrdValues : FunctionTestSetup
	{
		
		/* @override constructor */
		//public TestOrdValues(System.String name):base(name)
		//{
		//}
		
		/* @override */
		[TearDown]
		public override void  TearDown()
		{
			base.TearDown();
		}
		
		/* @override */
		[SetUp]
		public override void  SetUp()
		{
			// prepare a small index with just a few documents.  
			base.SetUp();
		}
		
		/// <summary>Test OrdFieldSource </summary>
		[Test]
		public virtual void  TestOrdFieldRank()
		{
			DoTestRank(ID_FIELD, true);
		}
		
		/// <summary>Test ReverseOrdFieldSource </summary>
		[Test]
		public virtual void  TestReverseOrdFieldRank()
		{
			DoTestRank(ID_FIELD, false);
		}
		
		// Test that queries based on reverse/ordFieldScore scores correctly
		private void  DoTestRank(System.String field, bool inOrder)
		{
			IndexSearcher s = new IndexSearcher(dir);
			ValueSource vs;
			if (inOrder)
			{
				vs = new OrdFieldSource(field);
			}
			else
			{
				vs = new ReverseOrdFieldSource(field);
			}
			
			Query q = new ValueSourceQuery(vs);
			Log("test: " + q);
			QueryUtils.Check(q, s);
			Hits h = s.Search(q);
			Assert.AreEqual(N_DOCS, h.Length(), "All docs should be matched!");
			System.String prevID = inOrder?"IE":"IC"; // smaller than all ids of docs in this test ("ID0001", etc.)
			
			for (int i = 0; i < h.Length(); i++)
			{
				System.String resID = h.Doc(i).Get(ID_FIELD);
				Log(i + ".   score=" + h.Score(i) + "  -  " + resID);
				Log(s.Explain(q, h.Id(i)));
				if (inOrder)
				{
					Assert.IsTrue(String.CompareOrdinal(resID, prevID) < 0, "res id " + resID + " should be < prev res id " + prevID);
				}
				else
				{
					Assert.IsTrue(String.CompareOrdinal(resID, prevID) > 0, "res id " + resID + " should be > prev res id " + prevID);
				}
				prevID = resID;
			}
		}
		
		/// <summary>Test exact score for OrdFieldSource </summary>
		[Test]
		public virtual void  TestOrdFieldExactScore()
		{
			DoTestExactScore(ID_FIELD, true);
		}
		
		/// <summary>Test exact score for ReverseOrdFieldSource </summary>
		[Test]
		public virtual void  TestReverseOrdFieldExactScore()
		{
			DoTestExactScore(ID_FIELD, false);
		}
		
		
		// Test that queries based on reverse/ordFieldScore returns docs with expected score.
		private void  DoTestExactScore(System.String field, bool inOrder)
		{
			IndexSearcher s = new IndexSearcher(dir);
			ValueSource vs;
			if (inOrder)
			{
				vs = new OrdFieldSource(field);
			}
			else
			{
				vs = new ReverseOrdFieldSource(field);
			}
			Query q = new ValueSourceQuery(vs);
			TopDocs td = s.Search(q, null, 1000);
			Assert.AreEqual(N_DOCS, td.totalHits, "All docs should be matched!");
			ScoreDoc[] sd = td.scoreDocs;
			for (int i = 0; i < sd.Length; i++)
			{
				float score = sd[i].score;
				System.String id = s.GetIndexReader().Document(sd[i].doc).Get(ID_FIELD);
				Log("-------- " + i + ". Explain doc " + id);
				Log(s.Explain(q, sd[i].doc));
				float expectedScore = N_DOCS - i;
				Assert.AreEqual(expectedScore, score, TEST_SCORE_TOLERANCE_DELTA, "score of result " + i + " shuould be " + expectedScore + " != " + score);
				System.String expectedId = inOrder?Id2String(N_DOCS - i):Id2String(i + 1); // reverse  ==> smaller values first 
				Assert.IsTrue(expectedId.Equals(id), "id of result " + i + " shuould be " + expectedId + " != " + score);
			}
		}
		
		/// <summary>Test caching OrdFieldSource </summary>
		[Test]
		public virtual void  TestCachingOrd()
		{
			DoTestCaching(ID_FIELD, true);
		}
		
		/// <summary>Test caching for ReverseOrdFieldSource </summary>
		[Test]
		public virtual void  TesCachingReverseOrd()
		{
			DoTestCaching(ID_FIELD, false);
		}
		
		// Test that values loaded for FieldScoreQuery are cached properly and consumes the proper RAM resources.
		private void  DoTestCaching(System.String field, bool inOrder)
		{
			IndexSearcher s = new IndexSearcher(dir);
			System.Object innerArray = null;
			
			bool warned = false; // print warning once
			
			for (int i = 0; i < 10; i++)
			{
				ValueSource vs;
				if (inOrder)
				{
					vs = new OrdFieldSource(field);
				}
				else
				{
					vs = new ReverseOrdFieldSource(field);
				}
				ValueSourceQuery q = new ValueSourceQuery(vs);
				Hits h = s.Search(q);
				try
				{
					Assert.AreEqual(N_DOCS, h.Length(), "All docs should be matched!");
					if (i == 0)
					{
						innerArray = q.ValSrc_ForNUnitTest.GetValues(s.GetIndexReader()).GetInnerArray();
					}
					else
					{
						Log(i + ".  compare: " + innerArray + " to " + q.ValSrc_ForNUnitTest.GetValues(s.GetIndexReader()).GetInnerArray());
						Assert.AreSame(innerArray, q.ValSrc_ForNUnitTest.GetValues(s.GetIndexReader()).GetInnerArray(), "field values should be cached and reused!");
					}
				}
				catch (System.NotSupportedException)
				{
					if (!warned)
					{
						System.Console.Error.WriteLine("WARNING: " + TestName() + " cannot fully test values of " + q);
						warned = true;
					}
				}
			}
			
			ValueSource vs2;
			ValueSourceQuery q2;
			Hits h2;
			
			// verify that different values are loaded for a different field
			System.String field2 = INT_FIELD;
			Assert.IsFalse(field.Equals(field2)); // otherwise this test is meaningless.
			if (inOrder)
			{
				vs2 = new OrdFieldSource(field2);
			}
			else
			{
				vs2 = new ReverseOrdFieldSource(field2);
			}
			q2 = new ValueSourceQuery(vs2);
			h2 = s.Search(q2);
			Assert.AreEqual(N_DOCS, h2.Length(), "All docs should be matched!");
			try
			{
				Log("compare (should differ): " + innerArray + " to " + q2.ValSrc_ForNUnitTest.GetValues(s.GetIndexReader()).GetInnerArray());
				Assert.AreNotSame(innerArray, q2.ValSrc_ForNUnitTest.GetValues(s.GetIndexReader()).GetInnerArray(), "different values shuold be loaded for a different field!");
			}
			catch (System.NotSupportedException)
			{
				if (!warned)
				{
					System.Console.Error.WriteLine("WARNING: " + TestName() + " cannot fully test values of " + q2);
					warned = true;
				}
			}
			
			// verify new values are reloaded (not reused) for a new reader
			s = new IndexSearcher(dir);
			if (inOrder)
			{
				vs2 = new OrdFieldSource(field);
			}
			else
			{
				vs2 = new ReverseOrdFieldSource(field);
			}
			q2 = new ValueSourceQuery(vs2);
			h2 = s.Search(q2);
			Assert.AreEqual(N_DOCS, h2.Length(), "All docs should be matched!");
			try
			{
				Log("compare (should differ): " + innerArray + " to " + q2.ValSrc_ForNUnitTest.GetValues(s.GetIndexReader()).GetInnerArray());
				Assert.AreNotSame(innerArray, q2.ValSrc_ForNUnitTest.GetValues(s.GetIndexReader()).GetInnerArray(), "cached field values should not be reused if reader as changed!");
			}
			catch (System.NotSupportedException)
			{
				if (!warned)
				{
					System.Console.Error.WriteLine("WARNING: " + TestName() + " cannot fully test values of " + q2);
					warned = true;
				}
			}
		}
		
		private System.String TestName()
		{
			return GetType().FullName;
		}
	}
}