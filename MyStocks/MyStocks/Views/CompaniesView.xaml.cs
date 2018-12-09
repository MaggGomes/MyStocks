using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using MyStocks.Models;
using MyStocks.ViewModels;

namespace MyStocks.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class CompaniesView : ContentPage
	{
        List<Company> companiesSelected;
        CompaniesListViewModel companies;

        public CompaniesView()
        {
            InitializeComponent();
            companies = new CompaniesListViewModel();
            BindingContext = companies;
        }

        async void GenerateGraphic(object sender, EventArgs e)
        {
            companiesSelected = new List<Company>();

            for (int i = 0; i < companies.Companies.Count; i++)
            {
                if (companies.Companies[i].Selected)
                {
                    companiesSelected.Add(companies.Companies[i]);
                }
            }

            if(companiesSelected.Count < 1 || companiesSelected.Count > 2)
            {
                await DisplayAlert("Info", "You must choose 1 or 3 companies.", "Ok");
            }
            else
            {
                await Navigation.PushAsync(new HistoryPage(companiesSelected, "20181002"));
            }
        }
    }
}