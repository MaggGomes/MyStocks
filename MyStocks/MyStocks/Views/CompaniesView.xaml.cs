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
        CompaniesListViewModel companies;

        public CompaniesView()
        {
            InitializeComponent();

            companies = new CompaniesListViewModel();
            BindingContext = companies;
        }

        async void GenerateGraphic(object sender, EventArgs e)
        {
            DependencyService.Get<Toast>().Show("dadadasda");


            await Navigation.PushAsync(new GraphView());


            /*int selected = 0;
            List<Company> selectedCompanies = new List<Company>();
            for (int i = 0; i < Companies.Count; i++)
            {
                if (Companies[i].Selected)
                {
                    selected++;
                    selectedCompanies.Add(Companies[i].Data);
                }
            }

            if (selected == 0)
            {
                await DisplayAlert("Invalid Selection", "No companies selected", "OK");
            }
            else if (selected > 2)
            {
                await DisplayAlert("Invalid Selection", "Can only select up to 2 companies", "OK");
            }
            else
            {
                await DisplayAlert("Valid Selection", "Selected " + selected + " companies", "OK");
            }*/

        }
    }
}