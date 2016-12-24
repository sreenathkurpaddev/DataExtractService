using DataExtractService.Shared.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataExtractService.Objects
{
    public class KeyEvent
    {
        private char keySeparator = '|';
        public String KeyEventId;
        public String MachineId;
        public String StockNumber;
        public String EmployeeId;
        public String EventType;
        public DateTime TimeStamp;
        internal string Key { get { return String.Concat(this.EmployeeId, keySeparator, this.EventType, keySeparator, this.MachineId); } }
        public override string ToString()
        {
            return $"KeyEventId : {this.KeyEventId}; MachineId : {this.MachineId}; StockNumber : {this.StockNumber}; EventType : {this.EventType}; TimeStamp : {this.TimeStamp}";
        }

        public KeyEvent()
        {

        }

        public KeyEvent(string machineId, string stockNumber, string employeeId, string eventTyp, DateTime timeStamp)
        {
            this.MachineId = machineId;
            this.StockNumber = stockNumber;
            this.EmployeeId = employeeId;
            this.EventType = eventTyp;
            this.TimeStamp = timeStamp;
        }
        public KeyEvent(string keyeventId, string machineId, string stockNumber, string employeeId, string eventTyp, DateTime timeStamp)
        {
            this.KeyEventId = keyeventId;
            this.MachineId = machineId;
            this.StockNumber = stockNumber;
            this.EmployeeId = employeeId;
            this.EventType = eventTyp;
            this.TimeStamp = timeStamp;
        }
    }

    public class IntegrationLog
    {
        public string KeyEventId;
        public bool IsSuccessFlg;
        public DateTime DateTimeSent;
        public string RequestJsonDatainString;
        public string ResponseCode;
        public string ResponseDescription;
        public int ResponseReceivedinSecs;

        public override string ToString()
        {
            return $"KeyEventId : {this.KeyEventId}; IsSuccessFlg : {this.IsSuccessFlg}; DateTimeSent : {DateTimeSent}; RequestJsonDatainString : {RequestJsonDatainString}; ResponseCode : {ResponseCode}; ResponseDescription :{ResponseDescription}; ResponseReceivedinSecs : {ResponseReceivedinSecs}";
        }
    }

    public class KeyEventWrapper
    {
        public IList<KeyEvent> KeyEvents { get; set; }
        public String DealerId;
        public String Token;

        public IDictionary<string, IEnumerable<KeyEvent>> GetKeyEventGrp ()
        {
            try
            {
                if (this.KeyEvents == null || this.KeyEvents.Count() == 0) return null;
                Dictionary<string, IList<KeyEvent>> keyevntsDict = new Dictionary<string, IList<KeyEvent>>();
                Dictionary<string, IEnumerable<KeyEvent>> keyevntsGrpDict = new Dictionary<string, IEnumerable<KeyEvent>>();
                foreach (var item in this.KeyEvents)
                {
                    IList<KeyEvent> _grpedEvents = null;
                    if (!keyevntsDict.ContainsKey(item.Key))
                    {
                        _grpedEvents = new List<KeyEvent>();
                        keyevntsDict.Add(item.Key, _grpedEvents);
                        _grpedEvents.Add(new KeyEvent(item.KeyEventId, item.MachineId, item.StockNumber, item.EmployeeId, item.EventType, item.TimeStamp));
                    }
                    else
                    {
                        _grpedEvents = keyevntsDict[item.Key];
                        _grpedEvents.Add(new KeyEvent(item.MachineId, item.StockNumber, item.EmployeeId, item.EventType, item.TimeStamp));
                    }
                }
                foreach (KeyValuePair<string, IList<KeyEvent>> kvp in keyevntsDict)
                {
                    keyevntsGrpDict.Add(kvp.Key, kvp.Value as IEnumerable<KeyEvent>);
                }

                return keyevntsGrpDict;
            }
            catch(Exception ex)
            {
                LogWrapper.Log($"Error in creating key event group. Error message is {ex.Message}", $"Thread id is : {System.Threading.Thread.CurrentThread.ManagedThreadId}", 1, System.Diagnostics.TraceEventType.Error);
                return null;
            }
        }

        public KeyEventWrapper()
        {
            KeyEvents = new List<KeyEvent>();
        }

        string GetEventsData()
        {
            string _kvstring = string.Empty;
            foreach (KeyEvent kv in this.KeyEvents)
                _kvstring += (kv?.ToString());
            return _kvstring;
        }
        public override string ToString()
        {
            return $"DealerId : {this.DealerId}; Token : {this.Token}; KeyEvents : {(this.KeyEvents != null && this.KeyEvents.Count > 0? GetEventsData() : "Null") }";
        }
    }
    public class KeyEventResponse
    {
        public Boolean Response;
        public String ResponseMessage;

        public override string ToString()
        {
            return $"Response : {this.Response}; ResponseMessage : {ResponseMessage}";
        }
    }

}

