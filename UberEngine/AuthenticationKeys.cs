using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UberEngine
{
    public class AuthenticationKeys
    {
        public string uberClientId { get; set; }
        public string uberServerToken { get; set; }
        public string uberClientSecret { get; set; }
        public static string uberAccessToken { get; set; }

        public AuthenticationKeys()
        {
            this.uberClientId = "3ihOozZlsQwhat85XL3TH_MPaA5prGFu";
            this.uberServerToken = "EkJlT8Nd5Q3HjYsSH-k3t7drGfY9yVWf8XbbkDq0";
            this.uberClientSecret = "fUTn8GU5wdYuPPHOcUobNHEKuYfsFzVn1cDrXMuZ";
        }

        public static string getAccessToken()
        {
            return uberAccessToken;
        }

        public static void setAccessToken(string token)
        {
            uberAccessToken = token;
        }
    }
}
