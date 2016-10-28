using BLUE.ChocAn.Library.Users;

namespace BLUE.ChocAn.Library.Users.Providers
{
    interface IProvider
    {
        bool ValidateMemberCard(Member member);
        bool ValidateMemberCard(int memberCardNumber);
        void BillChocAn();
        void ViewProviderDictionary();
    }
}
