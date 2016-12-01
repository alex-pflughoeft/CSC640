using BLUE.ChocAn.Library.Users;

namespace BLUE.ChocAn.Library.Users.Providers
{
    internal interface IProvider
    {
        bool ValidateMemberCard(Member member);
        bool BillChocAn();
    }
}