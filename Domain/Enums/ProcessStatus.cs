using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Enums
{
    public enum ProcessStatus
    {
        Success = 1,
        Fail = 2,
        Duplicated = 3,
        NotVerification = 4,
        NotFound = 5,
        NotPermission = 6
    }
}
