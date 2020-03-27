using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace MarketAPI.Models
{
    public class BuyItemResponse : BaseResponse
    {
        [JsonProperty("id")]
        public int? ID { get; set; }

        [JsonProperty("price")]
        public int? RawPrice { get; set; }

        [JsonIgnore]
        public double Price
        {
            get
            {
                if (RawPrice != null)
                {
                    return (double)RawPrice / 1000;
                }
                else
                {
                    return 0;
                }
            }
        }
    }
}
