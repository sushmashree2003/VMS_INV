using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.IssueManager
{
    public class VMS_Laptop_Application_DTO
    {
        public long VLAPAPPL_Id { get; set; }
        public long MI_Id { get; set; }
        public long HRME_Id { get; set; }
        public string VLAPAPPL_ApplicationID { get; set; }
        public DateTime? VLAPAPPL_ApplicationDate { get; set; }
        public DateTime? VLAPAPPL_FromDate { get; set; }
        public DateTime? VLAPAPPL_ToDate { get; set; }
        public decimal VLAPAPPL_TotalDays { get; set; }
        public string VLAPAPPL_Reason { get; set; }
        public string VLAPAPPL_ApplicationStatus { get; set; }
        public string VLAPAPPL_SupportingDocument { get; set; }
        public bool VLAPAPPL_ActiveFlag { get; set; }
        public DateTime? VLAPAPPL_CreatedDate { get; set; }
        public DateTime? VLAPAPPL_UpdatedDate { get; set; }
        public long VLAPAPPLAUT_Id { get; set; }
        public long IVRMUL_Id { get; set; }
        public long VLAPAPPLAUT_SanctioningLevel { get; set; }
        public String VLAPAPPLAUT_Remarks { get; set; }
        public DateTime? VLAPAPPLAUT_Date { get; set; }
        public bool VLAPAPPLAUT_ActiveFlg { get; set; }
        public long VLAPAPPLAUT_CreatedBy { get; set; }
        public DateTime? VLAPAPPLAUT_CreatedDate { get; set; }
        public long VLAPAPPLAUT_UpdatedBy { get; set; }
        public DateTime? VLAPAPPLAUT_UpdatedDate { get; set; }
        public long User_Id { get; set; }
        public long roleId { get; set; }
        public string msg { get; set; }
        public string HRME_EmployeeFirstName { get; set; }

        public Array alldata { get; set; }
        public Array editlist { get; set; }
        public Array Approvalrequestlist { get; set; }
        public Array applicationApproved { get; set; }
        public VMS_Laptop_Application_DTO[] get_approval_status { get; set; }
        public bool duplicate { get; set; }
    }
}
