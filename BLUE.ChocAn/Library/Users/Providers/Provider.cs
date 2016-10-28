using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLUE.ChocAn.Library.Users.Providers
{
    public class Provider : User, IProvider
    {
        #region Public Properties

        public override string Username { get { return "Provider"; } }
        public override UserRole CurrentRole { get { return UserRole.Provider; } }

        #endregion

        #region Constructors

        public Provider()
        {
            // Default Constructor
        }

        #endregion

        #region Public Methods

        public bool ValidateMemberCard(Member member)
        {
            member.ActivateCard();

            return true;
        }

        public bool ValidateMemberCard(int memberCardNumber)
        {
            // TODO: Get member corresponding to that card number
            Member member = new Member();

            member.ActivateCard();

            Console.WriteLine("Card Number \'{0}\' has been validated for member \'{1}\'.\n", member.CardNumber.ToString(), member.UserName);

            return true;
        }

        public void EnterService(int memberNumber)
        {
            // TODO: Enter a service here
        }

        public void ViewPendingCharges()
        {
            // TODO: Return the pending charges that have yet to be billed to ChocAn
        }

        public void BillChocAn()
        {
            // TODO: Bill ChocAn
        }

        public void ViewProviderDictionary()
        {
            // TODO: Vied the provider dictionary
        }

        #endregion
    }
}
