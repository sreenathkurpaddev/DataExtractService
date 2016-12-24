using DataExtractService.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataExtractService.DataAccess.Contracts
{
    public interface IDataAccess
    {
        Task<KeyEventWrapper> GetKeyEventsToProcessAsync();

        Task<int> LogKeyEventResponseAsync(IEnumerable<IntegrationLog> _log);
    }
}
