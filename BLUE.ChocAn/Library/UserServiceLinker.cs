using System;
using System.ComponentModel.DataAnnotations;

namespace BLUE.ChocAn.Library
{
    [TableName("chocan_userservice_linker")]
    public class UserServiceLinker : BaseTable
    {
        public override string ToString()
        {
            return string.Format("Service Code:\t\t{0}\nProvider Number:\t{1}\nMember Number:\t\t{2}\nDate of Service:\t{3}\nService Comments:\t{4}", this.ServiceCode.ToString(), this.ProviderNumber.ToString(), this.MemberNumber.ToString(), this.DateOfService.ToShortDateString(), this.ServiceComments);
        }

        public string ServiceCode { get; set; }

        public int ProviderNumber { get; set; }

        public int MemberNumber { get; set; }

        /// <summary>
        /// The date the service was rendered
        /// </summary>
        public DateTime DateOfService { get; set; }

        /// <summary>
        /// The comments about the service rendered
        /// </summary>
        public string ServiceComments { get; set; }

        /// <summary>
        /// If the service is paid to the provider or not
        /// </summary>
        public bool IsPaid { get; set; }

        /// <summary>
        /// The date created
        /// </summary>
        public DateTime DateCreated { get; set; }
    }
}
