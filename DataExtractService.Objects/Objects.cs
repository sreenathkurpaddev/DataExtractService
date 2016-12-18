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
        public String KeyEventId;
        public String MachineId;
        public String StockNumber;
        public String EmployeeId;
        public String EventType;
        public DateTime TimeStamp;
        public override string ToString()
        {
            return $"KeyEventId : {this.KeyEventId}; MachineId : {this.MachineId}; StockNumber : {this.StockNumber}; EventType : {this.EventType}; TimeStamp : {this.TimeStamp}";
        }
    }

    public class KeyEventWrapper
    {
        public IList<KeyEvent> KeyEvents { get; set; }
        public String DealerId;
        public String Token;
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

