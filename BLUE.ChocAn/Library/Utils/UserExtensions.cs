using BLUE.ChocAn.Library.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLUE.ChocAn.Library.Utils
{
    public static class UserExtensions
    {
        public static UserRole GetUserRole(this User user)
        {
            return (UserRole)user.UserRole;
        }
    }
}
