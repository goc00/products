using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DigevoUsers.Models.Response.Common {

    public class JsonResponseError {
        public string apiVersion { get; set; } = "1.0";
        public string context { get; set; } = "users";
        public JsonResponseErrorParams error { get; set; } = null;
    }

}