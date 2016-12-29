using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UberEngine
{

    public class Destination
    {
        public double latitude { get; set; }
        public double longitude { get; set; }
    }

    public class Pickup
    {
        public double latitude { get; set; }
        public double longitude { get; set; }
    }

    public class BookRideRequestOutput
    {
        public string status { get; set; }
        public string product_id { get; set; }
        public Destination destination { get; set; }
        public object driver { get; set; }
        public Pickup pickup { get; set; }
        public string request_id { get; set; }
        public object eta { get; set; }
        public object location { get; set; }
        public object vehicle { get; set; }
        public double surge_multiplier { get; set; }
        public bool shared { get; set; }
    }    
}
