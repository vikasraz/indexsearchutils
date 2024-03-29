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

using LuceneTestCase = Lucene.Net.Util.LuceneTestCase;

namespace Lucene.Net.Analysis
{
	
	[TestFixture]	
	public class TestPerFieldAnalzyerWrapper : LuceneTestCase
	{
		[Test]
		public virtual void  TestPerField()
		{
			System.String text = "Qwerty";
			PerFieldAnalyzerWrapper analyzer = new PerFieldAnalyzerWrapper(new WhitespaceAnalyzer());
			analyzer.AddAnalyzer("special", new SimpleAnalyzer());
			
			TokenStream tokenStream = analyzer.TokenStream("field", new System.IO.StringReader(text));
			Token token = tokenStream.Next();
			Assert.AreEqual("Qwerty", token.TermText(), "WhitespaceAnalyzer does not lowercase");
			
			tokenStream = analyzer.TokenStream("special", new System.IO.StringReader(text));
			token = tokenStream.Next();
			Assert.AreEqual("qwerty", token.TermText(), "SimpleAnalyzer lowercases");
		}
	}
}