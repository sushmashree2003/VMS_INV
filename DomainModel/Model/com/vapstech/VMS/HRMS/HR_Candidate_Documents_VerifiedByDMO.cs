using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.com.vapstech.VMS.HRMS
{
    [Table("HR_Candidate_Documents_VerifiedBy")]
    public class HR_Candidate_Documents_VerifiedByDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long HRCDDV_Id { get; set; } 
        public long HRCD_Id { get; set; } 
        public long HRME_Id { get; set; } 
        public string HRCDDV_Remarks { get; set; } 
        public string HRCDDV_RejectedFlg { get; set; } 
        public DateTime HRCDDV_DateTime { get; set; } 
        public DateTime HRCDDV_CreatedDate { get; set; }
        public DateTime HRCDDV_UpdatedDate { get; set; } 
        public long HRCDDV_CreatedBy { get; set; } 
        public long HRCDDV_UpdatedBy { get; set; } 
    }
}



