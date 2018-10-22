using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DigevoUsers.Controllers.Exceptions {
	public class NotValidDataException : Exception {

        public NotValidDataException():base() { }
        public NotValidDataException(string message): base(message) { }

    }
}