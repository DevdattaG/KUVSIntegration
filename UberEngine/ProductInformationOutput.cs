using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UberEngine
{ 
    public class PriceDetails
    {
        public List<object> service_fees { get; set; }
        public int cost_per_minute { get; set; }
        public string distance_unit { get; set; }
        public int minimum { get; set; }
        public int cost_per_distance { get; set; }
        public int @base { get; set; }
        public int cancellation_fee { get; set; }
        public string currency_code { get; set; }
    }

    public class Product
    {
        public bool upfront_fare_enabled { get; set; }
        public int capacity { get; set; }
        public string product_id { get; set; }
        public PriceDetails price_details { get; set; }
        public string image { get; set; }
        public bool cash_enabled { get; set; }
        public bool shared { get; set; }
        public string short_description { get; set; }
        public string display_name { get; set; }
        public string product_group { get; set; }
        public string description { get; set; }
    }

    public class ProductInformationOutput
    {
        public List<Product> products { get; set; }
    }

}
