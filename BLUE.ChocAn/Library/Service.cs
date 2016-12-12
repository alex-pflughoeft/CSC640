using System;
using System.ComponentModel.DataAnnotations;

namespace BLUE.ChocAn.Library
{
    [TableName("chocan_service")]
    public class Service : BaseTable
    {
        public override string ToString()
        {
            return string.Format("Service Name:\t{0}\nService Code:\t{1}\nService Fee:\t{2}", this.ServiceName, this.ServiceCode, this.ServiceFee.ToString("C"));
        }

        [PrimaryKey]
        public int ServiceId { get; set; }

        public string ServiceCode { get; set; }

        public string ServiceName { get; set; }

        public double ServiceFee { get; set; }
    }
}