using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.HRMS
{
    [Table("HR_Emp_FullFinal_Settlement_Approval")]
    public class HR_Emp_FullFinal_SettlementApprovalDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long HREFFSA_Id { get; set; }
        public long HREFFS_Id { get; set; }
        public long IVRMUL_Id { get; set; }
        public string HREFFSA_Remarks { get; set; }
        public DateTime HREFFSA_Date { get; set; }
        public long HREFFSA_SanctionLevel { get; set; }
        public bool HREFFSA_ActiveFlag { get; set; }
        public DateTime HREFFSA_CreatedDate { get; set; }
        public DateTime HREFFSA_UpdatedDate { get; set; }
        public long HREFFSA_CreatedBy { get; set; }
        public long HREFFSA_UpdatedBy { get; set; }
        public string HREFFSA_Status { get; set; }
        public decimal HREFFSA_Amount { get; set; }

    }
}
