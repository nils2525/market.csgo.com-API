using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace MarketAPI.Models
{
    public class GetMySteamIDResponse : BaseResponse
    {
        [JsonProperty("steamid32")]
        public int SteamID32 { get; set; }

        [JsonProperty("steamid64")]
        public long SteamID64 { get; set; }
    }
}
