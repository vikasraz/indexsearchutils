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

namespace Lucene.Net.Analysis
{
	
    /// <summary> This analyzer is used to facilitate scenarios where different
    /// fields require different analysis techniques.  Use {@link #addAnalyzer}
    /// to add a non-default analyzer on a field name basis.
    /// 
    /// <p>Example usage:
    /// 
    /// <pre>
    /// PerFieldAnalyzerWrapper aWrapper =
    /// new PerFieldAnalyzerWrapper(new StandardAnalyzer());
    /// aWrapper.addAnalyzer("firstname", new KeywordAnalyzer());
    /// aWrapper.addAnalyzer("lastname", new KeywordAnalyzer());
    /// </pre>
    /// 
    /// <p>In this example, StandardAnalyzer will be used for all fields except "firstname"
    /// and "lastname", for which KeywordAnalyzer will be used.
    /// 
    /// <p>A PerFieldAnalyzerWrapper can be used like any other analyzer, for both indexing
    /// and query parsing.
    /// </summary>
    public class PerFieldAnalyzerWrapper:Analyzer
    {
        private Analyzer defaultAnalyzer;
        private System.Collections.IDictionary analyzerMap = new System.Collections.Hashtable();
		
		
        /// <summary> Constructs with default analyzer.
        /// 
        /// </summary>
        /// <param name="defaultAnalyzer">Any fields not specifically
        /// defined to use a different analyzer will use the one provided here.
        /// </param>
        public PerFieldAnalyzerWrapper(Analyzer defaultAnalyzer)
        {
            this.defaultAnalyzer = defaultAnalyzer;
        }
		
        /// <summary> Defines an analyzer to use for the specified field.
        /// 
        /// </summary>
        /// <param name="fieldName">field name requiring a non-default analyzer
        /// </param>
        /// <param name="analyzer">non-default analyzer to use for field
        /// </param>
        public virtual void  AddAnalyzer(System.String fieldName, Analyzer analyzer)
        {
            analyzerMap[fieldName] = analyzer;
        }
		
        public override TokenStream TokenStream(System.String fieldName, System.IO.TextReader reader)
        {
            Analyzer analyzer = (Analyzer) analyzerMap[fieldName];
            if (analyzer == null)
            {
                analyzer = defaultAnalyzer;
            }
			
            return analyzer.TokenStream(fieldName, reader);
        }

        public override TokenStream ReusableTokenStream(System.String fieldName, System.IO.TextReader reader)
        {
            Analyzer analyzer = (Analyzer)analyzerMap[fieldName];
            if (analyzer == null)
                analyzer = defaultAnalyzer;

            return analyzer.ReusableTokenStream(fieldName, reader);
        }

        /// <summary>Return the positionIncrementGap from the analyzer assigned to fieldName </summary>
        public override int GetPositionIncrementGap(System.String fieldName)
        {
            Analyzer analyzer = (Analyzer) analyzerMap[fieldName];
            if (analyzer == null)
                analyzer = defaultAnalyzer;
            return analyzer.GetPositionIncrementGap(fieldName);
        }
		
        public override System.String ToString()
        {
            // {{Aroush-1.9}} 'analyzerMap.ToString()' may return a different value then Java.
            return "PerFieldAnalyzerWrapper(" + analyzerMap.ToString() + ", default=" + defaultAnalyzer + ")";
        }
    }
}