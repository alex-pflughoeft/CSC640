using System;
using System.ComponentModel.DataAnnotations;

namespace BLUE.ChocAn.Library
{
    [TableName("chocan_service")]
    public class Service : BaseTable
    {
        public override string ToString()
        {
            return string.Format("Service Code:\t{0}\nService Name:\t\t{1}", this.ServiceCode, this.ServiceName);
        }

        [PrimaryKey]
        public int ServiceId { get; set; }

        [StringLength(11, ErrorMessage = "Provider Number must not be greater than 11 characters long.")]
        public string ServiceCode { get; set; }

        [StringLength(45, ErrorMessage = "Provider Number must not be greater than 45 characters long.")]
        public string ServiceName { get; set; }
    }
}