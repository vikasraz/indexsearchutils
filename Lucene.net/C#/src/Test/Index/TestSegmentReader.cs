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

using Document = Lucene.Net.Documents.Document;
using Fieldable = Lucene.Net.Documents.Fieldable;
using MockRAMDirectory = Lucene.Net.Store.MockRAMDirectory;
using RAMDirectory = Lucene.Net.Store.RAMDirectory;
using WhitespaceAnalyzer = Lucene.Net.Analysis.WhitespaceAnalyzer;
using DefaultSimilarity = Lucene.Net.Search.DefaultSimilarity;
using Similarity = Lucene.Net.Search.Similarity;
using LuceneTestCase = Lucene.Net.Util.LuceneTestCase;

namespace Lucene.Net.Index
{
	
	[TestFixture]
	public class TestSegmentReader : LuceneTestCase
	{
		private RAMDirectory dir = new RAMDirectory();
		private Lucene.Net.Documents.Document testDoc = new Lucene.Net.Documents.Document();
		private SegmentReader reader = null;
		
		// public TestSegmentReader(System.String s)
		// {
		// }
		
		// This is needed if for the test to pass and mimic what happens wiht JUnit
		// For some reason, JUnit is creating a new member variable for each sub-test
		// but NUnit is not -- who is wrong/right, I don't know.
		private void SetUpInternal()        // {{Aroush-1.9}} See note above
		{
			dir = new RAMDirectory();
			testDoc = new Lucene.Net.Documents.Document();
			reader = null;
		}

		//TODO: Setup the reader w/ multiple documents
		[SetUp]
		public override void SetUp()
		{
			base.SetUp();
			SetUpInternal();
			DocHelper.SetupDoc(testDoc);
			SegmentInfo info = DocHelper.WriteDoc(dir, testDoc);
			reader = SegmentReader.Get(info);
		}
		
		[TearDown]
		public override void TearDown()
		{
			
		}
		
		[Test]
		public virtual void  Test()
		{
			Assert.IsTrue(dir != null);
			Assert.IsTrue(reader != null);
			Assert.IsTrue(DocHelper.nameValues.Count > 0);
			Assert.IsTrue(DocHelper.NumFields(testDoc) == DocHelper.all.Count);
		}
		
		[Test]
		public virtual void  TestDocument()
		{
			int i = 1;
			Assert.IsTrue(reader.NumDocs() == 1);
			Assert.IsTrue(reader.MaxDoc() >= 1);
			Lucene.Net.Documents.Document result = reader.Document(0);
			Assert.IsTrue(result != null);
			//There are 2 unstored fields on the document that are not preserved across writing
			Assert.IsTrue(DocHelper.NumFields(result) == DocHelper.NumFields(testDoc) - DocHelper.unstored.Count);
			System.Collections.IEnumerator e = result.Fields();
			while (e.MoveNext())
			{
				Lucene.Net.Documents.Field field = (Lucene.Net.Documents.Field) e.Current;
				Assert.IsTrue(field != null);
				Assert.IsTrue(DocHelper.nameValues.Contains(field.Name()));
			}
		}
		
		[Test]
		public virtual void  TestDelete()
		{
			Lucene.Net.Documents.Document docToDelete = new Lucene.Net.Documents.Document();
			DocHelper.SetupDoc(docToDelete);
			SegmentInfo info = DocHelper.WriteDoc(dir, docToDelete);
			SegmentReader deleteReader = SegmentReader.Get(info);
			Assert.IsTrue(deleteReader != null);
			Assert.IsTrue(deleteReader.NumDocs() == 1);
			deleteReader.DeleteDocument(0);
			Assert.IsTrue(deleteReader.IsDeleted(0) == true);
			Assert.IsTrue(deleteReader.HasDeletions() == true);
			Assert.IsTrue(deleteReader.NumDocs() == 0);
			try
			{
				deleteReader.Document(0);
				Assert.Fail();
			}
			catch (System.ArgumentException)
			{
				// expcected exception
			}
		}
		
		[Test]
		public virtual void  TestGetFieldNameVariations()
		{
			System.Collections.ICollection result = reader.GetFieldNames(IndexReader.FieldOption.ALL);
			Assert.IsTrue(result != null);
			Assert.IsTrue(result.Count == DocHelper.all.Count);
			for (System.Collections.IEnumerator iter = result.GetEnumerator(); iter.MoveNext(); )
			{
                System.String s = (System.String)iter.Current;
                Assert.IsTrue(DocHelper.nameValues.Contains(s) == true || s.Equals(""));
			}

			result = reader.GetFieldNames(IndexReader.FieldOption.INDEXED);
			Assert.IsTrue(result != null);
			Assert.IsTrue(result.Count == DocHelper.indexed.Count);
			for (System.Collections.IEnumerator iter = result.GetEnumerator(); iter.MoveNext(); )
			{
                System.String s = (System.String)iter.Current;
                Assert.IsTrue(DocHelper.indexed.Contains(s) == true || s.Equals(""));
			}
			
			result = reader.GetFieldNames(IndexReader.FieldOption.UNINDEXED);
			Assert.IsTrue(result != null);
			Assert.IsTrue(result.Count == DocHelper.unindexed.Count);

			//Get all indexed fields that are storing term vectors
			result = reader.GetFieldNames(IndexReader.FieldOption.INDEXED_WITH_TERMVECTOR);
			Assert.IsTrue(result != null);
			Assert.IsTrue(result.Count == DocHelper.termvector.Count);
			
			result = reader.GetFieldNames(IndexReader.FieldOption.INDEXED_NO_TERMVECTOR);
			Assert.IsTrue(result != null);
			Assert.IsTrue(result.Count == DocHelper.notermvector.Count);
		}
		
		[Test]
		public virtual void  TestTerms()
		{
			TermEnum terms = reader.Terms();
			Assert.IsTrue(terms != null);
			while (terms.Next() == true)
			{
				Term term = terms.Term();
				Assert.IsTrue(term != null);
				//System.out.println("Term: " + term);
				System.String fieldValue = (System.String) DocHelper.nameValues[term.Field()];
				Assert.IsTrue(fieldValue.IndexOf(term.Text()) != - 1);
			}
			
			TermDocs termDocs = reader.TermDocs();
			Assert.IsTrue(termDocs != null);
			termDocs.Seek(new Term(DocHelper.TEXT_FIELD_1_KEY, "field"));
			Assert.IsTrue(termDocs.Next() == true);
			
			termDocs.Seek(new Term(DocHelper.NO_NORMS_KEY, DocHelper.NO_NORMS_TEXT));
			Assert.IsTrue(termDocs.Next() == true);
			
			
			TermPositions positions = reader.TermPositions();
			positions.Seek(new Term(DocHelper.TEXT_FIELD_1_KEY, "field"));
			Assert.IsTrue(positions != null);
			Assert.IsTrue(positions.Doc() == 0);
			Assert.IsTrue(positions.NextPosition() >= 0);
		}
		
		[Test]
		public virtual void  TestNorms()
		{
			//TODO: Not sure how these work/should be tested
			/*
			try {
			byte [] norms = reader.norms(DocHelper.TEXT_FIELD_1_KEY);
			System.out.println("Norms: " + norms);
			Assert.IsTrue(norms != null);
			} catch (IOException e) {
			e.printStackTrace();
			Assert.IsTrue(false);
			}*/
			
			CheckNorms(reader);
		}
		
		public static void  CheckNorms(IndexReader reader)
		{
			// test omit norms
			for (int i = 0; i < DocHelper.fields.Length; i++)
			{
				Lucene.Net.Documents.Fieldable f = DocHelper.fields[i];
				if (f.IsIndexed())
				{
					Assert.AreEqual(reader.HasNorms(f.Name()), !f.GetOmitNorms());
					Assert.AreEqual(reader.HasNorms(f.Name()), !DocHelper.noNorms.Contains(f.Name()));
					if (!reader.HasNorms(f.Name()))
					{
						// test for fake norms of 1.0
						byte[] norms = reader.Norms(f.Name());
						Assert.AreEqual(norms.Length, reader.MaxDoc());
						for (int j = 0; j < reader.MaxDoc(); j++)
						{
							Assert.AreEqual(norms[j], DefaultSimilarity.EncodeNorm(1.0f));
						}
						norms = new byte[reader.MaxDoc()];
						reader.Norms(f.Name(), norms, 0);
						for (int j = 0; j < reader.MaxDoc(); j++)
						{
							Assert.AreEqual(norms[j], DefaultSimilarity.EncodeNorm(1.0f));
						}
					}
				}
			}
		}
		
		public virtual void  TestTermVectors()
		{
			TermFreqVector result = reader.GetTermFreqVector(0, DocHelper.TEXT_FIELD_2_KEY);
			Assert.IsTrue(result != null);
			System.String[] terms = result.GetTerms();
			int[] freqs = result.GetTermFrequencies();
			Assert.IsTrue(terms != null && terms.Length == 3 && freqs != null && freqs.Length == 3);
			for (int i = 0; i < terms.Length; i++)
			{
				System.String term = terms[i];
				int freq = freqs[i];
				Assert.IsTrue(DocHelper.FIELD_2_TEXT.IndexOf(term) != - 1);
				Assert.IsTrue(freq > 0);
			}
			
			TermFreqVector[] results = reader.GetTermFreqVectors(0);
			Assert.IsTrue(results != null);
			Assert.IsTrue(results.Length == 4, "We do not have 4 term freq vectors, we have: " + results.Length);
		}
		
		[Test]
		public virtual void  TestIndexDivisor()
		{
			dir = new MockRAMDirectory();
			testDoc = new Document();
			DocHelper.SetupDoc(testDoc);
			SegmentInfo si = DocHelper.WriteDoc(dir, testDoc);
			
			reader = SegmentReader.Get(si);
			reader.SetTermInfosIndexDivisor(3);
			TestDocument();
			TestDelete();
			TestGetFieldNameVariations();
			TestNorms();
			TestTerms();
			TestTermVectors();
		}
	}
}
