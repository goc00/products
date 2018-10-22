using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DigevoUsers.Models.Dto {

    public class ClienteParam {

        public int? idClienteParam { get; set; } = null;
        public decimal? id_cliente { get; set; } = null;
        public int? idParam { get; set; } = null;
        public string value { get; set; } = null;
        public DateTime? creationDate { get; set; } = null;
        public DateTime? modificationDate { get; set; } = null;

    }

}