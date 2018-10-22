using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DigevoUsers.Models.Dto
{

    public class Credential {

        public int? idCredential { get; set; } = null;
        public int? idProduct { get; set; } = null;
        public int? idUserIdentify { get; set; } = null;
        public int? idState { get; set; } = null;      
        public string password { get; set; }
        public DateTime? creationDate { get; set; } = null;
        public DateTime? modificationDate { get; set; } = null;

    }

}