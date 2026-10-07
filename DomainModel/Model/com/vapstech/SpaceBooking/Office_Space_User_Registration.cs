using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.SpaceBooking
{
    [Table("OS_User_Registration")]
    public class Office_Space_User_Registration : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long OSUREG_ID { get; set; }
        public long MI_ID  { get; set; }
        public string OSUREG_UserName { get; set; }
        public string OSUREG_EmailId { get; set; }
        public long OSUREG_MobileNo { get; set; }
        public string OSUREG_Password { get; set; }
        public bool OSUREG_ActiveFlag  { get; set; }
        public string OSUREG_BillingName { get; set; }
        public string OSUREG_BillingAdd1 { get; set; }
        public string OSUREG_BillingAdd2 { get; set; }
        public string OSUREG_BillingAdd3 { get; set; }
        public string OSUREG_BillingAdd4 { get; set; }
        public string OSUREG_BillingCity { get; set; }
        public long IVRMMC_ID { get; set; }
        public long IVRMMS_ID { get; set; }
        public long OSUREG_BillingPinCode { get; set; }        
        public string OSUREG_GSTNo { get; set; }

    }
}
