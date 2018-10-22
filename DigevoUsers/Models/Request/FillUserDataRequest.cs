using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DigevoUsers.Models.Request
{

    public class FillUserDataRequest
    {

        public decimal? idClient { get; set; } = null;
        public string[] tags { get; set; } = null;
        public string[] values { get; set; } = null;


    }
}