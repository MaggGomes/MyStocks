using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

using MyStocks.Models;

namespace MyStocks.Client
{
    class ApiClient
    {
        private static HttpClient client = new HttpClient();
        private static String baseUrl = "https://marketdata.websol.barchart.com/";
        private static String apiGetQuote = "getQuote.json?";
        private static String apiGetHistory = "getHistory.json?";
        private static String apiKey = "27575c3133d7d1115e0bc27927d43eba";
        
        public static void getQuote(string id, string date)
        {

        }

        public static async Task<ResponseCompaniesHistory> getHistory(String date, string symbol)
        {
            var query = string.Format(baseUrl + "{0}apikey={1}&symbol={2}&type=daily&startDate={3}", apiGetHistory, apiKey, symbol, date);
            HttpResponseMessage response = await client.GetAsync(query);
            
            if (response.StatusCode == HttpStatusCode.OK)
            {
                string json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<ResponseCompaniesHistory>(json);
            }
           
            return null;
        }
    }
}
