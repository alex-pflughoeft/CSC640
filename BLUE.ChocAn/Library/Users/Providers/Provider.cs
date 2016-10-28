using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLUE.ChocAn.Library.Users.Providers
{
    public class Provider : User, IProvider
    {
        public Provider()
        {
            // Default Constructor
        }

        public override string Username { get { return "Provider"; } }
        public override UserRole CurrentRole { get { return UserRole.Provider; } }

        public bool ValidateMemberCard(Member member)
        {
            // TODO: Validate the card here.
            Console.WriteLine("Card Number \'{0}\' has been validated.\n", member.MemberNumber.ToString());

            return true;
        }

        public bool ValidateMemberCard(int memberCardNumber)
        {
            // TODO: Validate the card here.
            Console.WriteLine("Card Number \'{0}\' has been validated.\n", memberCardNumber.ToString());

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
    }
}
