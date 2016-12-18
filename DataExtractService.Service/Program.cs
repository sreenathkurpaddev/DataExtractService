using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace DataExtractService.Service
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
      static void Main()
      {
        System.Diagnostics.EventLog.WriteEntry("Data Extract Service", "Yes. Started...");
        ServiceBase[] ServicesToRun;
        ServicesToRun = new ServiceBase[] 
            { 
                new DataExtractService() 
            };
        ServiceBase.Run(ServicesToRun);
      }
    }
}
