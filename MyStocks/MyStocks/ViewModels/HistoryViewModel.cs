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
        private List<Company> CompaniesSelected;
        String date;
        public bool Loading { get; set; }
        
        public HistoryViewModel(List<Company> companies = null, String date = null)

        {
            CompaniesHistory = new ObservableCollection<ResponseCompaniesHistory>();
            CompaniesSelected = companies;
            this.date = date;
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
