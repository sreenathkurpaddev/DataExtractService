using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataExtractService.Interface;
using System.Timers;
using System.Configuration;

namespace DataExtractService.Implementation
{
	public class DataExtractServiceProcessor : IDataExtractProcessor
	{
		Timer oTimer = null;
		public void StartTimer()
		{
			try
			{
				oTimer = new Timer(Convert.ToInt64(ConfigurationManager.AppSettings["PollingInterval"]));
				oTimer.Elapsed += new ElapsedEventHandler(StartDataExtractProcess);
				oTimer.Enabled = true;
			}
			catch (Exception ex)
			{

			}
		}
		private void StartDataExtractProcess(object source, ElapsedEventArgs e)
		{
			try
			{
				//add app logic here
			}
			catch (Exception ex)
			{

			}
		}
	}
}
