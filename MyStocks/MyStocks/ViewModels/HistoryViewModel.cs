using Newtonsoft.Json.Linq;
using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Threading.Tasks;

using MyStocks.Models;
using MyStocks.Client;

namespace MyStocks.ViewModels
{
    public class HistoryViewModel : BaseViewModel
    {
        public ObservableCollection<ResponseCompaniesHistory> CompaniesHistory;
        public ObservableCollection<ResponseCompaniesQuote> CompaniesQuotes;
        private List<Company> CompaniesSelected;
        String date;
        public bool Loading { get; set; }
        
        public HistoryViewModel(List<Company> companies = null, String date = null)
        {
            CompaniesHistory = new ObservableCollection<ResponseCompaniesHistory>();
            CompaniesQuotes = new ObservableCollection<ResponseCompaniesQuote>();
            CompaniesSelected = companies;
            this.date = date;
        }

        public async Task GetQuote()
        {
            Loading = true;
            ObservableCollection<ResponseCompaniesQuote> responses = new ObservableCollection<ResponseCompaniesQuote>();

            ResponseCompaniesQuote response = await ApiClient.getQuote(CompaniesSelected);
            responses.Add(response);
            
            CompaniesQuotes = responses;
        }

        public async Task GetHistory()
        {
            Loading = true;
            ObservableCollection<ResponseCompaniesHistory> responses = new ObservableCollection<ResponseCompaniesHistory>();
        
            foreach (Company company in CompaniesSelected)
            {
                ResponseCompaniesHistory response = await ApiClient.getHistory(date, company.Symbol);

                if (response != null)
                {
                    responses.Add(response);
                }
            }

            Loading = false;
            CompaniesHistory = responses;
        }
    }
}
