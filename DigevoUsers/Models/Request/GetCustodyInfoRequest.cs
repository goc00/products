using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DigevoUsers.Models.Request {

    public class GetCustodyInfoRequest
    {

        public string code { get; set; } = null;
        public int? idProduct { get; set; } = null;
        public int? restrictive { get; set; } = null;
        // Optional
        public int? idCustody { get; set; } = null;
        public string alias { get; set; } = null;

    }
}