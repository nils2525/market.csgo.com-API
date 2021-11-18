using MarketAPI.Models;
using SmartWebClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MarketAPI
{
    public class Service
    {
        /// <summary>
        /// The maximum amout of requests that can be send every second
        /// Used to prevent Ratelimit from market.com (https://market.csgo.com/docs-v2)
        /// </summary>
        private const int RequestsPerSecond = 5;

        private WebClient _client;

        /// <summary>
        /// The list of timestamps when the last requests were executed, used to prevent executing more requests than in <see cref="RequestsPerSecond"/> defined
        /// </summary>
        private List<DateTime> _requestTimeHistory = new List<DateTime>();

        private object _rateLimitLock = new object();

        public string Currency { get; private set; }
        public bool IsInitialized { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="apiKey"></param>
        /// <exception cref="ArgumentNullException" />
        public Service(string apiKey)
        {
            if (apiKey == null)
            {
                throw new ArgumentNullException(nameof(apiKey));
            }

            _client = new WebClient("https://market.csgo.com/");
            _client.DefaultQueryParameters.Add("key", apiKey);
            _client.TimespanBetweenRequests = new TimeSpan(0, 0, 0, 0, 250);
        }

        public async Task<bool> Init()
        {
            var balance = await GetBalanceAsync();
            if (!String.IsNullOrWhiteSpace(balance?.Currency))
            {
                Currency = balance.Currency;
                return IsInitialized = true;
            }
            return IsInitialized = false;
        }

        public async Task<GetMySteamIDResponse> GetMySteamIDAsync()
        {
            return await GetObjectAsync<GetMySteamIDResponse>("api/v2/get-my-steam-id");
        }

        public async Task<GetPriceListResponse> GetPriceListAsync()
        {
            return await GetObjectAsync<GetPriceListResponse>($"api/v2/prices/{Currency}.json");
        }

        public async Task<GetItemListResponse> GetItemListAsync()
        {
            return await GetObjectAsync<GetItemListResponse>($"api/v2/prices/class_instance/{Currency}.json");
        }

        public async Task<GetItemResult> GetItemAsync(string itemHashName)
        {
            return await GetObjectAsync<GetItemResult>("api/v2/search-list-items-by-hash-name-all", "hash_name[]", itemHashName);
        }

        public async Task<GetItemResult> GetItemsAsync(List<string> itemNames)
        {
            var dict = new List<(string, string)>();
            itemNames.ForEach(e => dict.Add(("list_hash_name[]", e)));

            return await GetObjectAsync<GetItemResult>("api/v2/search-list-items-by-hash-name-all", dict);
        }

        public async Task<GetItemHistoryResult> GetItemHistoryAsync(List<string> items)
        {
            //var dict = new List<(string, string)>();
            //items.ForEach(e => dict.Add(("list_hash_name[]", e)));

            GetItemHistoryResult result = null;
            foreach (var item in items)
            {
                var history = await GetObjectAsync<GetItemHistoryResult>("api/v2/get-list-items-info", "list_hash_name[]", item);
                if (result == null)
                {
                    result = history;
                }
                else
                {
                    foreach (var data in history.Data)
                    {
                        result.Data.Add(data.Key, data.Value);
                    }
                }
            }

            return result;
        }

        public async Task<GetItemSpecificResult> GetItemSpecificAsync(string itemHashName)
        {
            return await GetObjectAsync<GetItemSpecificResult>("api/v2/search-item-by-hash-name-specific", "hash_name", itemHashName);
        }

        public async Task<BuyItemResponse> BuyItemAsync(string itemHashName, double price)
        {
            return await GetObjectAsync<BuyItemResponse>("api/v2/buy", new List<(string, string)>() { ("hash_name", itemHashName), ("price", ((int)(price * 1000)).ToString()) });
        }

        public async Task<BuyItemResponse> BuyItemAsync(long id, double price)
        {
            return await GetObjectAsync<BuyItemResponse>("api/v2/buy", new List<(string, string)>() { ("id", id.ToString()), ("price", ((int)(price * 1000)).ToString()) });
        }

        public async Task<BuyItemResponse> BuyItemForAsync(string itemHashName, double price, int steam32ID, string tradeToken)
        {
            return await GetObjectAsync<BuyItemResponse>("api/v2/buy", new List<(string, string)>() { ("hash_name", itemHashName), ("price", ((int)(price * 1000)).ToString()), ("partner", steam32ID.ToString()), ("token", tradeToken) });
        }

        public async Task<BuyItemResponse> BuyItemForAsync(long id, double price, int steam32ID, string tradeToken)
        {
            return await GetObjectAsync<BuyItemResponse>("api/v2/buy", new List<(string, string)>() { ("id", id.ToString()), ("price", ((int)(price * 1000)).ToString()), ("partner", steam32ID.ToString()), ("token", tradeToken) });
        }


        public async Task<GetBalanceResponse> GetBalanceAsync()
        {
            return await GetObjectAsync<GetBalanceResponse>("api/v2/get-money");
        }

        public async Task<PingResult> PingAsync()
        {
            return await GetObjectAsync<PingResult>("api/v2/ping");
        }

        private async Task<T> GetObjectAsync<T>(string path, string queryKey, string queryValue)
        {
            return await GetObjectAsync<T>(path, new List<(string, string)>() { (queryKey, queryValue) });
        }

        private async Task<T> GetObjectAsync<T>(string path, List<(string, string)> queryParameters = null)
        {
            PreventRateLimitAsync();

            var requestResult = await _client.GetObjectAsync<T>(path, queryParameters);
            if (requestResult is BaseResponse response)
            {
                if (!response.IsSuccessfully)
                {
                    Logger.LogToConsole(Logger.LogType.Warning, response.ErrorMessage + response.Message);
                }
            }
            return requestResult;
        }

        private void PreventRateLimitAsync()
        {
            lock (_rateLimitLock)
            {
                bool first = true;
                while (_requestTimeHistory.Where(c => c.AddSeconds(1) >= DateTime.Now).Count() > RequestsPerSecond - 1)
                {
                    if (first)
                    {
                        first = false;
                        Logger.LogToConsole(Logger.LogType.Information, "Ratelimit wait");
                    }
                    Logger.LogToConsole(Logger.LogType.Information, "~", false);
                    Thread.Sleep(10);
                }

                _requestTimeHistory.RemoveAll(c => c.AddSeconds(1) <= DateTime.Now);
                _requestTimeHistory.Add(DateTime.Now);
            }
        }
    }
}
