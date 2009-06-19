using System;
using System.Collections.Generic;
using System.Text;

namespace TestLucene
{
    class DataBase
    {
        //static void Main(string[] args)
        //{
        //    string sql, sqlconn, id, titleColumnName, indexDirectory, indexColumnName, startID;
        //    sql = sqlconn = id = titleColumnName = indexDirectory = indexColumnName = startID = null;
        //    XmlDocument xdoc = new XmlDocument();
        //    xdoc.Load("indexer.xml");
        //    XmlNodeList xnl = xdoc.SelectSingleNode("body").ChildNodes;
        //    string tableid = "";
        //    //显示xml列表
        //    Console.WriteLine("药王谷数据索引创建程序\n");
        //    foreach (XmlNode xn in xnl)
        //    {
        //        XmlElement bb = (XmlElement)xn;
        //        Console.WriteLine(bb.GetAttribute("id") + " " + bb.GetAttribute("tableName"));
        //        tableid += bb.GetAttribute("id") + ",";
        //    }
        //    Console.WriteLine("请选择要索引的表:");
        //    id = Console.ReadLine();
        //    if (tableid.IndexOf(id) == -1)
        //    {
        //        Console.WriteLine("您选择的是一个不存在的表ID");
        //        return;
        //    }
        //    else
        //    {
        //        //取得选中的xml文件的各列的值
        //        foreach (XmlNode xn in xnl)
        //        {
        //            XmlElement bb = (XmlElement)xn;
        //            if (bb.GetAttribute("id") == id)
        //            {
        //                sqlconn = bb.ChildNodes[0].InnerText;
        //                sql = bb.ChildNodes[1].InnerText;
        //                indexDirectory = bb.ChildNodes[2].InnerText;
        //                titleColumnName = bb.ChildNodes[3].InnerText;
        //                indexColumnName = bb.ChildNodes[4].InnerText;
        //                startID = bb.ChildNodes[5].InnerText;
        //            }
        //        }

        //    }

        //    Console.WriteLine("是否要重建全部索引(全部重建需要20分钟以上,注意大小写),默认为否(Yes/N)");
        //    bool reCreate = Console.ReadLine() == "Yes";
        //    //非重新索引，改变sql语句
        //    if (!reCreate)
        //        sql += " where " + indexColumnName + ">" + startID;
        //    Console.WriteLine("正在创建索引。。。。。。");
        //    DateTime startTime = DateTime.Now;

        //    CreateIndex writer = new CreateIndex(indexDirectory, reCreate);
        //    writer.AddHtmlToDocument(sql, sqlconn, indexColumnName, titleColumnName);

        //    //取得索引后的最大ID写回xml文件
        //    foreach (XmlNode xn in xnl)
        //    {
        //        XmlElement bb = (XmlElement)xn;
        //        if (bb.GetAttribute("id") == id)
        //        {
        //            startID = bb.ChildNodes[5].InnerText = writer.id;
        //            xdoc.Save("indexer.xml");
        //            break;
        //        }
        //    }
        //    Console.WriteLine(writer.id);
        //    writer.Close();
        //    Console.WriteLine("创建索引完毕，共花费时间： " + (DateTime.Now - startTime));
        //}

        //private static void xmlWriter()
        //{
        //    XmlDocument xdoc = new XmlDocument();
        //    xdoc.Load("indexer.xml");
        //    XmlElement xe = xdoc.CreateElement("project");
        //    XmlAttribute xa = xdoc.CreateAttribute("tableName");
        //    xa.InnerText = "zhaoshang";
        //    xe.SetAttributeNode(xa);

        //    XmlElement sqlconn = xdoc.CreateElement("sqlconn");
        //    sqlconn.InnerText = "serklsajfd;";
        //    xe.AppendChild(sqlconn);

        //    XmlElement sql = xdoc.CreateElement("sql");
        //    sql.InnerText = "serklsajfd;";
        //    xe.AppendChild(sql);
        //    xdoc.DocumentElement.AppendChild(xe);
        //    xdoc.Save("indexer.xml");
        //}

    }
}
