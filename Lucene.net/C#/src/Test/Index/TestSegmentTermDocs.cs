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
using Field = Lucene.Net.Documents.Field;
using Directory = Lucene.Net.Store.Directory;
using MockRAMDirectory = Lucene.Net.Store.MockRAMDirectory;
using RAMDirectory = Lucene.Net.Store.RAMDirectory;
using WhitespaceAnalyzer = Lucene.Net.Analysis.WhitespaceAnalyzer;
using Similarity = Lucene.Net.Search.Similarity;
using LuceneTestCase = Lucene.Net.Util.LuceneTestCase;

namespace Lucene.Net.Index
{
	
	[TestFixture]
	public class TestSegmentTermDocs : LuceneTestCase
	{
		private Lucene.Net.Documents.Document testDoc = new Lucene.Net.Documents.Document();
		private Directory dir = new RAMDirectory();
		private SegmentInfo info;
		
		// got the idea for this from George's note in TestSegmentReader
		// it seems that JUnit creates a new instance of the class for each test invocation
		// while NUnit does not (seems like a flaw in JUnit, to be honest)
		// forcing the re-init of the variables for each run solves the problem
		private void SetUpInternal()
		{
			dir = new RAMDirectory();
			testDoc = new Lucene.Net.Documents.Document();
			info = null;
		}

		[SetUp]
		public override void SetUp()
		{
			base.SetUp();
			SetUpInternal();
			DocHelper.SetupDoc(testDoc);
			info = DocHelper.WriteDoc(dir, testDoc);
		}
		
		[Test]
		public virtual void  Test()
		{
			Assert.IsTrue(dir != null);
		}
		
		[Test]
		public virtual void  TestTermDocs()
		{
			TestTermDocs(1);
		}
		
		public virtual void  TestTermDocs(int indexDivisor)
		{
			//After adding the document, we should be able to read it back in
			SegmentReader reader = SegmentReader.Get(info);
			reader.SetTermInfosIndexDivisor(indexDivisor);
			Assert.IsTrue(reader != null);
			SegmentTermDocs segTermDocs = new SegmentTermDocs(reader);
			Assert.IsTrue(segTermDocs != null);
			segTermDocs.Seek(new Term(DocHelper.TEXT_FIELD_2_KEY, "field"));
			if (segTermDocs.Next() == true)
			{
				int docId = segTermDocs.Doc();
				Assert.IsTrue(docId == 0);
				int freq = segTermDocs.Freq();
				Assert.IsTrue(freq == 3);
			}
			reader.Close();
		}
		
		[Test]
		public virtual void  TestBadSeek()
		{
			TestBadSeek(1);
		}
		
		public virtual void  TestBadSeek(int indexDivisor)
		{
			{
				//After adding the document, we should be able to read it back in
				SegmentReader reader = SegmentReader.Get(info);
				reader.SetTermInfosIndexDivisor(indexDivisor);
				Assert.IsTrue(reader != null);
				SegmentTermDocs segTermDocs = new SegmentTermDocs(reader);
				Assert.IsTrue(segTermDocs != null);
				segTermDocs.Seek(new Term("textField2", "bad"));
				Assert.IsTrue(segTermDocs.Next() == false);
				reader.Close();
			}
			{
				//After adding the document, we should be able to read it back in
				SegmentReader reader = SegmentReader.Get(info);
				reader.SetTermInfosIndexDivisor(indexDivisor);
				Assert.IsTrue(reader != null);
				SegmentTermDocs segTermDocs = new SegmentTermDocs(reader);
				Assert.IsTrue(segTermDocs != null);
				segTermDocs.Seek(new Term("junk", "bad"));
				Assert.IsTrue(segTermDocs.Next() == false);
				reader.Close();
			}
		}
		
		[Test]
		public virtual void  TestSkipTo()
		{
			TestSkipTo(1);
		}
		
		public virtual void  TestSkipTo(int indexDivisor)
		{
			Directory dir = new RAMDirectory();
			IndexWriter writer = new IndexWriter(dir, new WhitespaceAnalyzer(), true);
			
			Term ta = new Term("content", "aaa");
			for (int i = 0; i < 10; i++)
				AddDoc(writer, "aaa aaa aaa aaa");
			
			Term tb = new Term("content", "bbb");
			for (int i = 0; i < 16; i++)
				AddDoc(writer, "bbb bbb bbb bbb");
			
			Term tc = new Term("content", "ccc");
			for (int i = 0; i < 50; i++)
				AddDoc(writer, "ccc ccc ccc ccc");
			
			// assure that we deal with a single segment  
			writer.Optimize();
			writer.Close();
			
			IndexReader reader = IndexReader.Open(dir);
			reader.SetTermInfosIndexDivisor(indexDivisor);
			Assert.AreEqual(indexDivisor, reader.GetTermInfosIndexDivisor());
			
			TermDocs tdocs = reader.TermDocs();
			
			// without optimization (assumption skipInterval == 16)
			
			// with next
			tdocs.Seek(ta);
			Assert.IsTrue(tdocs.Next());
			Assert.AreEqual(0, tdocs.Doc());
			Assert.AreEqual(4, tdocs.Freq());
			Assert.IsTrue(tdocs.Next());
			Assert.AreEqual(1, tdocs.Doc());
			Assert.AreEqual(4, tdocs.Freq());
			Assert.IsTrue(tdocs.SkipTo(0));
			Assert.AreEqual(2, tdocs.Doc());
			Assert.IsTrue(tdocs.SkipTo(4));
			Assert.AreEqual(4, tdocs.Doc());
			Assert.IsTrue(tdocs.SkipTo(9));
			Assert.AreEqual(9, tdocs.Doc());
			Assert.IsFalse(tdocs.SkipTo(10));
			
			// without next
			tdocs.Seek(ta);
			Assert.IsTrue(tdocs.SkipTo(0));
			Assert.AreEqual(0, tdocs.Doc());
			Assert.IsTrue(tdocs.SkipTo(4));
			Assert.AreEqual(4, tdocs.Doc());
			Assert.IsTrue(tdocs.SkipTo(9));
			Assert.AreEqual(9, tdocs.Doc());
			Assert.IsFalse(tdocs.SkipTo(10));
			
			// exactly skipInterval documents and therefore with optimization
			
			// with next
			tdocs.Seek(tb);
			Assert.IsTrue(tdocs.Next());
			Assert.AreEqual(10, tdocs.Doc());
			Assert.AreEqual(4, tdocs.Freq());
			Assert.IsTrue(tdocs.Next());
			Assert.AreEqual(11, tdocs.Doc());
			Assert.AreEqual(4, tdocs.Freq());
			Assert.IsTrue(tdocs.SkipTo(5));
			Assert.AreEqual(12, tdocs.Doc());
			Assert.IsTrue(tdocs.SkipTo(15));
			Assert.AreEqual(15, tdocs.Doc());
			Assert.IsTrue(tdocs.SkipTo(24));
			Assert.AreEqual(24, tdocs.Doc());
			Assert.IsTrue(tdocs.SkipTo(25));
			Assert.AreEqual(25, tdocs.Doc());
			Assert.IsFalse(tdocs.SkipTo(26));
			
			// without next
			tdocs.Seek(tb);
			Assert.IsTrue(tdocs.SkipTo(5));
			Assert.AreEqual(10, tdocs.Doc());
			Assert.IsTrue(tdocs.SkipTo(15));
			Assert.AreEqual(15, tdocs.Doc());
			Assert.IsTrue(tdocs.SkipTo(24));
			Assert.AreEqual(24, tdocs.Doc());
			Assert.IsTrue(tdocs.SkipTo(25));
			Assert.AreEqual(25, tdocs.Doc());
			Assert.IsFalse(tdocs.SkipTo(26));
			
			// much more than skipInterval documents and therefore with optimization
			
			// with next
			tdocs.Seek(tc);
			Assert.IsTrue(tdocs.Next());
			Assert.AreEqual(26, tdocs.Doc());
			Assert.AreEqual(4, tdocs.Freq());
			Assert.IsTrue(tdocs.Next());
			Assert.AreEqual(27, tdocs.Doc());
			Assert.AreEqual(4, tdocs.Freq());
			Assert.IsTrue(tdocs.SkipTo(5));
			Assert.AreEqual(28, tdocs.Doc());
			Assert.IsTrue(tdocs.SkipTo(40));
			Assert.AreEqual(40, tdocs.Doc());
			Assert.IsTrue(tdocs.SkipTo(57));
			Assert.AreEqual(57, tdocs.Doc());
			Assert.IsTrue(tdocs.SkipTo(74));
			Assert.AreEqual(74, tdocs.Doc());
			Assert.IsTrue(tdocs.SkipTo(75));
			Assert.AreEqual(75, tdocs.Doc());
			Assert.IsFalse(tdocs.SkipTo(76));
			
			//without next
			tdocs.Seek(tc);
			Assert.IsTrue(tdocs.SkipTo(5));
			Assert.AreEqual(26, tdocs.Doc());
			Assert.IsTrue(tdocs.SkipTo(40));
			Assert.AreEqual(40, tdocs.Doc());
			Assert.IsTrue(tdocs.SkipTo(57));
			Assert.AreEqual(57, tdocs.Doc());
			Assert.IsTrue(tdocs.SkipTo(74));
			Assert.AreEqual(74, tdocs.Doc());
			Assert.IsTrue(tdocs.SkipTo(75));
			Assert.AreEqual(75, tdocs.Doc());
			Assert.IsFalse(tdocs.SkipTo(76));
			
			tdocs.Close();
			reader.Close();
			dir.Close();
		}
		
		[Test]
		public virtual void  TestIndexDivisor()
		{
			dir = new MockRAMDirectory();
			testDoc = new Document();
			DocHelper.SetupDoc(testDoc);
			DocHelper.WriteDoc(dir, testDoc);
			TestTermDocs(2);
			TestBadSeek(2);
			TestSkipTo(2);
		}
		
		[Test]
		public virtual void  TestIndexDivisorAfterLoad()
		{
			dir = new MockRAMDirectory();
			testDoc = new Document();
			DocHelper.SetupDoc(testDoc);
			SegmentInfo si = DocHelper.WriteDoc(dir, testDoc);
			SegmentReader reader = SegmentReader.Get(si);
			Assert.AreEqual(1, reader.DocFreq(new Term("keyField", "Keyword")));
			try
			{
				reader.SetTermInfosIndexDivisor(2);
				Assert.Fail("did not hit IllegalStateException exception");
			}
			catch (System.SystemException)
			{
				// expected
			}
		}
		
		private void  AddDoc(IndexWriter writer, System.String value_Renamed)
		{
			Lucene.Net.Documents.Document doc = new Lucene.Net.Documents.Document();
			doc.Add(new Field("content", value_Renamed, Field.Store.NO, Field.Index.TOKENIZED));
			writer.AddDocument(doc);
		}
	}
}