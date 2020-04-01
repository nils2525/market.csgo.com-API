using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MarketAPI.Test
{
    [TestClass]
    public class TestService
    {
        private const string _apiKey = "";
        private static MarketAPI.Service _service;

        [ClassInitialize]
        public static void InitTests(TestContext context)
        {
            _service = new Service(_apiKey);
        }

        [TestMethod]
        public async Task GetMySteamIDTest()
        {
            Assert.IsTrue(!String.IsNullOrWhiteSpace((await _service.GetMySteamIDAsync()).SteamID64));
        }

        [TestMethod]
        public async Task GetPriceListTest()
        {
            Assert.IsTrue((await _service.GetPriceListAsync()).Items.Count > 0);
        }

        [TestMethod]
        public async Task GetItemTest()
        {
            Assert.IsNotNull(await _service.GetItemAsync("Chroma 2 Case"));
        }

        [TestMethod]
        public async Task GetItemListTest()
        {
            Assert.IsTrue((await _service.GetItemListAsync()).Items.Count > 0);
        }

        [TestMethod]
        public async Task GetItemHistoryTest()
        {
            Assert.IsTrue((await _service.GetItemHistoryAsync(new System.Collections.Generic.List<string>() { "Chroma 2 Case" })).Data?.Count > 0);
        }

        [TestMethod]
        public async Task GetItemSpecificTest()
        {
            Assert.IsNotNull(await _service.GetItemSpecificAsync("Chroma 2 Case"));
        }

        [TestMethod]
        public async Task BuyItemTest()
        {
            var item = await _service.GetItemSpecificAsync("Spectrum 2 Case");
            var buy = await _service.BuyItemAsync("Spectrum 2 Case", item.Data.First().Price);
        }

        [TestMethod]
        public async Task GetBalanceTest()
        {
            var balance = await _service.GetBalanceAsync();
            Assert.IsNotNull(balance);
        }
    }
}
