using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.FrontOffice
{
    public class IEMonthEndSubmissionAndApprovalReportDTO
    {
        public long MI_Id { get; set; }
        public long roleid { get; set; }
        public long Userid { get; set; }
        public Array fillyear { get; set; }
        public Array fillmonth { get; set; }
        public Array intistutename { get; set; }
        public Array GetGriddata { get; set; }
        public Array MultidocView { get; set; }
        public Array geteditdetils { get; set; }
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
        public string ISMCLMERFL_ApproverRemarks { get; set; }
        public long ISMMCLT_Id { get; set; }
        public long HRME_Id { get; set; }
        public bool ISMCLMER_ActiveFlag { get; set; }
        public DateTime? ISMCLMERFL_ApprovedDate { get; set; }
        public bool ISMCLMERFL_ActiveFlag { get; set; }
        public string HRME_EmployeeFirstName { get; set; }
        public string APPROVER_Name { get; set; }
        public long yearid { get; set; }
        public string rdbbutton { get; set; }
        public Array ielist { get; set; }
        public Array getApprovalDetails { get; set; }
        public filedto[] FilePath_Array { get; set; }
        public approvedto[] getReport_status { get; set; }
        public monthdto[] MONTHID { get; set; }
        public clientdto[] CLIENTID {get;set;}
        public Array iemonthendReport { get; set; }
        public class filedto
        {
            public string ISMCLMERFL_FilePath { get; set; }
            public string ISMCLMERFL_FileName { get; set; }
            public string ISMCLMERFL_FileDesc { get; set; }
        }

        public class approvedto
        {
            public long ISMCLMERFL_Id { get; set; }
            public string ISMCLMERFL_ApproverRemarks { get; set; }
        }

        public class monthdto
        {
            public long IVRM_Month_Id { get; set; }
        }
        public class clientdto
        {
            public long ISMMCLT_Id { get; set; }
        }
    }
}
