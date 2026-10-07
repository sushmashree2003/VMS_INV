using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.Inventory
{
    public class StatutoryTaxComplianceDTO
    {
        public long MI_Id { get; set; }
        public long UserId { get; set; }
        public long ASMAY_Id { get; set; }
        public long VMSSC_Id { get; set; }

        public long IMFY_Id { get; set; }
        public string VMSSC_Flag { get; set; }
        public string VMSSC_ReferenceNo { get; set; }
        public string VMSSC_Remarks { get; set; }
        public DateTime VMSSC_FromDate { get; set; }
        public DateTime VMSSC_ToDate { get; set; }
        public string VMSSC_OtherRemarks { get; set; }
        public bool VMSSC_CaseFlag { get; set; }
        public string VMSSC_CaseDtails { get; set; }
        public DateTime VMSSC_CreatedDate { get; set; }
        public DateTime VMSSC_UpdatedDate { get; set; }
        public long VMSSC_CreatedBy { get; set; }
        public long VMSSC_UpdatedBy { get; set; }
        public long VMSSCD_Id { get; set; }
        public string VMSSCD_Filename { get; set; }
        public string VMSSCD_Filepath { get; set; }
        public string VMSSCD_Filenametwo { get; set; }
        public string VMSSCD_Filepathtwo { get; set; }
        public string VMSSCD_Filenamethree { get; set; }
        public string VMSSCD_Filepaththree { get; set; }
        public string VMSSCD_Filepathfour { get; set; }
        public string VMSSCD_Filenamefour { get; set; }
        public string VMSSCD_Filepathfive { get; set; }
        public string VMSSCD_Filenamefive { get; set; }
        public string VMSSCD_Filepathsix { get; set; }
        public string VMSSCD_Filenamesix { get; set; }
        public string VMSSCD_Filepathseven { get; set; }
        public string VMSSCD_Filenameseven { get; set; }

        public bool VMSSCD_ActiveFlag { get; set; }
        public DateTime VMSSCD_CreatedDate { get; set; }
        public DateTime VMSSCD_UpdatedDate { get; set; }
        public long VMSSCD_CreatedBy { get; set; }
        public long VMSSCD_UpdatedBy { get; set; }
        public Array AllCompany { get; set; }
        public Array Year { get; set; }
        public Array ReferenceNo { get; set; }
        public Array viewData { get; set; }
        public Array Flagdata { get; set; }
        public string message { get; set; }
        public Array getviewfilesdetails { get; set; }
        public Array geteditdetails { get; set; }
        public Array geteditfilesdetails { get; set; }
        public Array geteditfilesdetailstwo { get; set; }
        public Array geteditfilesdetailsthree { get; set; }
        public Array geteditfilesdetailsfour { get; set; }
        public Array geteditfilesdetailsfive { get; set; }
        public Array geteditfilesdetailssix { get; set; }
        public Array geteditfilesdetailsseven { get; set; }
        public Array GetReminderdetails1 { get; set; }
        public Array GetReminderdetails2 { get; set; }
        public Array GetReminderdetails3 { get; set; }
        public Array GetReminderdetails4 { get; set; }

        public string VMSSC_Status { get; set; }
        public long VMSSCT_Id { get; set; }
        public string VMSSCT_consultantName { get; set; }
        public string VMSSCT_Address { get; set; }
        public string VMSSCT_Remarks { get; set; }
        public bool VMSSCT_ActiveFlag { get; set; }
        public DateTime VMSSCT_CreatedDate { get; set; }
        public DateTime VMSSCT_UpdatedDate { get; set; }
        public long VMSSCT_CreatedBy { get; set; }
        public long VMSSCT_UpdatedBy { get; set; }
        public bool VMSSCT_Flag { get; set; }
        public Array Consultant_Report { get; set; }
        public Array Income_CaseList { get; set; }
        public Array Income_ClosedCaseList { get; set; }
        public Array Income_CaseListtwo { get; set; }
        public Array Income_CaseListthree { get; set; }
        public Array Income_CaseListfour { get; set; }
        public Array Income_CaseListfive { get; set; }
        public Array Income_CaseListsix { get; set; }
        public Array Income_CaseListseven { get; set; }
        public Array Income_ClosedCaseListtwo { get; set; }
        public Array Income_ClosedCaseListthree { get; set; }
        public Array Income_ClosedCaseListfour { get; set; }
        public Array Income_ClosedCaseListfive { get; set; }
        public Array Income_ClosedCaseListsix { get; set; }
        public Array Income_ClosedCaseListseven { get; set; }


        public FILESLIST[] FILESLISTdata { get; set; }
        public class FILESLIST
        {
            public string VMSSCD_Filepath { get; set; }
            public string VMSSCD_Filename { get; set; }
        }
    }
}

