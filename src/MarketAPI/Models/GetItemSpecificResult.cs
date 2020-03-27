using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace MarketAPI.Models
{
    public class GetItemSpecificResult : BaseResponse
    {
        [JsonProperty("currency")]
        public string Currency { get; set; }

        [JsonProperty("data")]
        public List<ItemData> Data { get; set; }
    }
}
