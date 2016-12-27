using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DataExtractService.DataAccess.Contracts;
using DataExtractService.DataAccess.Implementation;
using DataExtractService.Objects;
namespace Test.DataAccess
{
    [TestClass]
    public class DataAccessTests
    {
        [TestMethod]
        public void GetKeyeventsforProcess()
        {
            IDataAccess _dal = new DataAccessImpl();
            var records =_dal.GetKeyEventsToProcessAsync();
            while(!records.IsCompleted)
            {
                // System.Threading.Thread.Sleep()
                Console.WriteLine("Awaiting...");
            }
            Assert.IsTrue(records.Result!= null);
        }
    }
}
