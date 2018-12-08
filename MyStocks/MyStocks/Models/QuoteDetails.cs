using System;

namespace MyStocks.Models
{
    public class QuoteDetails {
    
        public float open { get; set; }
        public float high { get; set; }
        public float low { get; set; }
        public float close { get; set; }
        public int volume { get; set; }
        public DateTime date { get; set; }
    }
}