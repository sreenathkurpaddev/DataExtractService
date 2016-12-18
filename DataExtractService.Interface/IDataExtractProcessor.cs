using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataExtractService.Interface
{
	public interface IDataExtractProcessor
	{
        Task Run();
	}
}
