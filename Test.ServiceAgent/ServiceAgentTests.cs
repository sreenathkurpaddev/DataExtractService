using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DataExtractService.Objects;
using DataExtractService.ServiceAgent.Contracts;
using DataExtractService.ServiceAgent.Implementation;
using DataExtractService.Shared.Logging;
using Newtonsoft.Json;

namespace Test.ServiceAgent
{
    [TestClass]
    public class ServiceAgentTests
    {
        [TestMethod]
        public void KeyEventsSuccess()
        {
            IDataServiceAgent _agent = new DataServiceAgent();
            var response = _agent.CallServicePostAsync<KeyEventWrapper, KeyEventResponse>(GetRequestObject());
            Assert.IsNotNull(response);
            Assert.IsNotNull(response.Result);
            Assert.IsNotNull(response.Result.Item1);
            var serviceResponse = response.Result.Item1;
            Assert.IsTrue(serviceResponse.Response);
            Assert.IsTrue(!String.IsNullOrEmpty(serviceResponse.ResponseMessage));

            var jsonRequestString = response.Result.Item2;
            Assert.IsTrue(!String.IsNullOrEmpty( jsonRequestString));

            var DateTimeSent = response.Result.Item3;
            var ResponseTime = response.Result.Item4;
            var IsSuccessflg = response.Result.Item5;
            Assert.IsTrue(IsSuccessflg);
        }

        private KeyEventWrapper GetRequestObject()
        {
            KeyEventWrapper kw = new KeyEventWrapper();
            KeyEvent key1 = new KeyEvent();
            key1.KeyEventId = "9e9ba8e3-7628-4b9e-8eab-33556b5a9b4f";            key1.MachineId = "Showroom";            key1.StockNumber = "39742A";            key1.EmployeeId = "342";            key1.EventType = "out";            key1.TimeStamp = DateTime.UtcNow;            kw.KeyEvents.Add(key1);            KeyEvent key2 = new KeyEvent();
            key2.KeyEventId = "17feb98c-6ba8-415a-b9ae-f00b4066040d";
            key2.MachineId = "Showroom";
            key2.StockNumber = "56275";
            key2.EmployeeId = "342";
            key2.EventType = "in";
            key2.TimeStamp = DateTime.UtcNow;            kw.KeyEvents.Add(key2);            kw.DealerId = "ACC01";
            kw.Token = "Fe86g3jk2";
            return kw;
        }
    }
}
