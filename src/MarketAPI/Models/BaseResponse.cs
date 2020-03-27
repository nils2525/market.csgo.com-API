using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace MarketAPI.Models
{
    public class BaseResponse
    {
        [JsonProperty("success")]
        public bool IsSuccessfully { get; set; }

        [JsonProperty("error")]
        public string ErrorMessage { get; set; }
    }
}
