using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.com.vapstech.HRMS
{
    [Table("HR_Master_PFVPF_Interest")]
    public class FinancialYearWisePFVPFInterestDMO
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long HRMPFVPFINT_Id { get; set; }
        public decimal HRMPFVPFINT_PFInterestRate { get; set; }
        public decimal HRMPFVPFINT_VPFInterestRate { get; set; }
        public bool HRMPFVPFINT_ActiveFlg { get; set; }
        public long MI_Id { get; set; }
        public long IMFY_Id{ get; set; }
        public DateTime HRMPFVPFINT_CreatedDate{ get; set; }
        public DateTime HRMPFVPFINT_UpdatedDate{ get; set; }
        public long HRMPFVPFINT_CreatedBy{ get; set; }
        public long HRMPFVPFINT_UpdatedBy{ get; set; }
    }
}
