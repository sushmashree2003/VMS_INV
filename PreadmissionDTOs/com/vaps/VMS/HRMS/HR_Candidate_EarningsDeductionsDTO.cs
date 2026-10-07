using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.VMS.HRMS
{
    public class HR_Candidate_EarningsDeductionsDTO
    {
        public long HRCED_Id { get; set; }
        public long MI_Id { get; set; }
        public long HRCD_Id { get; set; }
        public long HRMED_Id { get; set; }
        public decimal HRCED_Amount { get; set; }
        public string HRCED_Percentage { get; set; }
        public bool HRCED_ActiveFlag { get; set; }
        public string HRMED_EarnDedFlag { get; set; }
        public string HRMED_Name { get; set; }
        public DateTime? HRCED_CreatedDate { get; set; }
        public DateTime? HRCED_UpdatedDate { get; set; }
        public long? HRCED_CreatedBy { get; set; }
        public long? HRCED_UpdatedBy { get; set; }

    }

}