using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.HRMS
{
    [Table("HR_Employee_Salary_PaymentDetails")]
    public class HR_Employee_Salary_PaymentDetailsDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long HRESPDET_Id { get; set; }
        public long HRES_Id { get; set; }
        public long MI_Id { get; set; }
        public long HRME_Id { get; set; }
        public string HRESPDET_Year { get; set; }
        public string HRESPDET_Month { get; set; }
        public decimal HRESPDET_Percentage { get; set; }
        public decimal HRESPDET_Amount { get; set; }
        public DateTime? HRESPDET_PaymentDate { get; set; }
        public string HRESPDET_ModeOfPayment { get; set; }
        public string HRESPDET_Remarks { get; set; }
        public bool HRESPDET_ActiveFlg { get; set; }
        public DateTime? HRESPDET_CreatedDate { get; set; }
        public long? HRESPDET_CreatedBy { get; set; }
        public DateTime? HRESPDET_UpdatedDate { get; set; }
        public long? HRESPDET_UpdatedBy { get; set; }
    }
}
