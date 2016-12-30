using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UberEngine
{

    public class RideDestination
    {
        public double latitude { get; set; }
        public int eta { get; set; }
        public double longitude { get; set; }
    }

    public class Driver
    {
        public string phone_number { get; set; }
        public double rating { get; set; }
        public string picture_url { get; set; }
        public string name { get; set; }
        public object sms_number { get; set; }
    }

    public class RidePickup
    {
        public double latitude { get; set; }
        public string eta { get; set; }
        public double longitude { get; set; }
    }

    public class Location
    {
        public double latitude { get; set; }
        public int bearing { get; set; }
        public double longitude { get; set; }
    }

    public class Vehicle
    {
        public string make { get; set; }
        public string picture_url { get; set; }
        public string model { get; set; }
        public string license_plate { get; set; }
    }

    public class RideRequestStatusOutput
    {
        public string status { get; set; }
        public string product_id { get; set; }
        public Destination destination { get; set; }
        public Driver driver { get; set; }
        public Pickup pickup { get; set; }
        public string request_id { get; set; }
        public string eta { get; set; }
        public Location location { get; set; }
        public Vehicle vehicle { get; set; }
        public double surge_multiplier { get; set; }
        public bool shared { get; set; }
    }   
}
