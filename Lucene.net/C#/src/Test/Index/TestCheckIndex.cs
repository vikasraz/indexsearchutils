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
using MockRAMDirectory = Lucene.Net.Store.MockRAMDirectory;
using LuceneTestCase = Lucene.Net.Util.LuceneTestCase;
using WhitespaceAnalyzer = Lucene.Net.Analysis.WhitespaceAnalyzer;

namespace Lucene.Net.Index
{
	
	[TestFixture]
	public class TestCheckIndex : LuceneTestCase
	{
		
		[Test]
		public virtual void  TestDeletedDocs()
		{
			MockRAMDirectory dir = new MockRAMDirectory();
			IndexWriter writer = new IndexWriter(dir, new WhitespaceAnalyzer(), true);
			writer.SetMaxBufferedDocs(2);
			Document doc = new Document();
			doc.Add(new Field("field", "aaa", Field.Store.YES, Field.Index.TOKENIZED, Field.TermVector.WITH_POSITIONS_OFFSETS));
			for (int i = 0; i < 19; i++)
			{
				writer.AddDocument(doc);
			}
			writer.Close();
			IndexReader reader = IndexReader.Open(dir);
			reader.DeleteDocument(5);
			reader.Close();
			
			CheckIndex.out_Renamed = new System.IO.StringWriter();
			bool condition = CheckIndex.Check(dir, false);
			String message = CheckIndex.out_Renamed.ToString();
			Assert.IsTrue(condition, message);
		}
	}
}