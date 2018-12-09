using System.ComponentModel;
using System.Diagnostics;

namespace MyStocks.Models
{
    public class Company : INotifyPropertyChanged
    {
        public string Symbol { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        private bool _Selected;

        public bool Selected
        {
            get
            {
                return _Selected;
            }
            set
            {
                _Selected = value;
                OnPropertyChanged(nameof(Selected));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
    }
}
