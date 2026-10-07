using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.HRMS
{
   public class EmpLoanPayoutDTO
    {
       
        public long MI_Id { get; set; }
        public Array gmasterloanList { get; set; }
        public Array Employeelist { get; set; }
        public Array loandetails { get; set; }
        public Array modeOfPaymentdropdown { get; set; }
        public Array editdata { get; set; }
        public string retrunMsg { get; set; }
        public long roleId { get; set; }
        public long LogInUserId { get; set; }
        public long HRELDIS_Id { get; set; }
        public long HREL_Id { get; set; }
        public long HRME_Id { get; set; }
        public DateTime HRELDIS_DisbursedDate { get; set; }
        public long HRELT_Year { get; set; }
        public long HRELT_Month { get; set; }
        public decimal HRELDIS_Amount { get; set; }
        public string HRELDIS_PaymentMode { get; set; }
        public string HRELDIS_Remarks { get; set; }
        public bool HRELDIS_ActiveFlg { get; set; }
        public DateTime? HRELDIS_CreatedDate { get; set; }
        public long? HRELDIS_CreatedBy { get; set; }
        public long? HRELDIS_UpdatedBy { get; set; }
        public DateTime? HRELDIS_UpdatedDate { get; set; }
        public string HRME_EmployeeFirstName { get; set; }
        public string HRML_LoanType { get; set; }
        public string HREL_EmpLoanId { get; set; }
        public decimal? HREL_LoanAmount { get; set; }
        public decimal? HREL_SanctionedAmount { get; set; }

    }
}
