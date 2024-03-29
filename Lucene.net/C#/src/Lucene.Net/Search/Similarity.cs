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

using Term = Lucene.Net.Index.Term;
using SmallFloat = Lucene.Net.Util.SmallFloat;

namespace Lucene.Net.Search
{
	
	/// <summary>Expert: Scoring API.
	/// <p>Subclasses implement search scoring.
	/// 
	/// <p>The score of query <code>q</code> for document <code>d</code> correlates to the
	/// cosine-distance or dot-product between document and query vectors in a
	/// <a href="http://en.wikipedia.org/wiki/Vector_Space_Model">
	/// Vector Space Model (VSM) of Information Retrieval</a>.
	/// A document whose vector is closer to the query vector in that model is scored higher.
	/// 
	/// The score is computed as follows:
	/// 
	/// <P>
	/// <table cellpadding="1" cellspacing="0" border="1" align="center">
	/// <tr><td>
	/// <table cellpadding="1" cellspacing="0" border="0" align="center">
	/// <tr>
	/// <td valign="middle" align="right" rowspan="1">
	/// score(q,d) &nbsp; = &nbsp;
	/// <A HREF="#formula_coord">coord(q,d)</A> &nbsp;&middot;&nbsp;
	/// <A HREF="#formula_queryNorm">queryNorm(q)</A> &nbsp;&middot;&nbsp;
	/// </td>
	/// <td valign="bottom" align="center" rowspan="1">
	/// <big><big><big>&sum;</big></big></big>
	/// </td>
	/// <td valign="middle" align="right" rowspan="1">
	/// <big><big>(</big></big>
	/// <A HREF="#formula_tf">tf(t in d)</A> &nbsp;&middot;&nbsp;
	/// <A HREF="#formula_idf">idf(t)</A><sup>2</sup> &nbsp;&middot;&nbsp;
	/// <A HREF="#formula_termBoost">t.getBoost()</A>&nbsp;&middot;&nbsp;
	/// <A HREF="#formula_norm">norm(t,d)</A>
	/// <big><big>)</big></big>
	/// </td>
	/// </tr>
	/// <tr valigh="top">
	/// <td></td>
	/// <td align="center"><small>t in q</small></td>
	/// <td></td>
	/// </tr>
	/// </table>
	/// </td></tr>
	/// </table>
	/// 
	/// <p> where
	/// <ol>
	/// <li>
	/// <A NAME="formula_tf"></A>
	/// <b>tf(t in d)</b>
	/// correlates to the term's <i>frequency</i>,
	/// defined as the number of times term <i>t</i> appears in the currently scored document <i>d</i>.
	/// Documents that have more occurrences of a given term receive a higher score.
	/// The default computation for <i>tf(t in d)</i> in
	/// {@link Lucene.Net.Search.DefaultSimilarity#Tf(float) DefaultSimilarity} is:
	/// 
	/// <br>&nbsp;<br>
	/// <table cellpadding="2" cellspacing="2" border="0" align="center">
	/// <tr>
	/// <td valign="middle" align="right" rowspan="1">
	/// {@link Lucene.Net.Search.DefaultSimilarity#Tf(float) tf(t in d)} &nbsp; = &nbsp;
	/// </td>
	/// <td valign="top" align="center" rowspan="1">
	/// frequency<sup><big>&frac12;</big></sup>
	/// </td>
	/// </tr>
	/// </table>
	/// <br>&nbsp;<br>
	/// </li>
	/// 
	/// <li>
	/// <A NAME="formula_idf"></A>
	/// <b>idf(t)</b> stands for Inverse Document Frequency. This value
	/// correlates to the inverse of <i>docFreq</i>
	/// (the number of documents in which the term <i>t</i> appears).
	/// This means rarer terms give higher contribution to the total score.
	/// The default computation for <i>idf(t)</i> in
	/// {@link Lucene.Net.Search.DefaultSimilarity#Idf(int, int) DefaultSimilarity} is:
	/// 
	/// <br>&nbsp;<br>
	/// <table cellpadding="2" cellspacing="2" border="0" align="center">
	/// <tr>
	/// <td valign="middle" align="right">
	/// {@link Lucene.Net.Search.DefaultSimilarity#Idf(int, int) idf(t)}&nbsp; = &nbsp;
	/// </td>
	/// <td valign="middle" align="center">
	/// 1 + log <big>(</big>
	/// </td>
	/// <td valign="middle" align="center">
	/// <table>
	/// <tr><td align="center"><small>numDocs</small></td></tr>
	/// <tr><td align="center">&ndash;&ndash;&ndash;&ndash;&ndash;&ndash;&ndash;&ndash;&ndash;</td></tr>
	/// <tr><td align="center"><small>docFreq+1</small></td></tr>
	/// </table>
	/// </td>
	/// <td valign="middle" align="center">
	/// <big>)</big>
	/// </td>
	/// </tr>
	/// </table>
	/// <br>&nbsp;<br>
	/// </li>
	/// 
	/// <li>
	/// <A NAME="formula_coord"></A>
	/// <b>coord(q,d)</b>
	/// is a score factor based on how many of the query terms are found in the specified document.
	/// Typically, a document that contains more of the query's terms will receive a higher score
	/// than another document with fewer query terms.
	/// This is a search time factor computed in
	/// {@link #Coord(int, int) coord(q,d)}
	/// by the Similarity in effect at search time.
	/// <br>&nbsp;<br>
	/// </li>
	/// 
	/// <li><b>
	/// <A NAME="formula_queryNorm"></A>
	/// queryNorm(q)
	/// </b>
	/// is a normalizing factor used to make scores between queries comparable.
	/// This factor does not affect document ranking (since all ranked documents are multiplied by the same factor),
	/// but rather just attempts to make scores from different queries (or even different indexes) comparable.
	/// This is a search time factor computed by the Similarity in effect at search time.
	/// 
	/// The default computation in
	/// {@link Lucene.Net.Search.DefaultSimilarity#QueryNorm(float) DefaultSimilarity}
	/// is:
	/// <br>&nbsp;<br>
	/// <table cellpadding="1" cellspacing="0" border="0" align="center">
	/// <tr>
	/// <td valign="middle" align="right" rowspan="1">
	/// queryNorm(q)  &nbsp; = &nbsp;
	/// {@link Lucene.Net.Search.DefaultSimilarity#QueryNorm(float) queryNorm(sumOfSquaredWeights)}
	/// &nbsp; = &nbsp;
	/// </td>
	/// <td valign="middle" align="center" rowspan="1">
	/// <table>
	/// <tr><td align="center"><big>1</big></td></tr>
	/// <tr><td align="center"><big>
	/// &ndash;&ndash;&ndash;&ndash;&ndash;&ndash;&ndash;&ndash;&ndash;&ndash;&ndash;&ndash;&ndash;&ndash;
	/// </big></td></tr>
	/// <tr><td align="center">sumOfSquaredWeights<sup><big>&frac12;</big></sup></td></tr>
	/// </table>
	/// </td>
	/// </tr>
	/// </table>
	/// <br>&nbsp;<br>
	/// 
	/// The sum of squared weights (of the query terms) is
	/// computed by the query {@link Lucene.Net.Search.Weight} object.
	/// For example, a {@link Lucene.Net.Search.BooleanQuery boolean query}
	/// computes this value as:
	/// 
	/// <br>&nbsp;<br>
	/// <table cellpadding="1" cellspacing="0" border="0"n align="center">
	/// <tr>
	/// <td valign="middle" align="right" rowspan="1">
	/// {@link Lucene.Net.Search.Weight#SumOfSquaredWeights() sumOfSquaredWeights} &nbsp; = &nbsp;
	/// {@link Lucene.Net.Search.Query#GetBoost() q.getBoost()} <sup><big>2</big></sup>
	/// &nbsp;&middot;&nbsp;
	/// </td>
	/// <td valign="bottom" align="center" rowspan="1">
	/// <big><big><big>&sum;</big></big></big>
	/// </td>
	/// <td valign="middle" align="right" rowspan="1">
	/// <big><big>(</big></big>
	/// <A HREF="#formula_idf">idf(t)</A> &nbsp;&middot;&nbsp;
	/// <A HREF="#formula_termBoost">t.getBoost()</A>
	/// <big><big>) <sup>2</sup> </big></big>
	/// </td>
	/// </tr>
	/// <tr valigh="top">
	/// <td></td>
	/// <td align="center"><small>t in q</small></td>
	/// <td></td>
	/// </tr>
	/// </table>
	/// <br>&nbsp;<br>
	/// 
	/// </li>
	/// 
	/// <li>
	/// <A NAME="formula_termBoost"></A>
	/// <b>t.getBoost()</b>
	/// is a search time boost of term <i>t</i> in the query <i>q</i> as
	/// specified in the query text
	/// (see <A HREF="../../../../../queryparsersyntax.html#Boosting a Term">query syntax</A>),
	/// or as set by application calls to
	/// {@link Lucene.Net.Search.Query#SetBoost(float) setBoost()}.
	/// Notice that there is really no direct API for accessing a boost of one term in a multi term query,
	/// but rather multi terms are represented in a query as multi
	/// {@link Lucene.Net.Search.TermQuery TermQuery} objects,
	/// and so the boost of a term in the query is accessible by calling the sub-query
	/// {@link Lucene.Net.Search.Query#GetBoost() getBoost()}.
	/// <br>&nbsp;<br>
	/// </li>
	/// 
	/// <li>
	/// <A NAME="formula_norm"></A>
	/// <b>norm(t,d)</b> encapsulates a few (indexing time) boost and length factors:
	/// 
	/// <ul>
	/// <li><b>Document boost</b> - set by calling
	/// {@link Lucene.Net.Documents.Document#SetBoost(float) doc.setBoost()}
	/// before adding the document to the index.
	/// </li>
	/// <li><b>Field boost</b> - set by calling
	/// {@link Lucene.Net.Documents.Fieldable#SetBoost(float) field.setBoost()}
	/// before adding the field to a document.
	/// </li>
	/// <li>{@link #LengthNorm(String, int) <b>lengthNorm</b>(field)} - computed
	/// when the document is added to the index in accordance with the number of tokens
	/// of this field in the document, so that shorter fields contribute more to the score.
	/// LengthNorm is computed by the Similarity class in effect at indexing.
	/// </li>
	/// </ul>
	/// 
	/// <p>
	/// When a document is added to the index, all the above factors are multiplied.
	/// If the document has multiple fields with the same name, all their boosts are multiplied together:
	/// 
	/// <br>&nbsp;<br>
	/// <table cellpadding="1" cellspacing="0" border="0"n align="center">
	/// <tr>
	/// <td valign="middle" align="right" rowspan="1">
	/// norm(t,d) &nbsp; = &nbsp;
	/// {@link Lucene.Net.Documents.Document#GetBoost() doc.getBoost()}
	/// &nbsp;&middot;&nbsp;
	/// {@link #LengthNorm(String, int) lengthNorm(field)}
	/// &nbsp;&middot;&nbsp;
	/// </td>
	/// <td valign="bottom" align="center" rowspan="1">
	/// <big><big><big>&prod;</big></big></big>
	/// </td>
	/// <td valign="middle" align="right" rowspan="1">
	/// {@link Lucene.Net.Documents.Fieldable#GetBoost() f.getBoost}()
	/// </td>
	/// </tr>
	/// <tr valigh="top">
	/// <td></td>
	/// <td align="center"><small>field <i><b>f</b></i> in <i>d</i> named as <i><b>t</b></i></small></td>
	/// <td></td>
	/// </tr>
	/// </table>
	/// <br>&nbsp;<br>
	/// However the resulted <i>norm</i> value is {@link #EncodeNorm(float) encoded} as a single byte
	/// before being stored.
	/// At search time, the norm byte value is read from the index
	/// {@link Lucene.Net.Store.Directory directory} and
	/// {@link #DecodeNorm(byte) decoded} back to a float <i>norm</i> value.
	/// This encoding/decoding, while reducing index size, comes with the price of
	/// precision loss - it is not guaranteed that decode(encode(x)) = x.
	/// For instance, decode(encode(0.89)) = 0.75.
	/// Also notice that search time is too late to modify this <i>norm</i> part of scoring, e.g. by
	/// using a different {@link Similarity} for search.
	/// <br>&nbsp;<br>
	/// </li>
	/// </ol>
	/// 
	/// </summary>
	/// <seealso cref="#SetDefault(Similarity)">
	/// </seealso>
	/// <seealso cref="IndexWriter#SetSimilarity(Similarity)">
	/// </seealso>
	/// <seealso cref="Searcher#SetSimilarity(Similarity)">
	/// </seealso>
	[Serializable]
	public abstract class Similarity
	{
		/// <summary>The Similarity implementation used by default. </summary>
		private static Similarity defaultImpl = new DefaultSimilarity();
		
		/// <summary>Set the default Similarity implementation used by indexing and search
		/// code.
		/// 
		/// </summary>
		/// <seealso cref="Searcher#SetSimilarity(Similarity)">
		/// </seealso>
		/// <seealso cref="IndexWriter#SetSimilarity(Similarity)">
		/// </seealso>
		public static void  SetDefault(Similarity similarity)
		{
			Similarity.defaultImpl = similarity;
		}
		
		/// <summary>Return the default Similarity implementation used by indexing and search
		/// code.
		/// 
		/// <p>This is initially an instance of {@link DefaultSimilarity}.
		/// 
		/// </summary>
		/// <seealso cref="Searcher#SetSimilarity(Similarity)">
		/// </seealso>
		/// <seealso cref="IndexWriter#SetSimilarity(Similarity)">
		/// </seealso>
		public static Similarity GetDefault()
		{
			return Similarity.defaultImpl;
		}
		
		/// <summary>Cache of decoded bytes. </summary>
		private static readonly float[] NORM_TABLE = new float[256];
		
		/// <summary>Decodes a normalization factor stored in an index.</summary>
		/// <seealso cref="#EncodeNorm(float)">
		/// </seealso>
		public static float DecodeNorm(byte b)
		{
			return NORM_TABLE[b & 0xFF]; // & 0xFF maps negative bytes to positive above 127
		}
		
		/// <summary>Returns a table for decoding normalization bytes.</summary>
		/// <seealso cref="#EncodeNorm(float)">
		/// </seealso>
		public static float[] GetNormDecoder()
		{
			return NORM_TABLE;
		}
		
		/// <summary>Computes the normalization value for a field given the total number of
		/// terms contained in a field.  These values, together with field boosts, are
		/// stored in an index and multipled into scores for hits on each field by the
		/// search code.
		/// 
		/// <p>Matches in longer fields are less precise, so implementations of this
		/// method usually return smaller values when <code>numTokens</code> is large,
		/// and larger values when <code>numTokens</code> is small.
		/// 
		/// <p>That these values are computed under 
		/// {@link Lucene.Net.Index.IndexWriter#AddDocument(Lucene.Net.Documents.Document)} 
		/// and stored then using
		/// {@link #EncodeNorm(float)}.  
		/// Thus they have limited precision, and documents
		/// must be re-indexed if this method is altered.
		/// 
		/// </summary>
		/// <param name="fieldName">the name of the field
		/// </param>
		/// <param name="numTokens">the total number of tokens contained in fields named
		/// <i>fieldName</i> of <i>doc</i>.
		/// </param>
		/// <returns> a normalization factor for hits on this field of this document
		/// 
		/// </returns>
		/// <seealso cref="Lucene.Net.Documents.Field.SetBoost(float)">
		/// </seealso>
		public abstract float LengthNorm(System.String fieldName, int numTokens);
		
		/// <summary>Computes the normalization value for a query given the sum of the squared
		/// weights of each of the query terms.  This value is then multipled into the
		/// weight of each query term.
		/// 
		/// <p>This does not affect ranking, but rather just attempts to make scores
		/// from different queries comparable.
		/// 
		/// </summary>
		/// <param name="sumOfSquaredWeights">the sum of the squares of query term weights
		/// </param>
		/// <returns> a normalization factor for query weights
		/// </returns>
		public abstract float QueryNorm(float sumOfSquaredWeights);
		
		/// <summary>Encodes a normalization factor for storage in an index.
		/// 
		/// <p>The encoding uses a three-bit mantissa, a five-bit exponent, and
		/// the zero-exponent point at 15, thus
		/// representing values from around 7x10^9 to 2x10^-9 with about one
		/// significant decimal digit of accuracy.  Zero is also represented.
		/// Negative numbers are rounded up to zero.  Values too large to represent
		/// are rounded down to the largest representable value.  Positive values too
		/// small to represent are rounded up to the smallest positive representable
		/// value.
		/// 
		/// </summary>
		/// <seealso cref="Lucene.Net.Documents.Field#SetBoost(float)">
		/// </seealso>
		/// <seealso cref="SmallFloat">
		/// </seealso>
		public static byte EncodeNorm(float f)
		{
			return (byte) SmallFloat.FloatToByte315(f);
		}
		
		
		/// <summary>Computes a score factor based on a term or phrase's frequency in a
		/// document.  This value is multiplied by the {@link #Idf(Term, Searcher)}
		/// factor for each term in the query and these products are then summed to
		/// form the initial score for a document.
		/// 
		/// <p>Terms and phrases repeated in a document indicate the topic of the
		/// document, so implementations of this method usually return larger values
		/// when <code>freq</code> is large, and smaller values when <code>freq</code>
		/// is small.
		/// 
		/// <p>The default implementation calls {@link #Tf(float)}.
		/// 
		/// </summary>
		/// <param name="freq">the frequency of a term within a document
		/// </param>
		/// <returns> a score factor based on a term's within-document frequency
		/// </returns>
		public virtual float Tf(int freq)
		{
			return Tf((float) freq);
		}
		
		/// <summary>Computes the amount of a sloppy phrase match, based on an edit distance.
		/// This value is summed for each sloppy phrase match in a document to form
		/// the frequency that is passed to {@link #Tf(float)}.
		/// 
		/// <p>A phrase match with a small edit distance to a document passage more
		/// closely matches the document, so implementations of this method usually
		/// return larger values when the edit distance is small and smaller values
		/// when it is large.
		/// 
		/// </summary>
		/// <seealso cref="PhraseQuery.SetSlop(int)">
		/// </seealso>
		/// <param name="distance">the edit distance of this sloppy phrase match
		/// </param>
		/// <returns> the frequency increment for this match
		/// </returns>
		public abstract float SloppyFreq(int distance);
		
		/// <summary>Computes a score factor based on a term or phrase's frequency in a
		/// document.  This value is multiplied by the {@link #Idf(Term, Searcher)}
		/// factor for each term in the query and these products are then summed to
		/// form the initial score for a document.
		/// 
		/// <p>Terms and phrases repeated in a document indicate the topic of the
		/// document, so implementations of this method usually return larger values
		/// when <code>freq</code> is large, and smaller values when <code>freq</code>
		/// is small.
		/// 
		/// </summary>
		/// <param name="freq">the frequency of a term within a document
		/// </param>
		/// <returns> a score factor based on a term's within-document frequency
		/// </returns>
		public abstract float Tf(float freq);
		
		/// <summary>Computes a score factor for a simple term.
		/// 
		/// <p>The default implementation is:<pre>
		/// return idf(searcher.docFreq(term), searcher.maxDoc());
		/// </pre>
		/// 
		/// Note that {@link Searcher#MaxDoc()} is used instead of
		/// {@link IndexReader#NumDocs()} because it is proportional to
		/// {@link Searcher#DocFreq(Term)} , i.e., when one is inaccurate,
		/// so is the other, and in the same direction.
		/// 
		/// </summary>
		/// <param name="term">the term in question
		/// </param>
		/// <param name="searcher">the document collection being searched
		/// </param>
		/// <returns> a score factor for the term
		/// </returns>
		public virtual float Idf(Term term, Searcher searcher)
		{
			return Idf(searcher.DocFreq(term), searcher.MaxDoc());
		}
		
		/// <summary>Computes a score factor for a phrase.
		/// 
		/// <p>The default implementation sums the {@link #Idf(Term,Searcher)} factor
		/// for each term in the phrase.
		/// 
		/// </summary>
		/// <param name="terms">the terms in the phrase
		/// </param>
		/// <param name="searcher">the document collection being searched
		/// </param>
		/// <returns> a score factor for the phrase
		/// </returns>
		public virtual float Idf(System.Collections.ICollection terms, Searcher searcher)
		{
			float idf = 0.0f;
			System.Collections.IEnumerator i = terms.GetEnumerator();
			while (i.MoveNext())
			{
				idf += Idf((Term) i.Current, searcher);
			}
			return idf;
		}
		
		/// <summary>Computes a score factor based on a term's document frequency (the number
		/// of documents which contain the term).  This value is multiplied by the
		/// {@link #Tf(int)} factor for each term in the query and these products are
		/// then summed to form the initial score for a document.
		/// 
		/// <p>Terms that occur in fewer documents are better indicators of topic, so
		/// implementations of this method usually return larger values for rare terms,
		/// and smaller values for common terms.
		/// 
		/// </summary>
		/// <param name="docFreq">the number of documents which contain the term
		/// </param>
		/// <param name="numDocs">the total number of documents in the collection
		/// </param>
		/// <returns> a score factor based on the term's document frequency
		/// </returns>
		public abstract float Idf(int docFreq, int numDocs);
		
		/// <summary>Computes a score factor based on the fraction of all query terms that a
		/// document contains.  This value is multiplied into scores.
		/// 
		/// <p>The presence of a large portion of the query terms indicates a better
		/// match with the query, so implementations of this method usually return
		/// larger values when the ratio between these parameters is large and smaller
		/// values when the ratio between them is small.
		/// 
		/// </summary>
		/// <param name="overlap">the number of query terms matched in the document
		/// </param>
		/// <param name="maxOverlap">the total number of terms in the query
		/// </param>
		/// <returns> a score factor based on term overlap with the query
		/// </returns>
		public abstract float Coord(int overlap, int maxOverlap);
		
		
		/// <summary> Calculate a scoring factor based on the data in the payload.  Overriding implementations
		/// are responsible for interpreting what is in the payload.  Lucene makes no assumptions about
		/// what is in the byte array.
		/// <p>
		/// The default implementation returns 1.
		/// 
		/// </summary>
		/// <param name="fieldName">The fieldName of the term this payload belongs to
		/// </param>
		/// <param name="payload">The payload byte array to be scored
		/// </param>
		/// <param name="offset">The offset into the payload array
		/// </param>
		/// <param name="length">The length in the array
		/// </param>
		/// <returns> An implementation dependent float to be used as a scoring factor 
		/// </returns>
		public virtual float ScorePayload(System.String fieldName, byte[] payload, int offset, int length)
		{
			//Do nothing
			return 1;
		}
		static Similarity()
		{
			{
				for (int i = 0; i < 256; i++)
					NORM_TABLE[i] = SmallFloat.Byte315ToFloat((byte) i);
			}
		}
	}
}