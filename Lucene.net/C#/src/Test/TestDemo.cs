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
using IndexWriter = Lucene.Net.Index.IndexWriter;
using ParseException = Lucene.Net.QueryParsers.ParseException;
using QueryParser = Lucene.Net.QueryParsers.QueryParser;
using Directory = Lucene.Net.Store.Directory;
using RAMDirectory = Lucene.Net.Store.RAMDirectory;
using Analyzer = Lucene.Net.Analysis.Analyzer;
using StandardAnalyzer = Lucene.Net.Analysis.Standard.StandardAnalyzer;
using Hits = Lucene.Net.Search.Hits;
using IndexSearcher = Lucene.Net.Search.IndexSearcher;
using Query = Lucene.Net.Search.Query;
using LuceneTestCase = Lucene.Net.Util.LuceneTestCase;

namespace Lucene.Net
{
	
	/// <summary> A very simple demo used in the API documentation (src/java/overview.html).
	/// 
	/// </summary>
	/// <author>  Daniel Naber
	/// </author>
	[TestFixture]
	public class TestDemo : LuceneTestCase
	{
		
		[Test]
		public virtual void  TestDemo_Renamed_Method()
		{
			
			Analyzer analyzer = new StandardAnalyzer();
			
			// Store the index in memory:
			Directory directory = new RAMDirectory();
			// To store an index on disk, use this instead (note that the 
			// parameter true will overwrite the index in that directory
			// if one exists):
			//Directory directory = FSDirectory.getDirectory("/tmp/testindex", true);
			IndexWriter iwriter = new IndexWriter(directory, analyzer, true);
			iwriter.SetMaxFieldLength(25000);
			Document doc = new Document();
			System.String text = "This is the text to be indexed.";
			doc.Add(new Field("fieldname", text, Field.Store.YES, Field.Index.TOKENIZED));
			iwriter.AddDocument(doc);
			iwriter.Close();
			
			// Now search the index:
			IndexSearcher isearcher = new IndexSearcher(directory);
			// Parse a simple query that searches for "text":
			Lucene.Net.QueryParsers.QueryParser parser = new Lucene.Net.QueryParsers.QueryParser("fieldname", analyzer);
			Query query = parser.Parse("text");
			Hits hits = isearcher.Search(query);
			Assert.AreEqual(1, hits.Length());
			// Iterate through the results:
			for (int i = 0; i < hits.Length(); i++)
			{
				Document hitDoc = hits.Doc(i);
				Assert.AreEqual("This is the text to be indexed.", hitDoc.Get("fieldname"));
			}
			isearcher.Close();
			directory.Close();
		}
	}
}