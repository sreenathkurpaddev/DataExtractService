using DataExtractService.Objects;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataExtractService.DataAccess.Contracts
{
    public interface IDataAccess
    {
        Task<KeyEventWrapper> GetKeyEventsToProcessAsync();

        Task<int> LogKeyEventResponseAsync(IEnumerable<IntegrationLog> _log);
    }
}
