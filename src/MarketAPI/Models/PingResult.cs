using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace MarketAPI.Models
{
    public class PingResult : BaseResponse
    {
        [JsonProperty("ping")]
        public string Ping { get; set; }

        [JsonProperty("online")]
        public bool IsOnline { get; set; }

        [JsonProperty("p2p")]
        public bool IsP2P { get; set; }

        [JsonProperty("steamApiKey")]
        public bool IsSteamAPIKey { get; set; }
    }
}
