using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DigevoUsers.Models.Dto
{

    public class UserIdentify {

        public int? idUserIdentify { get; set; } = null;
        public decimal? id_cliente { get; set; } = null;
        public int? idChannel { get; set; } = null;
        public string value { get; set; }
        public DateTime? creationDate { get; set; } = null;

    }

}