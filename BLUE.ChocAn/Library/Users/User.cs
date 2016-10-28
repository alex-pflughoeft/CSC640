using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLUE.ChocAn.Library.Users
{
    public class User
    {
        public virtual string Username { get { return "Guest"; } }

        public override string ToString()
        {
            return "No User Specified";
        }

        public virtual UserRole CurrentRole { get { return UserRole.Guest; } }
    }

    public enum UserRole
    {
        Guest, // Essentially a role for public commands
        Member,
        Provider,
        Operator,
        Manager,
        Super, // The override role
        Debug,
    }
}
