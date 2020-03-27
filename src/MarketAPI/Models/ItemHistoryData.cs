using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MarketAPI.Models
{
    public class ItemHistoryData
    {
        private double _averagePrice;

        [JsonProperty("max")]
        public double MaxPrice { get; set; }

        [JsonProperty("min")]
        public double MinPrice { get; set; }

        [JsonProperty("average")]
        public double AveragePrice
        {
            get
            {
                return _averagePrice;
            }
            set
            {
                // The original value from the api is not correct. So we fix it
                _averagePrice = value * 0.1;
            }
        }

        [JsonProperty("history")]
        public List<List<double>> RawPriceHistory { get; set; } = new List<List<double>>();

        [JsonIgnore]
        public Dictionary<long, double> PriceHistory
        {
            get
            {
                var result = new Dictionary<long, double>();
                RawPriceHistory.ForEach(h =>
                {
                    if (h.Count == 2)
                    {
                        result.Add(long.Parse(h.First().ToString()), h.Skip(1).First());
                    }
                });
                return result;
            }
        }
    }
}
