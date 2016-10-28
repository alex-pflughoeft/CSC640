using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLUE.ChocAn.Library
{
    public class Service
    {
        #region Private Fields

        private DateTime _dateOfService;
        private string _serviceCode;

        #endregion

        #region Public Properties

        public DateTime DateOfService { get; set; }
        public string ServiceCode { get; set; }
        public string ServiceName { get { return "This Service"; } }

        [StringLength(9, ErrorMessage = "Provider Number must not be greater than 9 digits long.")]
        public string ProviderNumber
        {
            get { return this.ProviderNumber; }
            set
            {
                Validator.ValidateProperty(value, new ValidationContext(this, null, null) { MemberName = "ServiceComments" });
                this.ServiceComments = value;
            }
        }

        [StringLength(9, ErrorMessage = "Member Number must not be greater than 9 digits long.")]
        public string MemberNumber
        {
            get { return this.MemberNumber; }
            set
            {
                Validator.ValidateProperty(value, new ValidationContext(this, null, null) { MemberName = "ServiceComments" });
                this.ServiceComments = value;
            }
        }

        [StringLength(100, ErrorMessage = "Service Comments must not be greater than 100 characters long.")]
        public string ServiceComments 
        {
            get { return this.ServiceComments; }
            set
            {
                Validator.ValidateProperty(value, new ValidationContext(this, null, null) { MemberName = "ServiceComments" });
                this.ServiceComments = value;
            }
        }

        #endregion
    }
}
