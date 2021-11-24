using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Accounts
{
    public abstract class AccountDTO
    {
        public string Password { get; set; }

        public string FullName { get; set; }

        public string Email { get; set; }
    }
}
