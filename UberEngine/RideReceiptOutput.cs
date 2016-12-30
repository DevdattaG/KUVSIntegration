using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UberEngine
{

    public class Charge
    {
        public string amount { get; set; }
        public string type { get; set; }
        public string name { get; set; }
    }

    public class SurgeCharge
    {
        public string name { get; set; }
        public string amount { get; set; }
        public string type { get; set; }
    }

    public class ChargeAdjustment
    {
        public string amount { get; set; }
        public string type { get; set; }
        public string name { get; set; }
    }

    public class RideReceiptOutput
    {
        public string distance { get; set; }
        public string normal_fare { get; set; }
        public List<Charge> charges { get; set; }
        public List<ChargeAdjustment> charge_adjustments { get; set; }
        public SurgeCharge surge_charge { get; set; }
        public object total_owed { get; set; }
        public string distance_label { get; set; }
        public string total_charged { get; set; }
        public string request_id { get; set; }
        public string duration { get; set; }
        public string total_fare { get; set; }
        public string subtotal { get; set; }
        public string currency_code { get; set; }
    }
}
