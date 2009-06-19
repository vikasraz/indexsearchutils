using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Configuration;

namespace ISUtils.Database.Link
{
    public interface DataBaseLinker
    {
        /**/
        /// <summary>
        /// 与数据库建立连接
        /// </summary>
        /// <param name="connectString">数据库连接字符串</param>
        /// <returns>返回类型</returns>
        bool Connect(string connectString);
        /**/
        /// <summary>
        /// 与数据库建立连接
        /// </summary>
        /// <param name="connectStrs">数据库连接参数列表</param>
        /// <returns>返回类型</returns>
        bool Connect(params string[] connectStrs);
        /**/
        /// <summary>
        /// 执行数据库查询语句
        /// </summary>
        /// <param name="strSQL">结构化数据库查询语句</param>
        /// <returns>返回类型</returns>
        DataTable ExecuteSQL(string strSQL);
        /**/
        /// <summary>
        /// 执行数据库查询语句
        /// </summary>
        /// <param name="sql">结构化数据库查询语句</param>
        void ExecSQL(string sql);
        /**/
        /// <summary>
        /// 关闭数据库连接
        /// </summary>
        void Close();
    }
}
//SQL SERVER连接字符串
//Data Source（数据源）、Initial Catalog（初始编目）、User ID（用户ID）、和Password（密码）等元素一起，下面这些选项都是可用的：
//# Application Name（应用程序名称）：应用程序的名称。如果没有被指定的话，它的值为.NET SqlClient Data Provider（数据提供程序）.
//# AttachDBFilename／extended properties（扩展属性）／Initial File Name（初始文件名）：可连接数据库的主要文件的名称，包括完整路径名称。数据库名称必须用关键字数据库指定。
//# Connect Timeout（连接超时）／Connection Timeout（连接超时）：一个到服务器的连接在终止之前等待的时间长度（以秒计），缺省值为15。
//# Connection Lifetime（连接生存时间）：当一个连接被返回到连接池时，它的创建时间会与当前时间进行对比。如果这个时间跨度超过了连接的有效期的话，连接就被取消。其缺省值为0。
//# Connection Reset（连接重置）：表示一个连接在从连接池中被移除时是否被重置。一个伪的有效在获得一个连接的时候就无需再进行一个额外的服务器来回运作，其缺省值为真。
//# Current Language（当前语言）：SQL Server语言记录的名称。
//# Data Source（数据源）／Server（服务器）／Address（地址）／Addr（地址）／Network Address（网络地址）：SQL Server实例的名称或网络地址。
//# Encrypt（加密）：当值为真时，如果服务器安装了授权证书，SQL Server就会对所有在客户和服务器之间传输的数据使用SSL加密。被接受的值有true（真）、false（伪）、yes（是）和no（否）。
//# Enlist（登记）：表示连接池程序是否会自动登记创建线程的当前事务语境中的连接，其缺省值为真。
//# Database（数据库）／Initial Catalog（初始编目）：数据库的名称。
//# Integrated Security（集成安全）／Trusted Connection（受信连接）：表示Windows认证是否被用来连接数据库。它可以被设置成真、伪或者是和真对等的sspi，其缺省值为伪。
//# Max Pool Size（连接池的最大容量）：连接池允许的连接数的最大值，其缺省值为100。
//# Min Pool Size（连接池的最小容量）：连接池允许的连接数的最小值，其缺省值为0。
//# Network Library（网络库）／Net（网络）：用来建立到一个SQL Server实例的连接的网络库。支持的值包括： dbnmpntw (Named Pipes)、dbmsrpcn (Multiprotocol／RPC)、dbmsvinn(Banyan Vines)、dbmsspxn (IPX／SPX)和dbmssocn (TCP／IP)。协议的动态链接库必须被安装到适当的连接，其缺省值为TCP／IP。
//# Packet Size（数据包大小）：用来和数据库通信的网络数据包的大小。其缺省值为8192。
//# Password（密码）／Pwd：与帐户名相对应的密码。
//# Persist Security Info（保持安全信息）：用来确定一旦连接建立了以后安全信息是否可用。如果值为真的话，说明像用户名和密码这样对安全性比较敏感的数据可用，而如果值为伪则不可用。重置连接字符串将重新配置包括密码在内的所有连接字符串的值。其缺省值为伪。
//# Pooling（池）：确定是否使用连接池。如果值为真的话，连接就要从适当的连接池中获得，或者，如果需要的话，连接将被创建，然后被加入合适的连接池中。其缺省值为真。
//# User ID（用户ID）：用来登陆数据库的帐户名。
//# Workstation ID（工作站ID）：连接到SQL Server的工作站的名称。其缺省值为本地计算机的名称。

// 使用OLE DB方式连接常用数据库的连接字符串的设置
//SQL Server 使用 OLE DB 所设置的连接字符串:
//标准连接方式
//Provider=sqloledb;Data Source=datasource;Initial Catalog=DbName;User Id=username;Password=pwd;
//信任连接方式:
//Provider=sqloledb;Data Source=datasource;Initial Catalog=DbName;Integrated Security=true;
//------------------------------------------------------------------------------------------
//Access使用 OLE DB 所设置的连接字符串:
//标准连接方式:
//Provider=Microsoft.Jet.OLEDB.4.0;Data Source=\Path\Db.mdb;User Id=username;Password=pwd;
//工作组方式:
//Provider=Microsoft.Jet.OLEDB.4.0;Data Source=\Path\Db.mdb;Jet OLEDB:System Database=system.mdw;
//包含密码方式:
//Provider=Microsoft.Jet.OLEDB.4.0;Data Source=\Path\Db.mdb;Jet OLEDB:Database Password=pwd;
//------------------------------------------------------------------------------------------
//Oracle 使用 OLE DB 所设置的连接字符串:
//微软提供的标准安全连接方式:
//Provider=msdaora;Data Source=datasource;User Id=username;Password=PWD;
//Oracle 提供的标准安全连接方式:
//Provider=OraOLEDB;Data Source=MyOracleDB;User Id=username;Password=PWD;
//信任连接方式:
//Provider=OraOLEDB.Oracle;Data Source=datasource;OSAuthent=1;
//------------------------------------------------------------------------------------------
//Excel 使用 OLE DB 所设置的连接字符串
//标准连接方式:
//Provider=Microsoft.Jet.OLEDB.4.0;Data Source=C:\MyEcxel.xls;Extended Properties=" ";
//------------------------------------------------------------------------------------------
//Informix 使用 OLE DB 所设置的连接字符串
//IBM Informix OLE DB Provider:
//Provider=Ifxoledbc.2;User ID=username;Password=PWD;Data Source=dbName@serverName;Persist Security Info=true; 

  