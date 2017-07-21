using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

// Logf4Net setup.
[assembly: log4net.Config.XmlConfigurator(Watch = true)]

namespace Cerberus
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {

            Cerberus service = new Cerberus();
            if (Environment.UserInteractive)
            {
                service.RunAsConsole(args);
            }
            else
            {
                ServiceBase[] ServicesToRun;
                ServicesToRun = new ServiceBase[] { service };
                ServiceBase.Run(ServicesToRun);
            }
        }
    }
}
