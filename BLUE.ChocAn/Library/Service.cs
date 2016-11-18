using System;
using System.ComponentModel.DataAnnotations;

namespace BLUE.ChocAn.Library
{
    [TableName("chocan_service")]
    public class Service : BaseTable
    {
        [PrimaryKey]
        [StringLength(11, ErrorMessage = "Provider Number must not be greater than 11 characters long.")]
        public string ServiceCode { get; set; }

        [StringLength(45, ErrorMessage = "Provider Number must not be greater than 45 characters long.")]
        public string ServiceName { get; set; }
    }
}