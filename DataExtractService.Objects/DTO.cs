using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataExtractService.Objects
{
    public class DTO
    {
        [JsonProperty("prop1")]
        public string Property1 { get; set; }

        [JsonProperty("prop2")]
        public string Property2 { get; set; }

        [JsonProperty("prop3")]
        public string Property3 { get; set; }
    }
}

