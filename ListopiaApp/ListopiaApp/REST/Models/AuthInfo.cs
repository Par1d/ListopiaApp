using System;
using System.Collections.Generic;
using System.Text;

namespace ListopiaApp.REST.Models
{
    public class AuthInfo
    {
        public string access_token { get; set; }
        public string token_type { get; set; }
        public int expires_in { get; set; }
        public string username { get; set; }
        public DateTime issued { get; set; }
        public DateTime expires { get; set; }
    }
}
