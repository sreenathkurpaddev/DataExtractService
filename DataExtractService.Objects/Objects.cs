using DataExtractService.Shared.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

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

        /// <summary>
        /// Creates a unique group of key events identified by a key
        /// Key consists of : EmployeeId, EventType, MachineId
        /// </summary>
        /// <returns>Dictionary of unique key events</returns>
        public IDictionary<string, KeyEvent> GetKeyEventGrp ()
        {
            try
            {
                if (this.KeyEvents == null || this.KeyEvents.Count() == 0) return null;
                Dictionary<string, KeyEvent> keyevntsGrpDict = new Dictionary<string, KeyEvent>();
                foreach (var item in this.KeyEvents)
                {
                    if (!keyevntsGrpDict.ContainsKey(item.Key))
                    {
                        LogWrapper.Log($"Found new key to add to Group dictionary. Key is {item.Key}. Key event is {item.ToString()}", $"Thread id : {System.Threading.Thread.CurrentThread.ManagedThreadId}", 1, System.Diagnostics.TraceEventType.Verbose);
                        keyevntsGrpDict.Add(item.Key, new KeyEvent(item.KeyEventId, item.MachineId, item.StockNumber, item.EmployeeId, item.EventType, item.TimeStamp));
                    }
                    else
                    {
                        LogWrapper.Log($"Key {item.Key} already exists in Group dictionary. Key event is {item.ToString()} not getting added", $"Thread id : {System.Threading.Thread.CurrentThread.ManagedThreadId}", 1, System.Diagnostics.TraceEventType.Verbose);
                    }
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
        public int ResponseTimeinSecs;
        public string RequestJsonString;
        public DateTime RequestSentDateTime;
        public bool IsSuccessful;

        public override string ToString()
        {
            return $"Response : {this.Response}; ResponseMessage : {ResponseMessage}";
        }
    }

}

