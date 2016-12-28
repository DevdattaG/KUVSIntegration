﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UberEngine
{

    public class Price
    {
        public string localized_display_name { get; set; }
        public double distance { get; set; }
        public string display_name { get; set; }
        public string product_id { get; set; }
        public string high_estimate { get; set; }
        public double surge_multiplier { get; set; }
        public string minimum { get; set; }
        public string low_estimate { get; set; }
        public int duration { get; set; }
        public string estimate { get; set; }
        public string currency_code { get; set; }
    }  

    public class FareEstimateOutput
    {
        public List<Price> prices { get; set; }
    }    
}
