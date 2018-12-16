using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MyStocks.Models;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MyStocks.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class CompanyView : ContentPage
	{
		public CompanyView (CompanyQuote company)
		{
			InitializeComponent ();
            Title = company.name;
            BindingContext = company;
		}
	}
}