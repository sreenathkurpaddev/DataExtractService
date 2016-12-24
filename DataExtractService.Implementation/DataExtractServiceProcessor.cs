using DataExtractService.DataAccess.Contracts;
using DataExtractService.DataAccess.Implementation;
using DataExtractService.Interface;
using DataExtractService.Objects;
using DataExtractService.ServiceAgent.Contracts;
using DataExtractService.ServiceAgent.Implementation;
using DataExtractService.Shared.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataExtractService.Implementation
{
    public class DataExtractServiceProcessor : IDataExtractProcessor
    {
        private IDataAccess _dal;
        private IDataServiceAgent _serviceProxy;

        public DataExtractServiceProcessor()
        {
            LogWrapper.Log("Enter DataExtractServiceProcess .ctor", $"Thread id : {System.Threading.Thread.CurrentThread.ManagedThreadId}", 1, System.Diagnostics.TraceEventType.Information);
            _dal = new DataAccessImpl();
            _serviceProxy = new DataServiceAgent();
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
                //fetch all key events and retain it for later processing
                var originalKeyEvents = await _dal.GetKeyEventsToProcessAsync();
                
                //group unique keyevents for the service call
                //if there are more than one instance have the same key, then only one instance is considered in this grou.
                var grpedDictionary = originalKeyEvents.GetKeyEventGrp();


                //prepare keyevent wrapper. this will send only unique key events.
                KeyEventWrapper groupdWrapper = new KeyEventWrapper();
                groupdWrapper.DealerId = "ACC01";
                groupdWrapper.Token = "Fe86g3jk2";

                var keyEventsToSend = new List<KeyEvent>();
                foreach (KeyValuePair<string, KeyEvent> kvp in grpedDictionary)
                {
                    keyEventsToSend.Add(kvp.Value);
                }
                groupdWrapper.KeyEvents = keyEventsToSend;

                //var inputObj = GetRequestObject();

                LogWrapper.Log($"Calling web service with input object : {originalKeyEvents}", $"Thread id : {System.Threading.Thread.CurrentThread.ManagedThreadId}", 1, System.Diagnostics.TraceEventType.Information);
                var response = await _serviceProxy.CallExternalServicePostAsync<KeyEventWrapper, KeyEventResponse>(groupdWrapper);
                LogWrapper.Log($"Response received from web service : {response?.ToString()}", $"Thread id : {System.Threading.Thread.CurrentThread.ManagedThreadId}", 1, System.Diagnostics.TraceEventType.Information);

                if (response != null && response.Item1 != null)
                {
                    response.Item1.RequestJsonString = response.Item2;
                    response.Item1.RequestSentDateTime = response.Item3;
                    response.Item1.ResponseTimeinSecs = response.Item4;
                    response.Item1.IsSuccessful = response.Item5;

                    //stuff response onto all the key events as per the requirement to log same response on the original list of key events.
                    IEnumerable<IntegrationLog> log = CreateIntegrationLog(originalKeyEvents, response.Item1);

                    //log the response details 
                    var _dalResponse = await _dal.LogKeyEventResponseAsync(log);

                    if (_dalResponse == 1)
                        LogWrapper.Log("Logged Service call details successfully to the database", $"Thread id : {System.Threading.Thread.CurrentThread.ManagedThreadId}", 1, System.Diagnostics.TraceEventType.Information);
                    else
                        LogWrapper.Log("Failed to Log Service call details to the database", $"Thread id : {System.Threading.Thread.CurrentThread.ManagedThreadId}", 1, System.Diagnostics.TraceEventType.Error);
                }
                LogWrapper.Log("Exit DataExtractService Run method", $"Thread id : {System.Threading.Thread.CurrentThread.ManagedThreadId}", 1, System.Diagnostics.TraceEventType.Information);
            }
            catch(Exception ex)
            {
                LogWrapper.Log($"Run method threw an exception and exited the process. Error message : {ex.Message} ", $"Thread id : {System.Threading.Thread.CurrentThread.ManagedThreadId}", 1, System.Diagnostics.TraceEventType.Error);
            }
        }

        private IEnumerable<IntegrationLog> CreateIntegrationLog(KeyEventWrapper originalKeyEvents, KeyEventResponse response)
        {
            try
            {
                IList<IntegrationLog> logs = new List<IntegrationLog>();
                foreach (var item in originalKeyEvents.KeyEvents)
                {
                    try
                    {
                        IntegrationLog _log = new IntegrationLog();
                        _log.DateTimeSent = response.RequestSentDateTime;
                        _log.IsSuccessFlg = response.IsSuccessful;
                        _log.KeyEventId = item.KeyEventId;
                        _log.RequestJsonDatainString = response.RequestJsonString;
                        _log.ResponseDescription = response.ResponseMessage;
                        _log.ResponseReceivedinSecs = response.ResponseTimeinSecs;
                        logs.Add(_log);
                    }
                    catch(Exception ex)
                    {
                        LogWrapper.Log($"Error in generating Integration log for KeyeventId {item.KeyEventId}. Error is {ex.Message}. Skipping to next one", $"Thread id : {System.Threading.Thread.CurrentThread.ManagedThreadId}", 1, System.Diagnostics.TraceEventType.Error);
                    }
                }
                return logs as IEnumerable<IntegrationLog>;
            }
            catch(Exception ex)
            {
                LogWrapper.Log($"Error in generating Integration log. Error is {ex.Message}", $"Thread id : {System.Threading.Thread.CurrentThread.ManagedThreadId}", 1, System.Diagnostics.TraceEventType.Error);
                return null;
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
