using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DataExtractService.Implementation;
namespace Test.Service
{
    [TestClass]
    public class Test
    {
        [TestMethod]
        public void Test_Success()
        {
            DataExtractServiceProcessor _processor = new DataExtractServiceProcessor();
            var response = _processor.Run();
            while(!response.IsCompleted)
            {
                ;
            }
            
        }
    }
}
