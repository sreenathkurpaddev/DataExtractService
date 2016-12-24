using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataExtractService.Repository.Contracts
{
    public interface IRepository
	{
        Task InsertAsync(string procedureName, Dictionary<string, object> parameters);

        Task UpdateAsync(string procedureName, Dictionary<string, object> parameters);

        Task DeleteAsync(string procedureName, Dictionary<string, object> parameters);

        Task<object> GetAsync(string procedureName, Dictionary<string, object> parameters);


        void Insert(string procedureName, Dictionary<string, object> parameters);

		void Update(string procedureName, Dictionary<string, object> parameters);

		void Delete(string procedureName, Dictionary<string, object> parameters);

		object Get(string procedureName, Dictionary<string, object> parameters);

	}
}
