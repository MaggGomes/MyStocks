using System.Collections.ObjectModel;

using MyStocks.Models;

namespace MyStocks.ViewModels
{
    class CompaniesViewModel : BaseViewModel
    {
        public ObservableCollection<Company> Companies { get; set; }

        public CompaniesViewModel()
        {
            Companies = generateCompanies();
        }

        public ObservableCollection<Company> generateCompanies()
        {
            ObservableCollection<Company> companiesList = new ObservableCollection<Company>();

            companiesList.Add(new Company() { Symbol = "AMD", Name = "AMD", Image = "amd.png", Selected = false });
            companiesList.Add(new Company() { Symbol = "AAPL", Name="Apple", Image="apple.png", Selected = false });
            companiesList.Add(new Company() { Symbol = "FB", Name = "Facebook", Image = "facebook.png", Selected = false });
            companiesList.Add(new Company() { Symbol = "GOOGL", Name = "Google", Image = "google.png", Selected = false });
            companiesList.Add(new Company() { Symbol = "HPE", Name = "Hewlett Packard", Image = "hp.png", Selected = false });
            companiesList.Add(new Company() { Symbol = "IBM", Name = "IBM", Image = "ibm.png", Selected = false });
            companiesList.Add(new Company() { Symbol = "INTC", Name = "Intel", Image = "intel.png", Selected = false });
            companiesList.Add(new Company() { Symbol = "MSFT", Name = "Microsoft", Image = "microsoft.png", Selected = false });
            companiesList.Add(new Company() { Symbol = "ORCL", Name = "Oracle", Image = "oracle.png", Selected = false });
            companiesList.Add(new Company() { Symbol = "TWTR", Name = "Twitter", Image = "twitter.png", Selected = false });

            return companiesList;
        }
    }
}
