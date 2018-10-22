using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DigevoUsers.Models.Request
{

    public class LoginRequest {

        public int? idProduct { get; set; } = null;
        public int? idChannel { get; set; } = null;
        public string value { get; set; } = null; // any value
        public string password { get; set; } = null; // optional

    }
}