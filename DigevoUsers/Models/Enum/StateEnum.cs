using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DigevoUsers.Models.Enum {
    public enum StateEnum {
        ACTIVE                  = 1,
        INACTIVE                = 2,
        TEMPORAL_PASSWORD       = 3,
        TEMPORAL_PASSWORD_USED  = 4
    }
}