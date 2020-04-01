using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace MarketAPI.Models
{
    public class GetBalanceResponse : BaseResponse
    {
        [JsonProperty("money")]
        public double Balance { get; set; }

        [JsonProperty("currency")]
        public string Currency { get; set; }
    }
}
