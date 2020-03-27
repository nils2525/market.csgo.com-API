using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace MarketAPI.Models
{
    public class GetItemHistoryResult : BaseResponse
    {
        [JsonProperty("currency")]
        public string Currency { get; set; }

        [JsonProperty("data")]
        public Dictionary<string, ItemHistoryData> Data { get; set; } = new Dictionary<string, ItemHistoryData>();
    }
}
