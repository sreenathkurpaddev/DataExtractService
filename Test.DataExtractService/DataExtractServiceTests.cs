using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DataExtractService.Implementation;
using DataExtractService.NinjectKernel;
using System.Configuration;

namespace Test.DataExtractService
{
    [TestClass]
    public class DataExtractServiceTests
    {
        public DataExtractServiceTests()
        {
            ComponentKernel.LoadKernel(ConfigurationManager.AppSettings["NInjenctBindingsXmlPath"]);

        }
        [TestMethod]
        public void TestServiceConstructor()
        {
            DataExtractServiceProcessor processor = new DataExtractServiceProcessor();
            processor.Run();
        }
    }
}
