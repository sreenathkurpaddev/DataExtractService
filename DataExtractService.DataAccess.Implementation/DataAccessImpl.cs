using DataExtractService.DataAccess.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataExtractService.Objects;
using DataExtractService.Repository.Contracts;
using DataExtractService.Repository.Implementation;
using DataExtractService.Shared.Logging;
using System.Data;

namespace DataExtractService.DataAccess.Implementation
{
    public class DataAccessImpl : IDataAccess
    {
        private readonly IRepository _repo = null;
        //LogWrapper.Log(string.Format("", );
        public DataAccessImpl()
        {
            _repo = new Repository.Implementation.Repository();
        }

        public async Task<KeyEventWrapper> GetKeyEventsToProcessAsync()
        {
            try
            {
               LogWrapper.Log($"Calling Repository to fetch key events to call the service",$" Thread id : {System.Threading.Thread.CurrentThread.ManagedThreadId}", 1, System.Diagnostics.TraceEventType.Information);
                var recordsDbSet = await _repo.GetAsync("GetKeyEvents", null) as DataSet;
                var keyevents = MapDataSetToKeyEventWrapper(recordsDbSet) as KeyEventWrapper;
                return keyevents;
            }
            catch(Exception ex)
            {
                LogWrapper.Log($"Error in retrieving key events from repository. Error message is {ex.Message}", $" Thread id : {System.Threading.Thread.CurrentThread.ManagedThreadId}", 1, System.Diagnostics.TraceEventType.Error);
                return null;
            }
        }

        private KeyEventWrapper MapDataSetToKeyEventWrapper(DataSet recordsDbSet)
        {
            try
            {
                KeyEventWrapper wrapperObj = null;
                for (int i = 0; i < recordsDbSet?.Tables?[0]?.Rows?.Count; i++)
                {
                    try
                    {
                        KeyEvent key = new KeyEvent();
                        key.KeyEventId = recordsDbSet?.Tables[0]?.Rows?[i].ItemArray[0].ToString();
                        key.MachineId = recordsDbSet?.Tables[0]?.Rows?[i].ItemArray[1].ToString();
                        key.StockNumber = recordsDbSet?.Tables[0]?.Rows?[i].ItemArray[2].ToString();
                        key.EmployeeId = recordsDbSet?.Tables[0]?.Rows?[i].ItemArray[3].ToString();
                        key.EventType = recordsDbSet?.Tables[0]?.Rows?[i].ItemArray[4].ToString();
                        key.TimeStamp = Convert.ToDateTime(recordsDbSet?.Tables[0]?.Rows?[i].ItemArray[5].ToString());
                        if (wrapperObj == null)
                            wrapperObj = new KeyEventWrapper();
                        if (wrapperObj.KeyEvents == null)
                            wrapperObj.KeyEvents = new List<KeyEvent>();
                        wrapperObj.KeyEvents.Add(key);
                    }
                    catch(Exception ex)
                    {
                      LogWrapper.Log($"Error in constructing keyevent from dataset at row count {i}. Skipping to next row. Error message is {ex.Message}", $" Thread id : {System.Threading.Thread.CurrentThread.ManagedThreadId}", 1, System.Diagnostics.TraceEventType.Error);
                    }
                }
                return wrapperObj;
            }
            catch(Exception ex)
            {
                LogWrapper.Log($"Error in constructing key events from recordset. Error message is {ex.Message}", $" Thread id : {System.Threading.Thread.CurrentThread.ManagedThreadId}", 1, System.Diagnostics.TraceEventType.Error);
                return null;
            }
        }


        public async Task<int> LogKeyEventResponseAsync(IEnumerable<IntegrationLog> _logs)
        {
            try
            {
                
                DataTable _integrationLogTable = new DataTable();
                _integrationLogTable.Columns.Add("KeyEventId");
                _integrationLogTable.Columns.Add("IsSuccessFlg");
                _integrationLogTable.Columns.Add("DateTimeSent");
                _integrationLogTable.Columns.Add("RequestJsonDatainString");
                _integrationLogTable.Columns.Add("ResponseCode");
                _integrationLogTable.Columns.Add("ResponseDescription");
                _integrationLogTable.Columns.Add("ResponseReceivedinSecs");
                LogWrapper.Log($"Total count {_logs?.Count()} to log in Integration log table", $"Thread id : {System.Threading.Thread.CurrentThread.ManagedThreadId}", 1, System.Diagnostics.TraceEventType.Information);

                foreach (IntegrationLog log in _logs)
                {
                    LogWrapper.Log($"Adding integration log {log} to data table", $"Thread id : {System.Threading.Thread.CurrentThread.ManagedThreadId}", 1, System.Diagnostics.TraceEventType.Verbose);
                    _integrationLogTable.Rows.Add(log.KeyEventId, log.IsSuccessFlg, log.DateTimeSent, log.RequestJsonDatainString, log.ResponseCode, log.ResponseDescription, log.ResponseReceivedinSecs);
                }

                Dictionary<string, object> parameters = new Dictionary<string, object>();
                parameters.Add("@IntegrationLogTable", _integrationLogTable);

                LogWrapper.Log($"Calling repository to update integration log table", $"Thread id : {System.Threading.Thread.CurrentThread.ManagedThreadId}", 1, System.Diagnostics.TraceEventType.Information);
                await _repo.InsertAsync("UpdateIntegrationLog", parameters);

                LogWrapper.Log($"Call to update integration log table successful. Returning 1 as result", $"Thread id : {System.Threading.Thread.CurrentThread.ManagedThreadId}", 1, System.Diagnostics.TraceEventType.Information);

                return 1;
                    
            }
            catch (Exception ex)
            {
                LogWrapper.Log($"Error in retrieving key events from repository. Returning 0 as result. Error message is {ex.Message}", $" Thread id : {System.Threading.Thread.CurrentThread.ManagedThreadId}", 1, System.Diagnostics.TraceEventType.Error);
                return 0;
            }
        }
    }
}
