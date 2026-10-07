using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.FrontOffice
{
    [Table("VMS_DriverOt_Approval")]
    public class VMSDriverOTApprovalDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long VSMDTA_Id { get; set; }
        public long VMSDT_Id { get; set; }
        public string VSMDTA_Status { get; set; }
        public string VSMDTA_Remarks { get; set; }
        public long VSMDTA_ApprovedBy { get; set; }
        public DateTime? VSMDTA_ApprovedDate { get; set; }
        public long CreatedBy { get; set; }
        public long UpdatedBy { get; set; }
    }
}     
     
















       
