using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using MyStocks.Models;

namespace MyStocks.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class CompaniesView : ContentPage
	{
        private List<SelectableData<Company>> Companies { get; set; }

        public CompaniesView()
        {
            InitializeComponent();

            Companies = fillCompaniesList();
            CompaniesList.ItemsSource = Companies;
        }


        private List<SelectableData<Company>> fillCompaniesList()
        {
            Companies = new List<SelectableData<Company>>();
            List<Company> companies = new List<Company>();

            companies.Add(new Company("AMD", "AMD", "amd.png"));
            companies.Add(new Company("AAPL", "Apple", "apple.png"));
            companies.Add(new Company("FB", "Facebook", "facebook.png"));
            companies.Add(new Company("GOOGL", "Google", "google.png"));
            companies.Add(new Company("HPE", "Hewlett Packard", "hp.png"));
            companies.Add(new Company("IBM", "IBM", "ibm.png"));
            companies.Add(new Company("INTC", "Intel", "intel.png"));
            companies.Add(new Company("MSFT", "Microsoft", "microsoft.png"));
            companies.Add(new Company("ORCL", "Oracle", "oracle.png"));
            companies.Add(new Company("TWTR", "Twitter", "twitter.png"));

            for (int i = 0; i < companies.Count; i++)
            {
                Companies.Add(new SelectableData<Company>(companies[i]));
            }

            return Companies;
        }

        async void GenerateGraphic(object sender, EventArgs e)
        {
            int selected = 0;
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
            }

        }
    }
}