using System;
using System.Threading.Tasks;


namespace DataExtractService.ServiceAgent.Contracts
{
    public interface IDataServiceAgent
    {
 
        Task<Tuple<TOutput, string, DateTime, int, bool>> CallExternalServicePostAsync<T, TOutput>(T input) where T: class;
    }
}
