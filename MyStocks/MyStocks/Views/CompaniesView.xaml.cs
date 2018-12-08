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
        List<Company> companiesSelected = new List<Company>();
        CompaniesListViewModel companies;

        public CompaniesView()
        {
            InitializeComponent();
            companiesSelected = new List<Company>();
            companies = new CompaniesListViewModel();
            BindingContext = companies;
        }

        async void GenerateGraphic(object sender, EventArgs e)
        {
            DependencyService.Get<Toast>().Show("dadadasda");


            //await Navigation.PushAsync(new GraphView());
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
                await DisplayAlert("Invalid Selection", "No companies selected", "OK");
            } else
            {
                await DisplayAlert("Selection", "companies selected", "OK");
            }
        }
    }
}