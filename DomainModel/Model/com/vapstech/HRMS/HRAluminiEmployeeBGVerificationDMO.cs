using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.HRMS
{
    [Table("HR_Master_Employee_Verification")]
    public class HRAluminiEmployeeBGVerificationDMO 
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long HRMEVER_Id { get; set; }
        public long HRME_Id { get; set; }
        public DateTime? HRMEVER_GeneratedDate { get; set; }
        public string HRMEVER_VerificationCode { get; set; }
        public DateTime? HRMEVER_ExpiryDate { get; set; }
        public string HRMEMNO_RequestedBy { get; set; }
        public string HRMEVER_RequestedCompanyName { get; set; }
        public string HRMEVER_EmailId { get; set; }
        public long HRMEVER_ContactNo { get; set; }
        public bool HRMEVER_ActiveFlg { get; set; }
        public DateTime HRMEVER_CreatedDate { get; set; }
        public DateTime HRMEVER_UpdatedDate { get; set; }
        public long HRMEVER_CreatedBy { get; set; }
        public long HRMEVER_UpdatedBy { get; set; }

    }
}
