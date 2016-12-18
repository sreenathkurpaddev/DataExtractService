using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataExtractService.Interface;
using System.Timers;
using System.Configuration;
using DataExtractService.DataAccess.Contracts;
using DataExtractService.ServiceAgent.Contracts;
using DataExtractService.NinjectKernel;
using DataExtractService.Shared.Logging;
using DataExtractService.Objects;

namespace DataExtractService.Implementation
{
    public class DataExtractServiceProcessor : IDataExtractProcessor
    {
        private IDataAccess _dal;
        private IDataServiceAgent _serviceProxy;

        public DataExtractServiceProcessor()
        {
            LogWrapper.Log("Enter DataExtractServiceProcess .ctor", $"Thread id : {System.Threading.Thread.CurrentThread.ManagedThreadId}", 1, System.Diagnostics.TraceEventType.Information);
            _dal = ComponentKernel.GetInstance<IDataAccess>();
            _serviceProxy = ComponentKernel.GetInstance<IDataServiceAgent>();
            LogWrapper.Log("Exit DataExtractServiceProcess .ctor", $"Thread id : {System.Threading.Thread.CurrentThread.ManagedThreadId}", 1, System.Diagnostics.TraceEventType.Information);
        }
        
        /// <summary>
        /// Main process 
        /// </summary>
        /// <returns></returns>
        public async Task Run()
        {
            try
            {
                LogWrapper.Log("Enter DataExtractService Run method", $"Thread id : {System.Threading.Thread.CurrentThread.ManagedThreadId}", 1, System.Diagnostics.TraceEventType.Information);
                var inputObj = GetRequestObject();

                LogWrapper.Log($"Calling web service with input object : {inputObj}", $"Thread id : {System.Threading.Thread.CurrentThread.ManagedThreadId}", 1, System.Diagnostics.TraceEventType.Information);
                var response = await _serviceProxy.CallExternalServicePostAsync<KeyEventWrapper, KeyEventResponse>(inputObj);
                LogWrapper.Log($"Response received from web service : {response?.ToString()}", $"Thread id : {System.Threading.Thread.CurrentThread.ManagedThreadId}", 1, System.Diagnostics.TraceEventType.Information);

                LogWrapper.Log("Exit DataExtractService Run method", $"Thread id : {System.Threading.Thread.CurrentThread.ManagedThreadId}", 1, System.Diagnostics.TraceEventType.Information);
            }
            catch(Exception ex)
            {
                LogWrapper.Log($"Run method threw an exception and exited the process. Error message : {ex.Message} ", $"Thread id : {System.Threading.Thread.CurrentThread.ManagedThreadId}", 1, System.Diagnostics.TraceEventType.Error);
            }
        }

        private KeyEventWrapper GetRequestObject()
        {
            KeyEventWrapper kw = new KeyEventWrapper();
            KeyEvent key1 = new KeyEvent();
            key1.KeyEventId = "9e9ba8e3-7628-4b9e-8eab-33556b5a9b4f";            key1.MachineId = "Showroom";            key1.StockNumber = "39742A";            key1.EmployeeId = "342";            key1.EventType = "out";            key1.TimeStamp = DateTime.UtcNow;            kw.KeyEvents.Add(key1);            KeyEvent key2 = new KeyEvent();
            key2.KeyEventId = "17feb98c-6ba8-415a-b9ae-f00b4066040d";
            key2.MachineId = "Showroom";
            key2.StockNumber = "56275";
            key2.EmployeeId = "342";
            key2.EventType = "in";
            key2.TimeStamp = DateTime.UtcNow;            kw.KeyEvents.Add(key2);            kw.DealerId = "ACC01";
            kw.Token = "Fe86g3jk2";
            return kw;
        }
    }
}
