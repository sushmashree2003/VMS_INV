using System;

namespace PreadmissionDTOs.com.vaps.VMS.HRMS
{
    public class HR_Candidate_Documents_VerifiedByDTO
    {
        public long HRCDDV_Id { get; set; }
        public long HRMEDS_Id { get; set; }
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
