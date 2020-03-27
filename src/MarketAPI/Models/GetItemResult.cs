using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace MarketAPI.Models
{
    public class GetItemResult : BaseResponse
    {
        [JsonProperty("currency")]
        public string Currency { get; set; }

        [JsonProperty("data")]
        public Dictionary<string, List<ItemData>> Data { get; set; }
    }
}
