using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.IssueManager
{
    public class ISM_DailyReportGenerationDTO
    {
        public long ISMDRPT_Id { get; set; }
        public long MI_Id { get; set; }
        public long ISMMTCAT_Id { get; set; }
        public long DR_Notification { get; set; }
        public string sound { get; set; }
        public string DR_DATE { get; set; }
        public long HRME_Id { get; set; }
        public string HRME_Employeecode { get; set; }
        public long HRME_To { get; set; }
        public ISM_DailyReportGenerationDTO[] Apprval { get; set; }
        public string HRME_AppDownloadedDeviceId { get; set; }
        public long? HRME_MobileNo { get; set; }
        public long ISMTPL_Id { get; set; }
        public long ISMTPLAP_Id { get; set; }
        public long ISMTCR_Id { get; set; }
        public long Dept_Head_Id { get; set; }
        public DateTime? ISMDRPT_Date { get; set; }
        public string ISMDRPT_Remarks { get; set; }
        public decimal? ISMDRPT_TimeTakenInHrs { get; set; }
        public string ISMDRPT_Status { get; set; }
        public bool ISMDRPT_DeviationFlg { get; set; }
        public bool ISMDRPT_ExtraFlg { get; set; }
        public bool ISMDRPT_ActiveFlg { get; set; }
        public string HRME_DOJ { get; set; }
        public DateTime ISMTRR_RemarksDate { get; set; }
        public string HRME_DOC { get; set; }
        public string DATE { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public long ISMDRPT_CreatedBy { get; set; }
        public Array ProformaInvoice { get; set; }
        public long ISMDRPT_UpdatedBy { get; set; }
        public Array gettasklist { get; set; }
        public Array getbda_Report { get; set; }
        public Array getprioritylist { get; set; }
        public Array getstatuslist { get; set; }
        public Array telecallerblock { get; set; }
        public long UserId { get; set; }
        public long ASMAY_Id { get; set; }
        public long roleId { get; set; }
        public Temp_ISM_DailyReportGenerationDTO[] Temp_ISM_DailyReportGenerationDTO { get; set; }
        public bool returnval { get; set; }
        public string plannername { get; set; }
        public string ISMTRR_Remarks { get; set; }
        
        public Array plannerdetails { get; set; }
        public Array get_unratedemployeelist { get; set; }
        public Array get_feedbackTrainer { get; set; }
        public Array get_feedbackTrainee { get; set; }
        public Array get_feedback { get; set; }
        public long plannerlength { get; set; }
        public decimal? dailyreportcount { get; set; }
        public decimal? dailyreportothersdatecount { get; set; }
        public decimal? dailyreportblockedcount { get; set; }
        public bool Approvalflag { get; set; }
        public bool othersdayenable { get; set; }
        public Array dailyreportextensiondotherdate { get; set; }
        public Array assetsmappingdrblock { get; set; }
        public long? extensiondays { get; set; }
        public Array getsearchdailyreport { get; set; }
        public Array getdeviationreport { get; set; }
        public Array getdepartwisedeviationremrks { get; set; }
        public Array trainingMonitorPopUp { get; set; }
        public Array getdepartwisecategory { get; set; }
        public Array get_Status { get; set; }
        public Array get_report_effort { get; set; }
        public Array ISM_RegeneratedDR { get; set; }
        public string todayorothers { get; set; }
        public string reason { get; set; }
        public bool ISMDRPT_OrdersDateFlg { get; set; }
        public string ISMDRPT_OthersDateReason { get; set; }
        public string ISMDRPT_JeotaggedPhoto { get; set; }
        public string ISMDRPT_JeotaggedPhotoFath { get; set; }
        public string ISMDRPT_LocationLatLogValue { get; set; }
        public string ISMDRPT_LocationName { get; set; }
        public Array fileupload { get; set; }
        // EMAIL DETAILS
        public string employeename { get; set; }
        public string department { get; set; }
        public string desgination { get; set; }
        public string emailid { get; set; }
        public string gradename { get; set; }
        public string message { get; set; }
        public List<ISM_DailyReportGenerationDTO> getdeviationreportnew { get; set; }
        public decimal? completedper { get; set; }
        public decimal? deviationper { get; set; }
        public string ISMTCR_TaskNo { get; set; }
        public string HRMP_Name { get; set; }
        public string ISMTCR_Desc { get; set; }
        public string ISMDRPT_Statusnew { get; set; }
        public string ISMTCR_BugOREnhancementFlg { get; set; }
        public string ISMDRPT_TimeTakenInHrsnew { get; set; }
        public string ISMMCLT_ClientName { get; set; }
        public string assignedby { get; set; }
        public DateTime? ISMDRPT_Datenew { get; set; }
        public Array getextratasks { get; set; }
        public Array gettaskdetails { get; set; }
        public Array getemployeedetails { get; set; }
        public Array get_effortdetails { get; set; }
        public Array getdrnotsentdetails { get; set; }
        public long? HRMD_Id { get; set; }
        public long blockedcount { get; set; }
        public bool ISMDRPT_BlockedFlag { get; set; }
        public Array deviceArray { get; set; }
        public bool? ISMDRPT_HalfDayFlag { get; set; }
        public Array blockeddetails { get; set; }

        //=== Consolidate Report
        public string startdate { get; set; }
        public string enddate { get; set; }
        public Array get_report { get; set; }
        public Array demo_list { get; set; }
        //********** Notification **********//
        public Array notificationUser { get; set; }
        public Array deviation_create { get; set; }
        public long deviation_id { get; set; }
        public DateTime? startweek { get; set; }
        public DateTime? endweek { get; set; }
        public string Role_flag { get; set; }
        public bool roleflag { get; set; }
        public long IVRMRT_Id { get; set; }
        public string empname { get; set; }
        public string empdepartment { get; set; }
        public string reporttype { get; set; }
        public Array DepartmentList { get; set; }
        public Array Designation { get; set; }
        public Array get_userEmplist { get; set; }
        public Array get_userEmplistDemoRating { get; set; }
        public deptlist[] deptlist { get; set; }
        public desglist[] desglist { get; set; }
        public emplist[] emplist { get; set; }
        public moduletemp_dto[] moduletemp_dto { get; set; }
        public statustemp_dto[] statustemp_dto { get; set; }
        public clienttemp_dto[] clienttemp_dto { get; set; }
        public DateTime? start_date { get; set; }
        public DateTime? end_date { get; set; }
        public Array getplannerdetails { get; set; }
        public Array getgenerateddailyreportdetails { get; set; }
        public Array get_taskdetails { get; set; }
        public Array get_taskresponse { get; set; }
        public Array get_taskattachments { get; set; }
        public Array get_responseattachments { get; set; }
        public Array moduledropdown { get; set; }
        public Array clientdropdown { get; set; }
        public string ISMDRPT_FilePath { get; set; }
        public string ISMDRPT_FileName { get; set; }
        public Department_tempDTO[] Department_tempDTO { get; set; }
        public long? YesterdaysNotGeneratedCount { get; set; }
        public string DRNotSentRemarks { get; set; }
        public DateTime? ISMDRNSDR_DRNotSentDate { get; set; }
        public DateTime? ISMDRNSDR_RemarksDate { get; set; }
        public Array gettaskcategorychecklist { get; set; }
        public Array getchecklistarray { get; set; }
        public string ISMDRPTFL_FileName { get; set; }
        public string ISMDRPTFL_RefNo { get; set; }
        public string ISMDRPTFL_Remarks { get; set; }
        public string ISMDRPTFL_FilePath { get; set; }
        public string ISMMTCATCL_CheckListName { get; set; }
        public long ISMMTCATCL_Id { get; set; }
        public long ISMDRPTFL_Id { get; set; }
        public SaveDRNotSentRemarksDetails[] SaveDRNotSentRemarksDetails { get; set; }
        public Array drnotapprovedmessage { get; set; }
        public Array drnotapprovedmessageindividual { get; set; }
        public Array drblockeddetailslist { get; set; }
        public Array getfiledetails { get; set; }
        public Array daywise_drreport { get; set; }
        public Array droveralldetailsdatewise { get; set; }
        public long? ISMBE_Id { get; set; }
        public long? ISMEMN_ID { get; set; }
        public string ISMBE_Reason { get; set; }
        public long? ISMTPLTA_Id { get; set; }
        public long? ISMDR_Id { get; set; }
        public long? totalworkinghrsflag { get; set; }
        public string StatusFlg { get; set; }

        //HR Planner Details
        public string HRPLMG_ReminderContent { get; set; }
        public string HRPLMG_PlannerDetails { get; set; }
        public Array hrplannerdetails { get; set; }
        public Array CloseTaskCoutnDetails { get; set; }
        //added by sanjeev RTO
        public Array ReminderList { get; set; }
        public Array RTO_FOemplist { get; set; }
        public Array RTO_VehcalList { get; set; }
       public Array AdavanceList { get; set; }
        public Array ApplyList { get; set; }
        public string DRNotSentDate { get; set; }
        public Array DR_Generationreject { get; set; }
        public Array proxyNotMapped { get; set; }
        
    }
    public class Temp_ISM_DailyReportGenerationDTO
    {
        public long ISMDRPT_Id { get; set; }
        public long ISMTPL_Id { get; set; }
        public long ISMTCR_Id { get; set; }
        public string ISMDRPT_Remarks { get; set; }
        public decimal? ISMDRPT_TimeTakenInHrs { get; set; }
        public string ISMDRPT_Status { get; set; }
        public long extraflag { get; set; }
        public DateTime? ISMTPLTA_StartDate { get; set; }
        public DateTime? ISMTPLTA_EndDate { get; set; }
        public decimal? ISMTPLTA_EffortInHrs { get; set; }
        public string ISMTPLTA_Remarks { get; set; }
        public bool deviationflag { get; set; }
        public long? ISMDR_Id { get; set; }
        public long periodicityflag { get; set; }
        public string ISMDRPT_TimeTakenInHrsmin { get; set; }
        public string periodicityendflag { get; set; }
        public string ISMTAPL_Periodicity { get; set; }        
        public long? ISMTPLTA_Id { get; set; }
        public bool? ISMDRPT_CompoffApplicableflag { get; set; }
        public string[] attachmentArray { get; set; }
        public Temp_CategoryCheckList[] categorychecklist { get; set; }
        public Temp_TaskReponseList[] Temp_TaskReponseList { get; set; }
    }
    public class notificationdetailsDTO
    {
        public long HRME_Id { get; set; }
    }

    public class deptlist
    {
        public long HRMDC_ID { get; set; }
    }
    public class desglist
    {
        public long HRMDES_Id { get; set; }
    }
    public class emplist
    {
        public long HRME_Id { get; set; }
    }
    public class Department_tempDTO
    {
        public int HRMDC_ID { get; set; }
    }
    public class moduletemp_dto
    {
        public long IVRMM_Id { get; set; }
    }
    public class statustemp_dto
    {
        public string ISMMISTS_StatusName { get; set; }
    }
    public class clienttemp_dto
    {
        public long ISMMCLT_Id { get; set; }
    }
    public class Temp_CategoryCheckList
    {
        public string checklistname { get; set; }       
        public string filepath { get; set; }
        public string filename { get; set; }
        public string comments { get; set; }
        public string refno { get; set; }
        public long ISMMTCATCL_Id { get; set; }        
                      
    }
    public class Temp_TaskReponseList
    {       
        public string filepath { get; set; }
        public string filename { get; set; }                     
    }

    public class SaveDRNotSentRemarksDetails
    {
        public DateTime? drnotsentdate { get; set; }
        public string remarks { get; set; }
        public string Template { get; set; }
    }
}
