using Newtonsoft.Json;

namespace MarketAPI.Models
{
    public class ItemData
    {
        [JsonProperty("id")]
        public long ID { get; set; }

        [JsonProperty("class")]
        public long Class { get; set; }

        [JsonProperty("instance")]
        public int Instance { get; set; }

        [JsonProperty("price")]
        public int RawPrice { get; set; }

        [JsonIgnore]
        public double Price
        {
            get
            {
                return (double)RawPrice / 1000;
            }
        }
    }
}
