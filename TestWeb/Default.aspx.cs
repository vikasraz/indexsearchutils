using System;
using System.Data;
using System.Configuration;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using Lucene.Net.Documents;
using ISUtils.Common;
using ISUtils.Searcher;

public partial class _Default : System.Web.UI.Page 
{
    public string result;
    private const int portNum = 3312;
    //private const string hostName = "192.168.1.102";

    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(txtSearch.Text) && !string.IsNullOrEmpty(txtIP.Text))
        {
            
            result = txtSearch.Text+"\n";
            TcpClient client;
            NetworkStream ns;
            BinaryFormatter formater;
            DateTime now = DateTime.Now;
            try
            {
                client = new TcpClient(txtIP.Text, portNum);
                ns = client.GetStream();
                formater = new BinaryFormatter();
            }
            catch (Exception ex)
            {
                Response.Write(ex.StackTrace.ToString() + "<br>");
                return;
            }
            SearchInfo sinfo = new SearchInfo();
            QueryInfo info = new QueryInfo();
            info.IndexNames = txtIndexName.Text;
            info.SearchWords = txtSearch.Text;
            sinfo.Query = info;
            sinfo.HighLight = true;
            try
            {
                formater.Serialize(ns, sinfo);
            }
            catch (SerializationException se)
            {
                Response.Write(se.Message + "\n");
            }
            SearchResult sr= null;
            try
            {
                DateTime start = DateTime.Now;
                sr = (SearchResult)formater.Deserialize(ns);
                TimeSpan span = DateTime.Now - start;
                Response.Write("反序列化:"+span.TotalMilliseconds.ToString() + "毫秒<br>");
            }
            catch (SerializationException sep)
            {
                Response.Write(sep.Message + "\n");
            }
            finally
            {
                ns.Close();
            }
            TimeSpan tm = DateTime.Now - now;
            Response.Write(sr.ToString() + "<br>");
            foreach (SearchRecord record in sr.Records)
            {
                Response.Write("----------------------------------------<br>");
                foreach (SearchField field in record.Fields)
                {
                    Response.Write(field.Name+":\t"+field.Value + "<br>");
                }
            }
            Response.Write("搜索测试完成，花费时间：" + tm.TotalMilliseconds.ToString() + "毫秒\n");
        }
    }
    protected void btnExactSearch_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(txtSearch.Text) || string.IsNullOrEmpty(txtIP.Text))
            return;
        if (string.IsNullOrEmpty(txtFieldInclude.Text) && string.IsNullOrEmpty(txtFieldExclude.Text) &&
            string.IsNullOrEmpty(txtRangeFrom.Text))
            return;
        TcpClient client;
        NetworkStream ns;
        BinaryFormatter formater;
        DateTime now = DateTime.Now;
        try
        {
            client = new TcpClient(txtIP.Text, portNum);
            ns = client.GetStream();
            formater = new BinaryFormatter();
        }
        catch (Exception ex)
        {
            Response.Write(ex.StackTrace.ToString() + "<br>");
            return;
        }
        SearchInfo sinfo = new SearchInfo();
        QueryInfo info = new QueryInfo();
        info.IndexNames = txtIndexName.Text;
        if (!string.IsNullOrEmpty(txtFieldInclude.Text ) && !string.IsNullOrEmpty(txtWordsInclude.Text))
            info.FilterList.Add(new FilterCondition("", txtFieldInclude.Text, txtWordsInclude.Text));
        if (!string.IsNullOrEmpty(txtFieldExclude.Text) && !string.IsNullOrEmpty(txtWordsExclude.Text))
            info.ExcludeList.Add(new ExcludeCondition("", txtFieldExclude.Text, txtWordsExclude.Text));
        if (!string.IsNullOrEmpty(txtFieldRange.Text) && !string.IsNullOrEmpty(txtRangeFrom.Text) && !string.IsNullOrEmpty(txtRangeTo.Text))
            info.RangeList.Add(new RangeCondition("", txtFieldRange.Text, txtRangeFrom.Text, txtRangeTo.Text, RangeType.Date));
        sinfo.Query = info;
        try
        {
            formater.Serialize(ns, sinfo);
        }
        catch (SerializationException se)
        {
            Response.Write(se.Message + "\n");
        }
        SearchResult sr = null;
        try
        {
            DateTime start = DateTime.Now;
            sr = (SearchResult)formater.Deserialize(ns);
            TimeSpan span = DateTime.Now - start;
            Response.Write("反序列化:" + span.TotalMilliseconds.ToString() + "毫秒<br>");
        }
        catch (SerializationException sep)
        {
            Response.Write(sep.Message + "\n");
        }
        finally
        {
            ns.Close();
        }
        TimeSpan tm = DateTime.Now - now;
        Response.Write(sr.ToString() + "<br>");
        foreach (SearchRecord record in sr.Records)
        {
            Response.Write("----------------------------------------<br>");
            foreach (SearchField field in record.Fields)
            {
                Response.Write(field.Name + ":\t" + field.Value + "<br>");
            }
        }
        Response.Write("搜索测试完成，花费时间：" + tm.TotalMilliseconds.ToString() + "毫秒\n");
    }
}
