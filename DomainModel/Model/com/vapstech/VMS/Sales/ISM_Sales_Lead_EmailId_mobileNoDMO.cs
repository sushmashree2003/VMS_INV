using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.VMS.Sales
{
    [Table("ISM_Sales_Lead_EmailId_mobileNo")]
    public class ISM_Sales_Lead_EmailId_mobileNoDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long ISMSLEEM_Id { get; set; }
        public string ISMSLEEM_EmailId { get; set; }
        public string ISMSLEEM_MobileNo { get; set; }
        public string ISMSLEEM_DefaultFlag { get; set; }
        public long ISMSLE_Id { get; set; }


    }
}
