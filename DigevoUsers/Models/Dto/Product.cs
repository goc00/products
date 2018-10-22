using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DigevoUsers.Models.Dto
{
    public class Product {

        public int? idProduct { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string urlOperatorRegister { get; set; }

    }
}