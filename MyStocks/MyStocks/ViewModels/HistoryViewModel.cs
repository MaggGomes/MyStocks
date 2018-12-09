using Newtonsoft.Json.Linq;
using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using MyStocks.Models;
using MyStocks.Services;
using Xamarin.Forms;


namespace MyStocks.ViewModels
{
    public class HistoryViewModel : BaseViewModel
    {
        public ObservableCollection<List<CompanyDetails>> stockDetails { get; set; }
        public bool CanDraw { get; set; }
        private List<Company> CompaniesSelected;
        public ObservableCollection<CompanyStock> CompaniesStock { get; set; }
        String date;

        string  SliderDate_;
        public string SliderDate
        {
            get { return SliderDate_; }
            set
            {
                SetProperty(ref SliderDate_, value);
            }
        }

        double Minimum_ = 0;
        public double Minimum_Slider
        {
            get { return Minimum_; }
            set { SetProperty(ref Minimum_, value); }
        }

        double Maximum_ = 100;
        public double Maximum_Slider
        {
            get { return Maximum_; }
            set { SetProperty(ref Maximum_, value); }
        }

        double SliderValue_ = 50;
        public double SliderValue
        {
            get { return SliderValue_; }
            set {
                SetProperty(ref SliderValue_, value);
                Slider_ValueChanged(); 
            }
        }


        public HistoryViewModel(List<Company> companies = null, String date = null)

        {
            Title = "Past Days";
            //company = c;
            CompaniesSelected = companies;
            this.date = date;
            stockDetails = new ObservableCollection<List<CompanyDetails>>();
            CompaniesStock = new ObservableCollection<CompanyStock>();
        }

        public void LoadHistory()
        {
            if (IsBusy)
                return;

            IsBusy = true;
            for(int i=0;i< CompaniesSelected.Count; i++)
            {
                API.getHistory(CompaniesSelected[i].Id, date, LoadHistoryHandler);
            }
            
        }

        private void LoadHistoryHandler(IAsyncResult asyncResult)
        {
            try
            {
                CanDraw = false;
                API.CallHandler(asyncResult);
                var state = (State)asyncResult.AsyncState;
                string symbol = "";
                if (state.Status == HttpStatusCode.OK)
                {
                    Device.BeginInvokeOnMainThread(() => { stockDetails.Clear(); });
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

                        CompanyDetails sd = new CompanyDetails() { open = openValue, high = highValue, low = lowValue, close = closeValue, volume = volume, date = date, };
                        details.Add(sd);
                        
                    }
                    

                    for (int i=0;i< CompaniesSelected.Count; i++)
                    {
                        if (CompaniesSelected[i].Id == symbol)
                        {
                            CompanyStock Cs = new CompanyStock() { DisplayName = CompaniesSelected[i].Name, Details = details[0] };
                            CompaniesStock.Add(Cs);
                        }
                    }
                    SliderDate = details[0].date.ToString("dd/MM/yyyy");
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        
                        Debug.WriteLine("vou autorizar " + details.Count);
                        CanDraw = true;
                        stockDetails.Add(details);
                        SliderDate = stockDetails[0][0].date.ToString("dd/MM/yyyy");
                    });
                    
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
            finally
            {
                IsBusy = false;
            }
        }

        public void Slider_ValueChanged()
        {
            if(stockDetails!=null && stockDetails.Count > 0 && CompaniesStock!=null && CompaniesStock.Count>0)
            {
                int maxValue = stockDetails[0].Count;
                int position = (int)(Math.Floor(SliderValue_ / (100 / (double)maxValue)));
                
                if (position >= maxValue)
                    position = maxValue - 1;
                SliderDate = stockDetails[0][position].date.ToString("dd/MM/yyyy");
                
                for (int i=0;i< CompaniesStock.Count; i++)
                {
                    CompaniesStock[i].Details = stockDetails[i][position];
                }

            }
            
        }
    }
}
