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
using Index = Lucene.Net.Documents.Field.Index;
using Store = Lucene.Net.Documents.Field.Store;
using Directory = Lucene.Net.Store.Directory;
using FSDirectory = Lucene.Net.Store.FSDirectory;
using Analyzer = Lucene.Net.Analysis.Analyzer;
using StandardAnalyzer = Lucene.Net.Analysis.Standard.StandardAnalyzer;
using DefaultSimilarity = Lucene.Net.Search.DefaultSimilarity;
using Similarity = Lucene.Net.Search.Similarity;
using LuceneTestCase = Lucene.Net.Util.LuceneTestCase;

namespace Lucene.Net.Index
{
	
	/// <summary> Test that norms info is preserved during index life - including
	/// separate norms, addDocument, addIndexes, optimize.
	/// </summary>
	[TestFixture]
	public class TestNorms : LuceneTestCase
	{
		
		[Serializable]
		private class SimilarityOne : DefaultSimilarity
		{
			public SimilarityOne(TestNorms enclosingInstance)
			{
				InitBlock(enclosingInstance);
			}
			private void  InitBlock(TestNorms enclosingInstance)
			{
				this.enclosingInstance = enclosingInstance;
			}
			private TestNorms enclosingInstance;
			public TestNorms Enclosing_Instance
			{
				get
				{
					return enclosingInstance;
				}
				
			}
			public override float LengthNorm(System.String fieldName, int numTerms)
			{
				return 1;
			}
		}
		
		private const int NUM_FIELDS = 10;
		
		private Similarity similarityOne;
		private Analyzer anlzr;
		private int numDocNorms;
		private System.Collections.ArrayList norms;
		private System.Collections.ArrayList modifiedNorms;
		private float lastNorm = 0;
		private float normDelta = (float) 0.001;
		
		
		[SetUp]
		public override void SetUp()
		{
			base.SetUp();
			similarityOne = new SimilarityOne(this);
			anlzr = new StandardAnalyzer();
		}
		
		/// <summary> Test that norms values are preserved as the index is maintained.
		/// Including separate norms.
		/// Including merging indexes with seprate norms. 
		/// Including optimize. 
		/// </summary>
		[Test]
		public virtual void  _TestNorms()
		{
			// tmp dir
			System.String tempDir = System.IO.Path.GetTempPath();
			if (tempDir == null)
			{
				throw new System.IO.IOException("java.io.tmpdir undefined, cannot run test");
			}
			
			// test with a single index: index1
			System.IO.FileInfo indexDir1 = new System.IO.FileInfo(System.IO.Path.Combine(tempDir, "lucenetestindex1"));
			Directory dir1 = FSDirectory.GetDirectory(indexDir1);
			
			norms = new System.Collections.ArrayList();
			modifiedNorms = new System.Collections.ArrayList();
			
			CreateIndex(dir1);
			DoTestNorms(dir1);
			
			// test with a single index: index2
			System.Collections.ArrayList norms1 = norms;
			System.Collections.ArrayList modifiedNorms1 = modifiedNorms;
			int numDocNorms1 = numDocNorms;
			
			norms = new System.Collections.ArrayList();
			modifiedNorms = new System.Collections.ArrayList();
			numDocNorms = 0;
			
			System.IO.FileInfo indexDir2 = new System.IO.FileInfo(System.IO.Path.Combine(tempDir, "lucenetestindex2"));
			Directory dir2 = FSDirectory.GetDirectory(indexDir2);
			
			CreateIndex(dir2);
			DoTestNorms(dir2);
			
			// add index1 and index2 to a third index: index3
			System.IO.FileInfo indexDir3 = new System.IO.FileInfo(System.IO.Path.Combine(tempDir, "lucenetestindex3"));
			Directory dir3 = FSDirectory.GetDirectory(indexDir3);
			
			CreateIndex(dir3);
			IndexWriter iw = new IndexWriter(dir3, anlzr, false);
			iw.SetMaxBufferedDocs(5);
			iw.SetMergeFactor(3);
			iw.AddIndexes(new Directory[]{dir1, dir2});
			iw.Close();
			
			norms1.AddRange(norms);
			norms = norms1;
			modifiedNorms1.AddRange(modifiedNorms);
			modifiedNorms = modifiedNorms1;
			numDocNorms += numDocNorms1;
			
			// test with index3
			VerifyIndex(dir3);
			DoTestNorms(dir3);
			
			// now with optimize
			iw = new IndexWriter(dir3, anlzr, false);
			iw.SetMaxBufferedDocs(5);
			iw.SetMergeFactor(3);
			iw.Optimize();
			iw.Close();
			VerifyIndex(dir3);
			
			dir1.Close();
			dir2.Close();
			dir3.Close();
		}
		
		private void  DoTestNorms(Directory dir)
		{
			for (int i = 0; i < 5; i++)
			{
				AddDocs(dir, 12, true);
				VerifyIndex(dir);
				ModifyNormsForF1(dir);
				VerifyIndex(dir);
				AddDocs(dir, 12, false);
				VerifyIndex(dir);
				ModifyNormsForF1(dir);
				VerifyIndex(dir);
			}
		}
		
		private void  CreateIndex(Directory dir)
		{
			IndexWriter iw = new IndexWriter(dir, anlzr, true);
			iw.SetMaxBufferedDocs(5);
			iw.SetMergeFactor(3);
			iw.SetSimilarity(similarityOne);
			iw.SetUseCompoundFile(true);
			iw.Close();
		}
		
		private void  ModifyNormsForF1(Directory dir)
		{
			IndexReader ir = IndexReader.Open(dir);
			int n = ir.MaxDoc();
			for (int i = 0; i < n; i += 3)
			{
				// modify for every third doc
				int k = (i * 3) % modifiedNorms.Count;
				float origNorm = (float) ((System.Single) modifiedNorms[i]);
				float newNorm = (float) ((System.Single) modifiedNorms[k]);
				//System.out.println("Modifying: for "+i+" from "+origNorm+" to "+newNorm);
				//System.out.println("      and: for "+k+" from "+newNorm+" to "+origNorm);
				modifiedNorms[i] = (float) newNorm;
				modifiedNorms[k] = (float) origNorm;
				ir.SetNorm(i, "f" + 1, newNorm);
				ir.SetNorm(k, "f" + 1, origNorm);
			}
			ir.Close();
		}
		
		
		private void  VerifyIndex(Directory dir)
		{
			IndexReader ir = IndexReader.Open(dir);
			for (int i = 0; i < NUM_FIELDS; i++)
			{
				System.String field = "f" + i;
				byte[] b = ir.Norms(field);
				Assert.AreEqual(numDocNorms, b.Length, "number of norms mismatches");
				System.Collections.ArrayList storedNorms = (i == 1?modifiedNorms:norms);
				for (int j = 0; j < b.Length; j++)
				{
					float norm = Similarity.DecodeNorm(b[j]);
					float norm1 = (float) ((System.Single) storedNorms[j]);
					Assert.AreEqual(norm, norm1, 0.000001, "stored norm value of " + field + " for doc " + j + " is " + norm + " - a mismatch!");
				}
			}
		}
		
		private void  AddDocs(Directory dir, int ndocs, bool compound)
		{
			IndexWriter iw = new IndexWriter(dir, anlzr, false);
			iw.SetMaxBufferedDocs(5);
			iw.SetMergeFactor(3);
			iw.SetSimilarity(similarityOne);
			iw.SetUseCompoundFile(compound);
			for (int i = 0; i < ndocs; i++)
			{
				iw.AddDocument(NewDoc());
			}
			iw.Close();
		}
		
		// create the next document
		private Lucene.Net.Documents.Document NewDoc()
		{
			Lucene.Net.Documents.Document d = new Lucene.Net.Documents.Document();
			float boost = NextNorm();
			for (int i = 0; i < 10; i++)
			{
				Field f = new Field("f" + i, "v" + i, Lucene.Net.Documents.Field.Store.NO, Lucene.Net.Documents.Field.Index.UN_TOKENIZED);
				f.SetBoost(boost);
				d.Add(f);
			}
			return d;
		}
		
		// return unique norm values that are unchanged by encoding/decoding
		private float NextNorm()
		{
			float norm = lastNorm + normDelta;
			do 
			{
				float norm1 = Similarity.DecodeNorm(Similarity.EncodeNorm(norm));
				if (norm1 > lastNorm)
				{
					//System.out.println(norm1+" > "+lastNorm);
					norm = norm1;
					break;
				}
				norm += normDelta;
			}
			while (true);
			norms.Insert(numDocNorms, (float) norm);
			modifiedNorms.Insert(numDocNorms, (float) norm);
			//System.out.println("creating norm("+numDocNorms+"): "+norm);
			numDocNorms++;
			lastNorm = (norm > 10 ? 0 : norm); //there's a limit to how many distinct values can be stored in a ingle byte
			return norm;
		}
	}
}