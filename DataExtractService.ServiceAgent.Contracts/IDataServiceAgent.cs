using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataExtractService.ServiceAgent.Contracts
{
    public interface IDataServiceAgent
    {
        Task<string> CallExternalServiceAsync(string serviceURI, params object[] requestParameters);
    }
}
