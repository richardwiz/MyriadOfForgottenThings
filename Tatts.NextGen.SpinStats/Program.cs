using log4net;
using System;
using System.IO;
using System.Reflection;
using System.ServiceProcess;

namespace Tatts.NextGen.SpinStats
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            var path = Assembly.GetExecutingAssembly().Location;
            var fileInfo = new FileInfo(path);
            var dir = fileInfo.DirectoryName;
            log4net.Config.XmlConfigurator.ConfigureAndWatch(new FileInfo(string.Format("{0}\\log4net.config", dir)));

            var logger = LogManager.GetLogger(typeof(Program).ToString());

            logger.Info("Initialising Tatts.NextGen.Stats WindowsService");

            SpinStatsService svc = new SpinStatsService();
            svc.Logger = logger;

            ServiceBase[] servicesToRun = new ServiceBase[]
            {
                svc
            };

            if (Environment.UserInteractive)
            {
                var type = typeof(ServiceBase);
                const BindingFlags flags = BindingFlags.Instance | BindingFlags.NonPublic;
                var method = type.GetMethod("OnStart", flags);
                var onstop = type.GetMethod("OnStop", flags);

                foreach (var service in servicesToRun)
                {
                    method.Invoke(service, new object[] { null });
                }

                logger.Info(@"Service Started! - Press any key to stop");
                Console.ReadLine();

                if (onstop != null)
                {
                    foreach (var service in servicesToRun)
                    {
                        onstop.Invoke(service, null);
                    }
                }
            }
            else
            {
                logger.Info("Attempting to run NextGen Stats Service");
                ServiceBase.Run(servicesToRun);
            }
        }
    }
}
