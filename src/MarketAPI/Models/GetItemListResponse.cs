using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace MarketAPI.Models
{
    public class GetItemListResponse : BaseResponse
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
        public Dictionary<string, ItemInfo> Items { get; set; }

    }

    public class ItemInfo
    {
        [JsonProperty("market_hash_name")]
        public string MarketHashNme { get; set; }

        [JsonProperty("price")]
        public double Price { get; set; }

        [JsonProperty("buy_order")]
        public double? NextBuyOrder { get; set; }

        [JsonProperty("avg_price")]
        public double? AveragePrice { get; set; }

        [JsonProperty("popularity_7d")]
        public int? PopularityLastWeek { get; set; }
    }
}
