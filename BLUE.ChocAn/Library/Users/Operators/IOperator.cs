using BLUE.ChocAn.Library.Users;
using BLUE.ChocAn.Library.Users.Providers;

namespace BLUE.ChocAn.Library.Users.Operators
{
    interface IOperator
    {
        bool AddMember(Member member);
        bool DeleteMember(Member member);
        bool DeleteMember(int memberNumber);
        bool UpdateMember(Member member);
        bool UpdateMember(int memberNumber);
        bool AddProvider(Provider provider);
        bool DeleteProvider(Provider provider);
        bool DeleteProvider(int providerNumber);
        bool UpdateProvider(Provider provider);
        bool UpdateProvider(int providerNumber);
    }
}
