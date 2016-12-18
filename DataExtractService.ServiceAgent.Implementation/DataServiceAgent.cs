using DataExtractService.Objects;
using DataExtractService.ServiceAgent.Contracts;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using DataExtractService.Shared.Logging;
using System.Diagnostics;

namespace DataExtractService.ServiceAgent.Implementation
{
    public class DataServiceAgent : IDataServiceAgent
    {
        public async Task<TOutput> CallExternalServicePostAsync<T, TOutput>(T input) where T : class
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    String MdlUrl = ConfigurationManager.AppSettings["ServiceUri"];
                    LogWrapper.Log($"CallExternalServicePostAsync call started. Service Url: {MdlUrl}", $"Thread id : {System.Threading.Thread.CurrentThread.ManagedThreadId}", 1, TraceEventType.Information);

                    String json = JsonConvert.SerializeObject(input);
                    LogWrapper.Log($"Calling Service {MdlUrl} with Json data: {json}", $"Thread id : {System.Threading.Thread.CurrentThread.ManagedThreadId}", 1, TraceEventType.Information);

                    var response = await client.PostAsync(MdlUrl, new StringContent(json, Encoding.UTF8, "application/json"));
                    LogWrapper.Log($"Received {response.StatusCode} status code from Service {MdlUrl}", $"Thread id : {System.Threading.Thread.CurrentThread.ManagedThreadId}", 1, TraceEventType.Information);

                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    LogWrapper.Log($"Received {jsonResponse} from Service {MdlUrl}", $"Thread id : {System.Threading.Thread.CurrentThread.ManagedThreadId}", 1, TraceEventType.Information);

                    var resp = JsonConvert.DeserializeObject<TOutput>(jsonResponse);
                    LogWrapper.Log($"{(resp != null ? "Successfully" : "Failed to")} serialize Json to {resp.GetType()} from Service {MdlUrl}", $"Thread id : {System.Threading.Thread.CurrentThread.ManagedThreadId}", 1, TraceEventType.Information);

                    return resp;
                }
                catch (Exception ex)
                {                    LogWrapper.Log($"Error occurred in calling service. Error message is {ex.Message}", $"Thread id : {System.Threading.Thread.CurrentThread.ManagedThreadId}", 1, TraceEventType.Error);                }

                LogWrapper.Log($"Returning default response", $"Thread id : {System.Threading.Thread.CurrentThread.ManagedThreadId}", 1, TraceEventType.Warning);
                return default(TOutput);
            }
        }
    }
}
