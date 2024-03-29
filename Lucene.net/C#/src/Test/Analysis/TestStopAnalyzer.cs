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
	public class TestStopAnalyzer : LuceneTestCase
	{
		
		private StopAnalyzer stop = new StopAnalyzer();
		private System.Collections.Hashtable inValidTokens = new System.Collections.Hashtable();
		
		[SetUp]
		public override void  SetUp()
		{
			base.SetUp();
			stop = new StopAnalyzer();
			inValidTokens = new System.Collections.Hashtable();

			for (int i = 0; i < StopAnalyzer.ENGLISH_STOP_WORDS.Length; i++)
			{
				inValidTokens.Add(StopAnalyzer.ENGLISH_STOP_WORDS[i], StopAnalyzer.ENGLISH_STOP_WORDS[i]);
			}
		}
		
		[Test]
		public virtual void  TestDefaults()
		{
			Assert.IsTrue(stop != null);
			System.IO.StringReader reader = new System.IO.StringReader("This is a test of the english stop analyzer");
			TokenStream stream = stop.TokenStream("test", reader);
			Assert.IsTrue(stream != null);
			Token token = null;
			while ((token = stream.Next()) != null)
			{
				Assert.IsFalse(inValidTokens.Contains(token.TermText()));
			}
		}
		
		[Test]
		public virtual void  TestStopList()
		{
			System.Collections.Hashtable stopWordsSet = new System.Collections.Hashtable();
			stopWordsSet.Add("good", "good");
			stopWordsSet.Add("test", "test");
			stopWordsSet.Add("analyzer", "analyzer");
			StopAnalyzer newStop = new StopAnalyzer(stopWordsSet);
			System.IO.StringReader reader = new System.IO.StringReader("This is a good test of the english stop analyzer");
			TokenStream stream = newStop.TokenStream("test", reader);
			Assert.IsNotNull(stream);
			Token token = null;
			while ((token = stream.Next()) != null)
			{
				System.String text = token.TermText();
				Assert.IsFalse(stopWordsSet.Contains(text));
				Assert.AreEqual(1, token.GetPositionIncrement()); // by default stop tokenizer does not apply increments.
			}
		}

		[Test]
		public virtual void  TestStopListPositions()
		{
			bool defaultEnable = StopFilter.GetEnablePositionIncrementsDefault();
			StopFilter.SetEnablePositionIncrementsDefault(true);
			try
			{
				System.Collections.Hashtable stopWordsSet = new System.Collections.Hashtable();
				stopWordsSet.Add("good", "good");
				stopWordsSet.Add("test", "test");
				stopWordsSet.Add("analyzer", "analyzer");
				StopAnalyzer newStop = new StopAnalyzer(stopWordsSet);
				System.IO.StringReader reader = new System.IO.StringReader("This is a good test of the english stop analyzer with positions");
				int[] expectedIncr = new int[]{1, 1, 1, 3, 1, 1, 1, 2, 1};
				TokenStream stream = newStop.TokenStream("test", reader);
				Assert.IsNotNull(stream);
				Token token = null;
				int i = 0;
				while ((token = stream.Next()) != null)
				{
					System.String text = token.TermText();
					Assert.IsFalse(stopWordsSet.Contains(text));
					Assert.AreEqual(expectedIncr[i++], token.GetPositionIncrement());
				}
			}
			finally
			{
				StopFilter.SetEnablePositionIncrementsDefault(defaultEnable);
			}
		}
	}
}