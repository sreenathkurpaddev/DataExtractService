using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataExtractService.Shared.Logging
{
    [ConfigurationElementType(typeof(CustomTraceListenerData))]
    public class CustomListener : CustomTraceListener
    {
        public CustomListener()
        {
        }

        public override void TraceData(TraceEventCache eventCache, string source, TraceEventType eventType, int id, object data)
        {
            if (data is LogEntry)
            {
                this.Write(data);
            }
            else
            {
                this.Write(data.ToString());
            }
        }

        /// <summary>
        /// Writes an event to a database.
        /// Note: Requires an AppSettings parameter PolicyDatabase to be configured.
        /// </summary>
        /// <param name="data">Log Entry Object</param>
        public override void Write(object data)
        {
            LogEntry le = data as LogEntry;

            try
            {
                string msg = le.Message.Substring(0, Math.Min(le.Message.Length, 1500));
                //Message format will be as below
                //LOG_KEY=10^20090101^42^4321|Index out of bound...

                // take only 1500 characters.

                string log_key = msg.Split('|')[0].Split('=')[1];

                using (SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings["PolicyDatabase"].ToString()))
                {
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        conn.Open();
                        cmd.CommandText = "INSERT INTO dbo.EVENT_LOG (EVENT_ID,PRIORITY,SEVERITY,TIMESTAMP_LOCAL,TIMESTAMP_GMT,MACHINE_NAME,MESSAGE,LOG_KEY) VALUES (@EventID,@Priority,@Severity,@TimestampLocal,@TimestampGMT,@MachineName,@Message,@Log_Key)";
                        cmd.Parameters.Add("@EventID", SqlDbType.Int).Value = le.EventId;
                        cmd.Parameters.Add("@Priority", SqlDbType.Int).Value = le.Priority;
                        cmd.Parameters.Add("@Severity", SqlDbType.Int).Value = Convert.ToInt32(le.Severity);
                        cmd.Parameters.Add("@TimestampLocal", SqlDbType.DateTime).Value = le.TimeStamp.ToLocalTime();
                        cmd.Parameters.Add("@TimestampGMT", SqlDbType.DateTime).Value = le.TimeStamp.ToUniversalTime();
                        cmd.Parameters.Add("@MachineName", SqlDbType.NVarChar).Value = le.MachineName;
                        cmd.Parameters.Add("@Message", SqlDbType.NVarChar).Value = msg;
                        cmd.Parameters.Add("@Log_Key", SqlDbType.VarChar).Value = log_key;

                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch
            {
                // swallow any exception as this method can not recover from a failure.
                // If the application (using potentially the Logging application block) is configured to write same or lower severity messages
                // to an other sink/listener, this is how the application will be notified.
            }
        }

        // Implement inherited abstract member
        public override void Write(string message)
        {
        }
        // Implement inherited abstract member
        public override void WriteLine(string message)
        {
        }
    }
}
