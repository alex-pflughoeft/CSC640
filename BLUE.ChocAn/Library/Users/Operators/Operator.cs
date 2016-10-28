using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLUE.ChocAn.Library.Users.Operators
{
    public class Operator : User, IOperator
    {
        public Operator()
        {
            // Default Constructor
        }

        public override string Username { get { return "Operator"; } }
        public override UserRole CurrentRole { get { return UserRole.Operator; } }

        public bool AddMember()
        {
            string memberName;
            string memberNumber;
            string memberStreetAddress;
            string memberCity;
            string memberState;
            string memberZip;

            Console.WriteLine("Enter the Member Name:");
            memberName = Console.ReadLine();
            // TODO: Validate name

            Console.WriteLine("Enter the Member Number:");
            memberNumber = Console.ReadLine();
            // TODO: Validate number

            Console.WriteLine("Enter the Member Street Address:");
            memberStreetAddress = Console.ReadLine();
            // TODO: Validate

            Console.WriteLine("Enter the Member City:");
            memberCity = Console.ReadLine();
            // TODO: Validate

            Console.WriteLine("Enter the Member State:");
            memberState = Console.ReadLine();
            // TODO: Validate

            Console.WriteLine("Enter the Member Zip:");
            memberZip = Console.ReadLine();
            // TODO: Validate

            // Create the new member
            Member thisMember = new Member();
            thisMember.MemberName = memberName;
            thisMember.MemberNumber = Convert.ToInt32(memberNumber);
            thisMember.State = memberState;
            thisMember.City = memberCity;
            thisMember.ZipCode = memberZip;

            // TODO: Push this member to the database
            Console.WriteLine("Member \'{0}\' successfully added.\n", memberName);

            return true;
        }

        public bool DeleteMember(Member member)
        {
            throw new NotImplementedException();
        }

        public bool DeleteMember(int memberNumber)
        {
            throw new NotImplementedException();
        }

        public bool UpdateMember(Member member)
        {
            throw new NotImplementedException();
        }

        public bool UpdateMember(int memberNumber)
        {
            throw new NotImplementedException();
        }

        public bool AddProvider()
        {
            throw new NotImplementedException();
        }

        public bool DeleteProvider(Providers.Provider provider)
        {
            throw new NotImplementedException();
        }

        public bool DeleteProvider(int providerNumber)
        {
            throw new NotImplementedException();
        }

        public bool UpdateProvider(Member provider)
        {
            throw new NotImplementedException();
        }

        public bool UpdateProvider(int providerNumber)
        {
            throw new NotImplementedException();
        }
    }
}
