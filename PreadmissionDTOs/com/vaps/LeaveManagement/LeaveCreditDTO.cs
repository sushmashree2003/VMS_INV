using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.LeaveManagement
{
    public class LeaveCreditDTO : CommonParamDTO
    {
        //New Property for Searchable List Drop Down
        public int duplicateCount { get; set; }
        public Array departmentdropdown { get; set; }
        public Array institutionarray { get; set; }
        public Array Institution_List { get; set; }
        public Array Staff_List { get; set; }
        public Array leaveName { get; set; }
        public string MI_Name { get; set; }
        public bool checkbox_value { get; set; }
        public Array get_institution { get; set; }
        public Array report_details { get; set; }
        public Array leaverestriction { get; set; }
        // End of New Propeties for Searchable List Drop Down

        // Array for authorization report details
        public Array authorizationreport_details { get; set; }
        public Array academic_year_name { get; set; }
        public Array result { get; set; }
        public Array optionalleavedata { get; set; }
        public Array pagePrivilege { get; set; }
        public long HRMGT_Id { get; set; }
        public long ISMDRPT_Id { get; set; }
        public long MI_Id { get; set; }
        public long IVRMUL_Id { get; set; }
        public string HRMGT_EmployeeGroupType { get; set; }
        public LeaveCreditDTO[] Template { get; set; }
        public LeaveCreditDTO[] authorizationreport { get; set; }
        public LeaveCreditDTO[] selectedcompoff { get; set; }
        public LeaveCreditDTO[] selecteddr { get; set; }
        public Array deviationdetails { get; set; }
        public string EmailSMS { get; set; }
        //public string hrmE_EmployeeCode { get; set; }
        public string TemplateString { get; set; }
        public string retrunMsg { get; set; }
        public string HRMGT_Code { get; set; }
        public int HRMGT_Order { get; set; }
        public bool HRMGT_ActiveFlag { get; set; }
        public Array stf_types { get; set; }
        public Array employeelist { get; set; }
        public long HRMLD_Id { get; set; }
        public long HRML_Id { get; set; }
        public string HRELAP_Remarks { get; set; }

        //*****************************Leave_ReConsideration****************//
        
        public long HRERLAP_Id { get; set; }
        public DateTime HRERLAP_FromDate { get; set; }
        public DateTime HRERLAP_ToDate { get; set; }
        public decimal HRERLAP_TotalDays { get; set; }
        public string HRERLAP_LeaveReason { get; set; }
        public string HRERLAP_ApplicationStatus { get; set; }
        public bool HRERLAP_ActiveFlag { get; set; }
          
        //*****************************Leave_Restriction****************//
        public Department_TempDto[] selected_Dept { get; set; }
        public long HRLRD_Id { get; set; }
        public DateTime HRLRD_FromDate { get; set; }
        public DateTime HRLRD_ToDate { get; set; }
        public string HRLRD_Remarks { get; set; }
        public bool HRLRD_ActiveFlg { get; set; }
        public long HRLRD_CreatedBy { get; set; }
        public long HRLRD_UpdatedBy { get; set; }
        public DateTime HRLRD_CreatedDate { get; set; }
        public DateTime HRLRD_UpdatedDate { get; set; }
        public Array lrlist { get; set; }
        public Array leaverestriclist { get; set; }
        //----------------------HR_Master_Leave_Details-------------------------------
        public long? HRMD_Id { get; set; }
        public long HRMDES_Id { get; set; }
        public long HRMG_Id { get; set; }
        public string HRMLD_NoOfDays { get; set; }
        public long HRMLDCM_NoOfLeaves { get; set; }
        public int HRMLD_MaxLeaveApplicable { get; set; }
        public bool HRMLD_CarryForFlg { get; set; }
        public bool HRMLD_EncashFlg { get; set; }
        public decimal HRMLD_EncashAmount { get; set; }
        public DateTime? HRMLD_EncashOn { get; set; }
        public Array edit_lvblnce { get; set; }
        public List<LeaveCreditDTO> devicelist12 { get; set; }
        public string AMST_AppDownloadedDeviceId { get; set; }
        public Array get_deviationreport { get; set; }
        //---------------Department---------------------
        public string HRMD_DepartmentName { get; set; }
        public int HRMD_Order { get; set; }
        public bool HRMD_ActiveFlag { get; set; }
        public string MI_Id_liststring { get; set; }

        //---------------Designation---------------------
        public string HRMDES_DesignationName { get; set; }
        public int HRMDES_BasicAmount { get; set; }
        public int HRMDES_SanctionedSeats { get; set; }
        public bool HRMDES_DisplaySanctionedSeatsFlag { get; set; }
        public int HRMDES_Order { get; set; }
        public bool HRMDES_ActiveFlag { get; set; }
        public string selectedLeave { get; set; }


        //---------------Leave Name---------------------
        public string HRML_LeaveName { get; set; }
        public string HRML_LeaveCode { get; set; }
        public string HRML_LeaveDetails { get; set; }
        public bool HRML_LeaveCreditFlg { get; set; }
        public string HRML_LeaveType { get; set; }


        //--------------- Month---------------------
        public long IVRM_Month_Id { get; set; }
        public string IVRM_Month_Name { get; set; }
        public bool Is_Active { get; set; }
        public int IVRM_Month_Max_Days { get; set; }

        //--------------- EarningsDeductions---------------------

        public long HRMED_Id { get; set; }
        public long HRMEDT_Id { get; set; }
        public string HRMED_EarnDedName { get; set; }
        public string HRMED_EarnDedTypeFlag { get; set; }
        public string HRMED_AmountPercentFlag { get; set; }
        public string HRMED_AmountPercent { get; set; }
        public bool HRMED_ActiveFlag { get; set; }
        public decimal HRMED_RoundOffFlag { get; set; }
        public DateTime HRMED_EntryDate { get; set; }
        public long LoginId { get; set; }
        public bool HRME_ActiveFlag { get; set; }
        public long Master_Id { get; set; }

        //--------------------------------------------------------------------------------------------------
        public long HRME_Id { get; set; }
        public long HRMEIdproxy { get; set; }
        public long HRME_IdMy { get; set; }
        public string HRME_EmployeeFirstName { get; set; }
        public string HRME_EmployeeMiddleName { get; set; }
        public string HRME_EmployeeLastName { get; set; }
        public string HRME_EmployeeCode { get; set; }
        public string HRME_BiometricCode { get; set; }
        public long? HRME_RFCardId { get; set; }
        public string HRME_PerStreet { get; set; }
        public string HRME_PerArea { get; set; }
        public string HRME_PerCity { get; set; }
        public string HRME_PerAdd4 { get; set; }
        public long? HRME_PerStateId { get; set; }
        public long? HRME_PerCountryId { get; set; }
        public long? HRME_PerPincode { get; set; }
        public string HRME_LocStreet { get; set; }
        public string HRME_LocArea { get; set; }
        public string HRME_LocCity { get; set; }
        public string HRME_LocAdd4 { get; set; }
        public long? HRME_LocStateId { get; set; }
        public long? HRME_LocCountryId { get; set; }
        public long? HRME_LocPincode { get; set; }
        public long? IVRMMMS_Id { get; set; }
        public long? IVRMMG_Id { get; set; }
        public long? CasteCategoryId { get; set; }
        public long? CasteId { get; set; }
        public long? ReligionId { get; set; }
        public string HRME_FatherName { get; set; }
        public string HRME_MotherName { get; set; }
        public string HRME_SpouseName { get; set; }
        public string HRME_SpouseOccupation { get; set; }
        public long? HRME_SpouseMobileNo { get; set; }
        public string HRME_SpouseEmailId { get; set; }
        public string HRME_SpouseAddress { get; set; }
        public DateTime? HRME_DOB { get; set; }
        public DateTime? HRME_DOJ { get; set; }
        public DateTime? HRME_ExpectedRetirementDate { get; set; }
        public DateTime? HRME_PFDate { get; set; }
        public DateTime? HRME_ESIDate { get; set; }
        public long? HRME_MobileNo { get; set; }
        public string HRME_EmailId { get; set; }
        public string HRME_BloodGroup { get; set; }
        public string HRME_PaymentType { get; set; }
        public string HRME_BankAccountNo { get; set; }
        public string HRME_PFApplicableFlag { get; set; }
        public string HRME_PFMaxFlag { get; set; }
        public string HRME_PFFixedFlag { get; set; }
        public string HRME_PFAccNo { get; set; }
        public string HRME_ESIAccNo { get; set; }
        public string HRME_GratuityAccNo { get; set; }
        public string HRME_ESIApplicableFlag { get; set; }
        public string HRME_PhotoNo { get; set; }
        public string HRME_LeftFlag { get; set; }
        public DateTime HRME_DOL { get; set; }
        public string HRME_LeavingReason { get; set; }
        public string HRME_Height { get; set; }
        public string HRME_HeightUOM { get; set; }
        public int? HRME_Weight { get; set; }
        public string HRME_WeightUOM { get; set; }
        public string HRME_IdentificationMark { get; set; }
        public string HRME_ApprovalNo { get; set; }
        public string HRME_PANCardNo { get; set; }
        public string HRME_AadharCardNo { get; set; }
        public string HRME_SubstituteFlag { get; set; }
        public string HRME_NationalSSN { get; set; }
        public string HRME_SalaryType { get; set; }
        public string HRME_EmployeeOrder { get; set; }
        public long HRMEMNO_MobileNo { get; set; }


        //------HR_Emp_OB_Leave-------------------------------------------------------

        public long HREOBL_Id { get; set; }
        public long HRMLY_Id { get; set; }
        public DateTime HREOBL_Date { get; set; }
        public decimal HREOBL_OBLeaves { get; set; }
        public string leave_ob_name { get; set; }


        //----------------------[HR_Emp_Leave_Trans]-----------------------

        public long HRELT_Id { get; set; }
        public long HRELT_LeaveId { get; set; }
        public DateTime HRELT_FromDate { get; set; }
        public DateTime HRELT_ToDate { get; set; }
        public decimal HRELT_TotDays { get; set; }
        public DateTime HRELT_Reportingdate { get; set; }
        public string HRELT_LeaveReason { get; set; }
        public string HRELT_Status { get; set; }
        public bool HRELT_ActiveFlag { get; set; }
        public bool LOPAdjest { get; set; }
        public string HRELT_SupportingDocument { get; set; }


        //----------------------------HR_Master_Leave_Details_CFMonth-----------------------------------------

        public long HRMLDCFM_Id { get; set; }
        public long HRMLDCF_Id { get; set; }
        public int HRMLDCFM_MonthId { get; set; }


        ////-----------------------------HR_Leave_Transaction-----------------------------------------------------------
        //public long HRLT_Id { get; set; }
        //public float HRLT_Availed { get; set; }
        //public DateTime HRLT_From_Date { get; set; }
        //public char HRLT_LOP_Flag { get; set; }
        //public DateTime HRLT_To_Date { get; set; }
        //public string HRLT_REMARKS { get; set; }


        //--------------------------[HR_Emp_Leave_Credit]---------------------------------------------
        public long HRELC_Id { get; set; }
        public DateTime HRELC_Date { get; set; }
        public string HRELC_LCMonth { get; set; }
        public int HRELC_CrLeaves { get; set; }

        //-----------------------------[HR_Master_Grade]----------------------------------------------

        public string HRMG_GradeName { get; set; }
        public string HRMG_GradeDisplayName { get; set; }
        public string HRMG_PayScaleRange { get; set; }
        public decimal HRMG_PayScaleFrom { get; set; }
        public decimal HRMG_IncrementOf { get; set; }
        public decimal HRMG_PayScaleTo { get; set; }
        public int HRMG_Order { get; set; }
        public bool HRMG_ActiveFlag { get; set; }

        //----------------------------HR_Leave_Authorisation----------------------------------------------------------
        public long HRLA_Id { get; set; }
        public string HRLA_EmailTo { get; set; }
        public string HRLA_EmailCC { get; set; }

        //------------------------HR_Leave_Auth_OrderNo------------------------------------

        public long HRLAON_Id { get; set; }

        public long HRLAON_SanctionLevelNo { get; set; }
        public bool HRLAON_FinalFlg { get; set; }

        //-------------------------[HR_Emp_Leave_Trans_Details]--------------------------------------
        public long HRELTD_Id { get; set; }
        public DateTime HRELTD_FromDate { get; set; }
        public DateTime HRELTD_ToDate { get; set; }
        public decimal HRELTD_TotDays { get; set; }
        public bool HRELTD_LWPFlag { get; set; }
        public tempDTO[] emplist { get; set; }

        //online Leave 
        public string HRML_WhenToApplyFlg { get; set; }
        public decimal HRML_NoOfDays { get; set; }
        public long HRELS_Id { get; set; }
        public decimal HRELS_OBLeaves { get; set; }
        public decimal HRELS_CreditedLeaves { get; set; }
        public decimal HRELS_TotalLeaves { get; set; }
        public decimal HRELS_TransLeaves { get; set; }
        public int HRELS_EncashedLeaves { get; set; }
        public decimal HRELS_CBLeaves { get; set; }
        public long UserId { get; set; }

        //--save online leave application
        public long HRELAP_Id { get; set; }
        public string HRELAP_ApplicationID { get; set; }
        public DateTime? HRELAP_ApplicationDate { get; set; }
        public DateTime? HRELAP_FromDate { get; set; }
        public DateTime? HRELAP_ToDate { get; set; }
        public DateTime HRELAP_FromDateapprv { get; set; }
        public DateTime HRELAP_ToDateapprv { get; set; }
        public decimal HRELAP_TotalDaysapprv { get; set; }
        public decimal HRELAP_TotalDays { get; set; }
        public string HRELAP_LeaveReason { get; set; }
        public long HRELAP_ContactNoOnLeave { get; set; }
        public DateTime? HRELAP_ReportingDate { get; set; }
        public string HRELAP_ApplicationStatus { get; set; }

        public Array master_eventlist { get; set; }
        //MB
        public LeaveCreditDTO[] temp_table_data { get; set; }
        //
        public string HRELAP_SanctioningLevel { get; set; }
        public bool HRELAP_FinalFlag { get; set; }
        public bool HRELAP_ActiveFlag { get; set; }
        public string HRELAP_SupportingDocument { get; set; }

        //-----------------[HR_Master_LeaveYear]-------------------------------
        public string HRMLY_LeaveYear { get; set; }
        public DateTime HRMLY_FromDate { get; set; }
        public DateTime HRMLY_ToDate { get; set; }
        public bool HRMLY_ActiveFlag { get; set; }
        public Array commentlist { get; set; }
        public Array approverList { get; set; }
        public Array proxytaskList { get; set; }
        public Array proxytaskListview { get; set; }
        //----------------------HR_Master_Leave_Details_CreditMonth-------------------------------
        public long HRMLDCM_Id { get; set; }
        public long HRMLDCM_LCMonthCode { get; set; }
        //----------------------HR_Master_Leave_Details_CF-------------------------------        
        public bool HRMLDCF_MaxLeaveAplFlg { get; set; }
        public int HRMLDCF_MaxLeaveCF { get; set; }

        //----------------------[HR_Master_Leave_Details_EnCash]-------------------------------   
        public long HRMLDEC_Id { get; set; }
        public bool HRMLDEC_ServiceAplFlg { get; set; }
        public string HRMLDEC_ServiceYear { get; set; }
        public string HRMLDEC_ServiceMonth { get; set; }
        public string HRMLDEC_ServiceDays { get; set; }
        public bool HRMLDEC_MaxLeaveFlg { get; set; }
        public int HRMLDEC_MaxLeaves { get; set; }
        public bool HRMLDEC_MinLeaveFlg { get; set; }
        public int HRMLDEC_MinLeaves { get; set; }
        public string HRMLDEC_ScheduleFlg { get; set; }
        public string HRMLDEC_ScheduleYear { get; set; }
        public string HRMLDEC_ScheduleMonth { get; set; }
        public bool HRMLDEC_VariableFixedFlg { get; set; }
        public decimal HRMLDEC_FixedAmount { get; set; }

        //--------------------------Leave Report Store Procedure---------------------------
        public string multipleEmp { get; set; }
        public long Month { get; set; }
        public long Year { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }


        //--------------------[HR_Emp_Leave_Appl_Authorisation]---------------------------

        public long HRELAPA_Id { get; set; }
        public string HRELAPA_SanctioningLevel { get; set; }
        public string HRELAPA_Remarks { get; set; }
        public bool? HRELAPA_FinalFlag { get; set; }

        //-----------------END-------------------------------
        public LeaveCreditDTO[] auth_array { get; set; }
        public LeaveCreditDTO[] emp_array { get; set; }
        public Array emp_trans { get; set; }
        public Array earnded { get; set; }
        public Array Designation_types { get; set; }
        public Array Department_types { get; set; }
        public Array credit_month { get; set; }
        public Array leave_name { get; set; }
        public Array countrecon { get; set; }
        public Array lappcount { get; set; }
        public Array leave_namelist { get; set; }
        public LeaveCreditDTO[] multi_leave_name { get; set; }
        public Array leave_code { get; set; }
        public Array get_emp { get; set; }
        public Array get_emp_lop { get; set; }
        public Array get_emp_other { get; set; }
        public Array get_auth { get; set; }
        public Array leavearrayyear { get; set; }
        public Array get_leaveDetails { get; set; }
        public LeaveCreditDTO[] emptypes { get; set; }
        public LeaveCreditDTO[] emplop { get; set; }
        public LeaveCreditDTO[] desgtypes { get; set; }
        public long leavecode { get; set; }
        public Array online_leave { get; set; }
        public LeaveCreditDTO[] leaveobtype { get; set; }
        public LeaveCreditDTO[] empdept { get; set; }
        public LeaveCreditDTO[] empdesg { get; set; }
        public int count { get; set; }
        public bool returnval { get; set; }
        public bool Edit_flag { get; set; }
        public LeaveCreditMonthDTO[] leaveCM { get; set; }
        public LeaveDetailsCFDTO[] leaveCf { get; set; }
        public Array activityIds { get; set; }
        public Array activitycf { get; set; }
        public string returnduplicatestatus { get; set; }
        public Array master_loplist { get; set; }
        public Array edit_auth { get; set; }
        public Array grade_name { get; set; }
        public Array authData { get; set; }
        public Array get_year { get; set; }
        public LeaveCreditDTO[] selectedMonthList { get; set; }
        public LeaveCreditDTO[] selectedcarryMonthList { get; set; }
        //public LeaveCreditDTO[] selectedreport { get; set; }
        public long[] selectedreport { get; set; }
        public LeaveCreditDTO[] get_leave_status { get; set; }
        public Array get_leavestatus { get; set; }
        public Array leave_year_id { get; set; }
        public string message { get; set; }
        public bool monthwisecredit { get; set; }
        public LeaveCreditDTO[] frmToDates { get; set; }


        public LeaveCreditDTO[] Emp_types { get; set; }
        public LeaveCreditDTO[] dept_types { get; set; }
        public LeaveCreditDTO[] desig_types { get; set; }
        public LeaveCreditDTO[] grade_types { get; set; }

        public Array loplist { get; set; }
        // public string message { get; set; }
        public long asmay_id { get; set; }
        public string returnmsg { get; set; }
        public Array get_emp_lops { get; set; }
        public Array leavehistory { get; set; }
        public Array duplicates { get; set; }
        public string HRELAPD_InTime { get; set; }
        public string HRELAPD_OutTime { get; set; }
        public string HRELAP_CompOffCreditApplFlg { get; set; }
        public string HRELAPA_InTime { get; set; }
        public string HRELAPA_OutTime { get; set; }
        public string leaveauthorizeusername { get; set; }
        public Array Getviewremarksdetails { get; set; }
        public DateTime? HRELAPA_FromDate { get; set; }
        public DateTime? HRELAPA_ToDate { get; set; }
        public decimal? HRELAPA_TotalDays { get; set; }
        public string HRELAPA_LeaveStatus { get; set; }

        //Manual Leave Credit
        public string OnLoadOrChangeInstFlag { get; set; }
        public Array getuserinstitution { get; set; }
        public SelectedEmployees[] SelectedEmployees { get; set; }
        public string authorizationmessage { get; set; }
        public string onchangeoronload { get; set; }
        public Array getemployeeleavedetails { get; set; }
        public Array getemployeehistroyleavedetails { get; set; }
        public Array employeeleavedetails { get; set; }
        public approvaluser_array[] approvaluser_array { get; set; }
        public doc_list2[] doc_list2 { get; set; }

        public class CreditLeaveDTO
        {
            public string returnmsg { get; set; }        
            public long MI_Id { get; set; }         
        }

        public class LeaveApprovalDTO
        {
            public long HRME_Id { get; set; }
            public long MI_Id { get; set; }
            public long LoginId { get; set; }
            public Array activityIds { get; set; }
            public long HRELAP_Id { get; set; }
            public string AMST_AppDownloadedDeviceId { get; set; }
            public string message { get; set; }
            public string status { get; set; }
            public ApprovalStatus[] get_leave_status { get; set; }
        }


    }

    public class ApprovalStatus
    {
        public long HRME_Id { get; set; }
        public string HRELAP_ApplicationID { get; set; }
        public string HRELAP_LeaveReason { get; set; }
        public string HRELAPA_Remarks { get; set; }
        public DateTime HRELAP_FromDateapprv { get; set; }
        public DateTime HRELAP_ToDateapprv { get; set; }
        public DateTime HRELAP_FromDate { get; set; }
        public DateTime HRELAP_ToDate { get; set; }        
        public decimal HRELAP_TotalDays { get; set; }
        public decimal? HRELAP_TotalDaysapprv { get; set; }
        public long HRMLY_Id { get; set; }
        public long HRML_Id { get; set; }
        public string Reason { get; set; }
        public string Type { get; set; }
    }


    public class tempDTO
    {
        public long HRME_Id { get; set; }
        public decimal HRELS_CreditedLeaves { get; set; }
        public decimal HRELS_TotalLeaves { get; set; }
        public long? HRELS_Id { get; set; }
        public decimal? HRELS_CBLeaves { get; set; }
        public long HRMLY_Id { get; set; }
        public long HRML_Id { get; set; }
        public string Reason { get; set; }
        public string Type { get; set; }
    }

    public class deviationdetailDTO
    {
        public string DEPARTMENT { get; set; }
        public string MON_YR1 { get; set; }
        public string DEVI1 { get; set; }
        public string COMP1 { get; set; }
    }

    public class GroupType_TempDto
    {
        public long HRMGT_Id { get; set; }
        public string HRMGT_EmployeeGroupType { get; set; }
    }
    public class Department_TempDto
    {
        public long? HRMD_Id { get; set; }
        public long? HRME_Id { get; set; }
        public string HRMD_DepartmentName { get; set; }
    }
    public class Designation_TempDto
    {
        public long HRMDES_Id { get; set; }
        public string HRMDES_DesignationName { get; set; }
    }
    public class Grade_TempDto
    {
        public long HRMG_Id { get; set; }
        public string HRMG_GradeName { get; set; }
    }

    public class DEVDTO
    {
        public string HRME_Id { get; set; }
        public string HRMDC_Name { get; set; }
        public string HRMDC_ID { get; set; }
        public string DevPer { get; set; }
        public string CompPer { get; set; }
        public string MonthYear { get; set; }
    }

    public class SelectedEmployees
    {
        public long HRME_Id { get; set; }
        public SelectedEmployeesLeaveDetails[] leavedetails { get; set; }
    }
    public class SelectedEmployeesLeaveDetails
    {
        public long HRML_Id { get; set; }
        public long leavecount { get; set; }
        public long HRELS_Id { get; set; }
    }

    public class approvaluser_array
    {
        public string ApprovalEmpName { get; set; }
        public long Approval_HRME_Id { get; set; }
        public long ApprovalLevelNo { get; set; }
        public bool ApprovalFinalFlag { get; set; }
    }

    public class doc_list2
    {
        public string HRELAPA_Remarks { get; set; }     
        public string HRELAPD_InTime { get; set; }     
        public string HRELAPD_OutTime { get; set; }
        public string Status { get; set; }
        public long HRELAP_Id { get; set; }
        public long HRME_Id { get; set; }
    }

}