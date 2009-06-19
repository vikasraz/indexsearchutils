using System.Collections.Generic;
using System.ServiceProcess;
using System.Text;

namespace Indexer
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        //[STAThread]   //<!--this   line   here   
        static void Main()
        {
            ServiceBase[] ServicesToRun;

            // 同一进程中可以运行多个用户服务。若要将
            // 另一个服务添加到此进程中，请更改下行以
            // 创建另一个服务对象。例如，
            //
            //   ServicesToRun = new ServiceBase[] {new Service1(), new MySecondUserService()};
            //
            ServicesToRun = new ServiceBase[] { new IndexService() };

            ServiceBase.Run(ServicesToRun);
        }
    }
}