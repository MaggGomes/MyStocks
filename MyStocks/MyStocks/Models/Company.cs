using System;
using System.Collections.Generic;
using System.Text;

namespace MyStocks.Models
{
    class Company
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }

        public Company(string id, string name, string image)
        {
            this.Id = id;
            this.Name = name;
            this.Image = image;
        }
    }
}
