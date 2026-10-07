using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.FrontOffice
{
    public class IEMontEndReportForReportSubmissionDTO
    {
        public long MI_Id { get; set; }
        public long roleid { get; set; }
        public long Userid { get; set; }
        public Array Year { get; set; }
        public Array Month { get; set; }
        public Array intistutename { get; set; }
        public Array GetGriddata { get; set; }
        public Array MultidocView { get; set; }
        public Array geteditdetils { get; set; }
        public Array empdetils { get; set; }
        public string message { get; set; }
        public long IVRM_Month_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long ISMCLMERFL_Id { get; set; }
        public long ISMCLMER_Id { get; set; }
        public string ISMCLMERFL_FileName { get; set; }
        public string ISMCLMERFL_FileDesc { get; set; }
        public string ISMCLMERFL_FilePath { get; set; }
        public bool ISMCLMERFL_ApprovedFlg { get; set; }
        public long ISMCLMERFL_ApprovedBy_HRME_Id { get; set; }
        public long ISMCLMERFL_ApproverRemarks { get; set; }
        public long ISMMCLT_Id { get; set; }
        public long HRME_Id { get; set; }
        public bool ISMCLMER_ActiveFlag { get; set; }
        public string HRME_EmployeeFirstName { get; set; }
        public long IMMM_ID { get; set; }
        public DateTime ISMCLMERFL_ApprovedDate { get; set; }
        public bool ISMCLMERFL_ActiveFlag { get; set; }
        public Array getApprovalDetails { get; set; }
        public filedto[] FilePath_Array { get; set; }
        public approvedto[] getReport_status { get; set; }
        public Array Master_MER_Modules { get; set; }
        
        public class filedto
        {
            public string ISMCLMERFL_FilePath { get; set; }
            public string ISMCLMERFL_FileName { get; set; }
            public string ISMCLMERFL_FileDesc { get; set; }
            public long IMMM_ID { get; set; }
        }

        public class approvedto
        {
            public long ISMCLMERFL_Id { get; set; }
            public string ISMCLMERFL_ApproverRemarks { get; set; }
        }


    }
}
