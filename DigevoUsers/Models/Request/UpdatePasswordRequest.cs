using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DigevoUsers.Models.Request
{

    public class UpdatePasswordRequest
    {

        public decimal? idClient { get; set; } = null;
        public int? idProduct { get; set; } = null;
        public int? idChannel { get; set; } = null;
        public string oldPassword { get; set; } = null;
        public string newPassword { get; set; } = null;
        public string newPasswordRe { get; set; } = null;

    }
}