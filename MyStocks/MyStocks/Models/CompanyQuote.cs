using System;
using System.Collections.Generic;
using System.Text;

namespace MyStocks.Models
{
    public class CompanyQuote
    {
        public string symbol { get; set; }
        public string exchange { get; set; }
        public string name { get; set; }
        public string dayCode { get; set; }
        public DateTime serverTimestamp { get; set; }
        public string mode { get; set; }
        public double lastPrice { get; set; }
        public DateTime tradeTimestamp { get; set; }
        public double netChange { get; set; }
        public double percentChange { get; set; }
        public string unitCode { get; set; }
        public double open { get; set; }
        public double high { get; set; }
        public double low { get; set; }
        public double close { get; set; }
        public string flag { get; set; }
        public int volume { get; set; }
    }
}
