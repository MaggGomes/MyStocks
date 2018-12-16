using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Diagnostics;

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
        
        public static async Task<ResponseCompaniesQuote> getQuote(List<Company> companies)
        {
            string symbol1 = companies[0].Symbol;
            string symbol2 = "";

            if(companies.Count == 2)
            {
                symbol2 = "%2C" + companies[1].Symbol;
            }

            var query = string.Format(baseUrl + "{0}apikey={1}&symbols={2}{3}", apiGetQuote, apiKey, symbol1, symbol2);

            HttpResponseMessage response = await client.GetAsync(query);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                string json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<ResponseCompaniesQuote>(json);
            }

            return null;
        }

        public static async Task<ResponseCompaniesHistory> getHistory(string frequency, string symbol)
        {
            var query = string.Format(baseUrl + "{0}apikey={1}&symbol={2}&type={3}&startDate=20171002", apiGetHistory, apiKey, symbol, frequency);
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
