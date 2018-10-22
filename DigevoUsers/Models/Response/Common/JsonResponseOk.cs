using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DigevoUsers.Models.Response.Common
{

    public class JsonResponseOk {
        public string apiVersion { get; set; } = "1.0";
        public string context { get; set; } = "users";
        public object data { get; set; } = null;
    }

}