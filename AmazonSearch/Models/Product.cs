using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AmazonSearch.Models
{
    public class Product
    {
        public String Title { get; set; }
        public String DetailPageLink { get; set; }
        public Nullable<double> LowestNewPrice { get; set; }
        public Nullable<double> LowestUsedPrice { get; set; }
    }
}