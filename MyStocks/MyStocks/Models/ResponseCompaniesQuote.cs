using System;
using System.Collections.Generic;
using System.Text;

namespace MyStocks.Models
{
    public class ResponseCompaniesQuote
    {
        public Status status { get; set; }
        public List<CompanyQuote> results { get; set; }
    }
}
