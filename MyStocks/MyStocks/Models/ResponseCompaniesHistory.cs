using System;
using System.Collections.Generic;
using System.Text;

namespace MyStocks.Models
{
    public class ResponseCompaniesHistory
    {
        public Status status { get; set; }
        public List<CompanyDetails> results { get; set; }
    }
}
