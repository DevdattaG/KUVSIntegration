using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UberEngine
{
    public class AccessCredentials
    {
        public double last_authenticated { get; set; }
        public string access_token { get; set; }
        public double expires_in { get; set; }
        public string token_type { get; set; }
        public string scope { get; set; }
        public string refresh_token { get; set; }
    }

}
