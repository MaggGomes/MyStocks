using Newtonsoft.Json.Linq;
using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;

using MyStocks.Models;
using MyStocks.Client;

using Xamarin.Forms;


namespace MyStocks.ViewModels
{
    public class HistoryViewModel : BaseViewModel
    {
        public ObservableCollection<List<CompanyDetails>> CompaniesDetails;
        private List<Company> CompaniesSelected;
        String date;
        public bool CanDraw { get; set; }
        
        public HistoryViewModel(List<Company> companies = null, String date = null)

        {
            CompaniesDetails = new ObservableCollection<List<CompanyDetails>>();
            CompaniesSelected = companies;
            this.date = date;
        }

        public void LoadHistory()
        {
            for(int i=0;i< CompaniesSelected.Count; i++)
            {
                ApiClient.getHistory(date, CompaniesSelected[i].Symbol, LoadHistoryHandler);
            }
            
        }
        private void LoadHistoryHandler(IAsyncResult asyncResult)
        {
            try
            {
                CanDraw = false;
                ApiClient.CallHandler(asyncResult);
                var state = (State)asyncResult.AsyncState;
                string symbol = "";

                if (state.Status == HttpStatusCode.OK)
                {
                    Device.BeginInvokeOnMainThread(() => { CompaniesDetails.Clear(); });
                    Debug.WriteLine("rsponse " + state.Response);
                    JObject response = JObject.Parse(state.Response);
                    List<CompanyDetails> details = new List<CompanyDetails>();
                    foreach (JObject o in response["results"].Children<JObject>())
                    {
                        DateTime date = DateTime.ParseExact((string)o["tradingDay"], "yyyy-MM-dd",null);
                        Debug.WriteLine("data " + date + " " + o["tradingDay"]);
                        float openValue = (float)o["open"];
                        float highValue = (float)o["high"];
                        float lowValue = (float)o["low"];
                        float closeValue = (float)o["close"];
                        int volume = (int)o["volume"];
                        symbol = (string)o["symbol"];

                        Debug.WriteLine("recebi " + openValue + " " + highValue + " " + lowValue + " " + closeValue + " " + volume);

                        CompanyDetails sd = new CompanyDetails() { open = openValue, high = highValue, low = lowValue, close = closeValue, volume = volume, timestamp = date, };
                        details.Add(sd);
                        
                    }
                    
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        Debug.WriteLine("vou autorizar " + details.Count);
                        CanDraw = true;
                        CompaniesDetails.Add(details);
                    });
                    
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }
    }
}
