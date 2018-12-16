using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Rg.Plugins.Popup.Services;

using MyStocks.Models;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MyStocks.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class CompanniesModalView
	{
        string frequency = "Daily";
        int points = 7;
        List<Company> companiesSelected = new List<Company>();
		public CompanniesModalView (List<Company> companiesSelected)
		{
			InitializeComponent ();
            this.companiesSelected = companiesSelected;
            frequencyPicker.SelectedIndex = 0;
        }

        void OnSliderValueChanged(object sender, ValueChangedEventArgs args)
        {
            points = (int)args.NewValue;
        }

        private void OnFrequencySelected(object sender, EventArgs e)
        {
            frequency = frequencyPicker.Items[frequencyPicker.SelectedIndex];
        }

        async void GenerateHistory(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new HistoryView(companiesSelected, frequency, points));
            await PopupNavigation.Instance.PopAsync();
        }
    }
}