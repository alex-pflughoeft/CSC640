using System;
using System.ComponentModel.DataAnnotations;

namespace BLUE.ChocAn.Library
{
    [TableName("chocan_userservice_linker")]
    public class UserServiceLinker : BaseTable
    {
        public override string ToString()
        {
            return string.Format("Service Code:\t{0}\nProvider Number:\t\t{1}\nMember Number:\t{2}\nDate of Service:\t\t{3}\nService Comments:\t\t{4}", this.ServiceCode.ToString(), this.ProviderNumber.ToString(), this.MemberNumber.ToString(), this.DateOfService.Value.ToShortDateString(), this.ServiceComments);
        }

        [PrimaryKey]
        [StringLength(11, ErrorMessage = "Service Code must not be greater than 11 digits long.")]
        public string ServiceCode { get; set; }

        [StringLength(11, ErrorMessage = "Provider Number must not be greater than 11 digits long.")]
        public int ProviderNumber { get; set; }

        [StringLength(11, ErrorMessage = "Member Number must not be greater than 11 digits long.")]
        public int MemberNumber { get; set; }

        public DateTime? DateOfService { get; set; }

        [StringLength(255, ErrorMessage = "Provider Number must not be greater than 255 characters long.")]
        public string ServiceComments { get; set; }

        public bool IsCharged { get; set; }

        public DateTime? PaymentDueDate { get; set; }

        public DateTime? DatePaid { get; set; }

        public DateTime? DateCreated { get; set; }
    }
}
