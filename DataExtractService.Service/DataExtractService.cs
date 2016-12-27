using App.Core.Logging;
using DataExtractService.Implementation;
using DataExtractService.Interface;
using System;
using System.Configuration;
using System.ServiceProcess;
using System.Timers;

namespace DataExtractService.Service
{
    public partial class DataExtractService : ServiceBase
  {
        private System.Timers.Timer _timer = null;

        public DataExtractService()
        {
            
            LogWrapper.Log("Data Extract Service constructor started", $"Thread id: {System.Threading.Thread.CurrentThread.ManagedThreadId}", 1, System.Diagnostics.TraceEventType.Information);
            InitializeComponent();
            StartTimer();
            LogWrapper.Log("Data Extract Service constructor completed", $"Thread id: {System.Threading.Thread.CurrentThread.ManagedThreadId}", 1, System.Diagnostics.TraceEventType.Information);

        }

        private void StartTimer()
        {
            try
            {
                int serviceInterval = 5000;
                int.TryParse(ConfigurationManager.AppSettings["ServiceRunInterval"], out serviceInterval);
                LogWrapper.Log($"Setting the timer interval at : {serviceInterval/1000/60} minutes.", $"Thread id: {System.Threading.Thread.CurrentThread.ManagedThreadId}", 1, System.Diagnostics.TraceEventType.Information);
                _timer = new System.Timers.Timer(serviceInterval);
                _timer.Elapsed += new ElapsedEventHandler(TimerElapsedHandler);
            }
            catch (Exception ex)
            {
                LogWrapper.Log($"Error occurred in Start Timer method. Error message : {ex.Message}", $"Thread id: {System.Threading.Thread.CurrentThread.ManagedThreadId}", 1, System.Diagnostics.TraceEventType.Information);
            }
        }

        private void TimerElapsedHandler(object sender, ElapsedEventArgs eventArguments)
        {
            _timer.Enabled = false;
            try
            {
                LogWrapper.Log($"Timer Elapsed event triggered. Running data extract processor at : {eventArguments.SignalTime} ", $"Thread id: {System.Threading.Thread.CurrentThread.ManagedThreadId}", 1, System.Diagnostics.TraceEventType.Information);
                IDataExtractProcessor backGroundProcessor = new DataExtractServiceProcessor();
                backGroundProcessor.Run();
                LogWrapper.Log($"Data Extract processor completed", $"Thread id: {System.Threading.Thread.CurrentThread.ManagedThreadId}", 1, System.Diagnostics.TraceEventType.Information);
            }
            catch (Exception ex)
            {
                LogWrapper.Log($"Error occurred in calling data extract method. Error message : {ex.Message}", $"Thread id: {System.Threading.Thread.CurrentThread.ManagedThreadId}", 1, System.Diagnostics.TraceEventType.Information);
            }
            finally
            {
                _timer.Enabled = true;
            }
        }

        protected override void OnStart(string[] args)
        {
            LogWrapper.Log($"Service Started...", $"Thread id: {System.Threading.Thread.CurrentThread.ManagedThreadId}", 1, System.Diagnostics.TraceEventType.Information);
        }

        protected override void OnStop()
        {
            _timer.Enabled = false;
            LogWrapper.Log($"Service Stopped...", $"Thread id: {System.Threading.Thread.CurrentThread.ManagedThreadId}", 1, System.Diagnostics.TraceEventType.Information);
        }
    }
}
