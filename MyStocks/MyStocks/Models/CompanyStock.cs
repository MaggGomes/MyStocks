using System;
using System.ComponentModel;
using System.Diagnostics;
using Xamarin.Forms;

namespace MyStocks.Models
{
    public class CompanyStock : INotifyPropertyChanged
    {
        public string DisplayName { get; set; }

        private QuoteDetails _Details;
        public QuoteDetails Details
        {
            get
            {
                return _Details;
            }
            set
            {
                _Details = value;
                OnPropertyChanged(nameof(Details));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string propertyName)
        {

            Debug.WriteLine("recebi update de company stock");
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler == null) return;
            handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}