using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.HRMS
{
    public class HRMasterLoanDTO:CommonParamDTO
    {
    public long HRMLN_Id { get; set; }
    public long MI_Id { get; set; }
    public string HRML_LoanType { get; set; }
        public decimal? HRML_Max { get; set; }
        public bool HRMLN_ActiveFlag { get; set; }
    public Array gmasterloanList { get; set; }
    public string retrunMsg { get; set; }
    public long roleId { get; set; }
        public long LogInUserId { get; set; }

        public bool? HRML_InterestFreeLoanFlg { get; set; }
        public decimal? HRML_InterestAmount { get; set; }
        public long? HRML_MaxNoOfInstallments { get; set; }
        public bool? HRML_ScholarshipFlg { get; set; }
    }
}
