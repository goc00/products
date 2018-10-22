using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DigevoUsers.Models.Request {

    public class CustodyRequest {

        public int? idProduct { get; set; } = null;
        public string value { get; set; } = null;
        public int? idChannel { get; set; } = null;
        public string valChannel { get; set; } = null;
        public string url { get; set; } = null;
        public bool sendSms { get; set; } = false;

        public string code { get; set; }
        public string userName { get; set; }
        public string password { get; set; }
        public string ani { get; set; }

    }
}