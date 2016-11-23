using System;
using System.ComponentModel.DataAnnotations;

namespace BLUE.ChocAn.Library
{
    [TableName("chocan_userservice_linker")]
    public class UserServiceLinker
    {
        [PrimaryKey]
        [StringLength(11, ErrorMessage = "Service Code must not be greater than 11 digits long.")]
        public int ServiceCode { get; set; }

        [StringLength(11, ErrorMessage = "Provider Number must not be greater than 11 digits long.")]
        public int ProviderNumber { get; set; }

        [StringLength(11, ErrorMessage = "Member Number must not be greater than 11 digits long.")]
        public int MemberNumber { get; set; }

        public DateTime DateOfService { get; set; }

        [StringLength(255, ErrorMessage = "Provider Number must not be greater than 255 characters long.")]
        public string ServiceComments { get; set; }

        public bool IsCharged { get; set; }
    }
}
