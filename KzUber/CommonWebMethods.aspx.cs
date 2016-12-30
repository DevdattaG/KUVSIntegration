using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;
using System.Collections.Specialized;
using System.Net;
using System.IO;
using UberEngine;
using Newtonsoft.Json;

public partial class CommonWebMethods : System.Web.UI.Page
{    
    protected void Page_Load(object sender, EventArgs e)
    {
       
    }

    [WebMethod]
    public static NearbyRidesTimeOutput getTimeEstimatesForUserLocation(double latitude, double longitude)
    {
        NearbyRidesTimeOutput outputData = new NearbyRidesTimeOutput();
        AuthenticationKeys auth = new AuthenticationKeys();
        try
        {
            string uri = "https://sandbox-api.uber.com/v1/estimates/time?start_latitude=" + latitude + "&start_longitude=" + longitude;
            var webRequest = (HttpWebRequest)WebRequest.Create(uri);
            string authToken = "Token " + auth.uberServerToken;
            webRequest.Headers.Add("Authorization", authToken);
            webRequest.Method = "GET";
            var webResponse = (HttpWebResponse)webRequest.GetResponse();
            if ((webResponse.StatusCode == HttpStatusCode.OK) && (webResponse.ContentLength > 0))
            {
                var reader = new StreamReader(webResponse.GetResponseStream());
                string s = reader.ReadToEnd();
                outputData = JsonConvert.DeserializeObject<NearbyRidesTimeOutput>(s);
                return outputData;
            }
            else
            {
                Console.WriteLine("Error");
                return null;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return null;
        }        
    }

    [WebMethod]
    public static FareEstimateOutput getEstimatesForUserLocation(double latitude, double longitude, double destinationLatitude, double destinationLongitude)
    {
        FareEstimateOutput outputData = new FareEstimateOutput();
        AuthenticationKeys auth = new AuthenticationKeys();
        try
        {
            string uri = "https://api.uber.com/v1.2/estimates/price?start_latitude=" + latitude + "&start_longitude=" + longitude + "&end_latitude=" + destinationLatitude + "&end_longitude=" + destinationLongitude;
            var webRequest = (HttpWebRequest)WebRequest.Create(uri);
            string authToken = "Token " + auth.uberServerToken;
            webRequest.Headers.Add("Authorization", authToken);
            webRequest.Method = "GET";
            var webResponse = (HttpWebResponse)webRequest.GetResponse();
            if ((webResponse.StatusCode == HttpStatusCode.OK) && (webResponse.ContentLength > 0))
            {
                var reader = new StreamReader(webResponse.GetResponseStream());
                string s = reader.ReadToEnd();
                outputData = JsonConvert.DeserializeObject<FareEstimateOutput>(s);
                return outputData;
            }
            else
            {
                Console.WriteLine("Error");
                return null;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return null;
        }
    }

    [WebMethod]
    public static void getUserLoginAuthentication()
    {        
        AuthenticationKeys auth = new AuthenticationKeys();
        
            //string url = "https://login.uber.com/oauth/v2/authorize?client_id=" + auth.uberClientId + "&response_type=code&scope=request+profile";
            //HttpContext.Current.Response.Redirect(url);            
        HttpContext.Current.Response.Redirect("http://www.google.com", false); 
        
    }

    [WebMethod]
    public static int getAuthToken(string authCode)
    {
        AuthenticationKeys auth = new AuthenticationKeys();
        AccessCredentials outputData = new AccessCredentials();
        try
        {
            string uri = "https://login.uber.com/oauth/v2/token?client_secret=" + auth.uberClientSecret + "&client_id=" + auth.uberClientId + "&grant_type=authorization_code&redirect_uri=http://localhost:63685/KzUber/booking.html&code=" + authCode;
            var webRequest = (HttpWebRequest)WebRequest.Create(uri);
            webRequest.Method = "POST";
            var webResponse = (HttpWebResponse)webRequest.GetResponse();
            if ((webResponse.StatusCode == HttpStatusCode.OK) && (webResponse.ContentLength > 0))
            {
                var reader = new StreamReader(webResponse.GetResponseStream());
                string s = reader.ReadToEnd();
                outputData = JsonConvert.DeserializeObject<AccessCredentials>(s);
                AuthenticationKeys.setAccessToken(outputData.access_token);
                return 1;
            }
            else
            {
                Console.WriteLine("Error");
                return -1;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return 0;
        }
    }


    [WebMethod]
    public static ProductInformationOutput getProductInformation(double latitude, double longitude)
    {
        ProductInformationOutput outputData = new ProductInformationOutput();
        AuthenticationKeys auth = new AuthenticationKeys();
        try
        {
            string uri = "https://api.uber.com/v1.2/products?latitude=" + latitude + "&longitude=" + longitude;
            var webRequest = (HttpWebRequest)WebRequest.Create(uri);
            string authToken = "Token " + auth.uberServerToken;
            webRequest.Headers.Add("Authorization", authToken);
            webRequest.Method = "GET";
            var webResponse = (HttpWebResponse)webRequest.GetResponse();
            if ((webResponse.StatusCode == HttpStatusCode.OK) && (webResponse.ContentLength > 0))
            {
                var reader = new StreamReader(webResponse.GetResponseStream());
                string s = reader.ReadToEnd();
                outputData = JsonConvert.DeserializeObject<ProductInformationOutput>(s);
                return outputData;
            }
            else
            {
                Console.WriteLine("Error");
                return null;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return null;
        }
    }

    [WebMethod]
    public static RequestEstimateOutput getRequestEstimate(string productID, double latitude, double longitude, double destLat, double destLong)
    {
        AuthenticationKeys auth = new AuthenticationKeys();
        RequestEstimateOutput outputData = new RequestEstimateOutput();
        try
        {
            string uri = "https://sandbox-api.uber.com/v1.2/requests/estimate";
            var webRequest = (HttpWebRequest)WebRequest.Create(uri);
            string authToken = "Bearer " + AuthenticationKeys.getAccessToken();
            webRequest.Headers.Add("Authorization", authToken);
            webRequest.ContentType = "application/json";
            //webRequest.Headers.Add("Accept-Language", "en_US");
            //webRequest.Headers.Add("Content-Type", "application/json");
            webRequest.Method = "POST";

            using (var streamWriter = new StreamWriter(webRequest.GetRequestStream()))
            {
                string json = "{\"product_id\":\"" + productID + "\",\"start_latitude\":" + latitude + ",\"start_longitude\":" + longitude + ",\"end_latitude\":" + destLat + ",\"end_longitude\":"+destLong+"}";

                streamWriter.Write(json);
                streamWriter.Flush();
            }

            var webResponse = (HttpWebResponse)webRequest.GetResponse();
            if ((webResponse.StatusCode == HttpStatusCode.OK) && (webResponse.ContentLength > 0))
            {
                var reader = new StreamReader(webResponse.GetResponseStream());
                string s = reader.ReadToEnd();
                outputData = JsonConvert.DeserializeObject<RequestEstimateOutput>(s);
                return outputData;
            }
            else
            {
                Console.WriteLine("Error");
                return null;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return null;
        }
    }

    [WebMethod]
    public static BookRideRequestOutput bookRideRequest(string productID, double latitude, double longitude, double destLat, double destLong, string fareId, string surgeConfirmationId)
    {
        AuthenticationKeys auth = new AuthenticationKeys();
        BookRideRequestOutput outputData = new BookRideRequestOutput();
        try
        {
            string uri = "https://sandbox-api.uber.com/v1/requests";
            var webRequest = (HttpWebRequest)WebRequest.Create(uri);
            string authToken = "Bearer " + AuthenticationKeys.getAccessToken();
            webRequest.Headers.Add("Authorization", authToken);
            webRequest.ContentType = "application/json";
            //webRequest.Headers.Add("Accept-Language", "en_US");
            //webRequest.Headers.Add("Content-Type", "application/json");
            webRequest.Method = "POST";

            using (var streamWriter = new StreamWriter(webRequest.GetRequestStream()))
            {
                string json = "";
                if (fareId == "NULL")
                {
                    if (surgeConfirmationId == "NULL")
                    {
                        json = "{\"product_id\":\"" + productID + "\",\"start_latitude\":" + latitude + ",\"start_longitude\":" + longitude + ",\"end_latitude\":" + destLat + ",\"end_longitude\":" + destLong + "}";
                    }
                    else
                    {
                        json = "{\"product_id\":\"" + productID + "\",\"surge_confirmation_id\":\"" + surgeConfirmationId + "\",\"start_latitude\":" + latitude + ",\"start_longitude\":" + longitude + ",\"end_latitude\":" + destLat + ",\"end_longitude\":" + destLong + "}";
                    }
                }
                else
                {
                    json = "{\"product_id\":\"" + productID + "\",\"fare_id\":\"" + fareId + "\",\"start_latitude\":" + latitude + ",\"start_longitude\":" + longitude + ",\"end_latitude\":" + destLat + ",\"end_longitude\":" + destLong + "}";
                }                

                streamWriter.Write(json);
                streamWriter.Flush();
            }

            var webResponse = (HttpWebResponse)webRequest.GetResponse();
            if ((webResponse.StatusCode == HttpStatusCode.Accepted) && (webResponse.ContentLength > 0))
            {
                var reader = new StreamReader(webResponse.GetResponseStream());
                string s = reader.ReadToEnd();
                outputData = JsonConvert.DeserializeObject<BookRideRequestOutput>(s);
                return outputData;
            }
            else
            {
                Console.WriteLine("Error");
                return null;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return null;
        }
    }

    [WebMethod]
    public static RideRequestStatusOutput getRideRequestStatus(string requestID)
    {
        RideRequestStatusOutput outputData = new RideRequestStatusOutput();
        AuthenticationKeys auth = new AuthenticationKeys();
        try
        {
            string uri = "https://sandbox-api.uber.com/v1/requests/"+requestID;
            var webRequest = (HttpWebRequest)WebRequest.Create(uri);
            string authToken = "Bearer " + AuthenticationKeys.getAccessToken();
            webRequest.Headers.Add("Authorization", authToken);
            webRequest.ContentType = "application/json";
            webRequest.Method = "GET";
            var webResponse = (HttpWebResponse)webRequest.GetResponse();
            if ((webResponse.StatusCode == HttpStatusCode.OK) && (webResponse.ContentLength > 0))
            {
                var reader = new StreamReader(webResponse.GetResponseStream());
                string s = reader.ReadToEnd();
                outputData = JsonConvert.DeserializeObject<RideRequestStatusOutput>(s);
                return outputData;
            }
            else
            {
                Console.WriteLine("Error");
                return null;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return null;
        }
    }

    [WebMethod]
    public static int autoAcceptRequest(string requestID)
    {
        AuthenticationKeys auth = new AuthenticationKeys();        
        try
        {
            string uri = "https://sandbox-api.uber.com/v1/sandbox/requests/" + requestID;
            var webRequest = (HttpWebRequest)WebRequest.Create(uri);
            string authToken = "Bearer " + AuthenticationKeys.getAccessToken();
            webRequest.Headers.Add("Authorization", authToken);
            webRequest.ContentType = "application/json";
            webRequest.Method = "PUT";
            using (var streamWriter = new StreamWriter(webRequest.GetRequestStream()))
            {
                string json = "{\"status\":\"accepted\"}";

                streamWriter.Write(json);
                streamWriter.Flush();
            }
            var webResponse = (HttpWebResponse)webRequest.GetResponse();
            return 1;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return 0;
        }
    }

    [WebMethod]
    public static UserRideMapOutput getUserRideMap(string requestID)
    {
        UserRideMapOutput outputData = new UserRideMapOutput();
        AuthenticationKeys auth = new AuthenticationKeys();
        try
        {
            string uri = "https://sandbox-api.uber.com/v1/requests/" + requestID + "/map";
            var webRequest = (HttpWebRequest)WebRequest.Create(uri);
            string authToken = "Bearer " + AuthenticationKeys.getAccessToken();
            webRequest.Headers.Add("Authorization", authToken);
            webRequest.ContentType = "application/json";
            webRequest.Method = "GET";
            var webResponse = (HttpWebResponse)webRequest.GetResponse();
            if ((webResponse.StatusCode == HttpStatusCode.OK) && (webResponse.ContentLength > 0))
            {
                var reader = new StreamReader(webResponse.GetResponseStream());
                string s = reader.ReadToEnd();
                outputData = JsonConvert.DeserializeObject<UserRideMapOutput>(s);
                return outputData;
            }
            else
            {
                Console.WriteLine("Error");
                return null;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return null;
        }
    }

    [WebMethod]
    public static int autoUpdateRequestStatus(string requestID, string updateStatus)
    {
        AuthenticationKeys auth = new AuthenticationKeys();
        try
        {
            string uri = "https://sandbox-api.uber.com/v1/sandbox/requests/" + requestID;
            var webRequest = (HttpWebRequest)WebRequest.Create(uri);
            string authToken = "Bearer " + AuthenticationKeys.getAccessToken();
            webRequest.Headers.Add("Authorization", authToken);
            webRequest.ContentType = "application/json";
            webRequest.Method = "PUT";
            using (var streamWriter = new StreamWriter(webRequest.GetRequestStream()))
            {
                string json = "{\"status\":\"" + updateStatus + "\"}";

                streamWriter.Write(json);
                streamWriter.Flush();
            }
            var webResponse = (HttpWebResponse)webRequest.GetResponse();
            return 1;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return 0;
        }
    }

    [WebMethod]
    public static RideRequestStatusOutput getRideReceiptData(string requestID)
    {
        RideRequestStatusOutput outputData = new RideRequestStatusOutput();
        AuthenticationKeys auth = new AuthenticationKeys();
        try
        {
            string uri = "https://sandbox-api.uber.com/v1/requests/" + requestID;
            var webRequest = (HttpWebRequest)WebRequest.Create(uri);
            string authToken = "Bearer " + AuthenticationKeys.getAccessToken();
            webRequest.Headers.Add("Authorization", authToken);
            webRequest.ContentType = "application/json";
            webRequest.Method = "GET";
            var webResponse = (HttpWebResponse)webRequest.GetResponse();
            if ((webResponse.StatusCode == HttpStatusCode.OK) && (webResponse.ContentLength > 0))
            {
                var reader = new StreamReader(webResponse.GetResponseStream());
                string s = reader.ReadToEnd();
                outputData = JsonConvert.DeserializeObject<RideRequestStatusOutput>(s);
                return outputData;
            }
            else
            {
                Console.WriteLine("Error");
                return null;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return null;
        }
    }

    [WebMethod]
    public static RideReceiptOutput generateRideReceipt(string requestID)
    {
        RideReceiptOutput outputData = new RideReceiptOutput();
        AuthenticationKeys auth = new AuthenticationKeys();
        try
        {
            string uri = "https://sandbox-api.uber.com/v1/requests/" + requestID + "/receipt";
            var webRequest = (HttpWebRequest)WebRequest.Create(uri);
            string authToken = "Bearer " + AuthenticationKeys.getAccessToken();
            webRequest.Headers.Add("Authorization", authToken);
            webRequest.ContentType = "application/json";
            webRequest.Method = "GET";
            var webResponse = (HttpWebResponse)webRequest.GetResponse();
            if ((webResponse.StatusCode == HttpStatusCode.OK) && (webResponse.ContentLength > 0))
            {
                var reader = new StreamReader(webResponse.GetResponseStream());
                string s = reader.ReadToEnd();
                outputData = JsonConvert.DeserializeObject<RideReceiptOutput>(s);
                return outputData;
            }
            else
            {
                Console.WriteLine("Error");
                return null;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return null;
        }
    }

    [WebMethod]
    public static int cancelRideRequest(string requestID)
    {
        AuthenticationKeys auth = new AuthenticationKeys();
        try
        {
            string uri = "https://sandbox-api.uber.com/v1/requests/" + requestID;
            var webRequest = (HttpWebRequest)WebRequest.Create(uri);
            string authToken = "Bearer " + AuthenticationKeys.getAccessToken();
            webRequest.Headers.Add("Authorization", authToken);
            webRequest.ContentType = "application/json";
            webRequest.Method = "DELETE";
            var webResponse = (HttpWebResponse)webRequest.GetResponse();
            return 1;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return 0;
        }
    }
}