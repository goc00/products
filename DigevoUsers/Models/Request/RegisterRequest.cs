﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DigevoUsers.Models.Request
{

    public class RegisterRequest {

        public int? idProduct { get; set; } = null;
        public int? idChannel { get; set; } = null;
        public int? idGuide { get; set; } = null;
        public string value { get; set; } = null; // any value
        public string password { get; set; } = null;    // optional
        public string code { get; set; } = null;        // optional, generated by custody process
        public string codeType { get; set; } = null;    // optional (if code is setted, need codeType)

        // Platform
        public string ani { get; set; } = null;
        public string email { get; set; } = null;
        public string usuario { get; set; } = null;
        //public string pass { get; set; } = null; // resolved with password attribute
        public decimal? id_operador { get; set; } = null;

    }
}