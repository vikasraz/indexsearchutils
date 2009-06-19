using System;
using System.Runtime.InteropServices;
using seg.result;
using Lucene.Net.Analysis;

namespace ConsoleApplication1
{
	/// <summary>
	/// 软件来源 http://www.lietu.com
	/// </summary>
	class Class1
	{
		/// <summary>
		/// 应用程序的主入口点。
		/// </summary>
		[DllImport("Kernel32.DLL", SetLastError=true)] 
		public static extern bool SetEnvironmentVariable(string lpName, string lpValue); 

		[STAThread]
        //static void Main(string[] args)
        //{
        //    SetEnvironmentVariable( "dic.dir", "F:/lwh/TestLucene/TestLucene/dic");
        //    //
        //    // TODO: 在此处添加代码以启动应用程序
        //    //
        //    testCnAnalyzer();
        //    System.Console.Read();
        //}
		
		public static void testCnAnalyzer() 
		{
			System.IO.TextReader input;

            try
            {
                CnTokenizer.makeTag = true;
            }
            //catch()
            //{
            //}
            finally
            {
                string sentence = "邀请王振国今年9月参加在洛杉矶举行的30届美国治癌成就大奖会";

                input = new System.IO.StringReader(sentence);
                TokenStream tokenizer = new seg.result.CnTokenizer(input);

                for (Token t = tokenizer.Next(); t != null; t = tokenizer.Next())
                {
                    System.Console.WriteLine(t.TermText() + " " + t.StartOffset() + " "
                        + t.EndOffset() + " " + t.Type());
                }
            }
		}
	}
}
