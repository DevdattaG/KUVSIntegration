using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UberEngine
{
    [Serializable]
    public class AccessCredentials
    {
        public double last_authenticated { get; set; }
        public string access_token { get; set; }
        public double expires_in { get; set; }
        public string token_type { get; set; }
        public string scope { get; set; }
        public string refresh_token { get; set; }

        public AccessCredentials(double lastAuth, string token, double expiresIn, string type, string scope, string refresh)
        {
            this.last_authenticated = lastAuth;
            this.access_token = token;
            this.expires_in = expiresIn;
            this.token_type = type;
            this.scope = scope;
            this.refresh_token = refresh;
        }
        public AccessCredentials()
        {
        }

        public string getAccessToken()
        {
            return this.access_token;
        }

        public string getRefreshToken()
        {
            return this.refresh_token;
        }

        public double getExpiryTime()
        {
            return this.expires_in;
        }
    }

    public class MapAccessCredentials
    {
        public double last_authenticated { get; set; }
        public string access_token { get; set; }
        public double expires_in { get; set; }
        public string token_type { get; set; }
        public string scope { get; set; }
        public string refresh_token { get; set; }
    }

}
