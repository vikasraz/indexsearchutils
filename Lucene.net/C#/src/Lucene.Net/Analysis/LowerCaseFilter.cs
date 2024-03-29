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
	
    /// <summary> Normalizes token text to lower case.
    /// 
    /// </summary>
    /// <version>  $Id: LowerCaseFilter.java 564715 2007-08-10 18:34:33Z mikemccand $
    /// </version>
    public sealed class LowerCaseFilter : TokenFilter
    {
        public LowerCaseFilter(TokenStream in_Renamed) : base(in_Renamed)
        {
        }
		
        public override Token Next(Token result)
        {
            result = input.Next(result);
            if (result != null)
            {

                char[] buffer = result.TermBuffer();
                int length = result.termLength;
                for (int i = 0; i < length; i++)
                    buffer[i] = System.Char.ToLower(buffer[i]);

                return result;
            }
            else
                return null;
        }
    }
}