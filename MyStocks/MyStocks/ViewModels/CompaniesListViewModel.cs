using System;
using System.Collections.Generic;
using System.Text;

using MyStocks.Models;

namespace MyStocks.ViewModels
{
    class CompaniesListViewModel : BaseViewModel
    {
        public List<Company> Companies { get; set; }

        public CompaniesListViewModel()
        {
            Companies = generateCompanies();
        }

        public List<Company> generateCompanies()
        {
            List<Company> companiesList = new List<Company>();

            companiesList.Add(new Company() { Id = "AMD", Name = "AMD", Image = "amd.png", Selected = false });
            companiesList.Add(new Company() { Id="AAPL", Name="Apple", Image="apple.png", Selected = false });
            companiesList.Add(new Company() { Id = "FB", Name = "Facebook", Image = "facebook.png", Selected = false });
            companiesList.Add(new Company() { Id = "GOOGL", Name = "Google", Image = "google.png", Selected = false });
            companiesList.Add(new Company() { Id = "HPE", Name = "Hewlett Packard", Image = "hp.png", Selected = false });
            companiesList.Add(new Company() { Id = "IBM", Name = "IBM", Image = "ibm.png", Selected = false });
            companiesList.Add(new Company() { Id = "INTC", Name = "Intel", Image = "intel.png", Selected = false });
            companiesList.Add(new Company() { Id = "MSFT", Name = "Microsoft", Image = "microsoft.png", Selected = false });
            companiesList.Add(new Company() { Id = "ORCL", Name = "Oracle", Image = "oracle.png", Selected = false });
            companiesList.Add(new Company() { Id = "TWTR", Name = "Twitter", Image = "twitter.png", Selected = false });

            return companiesList;
        }
    }
}
