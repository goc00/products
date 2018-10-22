using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DigevoUsers.Models.Response {

    public class CreateCustodyResponse
    {
        public int? idClient { get; set; }
        public int? idCustody { get; set; }
        public string alias { get; set; }
        public string code { get; set; }
        public string url { get; set; }
  
    }

}