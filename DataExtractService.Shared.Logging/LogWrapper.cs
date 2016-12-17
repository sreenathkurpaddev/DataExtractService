using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using System.Diagnostics;

namespace DataExtractService.Shared.Logging
{
    public static class BusinessLogger
    {
        public const string AllEventsCategory = "AllEvents";

        public static void Log(string message, string key, int priority, TraceEventType severity)
        {
            Log(message, key, priority, severity, 0);
        }

        public static void Log(string message, string key, int priority, TraceEventType severity, int eventId)
        {
            Log(AllEventsCategory, message, key, priority, severity, eventId);
        }

        /// <summary>
        /// Writes an event in one or multiple storage systems (db/file/windows event viewer log).
        /// </summary>
        public static void Log(string category, string message, string key, int priority, TraceEventType severity, int eventId)
        {
            try
            {
                LogEntry le = CreateLogEntry(category, message, key, priority, severity, eventId);
                if (Logger.ShouldLog(le)) // verify that the event can be logged.
                    Logger.Write(le);
            }
            catch
            {
                // If logging itself fails we will swallow the exception,not logging any exception
            }
        }

        private static LogEntry CreateLogEntry(string category, string message, string key, int priority, TraceEventType severity, int eventId)
        {
            LogEntry le = new LogEntry();

            le.Categories.Add(category); // simplification: only one category.
            le.EventId = eventId;
            le.Priority = priority;
            le.Severity = severity;
            le.TimeStamp = DateTime.Now;
            le.MachineName = Environment.MachineName;

            // used mainly for the custom db trace listener to be able to write to the LOG_KEY column table.
            le.Message = String.Format("LOG_KEY={0}|{1}", key, message);
            return le;
        }
    }
}
