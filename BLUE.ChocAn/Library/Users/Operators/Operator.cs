using BLUE.ChocAn.Library.Database;
using BLUE.ChocAn.Library.Users.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLUE.ChocAn.Library.Users.Operators
{
    public class Operator : User
    {
        #region Public Properties

        public override string Username { get { return "operator"; } }
        public override UserRole CurrentRole { get { return UserRole.Operator; } }

        #endregion

        #region Constructors

        public Operator()
        {
            // Default Constructor
        }

        #endregion
    }
}