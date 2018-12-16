using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Diagnostics;

using MyStocks.Models;
using MyStocks.Client;

namespace MyStocks.ViewModels
{
    public class HistoryViewModel : BaseViewModel
    {
        public ObservableCollection<ResponseCompaniesHistory> CompaniesHistory;
        public ObservableCollection<ResponseCompaniesQuote> CompaniesQuotes;
        private List<Company> CompaniesSelected;
        string frequency;
        public bool Loading { get; set; }
        
        public HistoryViewModel(List<Company> companies = null, string frequency = null)
        {
            CompaniesHistory = new ObservableCollection<ResponseCompaniesHistory>();
            CompaniesQuotes = new ObservableCollection<ResponseCompaniesQuote>();
            CompaniesSelected = companies;
            this.frequency = frequency;
        }

        public async Task GetQuote()
        {
            Loading = true;
            ObservableCollection<ResponseCompaniesQuote> responses = new ObservableCollection<ResponseCompaniesQuote>();

            ResponseCompaniesQuote response = await ApiClient.getQuote(CompaniesSelected);
            responses.Add(response);
            
            CompaniesQuotes = responses;
        }

        public async Task GetHistory(int points)
        {
            Loading = true;
            ObservableCollection<ResponseCompaniesHistory> responses = new ObservableCollection<ResponseCompaniesHistory>();
        
            foreach (Company company in CompaniesSelected)
            {
                ResponseCompaniesHistory response = await ApiClient.getHistory(frequency, company.Symbol);

                if (response != null)
                {
                    List<CompanyDetails> results = new List<CompanyDetails>();

                    Debug.WriteLine("points:" + points);
                    Debug.WriteLine("count:" + response.results.Count);
                    Debug.WriteLine("count:" + (response.results.Count-points));



                    for (int i = response.results.Count-points; i < response.results.Count; i++)
                    {
                        results.Add(response.results[i]);
                        Debug.WriteLine(i);
                    }

                    response.results = results;

                    responses.Add(response);
                }
            }

            Loading = false;
            CompaniesHistory = responses;
        }
    }
}
