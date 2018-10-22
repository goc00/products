using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DigevoUsers.Models.Dto {

    public class Param {

        public int? idParam { get; set; } = null;
        public string name { get; set; } = null;
        public string description { get; set; } = null;
        public string tag { get; set; } = null;
        public DateTime? creationDate { get; set; } = null;
        public DateTime? modificationDate { get; set; } = null;

    }

}