using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.FrontOffice
{
    public class FO_Emp_PunchDTO
    {
        public long FOEP_Id { get; set; }
        public long MI_Id { get; set; }
        public long HRME_Id { get; set; }
        public long roleId { get; set; }
        public string emailId { get; set; }
        public string HRME_AppDownloadedDeviceId { get; set; }
        public long MobileNo { get; set; }

        public DateTime? FOEP_PunchDate { get; set; }
        public bool FOEP_HolidayPunchFlg { get; set; }
        public bool FOEP_Flag { get; set; }
        public string FOEPD_PunchTime { get; set; }
        public string InOutFlg { get; set; }
        public string HRME_BiometricCode { get; set; }
        public string FOEPD_Temperature { get; set; }
        public string HRME_RFCardId { get; set; }
        public string Status { get; set; }
        public string username { get; set; }
        public bool returnval { get; set; }
        public long FOEPD_Id { get; set; }
        public string FOEPD_InOutFlg { get; set; }
        public FO_Emp_PunchDTO[] temp1 { get; set; }
        public string json { get; set; }
        public string EmployeeName { get; set; }
        public string InTime { get; set; }
        public string LateBy { get; set; }
        public string EntryTime { get; set; }
        public string OutTime { get; set; }
        public string ExitTime { get; set; }
        public string EarlyBy { get; set; }
        public Array pagePrivilege { get; set; }
        public Array Department_types { get; set; }
        public Array stf_types { get; set; }
        public Array Designation_types { get; set; }
        public Array holiday_types { get; set; }
        public Array employeelist { get; set; }
        public Array sfname { get; set; }
        public Array emplist { get; set; }
        public DateTime empdate { get; set; }
        public Array editvalues { get; set; }
        public Array logapprovallist { get; set; }
        public Array yearlist { get; set; }
        public Array monthlist { get; set; }
        public long HRMLY_LeaveYear { get; set; }
        public long IVRM_Month_Id { get; set; }
        public long UserId { get; set; }
        public long FOEPARD_Id { get; set; }
        public string returnmsg { get; set; }

        //punch missing request
        public string HRME_EmployeeFirstName { get; set; }
        public DateTime? HRME_DOB { get; set; }
        public DateTime? HRME_DOJ { get; set; }
        public long? HRME_MobileNo { get; set; }
        public string HRME_EmailId { get; set; }
        public string HRME_EmployeeCode { get; set; }
        public string HRMDES_DesignationName { get; set; }
        public Array empname { get; set; }

        public Array employees { get; set; }

        public Array griddata { get; set; }
        public Array getBasicData { get; set; }

        ////////////////////////////////generalrequest////////////////////////////////
        public string FOEPAR_Remarks { get; set; }
        public DateTime? FOEPAR_PunchDate { get; set; }
        public string retrunMsg { get; set; }
        public string FOEPARD_FilePath { get; set; }
        public string FOEPARD_Time { get; set; }
        public string FOEPARD_Remarks { get; set; }
        public string FOEPARD_InOutFlg { get; set; }
        public long FOEPAR_Id { get; set; }
        public Array punchapproveddetails { get; set; }
        public Array punchrequest { get; set; }
        public Array punchdetails { get; set; }
        public Array approvaldata { get; set; }
        public string FOEPARA_ApprovedTime { get; set; }
        public string FOEPARA_Remarks { get; set; }
        public bool FOEPARA_ApprovalFlg { get; set; }
    }

}
