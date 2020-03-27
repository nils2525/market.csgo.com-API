using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace MarketAPI.Models
{
    public class GetPriceListResponse : BaseResponse
    {
        //ToDo: The request does not return a full Millis value
        [JsonProperty("time")]
        public long Millis { get; set; }

        [JsonIgnore]
        public DateTime Timestamp
        {
            get
            {
                return new DateTime(Millis);
            }
        }

        [JsonProperty("currency")]
        public string Currency { get; set; }

        [JsonProperty("items")]
        public List<PriceListItem> Items { get; set; }

    }

    public class PriceListItem
    {
        [JsonProperty("market_hash_name")]
        public string MarketHashNme { get; set; }

        [JsonProperty("volume")]
        public int Volume { get; set; }

        [JsonProperty("price")]
        public double Price { get; set; }
    }
}
