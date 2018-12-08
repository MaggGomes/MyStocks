using System.ComponentModel;
using System.Diagnostics;

namespace MyStocks.Models
{
    class Company : INotifyPropertyChanged
    {
        public string Id { get; set; }
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
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(property));
            }      
        }
    }
}
