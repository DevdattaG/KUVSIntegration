using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UberEngine
{

    public class FareBreakdown
    {
        public double high_amount { get; set; }
        public string display_amount { get; set; }
        public string display_name { get; set; }
        public double low_amount { get; set; }
    }

    public class Fare
    {
        public double value { get; set; }
        public string fare_id { get; set; }
        public int expires_at { get; set; }
        public string display { get; set; }
        public string currency_code { get; set; }
    }

    public class Estimate
    {
        public string surge_confirmation_href { get; set; }
        public int high_estimate { get; set; }
        public int expires_at { get; set; }
        public string surge_confirmation_id { get; set; }
        public int minimum { get; set; }
        public int low_estimate { get; set; }
        public List<FareBreakdown> fare_breakdown { get; set; }
        public double surge_multiplier { get; set; }
        public string display { get; set; }
        public string currency_code { get; set; }
    }

    public class Trip
    {
        public string distance_unit { get; set; }
        public int duration_estimate { get; set; }
        public double distance_estimate { get; set; }
    }

    public class RequestEstimateOutput
    {
        public Estimate estimate { get; set; }
        public Fare fare { get; set; }
        public Trip trip { get; set; }
        public int pickup_estimate { get; set; }
    }
}
