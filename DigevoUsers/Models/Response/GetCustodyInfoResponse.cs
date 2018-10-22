using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DigevoUsers.Models.Response {

    public class GetCustodyInfoResponse
    {

        public int idCustody { get; set; }
        public int idProduct { get; set; }
        public int idClient { get; set; }
        public string value { get; set; }
        public string alias { get; set; }
        public string code { get; set; }
        public bool active { get; set; }
        public DateTime expirationDate { get; set; }
        public DateTime creationDate { get; set; }

    }

}