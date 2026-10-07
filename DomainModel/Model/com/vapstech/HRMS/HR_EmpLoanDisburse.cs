using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
namespace DomainModel.Model.com.vapstech.HRMS
{
    [Table("HR_Emp_Loan_Disburse")]
    public class HR_EmpLoanDisburse
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long HRELDIS_Id { get; set; }
        public long HREL_Id { get; set; }
        public long HRME_Id { get; set; }
        public long MI_Id { get; set; }
        public DateTime HRELDIS_DisbursedDate { get; set; }
        public long HRELDIS_Year { get; set; }
        public long HRELDIS_Month { get; set; }
        public decimal HRELDIS_Amount { get; set; }
        public string HRELDIS_PaymentMode { get; set; }
        public string HRELDIS_Remarks { get; set; }
       
        public bool HRELDIS_ActiveFlg { get; set; }
        public DateTime? HRELDIS_CreatedDate { get; set; }
        public long? HRELDIS_CreatedBy { get; set; }
        public long? HRELDIS_UpdatedBy { get; set; }
        public DateTime? HRELDIS_UpdatedDate { get; set; }
    


    }
}
