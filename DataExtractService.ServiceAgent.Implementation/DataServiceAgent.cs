using DataExtractService.ServiceAgent.Contracts;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DataExtractService.ServiceAgent.Implementation
{
    public class DataServiceAgent : IDataServiceAgent
    {
        public async Task<string> CallExternalServiceAsync(string serviceURI, params object[] requestParameters)
        {
            string JSONResponse = string.Empty;
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(serviceURI);
                request.ContentType = "application/json";
                request.MediaType = "GET";
                string userName = ConfigurationManager.AppSettings["UserName"];
                string password = ConfigurationManager.AppSettings["Password"];
                string encodedAuthentication = Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes(String.Format($"{userName}:{password}")));
                request.Headers.Add("Authorization", "Basic " + encodedAuthentication);
                request.Headers.Add("Authorization:" + ConfigurationManager.AppSettings["ApiKey"]);
                var httpResponse = await request.GetResponseAsync();
                Stream JSONStream = httpResponse.GetResponseStream();
                StreamReader reader = new StreamReader(JSONStream);
                JSONResponse = reader.ReadToEnd();
            }
            catch (Exception ex)
            {
                throw;
            }
            return JSONResponse;
        }
    }
}
