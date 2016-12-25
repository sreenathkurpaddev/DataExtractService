using DataExtractService.ServiceAgent.Contracts;
using DataExtractService.Shared.Logging;
using Newtonsoft.Json;
using System;
using System.Configuration;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DataExtractService.ServiceAgent.Implementation
{
    public class DataServiceAgent : IDataServiceAgent
    {
        public DataServiceAgent()
        {

        }
        public async Task<Tuple<TOutput, string, DateTime, int, bool>> CallServicePostAsync<T, TOutput>(T input) where T : class
        {
            using (HttpClient client = new HttpClient())
            {
                Tuple<TOutput, string, DateTime, int, bool> tuple = null;
                String json = string.Empty;
                try
                {
                    Stopwatch sw = new Stopwatch();
                    
                    String MdlUrl = ConfigurationManager.AppSettings["ServiceUri"];
                    LogWrapper.Log($"CallExternalServicePostAsync call started. Service Url: {MdlUrl}", $"Thread id : {System.Threading.Thread.CurrentThread.ManagedThreadId}", 1, TraceEventType.Information);

                    json = JsonConvert.SerializeObject(input);
                    LogWrapper.Log($"Calling Service {MdlUrl} with Json data: {json}", $"Thread id : {System.Threading.Thread.CurrentThread.ManagedThreadId}", 1, TraceEventType.Information);

                    sw.Start();
                    var response = await client.PostAsync(MdlUrl, new StringContent(json, Encoding.UTF8, "application/json"));
                    LogWrapper.Log($"Received {response.StatusCode} status code  in {sw.ElapsedMilliseconds} ms from Service {MdlUrl}", $"Thread id : {System.Threading.Thread.CurrentThread.ManagedThreadId}", 1, TraceEventType.Information);
                    sw.Stop();

                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    LogWrapper.Log($"Received {jsonResponse} from Service url  {MdlUrl}", $"Thread id : {System.Threading.Thread.CurrentThread.ManagedThreadId}", 1, TraceEventType.Information);

                    var resp = JsonConvert.DeserializeObject<TOutput>(jsonResponse);
                    LogWrapper.Log($"{(resp != null ? "Successfully" : "Failed to")} serialize Json to {resp.GetType()} from Service {MdlUrl}", $"Thread id : {System.Threading.Thread.CurrentThread.ManagedThreadId}", 1, TraceEventType.Information);

                    tuple = new Tuple<TOutput, string, DateTime, int, bool>(resp, json, DateTime.UtcNow, Convert.ToInt32(sw.ElapsedMilliseconds / 1000), true);

                    return tuple;
                }
                catch (Exception ex)
                {                    LogWrapper.Log($"Error occurred in calling service. Error message is {ex.Message}", $"Thread id : {System.Threading.Thread.CurrentThread.ManagedThreadId}", 1, TraceEventType.Error);                    tuple = new Tuple<TOutput, string,DateTime, int, bool>(default(TOutput),json, DateTime.UtcNow, 0, false);                }
                return tuple;
            }
        }
    }
}
