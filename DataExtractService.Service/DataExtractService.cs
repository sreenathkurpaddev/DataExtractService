using DataExtractService.NinjectKernel;
using System;
using System.Configuration;
using System.ServiceProcess;
using System.Threading;

namespace DataExtractService.Service
{
    public partial class DataExtractService : ServiceBase
  {
    static private Thread serviceThread = new Thread(new ThreadStart(StartDataExtractProcess));
        public DataExtractService()
        {
            InitializeComponent();
            ComponentKernel.LoadKernel(ConfigurationManager.AppSettings["NInjenctBindingsXmlPath"]);
        }

    private static void StartDataExtractProcess()
    {
      try
      {
      }
            catch(Exception ex)
            {

            }
    }

    protected override void OnStart(string[] args)
    {
            try
            {
                serviceThread.Start();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
    }

    protected override void OnStop()
    {
            try
            {
                serviceThread.Abort();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
    }
  }
}
