using PreadmissionDTOs.com.vaps.FrontOffice;
using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.BirthDay
{
    public class BirthDayDTO
    {

        //------ public
        long HRMET_Id { get; set; }
        public long MI_Id { get; set; }
        //For Unauthorized Leave Count Report

        public Array getmonth { get; set; }
        public Array getyear { get; set; }
        public long IVRM_Month_Id { get; set; }
        public string IVRM_Month_Name { get; set; }
        public string Month_Name { get; set; }

        //The above ones are for Unauthorized Leave Count Report

        public string HRMET_EmployeeType { get; set; }
        public bool HRMET_ActiveFlag { get; set; }
        public Array employeeRetingdata { get; set; }
        public Array employeeTypeList { get; set; }
        public string employeetpList { get; set; }
        public string employeeDeptList { get; set; }
        public string employeeDesgList { get; set; }
        public string retrunMsg { get; set; }
        public long roleId { get; set; }
        public DateTime? HRME_DOB { get; set; }

        public long HRME_Id { get; set; }
        public long Id { get; set; }
        public string HRME_EmployeeFirstName { get; set; }
        public string HRME_EmployeeMiddleName { get; set; }
        public string HRME_EmployeeLastName { get; set; }
        public bool? HRME_LeftFlag { get; set; }
        public long HRME_MobileNo { get; set; }
        public string HRME_EmailId { get; set; }
        public string ccmail { get; set; }
        public long mappeduser { get; set; }
        //----
        public long ISMCLMER_Id { get; set; }
        public long yearid { get; set; }
        public long asmcL_Id { get; set; }
        public long asmC_Id { get; set; }
        public long sectionid { get; set; }
        public string AMST_SOL { get; set; }
        public long AMST_Id { get; set; }
        public long UserId { get; set; }
        public string stuFN { get; set; }
        public string stuMN { get; set; }
        public string stuLN { get; set; }
        public string regno { get; set; }
        public string admno { get; set; }
        public string Datetype { get; set; }
        //-------------------------------
        public string AMST_FirstName { get; set; }
        public string AMST_AdmNo { get; set; }
        //public long MI_ID { get; set; }
        public long ASMAY_Id { get; set; }
        public string MI_Name { get; set; }
        public Array get_mi_list1 { get; set; }
        public Array proxylist { get; set; }       
        public DateTime? amst_dob { get; set; }

        public string ASMCL_ClassName { get; set; }

        public string ASMC_SectionName { get; set; }
        public Array sms_mail_count { get; set; }
        public Array studentDetails { get; set; }
        public Array staffList { get; set; }
        public Array classDrpDwn { get; set; }
        public Array sectionDrpDwn { get; set; }
        public Array studentlist { get; set; }
        public Array Leavepending { get; set; }
        public Array companylist { get; set; }
        public Array drrjectedreport { get; set; }
        public Array pagePrivilege { get; set; }
        public string flags { get; set; }
        public string flag { get; set; }
        public string flagl { get; set; }
        public string day { get; set; }
        public string months { get; set; }

        public string days1 { get; set; }
        public string days2 { get; set; }

        public string all1 { get; set; }
        public Array accyear { get; set; }
        public int count { get; set; }
        //public DateTime? Start_Date { get; set; }
        public DateTime? date11 { get; set; }
        public long AMST_MobileNo { get; set; }
        public string AMST_emailId { get; set; }
        //public long[] SelectedActivityDetails_stu { get; set; }
        public MasterEmployeeDTO[] SelectedActivityDetails1_stf { get; set; }
        public MasterEmployeeDTO[] SelectedActivityDetails_stu { get; set; }
        public string sms_text { get; set; }

        public string rdbbutton { get; set; }
        public string studentName { get; set; }
        public string employeeName { get; set; }
        public BirthDayDTO[] selectedStudent { get; set; }
        public BirthDayDTO[] selectedEmployees { get; set; }
        public string smsflag { get; set; }
        public string emailflag { get; set; }
        public string emailStatus { get; set; }
        public string smsStatus { get; set; }
        public DateTime? start_date { get; set; }
        public DateTime? end_date { get; set; }
        public Array mail_count_list { get; set; }
        public string SearchColumn { get; set; }
        public string EnteredData { get; set; }
        public string message { get; set; }
        public bool staffChecked { get; set; }
        public bool studChecked { get; set; }
        public string To_FLag { get; set; }
        public string PhotoPath { get; set; }
        public Array fillmonth { get; set; }
        public Array fillyear { get; set; }
        public long year { get; set; }
        public int month { get; set; }
        public DateTime? date12 { get; set; }
        public long ssb { get; set; }
        public long esb { get; set; }
        public DateTime? Fromdate { get; set; }
        public DateTime? Todate { get; set; }
        public Array staffDetails { get; set; }
        public Array Blockeddrdata { get; set; }
        public Array workinghourDetails { get; set; }
        public Array compoffadjtelig { get; set; }
        public Array punchmissingapproval { get; set; }

        public int smscount { get; set; }
        public int emailcount { get; set; }
        public int workingEmployee { get; set; }
        public int missingPhoto { get; set; }
        public int missingEmailId { get; set; }
        public int missingContactNumber { get; set; }
        public int onlylateinDetailssms { get; set; }
        public int onlylateinDetailsEmail { get; set; }
        public int onlyEarlyOutDetailssms { get; set; }
        public int onlyEarlyOutDetailsEmail { get; set; }
        public int onlyAbsentDetailssms { get; set; }
        public int onlyAbsentDetailsEmail { get; set; }
        public Array lunchdata { get; set; }
        public Array columnnames { get; set; }
        public Array filltypes { get; set; }
        public Array Dailyreptdetails { get; set; }
        public Array staffleaveDetails { get; set; }
        public VMSConsolidateReportTempDTO[] ConsolidateReportTempDTO { get; set; }
        public long HRERLAP_Id { get; set; }
        public string HRERLAP_Remarks { get; set; }
        public string HRERLAP_ApplicationStatus { get; set; }
        public DateTime HRERLAP_FromDate { get; set; }
        public DateTime HRERLAP_ToDate { get; set; }
        public BirthDayDTO[] rcLeaveRecords { get; set; }
        public Array ConsolidateSalaryReport { get; set; }
        public string institutename { get; set; }
        public string monthname { get; set; }
        public Array punchdates { get; set; }
        public Array HRCOMMENTS { get; set; }
        public DateTime HRELT_FromDate { get; set; }
        public DateTime HRELT_ToDate { get; set; }
        public Array LeaveDetails { get; set; }
        public Array currentsalary { get; set; }
        public Array rejectedDrList { get; set; }
        public Array Halfdaydrdata { get; set; }
        public DateTime? ISMDRPT_Date { get; set; }
        public Array holidaylist { get; set; }
        public DateTime? FOMHWDD_FromDate { get; set; }
        public DateTime? FOMHWDD_ToDate { get; set; }
        public decimal HRELS_CBLeaves { get; set; }
        public Array Compoffdetails { get; set; }
        public Array griddatalist { get; set; }
        public Array approvedlist { get; set; }
        public string imgname { get; set; }
        public long drnotapprovedcount { get; set; }

        public Array salaryGenerated { get; set; }
        public employeeArraysliust[] employeeArray { get; set; }
        public Monthlist1[] Monthlist { get; set; }
        public Array drblockedReport { get; set; }
        public attendenceapprovalrecord[] AttendenceApprovedDTO { get; set; }
        //For Unauthorised Leave Count Report
        public Array institutionlist { get; set; }
        public long HRMLY_LeaveYear { get; set; }

        public Array gettodate { get; set; }

        public DateTime? from_date { get; set; }
        public DateTime? to_date { get; set; }
        public institutelist[] selected_Inst { get; set; }
        public departmentlist[] selected_Dept { get; set; }
        public string radioflag { get; set; }
        public decimal LOP { get; set; }

        // Consolidated Emploeee Report
        public Array monthendreportclientwise { get; set; }
        public Array monthendreportotherdata { get; set; }
        public Array concallwithlopemp { get; set; }
        public Array concallextention { get; set; }
        public Array compOffAdjest { get; set; }
        
        public class LopDetails
        {
            public long MI_Id { get; set; }
            public long userid { get; set; }
            public long UserId { get; set; }
            public long HRALD_ID { get; set; }
            public string HRME_EmployeeFirstName { get; set; }
            public string HRALD_Type { get; set; }
            public string DepartmentName { get; set; }
            public string Loptype { get; set; }
            public long HRME_Id { get; set; }
            public long Id { get; set; }
            public string HRALD_Remarks { get; set; }
            public string HRALD_File { get; set; }
            public string returnmsg { get; set; }
            public DateTime lopdate { get; set; }
            public Array employees { get; set; }
            public Array griddata { get; set; }


        }


    }
    public class attendenceapprovalrecord
    {

        public string status { get; set; }
        public string HREAA_Remarks { get; set; }
        public decimal HREAA_LOP { get; set; }

        public long ecode { get; set; }
        public long hrmE_Id { get; set; }
    }
    public class employeeArraysliust
    {
        public long MI_Id { get; set; }
    }
    public class Monthlist1
    {
        public long IVRM_Month_Id { get; set; }
    }

    // For Unauthorized Leave Count Report
    public class institutelist
    {
        public long MI_Id { get; set; }
        public string MI_Name { get; set; }
    }
    public class departmentlist
    {
        public long HRMD_Id { get; set; }
        public string HRMD_DepartmentName { get; set; }
    }
}
