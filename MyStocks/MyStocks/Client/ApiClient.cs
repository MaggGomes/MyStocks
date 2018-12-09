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


        public static async Task<ResponseCompaniesHistory> getHistory1(String date, string symbol)
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


        public static void getHistory(String date, string id, AsyncCallback callback)
        {
            var uri = string.Format(baseUrl + "{0}apikey={1}&symbol={2}&type=daily&startDate={3}", apiGetHistory, apiKey, id, date);
            var state = new State();
            CallWebAsync(uri, state, callback);
        }

        private static void CallWebAsync(string uri, State state, AsyncCallback callback)
        {
            var request = HttpWebRequest.Create(uri);
            request.Method = "GET";
            state.Request = request;
            request.BeginGetResponse(callback, state);
        }

        public static void CallHandler(IAsyncResult asyncResult)
        {
            var state = (State)asyncResult.AsyncState;
            var request = state.Request;

            try
            {
                using (HttpWebResponse response = request.EndGetResponse(asyncResult) as HttpWebResponse)
                {
                    state.Status = response.StatusCode;
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                        {
                            state.Response = reader.ReadToEnd();
                        }
                    }
                }
            }
            catch (Exception e)
            {
                state.Response = e.Message;
            }
        }
    }
}
