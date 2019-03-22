using System;
using System.Collections.Generic;
using System.Text;

namespace ListopiaApp.REST.Models
{
    class Login
    {
        public string grant_type { get { return "password"; } }
        public string username { get; set; }
        public string password { get; set; }
    }
}
