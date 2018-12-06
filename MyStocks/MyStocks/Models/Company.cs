using System;
using System.Collections.Generic;
using System.Text;

namespace MyStocks.Models
{
    class Company
    {
        /*public string Id { get; set; }
        public string Name { get; set; }
        public double Quote { get; set; }*/

        public string Name { get; set; }
        public string Symbol { get; set; }
        public string ImageSource { get; set; }

        public Company(string name, string symbol, string imageSource)
        {
            this.Name = name;
            this.Symbol = symbol;
            this.ImageSource = imageSource;
        }
    }
}
