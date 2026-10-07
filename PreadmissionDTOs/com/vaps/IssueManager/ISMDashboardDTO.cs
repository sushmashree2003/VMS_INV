using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.IssueManager
{
    public class ISMDashboardDTO : CommonParamDTO
    {
        //dinner
        public Array dinnerorderlist { get; set; }
        public int VASEW_Id { get; set; }
        public DateTime VASEW_Date { get; set; }
        public bool VASEW_Activeflag { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public int CreatedBy { get; set; }
        public int UpdatedBy { get; set; }
        //dinner


        public long UserId { get; set; }
        public string Auto_id { get; set; }
        public string Minor { get; set; }
        public string Blocker { get; set; }
        public string Critical { get; set; }
        public string Major { get; set; }
        public string DepartmentName { get; set; }
        public long ISMMAC_Id { get; set; }
        public long MI_Id { get; set; }
        public long IVRMMS_Id { get; set; }
        public string Role_flag { get; set; }
        public string flag { get; set; }
        public long yearid { get; set; }
        public int month { get; set; }
        public string employeename { get; set; }
        public string HRME_Photo { get; set; }
        public string departmnt { get; set; }
        public string desgination { get; set; }
        public string gradename { get; set; }
        public string approvedBy { get; set; }
        public string emailid { get; set; }
        public string FOEPD_InOutFlg { get; set; }
        public DateTime? ISMDRPT_Date { get; set; }
        public long IVRMRT_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long? HRMD_Id { get; set; }
        public long? HRMDES_Id { get; set; }
        public long ISMMPR_Id { get; set; }
        public Array callCentreDetails { get; set; }

        public string ISMMPR_ProjectName { get; set; }
        public long HRME_Id { get; set; }
        public string HRME_EmployeeFirstName { get; set; }
        public long HRME_EmployeeCode { get; set; }
        public string Status { get; set; }
        public string HRME_EmployeeMiddleName { get; set; }
        public string HRME_EmployeeLastName { get; set; }
        public string HRMD_DepartmentName { get; set; }
        public string HRMDES_DesignationName { get; set; }
        public long IVRMM_Id { get; set; }
        public string IVRMM_ModuleName { get; set; }
        public string IVRMM_ModuleDesc { get; set; }
        public long ISMMMD_Id { get; set; }
        public long ISMMCLT_Id { get; set; }
        public string ISMMCLT_ClientName { get; set; }
        public long ISMTCR_Id { get; set; }
        public string ISMTCR_TaskNo { get; set; }
        public string ISMTCR_BugOREnhancementFlg { get; set; }
        public DateTime? ISMTCR_CreationDate { get; set; }
        public string ISMTCR_Title { get; set; }
        public long HRMPR_Id { get; set; }
        public string HRMP_Name { get; set; }
        public string message { get; set; }
        public string ISMTCR_Desc { get; set; }
        public string ISMTCR_Status { get; set; }
        public bool ISMTCR_ReOpenFlg { get; set; }
        public DateTime? ISMTCR_ReOpenDate { get; set; }
        public long ISMTCRASTO_Id { get; set; }
        public DateTime ISMTCRASTO_AssignedDate { get; set; }
        public long ISMTCRASTO_AssignedBy { get; set; }
        public string ISMTCRASTO_Remarks { get; set; }
        public DateTime? ISMTCRASTO_StartDate { get; set; }
        public DateTime? ISMTCRASTO_EndDate { get; set; }
        public long ISMTCRASTO_EffortInHrs { get; set; }
        public bool ISMTCRASTO_ActiveFlg { get; set; }
        public string assignedby { get; set; }
        public string approvedby { get; set; }
        public string ISMTPLTA_EffortInHrsnew { get; set; }
        public string DeviceID { get; set; }
        public long ISMTPLTA_Id { get; set; }
        public long ISMTPL_Id { get; set; }
        public string ISMTPLTA_Remarks { get; set; }
        public DateTime? ISMTPLTA_StartDate { get; set; }
        public DateTime? ISMTPLTA_EndDate { get; set; }
        public long ISMTPLTA_EffortInHrs { get; set; }
        public DateTime? ISMTPLTA_FinishedDate { get; set; }
        public long ISMTPLAPTA_Id { get; set; }
        public decimal HRELS_TotalLeaves { get; set; }
        public decimal HRELS_CBLeaves { get; set; }
        public string FOMHWDD_Name { get; set; }
        public string FOHTWD_HolidayWDType { get; set; }
        public string MI_Name { get; set; }
        public long totalcount { get; set; }
        public DateTime? FOMHWDD_FromDate { get; set; }
        public Array IsmDashBoard { get; set; }
        public Array Reapprovedmessage { get; set; }
        public long projectCount { get; set; }
       
        
        public long taskCount { get; set; }
        public long openCount { get; set; }
        public long closedCount { get; set; }
        public long completedCount { get; set; }
        public long inprogressCount { get; set; }
        public Array holidaylist { get; set; }
        public Array employeedetails { get; set; }
        public Array birthdaylist { get; set; }
        public Array leavedetails { get; set; }
        public Array upcomingholiday { get; set; }
        public Array lateindata { get; set; }
        public Array DashbaordLegalDetailsCount { get; set; }
        public Array lateindataprevious { get; set; }
        public Array attendancedata { get; set; }
        public Array pushnotification { get; set; }
        public Array demo_list { get; set; }
        public DateTime? HRME_DOB { get; set; }
        public long FOEP_Id { get; set; }
        public DateTime? FOEP_PunchDate { get; set; }
        public DateTime? lastpunchdate { get; set; }
        public Array getdeviationreport { get; set; }
        public Array punchdetails { get; set; }
        public Array punchdetailsprevious { get; set; }
        public Array visitorcheckoutdetails { get; set; }
        public long totdayspresent { get; set; }
        public long totholiday { get; set; }
        public Array projectcount { get; set; }
        public Array totalopentask { get; set; }
        public Array totalinprogresstask { get; set; }
        public Array totalpendingtask { get; set; }
        public DateTime fromdate { get; set; }
        public DateTime todate { get; set; }
        public Array getabsentlist { get; set; }
        public Array issueslist { get; set; }
        public Array prioritylist { get; set; }
        public Array prioritylistadmin { get; set; }
        public Array prioritypivot { get; set; }
        public Array eventlist { get; set; }
        public Array demo_list_tellicaller { get; set; }

        public Array pendingcount { get; set; }
        public Array generatedcount { get; set; }
        public Array griddata { get; set; }
        public Array bondCompletingEmpList { get; set; }
        public Array proxyNotMapped { get; set; }
        public Array getdeviationdepartmentwise { get; set; }
        public Array plannernotgeneratedreport { get; set; }
        public Array totapplied { get; set; }
        public Array totapproved { get; set; }
        public Array totrejected { get; set; }
        public Array totpending { get; set; }
        public Array blocklist { get; set; }
        public Array totalemployees { get; set; }
        public long HRMEMNO_MobileNo { get; set; }
        public long checkdeptheadcode { get; set; }
        public Array mappedemployeedetails { get; set; }
        public Array mappedemployeeissuedetails { get; set; }
        public Array get_statuslist { get; set; }
        public Array interdeptmenttasklist { get; set; }
        public Array interdeptment_list { get; set; }
        public Array getadvancetada { get; set; }
         public Array getadvancesalary { get; set; }
        public string TypeFlg { get; set; }
        public Array planerapproval { get; set; }
        public Array drnotapprovedmessage { get; set; }
        public Array Laptopnotapprovedlist { get; set; }
        public Array blockeddetails { get; set; }
        public long DR_NotApprovedTL { get; set; }
        public long blockedcount { get; set; }
        public Array DashbaordAttendanceDetailsCount { get; set; }
        public Array DashbaordPlannerDetailsCount { get; set; }
        public Array DashbaordPlannerApprovedDetailsCount { get; set; }
        public Array DashbaordPettyCashDetailsCount { get; set; }
        public Array DashbaordHrmssalaryDetailsCount { get; set; }
        public Array DashbaordInventoryDetailsCount { get; set; }
        public Array DashboardLeaveDetailsCount { get; set; }
        public Array DashboardPurchaseIndentaseDetailsCount { get; set; }
        public Array DashboardPurchaseOrderDetailsCount { get; set; }
        public Array DashboardDriverIndentDetailsCount { get; set; }
        public Array DashboardChequeBookDetailsCount { get; set; }
        public Array DashboardTADAAppliedDetailsCount { get; set; }
        public Array DashboardVisitorAppointmentDetailsCount { get; set; }
        public Array DashboardHWHealthcheckupdetailsCount { get; set; }
        public Array hrremindersplannerdetails { get; set; }
        public Array CloseTaskCoutnDetails { get; set; }
        public Array Graduitydetails { get; set; }
        public Array EmployeePLAmount { get; set; }
        //added by praveenkumar
        public Array Leadfollowpopup { get; set; }
        public Array TADA_AdavancePayments_popup { get; set; }
        public Array TADA_AdvsalesPaymentpopup { get; set; }

        public Array TADA_Paymentpopup { get; set; }
        //tennet 
        public Array DashbaordTenantLeft { get; set; }
        public Array DashbaordTenantTotalAmountPaid { get; set; }
        public Array DashbaordTenantBalanceAmount { get; set; }
        public Array DashbaordTenantJoined { get; set; }
        public Array DashbaordTenantagreementend { get; set; }
        public Array DashbaordTenantDetailsCount { get; set; }

        public Array DashbaordSalesDetailsCount { get; set; }

        public Array DashbaordSalesDetailsCountCurrentDate { get; set; }

        public Array StatewiseLeadListDetails { get; set; }
        
        public Array tenanttasklist { get; set; }
        public Array Ratingfetchemployee { get; set; }

        //add by raj
        public Array emp_deatils { get; set; }
        public string HRME_QRCode { get; set; }
        public bool Qrflag { get; set; }

        // praven Gouda 08-10-2023

         public Array departmentname { get; set; }
         public Array departmentcount { get; set; }
        public Array lateinearlyout { get; set; }
        public Array unotherisedLeavedeatils { get; set; }
        public Array QRcode_emp_deatils { get; set; }
        public Array Sataewiselead { get; set; }
        public long HRMB_Id { get; set; }
        public long HRMF_Id { get; set; }
        public long HRMAR_Id { get; set; }
        public DateTime CURRENT_DATE { get; set; }
        public Array QRcodeActivityDetails { get; set; }
        public VMCheckoutlist[] visitorcheckoutlist { get; set; }
        public trainigProcessIssueRlist[] trainigProcessIssueR { get; set; }
        public salaryUpdationDTO[] selectedEmpdetails { get; set; }

        public Array GetReport { get; set; }
        public Array momdrblockreport { get; set; }
        //vendoronboarding
        public Array invvendoronboardreport { get; set; }
        public Array RatingDetails { get; set; }

        public Array Dashboardloandetails { get; set; }
        public Array proxyleavedetails { get; set; }
        public Array proxytaskslist { get; set; }        
        //ada.ms
        public Array admindata { get; set; }
        public Array dashboardmoduleratingreport { get; set; }
        public Array Dev_Completedtask { get; set; }
        public Array getbda_Report { get; set; }
        public Array Sales_Completedtask { get; set; }
        public string type { get; set; }
        public string date { get; set; }
        public string Department_Code { get; set; }
        public Array fileupload { get; set; }
        public Array get_prNo { get; set; }
        public class VMCheckoutlist
        {
            public long VMMV_Id { get; set; }
            public string VMMV_Remarks { get; set; }
        }
        public class trainigProcessIssueRlist
        {
            public long TMO_Id { get; set; }
            public string TMO_Remark { get; set; }
        }
        //Praveen
        public long HRMDC_Id { get; set; }
        public string Flags { get; set; }
        public string dept_code { get; set; }    

    }

   
}