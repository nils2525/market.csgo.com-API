using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace MarketAPI.Models
{
    public class GetMySteamIDResponse : BaseResponse
    {
        [JsonProperty("steamid32")]
        public string SteamID32 { get; set; }

        [JsonProperty("steamid64")]
        public string SteamID64 { get; set; }
    }
}
