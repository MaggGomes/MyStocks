using System;

namespace MyStocks.Models
{
    public class CompanyDetails {

        public string symbol { get; set; }
        public DateTime timestamp { get; set; }
        public string tradingDay { get; set; }
        public float open { get; set; }
        public float high { get; set; }
        public float low { get; set; }
        public float close { get; set; }
        public int volume { get; set; }
        public object openInterest { get; set; }
    }
}