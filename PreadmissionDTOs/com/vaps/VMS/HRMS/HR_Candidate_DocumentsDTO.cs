using System;

namespace PreadmissionDTOs.com.vaps.VMS.HRMS

{
    public class HR_Candidate_DocumentsDTO : CommonParamDTO
    {
        public readonly long UserId;

        public long HRCDD_Id { get; set; } 
        public long HRCD_Id { get; set; } 
        public string HRCDD_DocumentPath { get; set; } 
        public DateTime HRCDD_CreatedDate { get; set; } 
        public DateTime HRCDD_UpdatedDate { get; set; } 
        public long HRCDD_CreatedBy { get; set; } 
        public long HRCDD_UpdatedBy { get; set; }
        public string ReturnMsg { get; set; }
        public string ismclT_FilePath { get; set; }
        public string HRCDD_DocumentName { get; set; }

    }
}
