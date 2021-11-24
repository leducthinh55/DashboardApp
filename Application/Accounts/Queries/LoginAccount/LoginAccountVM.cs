using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Accounts.Queries.LoginAccount
{
    public class LoginAccountVM
    {
        public string Username { get; set; }

        public string FullName { get; set; }

        public string JwtToken { get; set; }

        public string RefreshToken { get; set; }
    }
}
