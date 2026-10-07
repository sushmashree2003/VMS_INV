using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace DomainModel.Model.com.vapstech.HRMS
{
    [Table("HR_Emp_FullFinal_Settlement")]
    public  class HR_Emp_FullFinal_SettlementDMO  
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long HREFFS_Id { get; set; }
        public long MI_Id { get; set; }
        public long HRME_Id { get; set; }
        public DateTime HREFFS_EntryDate { get; set; }
        public int HREFFS_AdvYear { get; set; }
        public string HREFFS_AdvMonth { get; set; }
        public decimal HREFFS_AppliedAmount { get; set; }
        public string HREFFS_Remarks { get; set; }
        public bool? HREFFS_ActiveFlag { get; set; }
        public string HREFFS_AdvStatus { get; set; }
        public decimal HREFFS_SanctinedAmount { get; set; }
        public string HREFFS_ModeOfPayment { get; set; }
        public long HREFFS_ReferenceNo { get; set; }
        public DateTime? HREFFS_CreatedDate { get; set; }
        public DateTime? HREFFS_UpdatedDate { get; set; }
        public long? HREFFS_CreatedBy { get; set; }
        public long? HREFFS_UpdatedBy { get; set; }
    }
}
