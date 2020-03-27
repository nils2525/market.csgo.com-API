using MarketAPI.Models;
using SmartWebClient;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MarketAPI
{
    public class Service
    {
        private WebClient _client;

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

        public async Task<GetMySteamIDResponse> GetMySteamIDAsync()
        {
            return await GetObjectAsync<GetMySteamIDResponse>("api/v2/get-my-steam-id");
        }

        public async Task<GetPriceListResponse> GetPriceListAsync()
        {
            return await GetObjectAsync<GetPriceListResponse>("api/v2/prices/USD.json");
        }

        public async Task<GetItemListResponse> GetItemListAsync()
        {
            return await GetObjectAsync<GetItemListResponse>("api/v2/prices/class_instance/USD.json");
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
            var dict = new List<(string, string)>();
            items.ForEach(e => dict.Add(("list_hash_name[]", e)));

            return await GetObjectAsync<GetItemHistoryResult>("api/v2/get-list-items-info", dict);
        }

        public async Task<GetItemSpecificResult> GetItemSpecificAsync(string itemHashName)
        {
            return await GetObjectAsync<GetItemSpecificResult>("api/v2/search-item-by-hash-name-specific", "hash_name", itemHashName);
        }

        public async Task<BuyItemResponse> BuyItemAsync(string itemHashName, double price)
        {
            return await GetObjectAsync<BuyItemResponse>("api/v2/buy", new List<(string, string)>() { ("hash_name", itemHashName), ("price", ((int)(price * 1000)).ToString()) });
        }

        public async Task<BuyItemResponse> BuyItemAsync(int id, double price)
        {
            return await GetObjectAsync<BuyItemResponse>("api/v2/buy", new List<(string, string)>() { ("id", id.ToString()), ("price", ((int)(price * 1000)).ToString()) });
        }

        private async Task<T> GetObjectAsync<T>(string path, string queryKey, string queryValue)
        {
            return await GetObjectAsync<T>(path, new List<(string, string)>() { (queryKey, queryValue) });
        }

        private async Task<T> GetObjectAsync<T>(string path, List<(string, string)> queryParameters = null)
        {
            var requestResult = await _client.GetObjectAsync<T>(path, queryParameters);
            if (requestResult is BaseResponse response)
            {
                if (!response.IsSuccessfully)
                {
                    Console.WriteLine("Warning: " + response.ErrorMessage);
                }
            }
            return requestResult;
        }
    }
}
