using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.IssueManager
{
    public class ISM_PlannerCreationDTO : CommonParamDTO
    {
        public Master_NumberingDTO transnumbconfigurationsettingsss { get; set; }
        public string returnduplicatestatus { get; set; }
        public bool returnval { get; set; }
        public bool already_cnt { get; set; }
        public string message { get; set; }
        public long UserId { get; set; }
        public string Auto_id { get; set; }
        public long ISMMAC_Id { get; set; }
        public long MI_Id { get; set; }
        public long completdcount { get; set; }
        public string sound { get; set; }
        public string Role_flag { get; set; }
        public DateTime? startDate { get; set; }
        public DateTime? endDate { get; set; }
        public string employeename { get; set; }
        public string departmnt { get; set; }
        public string desgination { get; set; }
        public string gradename { get; set; }
        public string approvedBy { get; set; }
        public string emailid { get; set; }
        public DateTime? ISMDRPT_Date { get; set; }
        public long IVRMRT_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long HRMD_Id { get; set; }
        public long ISMMPR_Id { get; set; }        
        public long HRME_Id { get; set; }
        public long? view { get; set; } 
        public string HRME_EmployeeFirstName { get; set; }
        public string HRME_EmployeeMiddleName { get; set; }
        public string HRME_EmployeeLastName { get; set; }
        public string HRMD_DepartmentName { get; set; }
        public long IVRMM_Id { get; set; }
        public string IVRMM_ModuleName { get; set; }
        public string IVRMM_ModuleDesc { get; set; }
        public long ISMMMD_Id { get; set; }
        public long? ISMMCLT_Id { get; set; }
        public string ISMMCLT_ClientName { get; set; }
        public long ISMTCR_Id { get; set; }
        public string ISMTCR_TaskNo { get; set; }
        public string ISMTCR_BugOREnhancementFlg { get; set; }
        public DateTime? ISMTCR_CreationDate { get; set; }
        public string ISMTCR_Title { get; set; }
        public long HRMPR_Id { get; set; }
        public string HRMP_Name { get; set; }
        public string ISMTCR_Desc { get; set; }
        public string ISMTCR_Status { get; set; }
        public bool ISMTCR_ReOpenFlg { get; set; }
        public DateTime? ISMTCR_ReOpenDate { get; set; }
        public bool ISMTCR_ActiveFlg { get; set; }

        public long IVRM_Month_Id { get; set; }
        public string IVRM_Month_Name { get; set; }
        public Array monthlist { get; set; }
        public Array deviationreport { get; set; }
        public Array drnotapprovedmessage { get; set; }
        public Array categorylist { get; set; }
        public Array get_userEmplist { get; set; }
        public bool? ISMTPLTA_PreviousTask { get; set; }
        public long ISMTCRASTO_Id { get; set; }
        public DateTime ISMTCRASTO_AssignedDate { get; set; }
        public long ISMTCRASTO_AssignedBy { get; set; }
        public string ISMTCRASTO_Remarks { get; set; }
        public DateTime? ISMTCRASTO_StartDate { get; set; }
        public DateTime? ISMTCRASTO_EndDate { get; set; }
        public decimal ISMTCRASTO_EffortInHrs { get; set; }
        public bool ISMTCRASTO_ActiveFlg { get; set; }
        public string assignedby { get; set; }
        public string approvedby { get; set; }
        public string ISMTPLTA_EffortInHrsnew { get; set; }
        public string ISMTPLAPTA_EffortInHrsnew { get; set; }

        public long ISMTPLTA_Id { get; set; }
        public long ISMTPL_Id { get; set; }
        public string ISMTPLTA_Remarks { get; set; }
        public DateTime? ISMTPLTA_StartDate { get; set; }
        public DateTime? ISMTPLTA_EndDate { get; set; }
        public decimal ISMTPLTA_EffortInHrs { get; set; }
        public DateTime? ISMTPLTA_FinishedDate { get; set; }
        public string ISMTPLTA_Status { get; set; }
        public string ISMTPLTA_Statusnew { get; set; }
        public string ISMTPLAPTA_Statusnew { get; set; }
        public string taskStatus { get; set; }
        public bool ISMTPLTA_DeviationFlg { get; set; }
        public bool ISMTPLTA_ExtraFlg { get; set; }
        public decimal ISMTPLTA_TimeTakenInHrs { get; set; }

        public bool ISMTPLTA_ActiveFlg { get; set; }
        public long ISMTPLTA_CreatedBy { get; set; }
        public long ISMTPLTA_UpdatedBy { get; set; }

        public long ISMTPL_PlannedBy { get; set; }
        public string ISMTPL_PlannerName { get; set; }
        public string ISMTPL_Remarks { get; set; }
        public DateTime? ISMTPL_StartDate { get; set; }
        public DateTime? ISMTPL_EndDate { get; set; }
        public DateTime? WeekStart { get; set; }
        public DateTime? WeekEnd  { get; set; }
        public decimal ISMTPL_TotalHrs { get; set; }
        public bool? ISMTPL_ApprovalFlg { get; set; }
        public long ISMTPL_ApprovedBy { get; set; }
        public bool ISMTPL_ActiveFlg { get; set; }

        public bool ISMTPLTA_ApprovalFlg { get; set; }
        public DateTime? planDate { get; set; }

        public string plannedby { get; set; }
        public long ISMTPL_CreatedBy { get; set; }
        public long ISMTPL_UpdatedBy { get; set; }
        public DateTime plannerMaxdate { get; set; }
        public DateTime? plannerDate { get; set; }
        public string ISMTCR_BugOREnhancement { get; set; }
        public long? ISMTPLAP_Id { get; set; }
        public string ISMTPLAP_Remarks { get; set; }
        public bool ISMTPLAP_ActiveFlg { get; set; }
        public long ISMTPLAP_CreatedBy { get; set; }
        public long ISMTPLAP_UpdatedBy { get; set; }


        public long ISMTPLAPTA_Id { get; set; }
        public string ISMTPLAPTA_Remarks { get; set; }
        public DateTime? ISMTPLAPTA_StartDate { get; set; }
        public DateTime? ISMTPLAPTA_EndDate { get; set; }
        public decimal ISMTPLAPTA_EffortInHrs { get; set; }
        public DateTime? ISMTPLAPTA_FinishedDate { get; set; }
        public string ISMTPLAPTA_Status { get; set; }
        public bool ISMTPLAPTA_DeviationFlg { get; set; }
        public bool ISMTPLAPTA_ExtraTaskFlg { get; set; }
        public decimal ISMTPLAPTA_TimeTakenInHrs { get; set; }
        public bool? ISMTPLAPTA_RejectedFlg { get; set; }
        public bool? ISMTPLAPTA_ApprovalFlg { get; set; }
        public bool ISMTPLAPTA_UnPlannedFlg { get; set; }
        public bool ISMTPLAPTA_ActiveFlg { get; set; }
        public long ISMTPLAPTA_CreatedBy { get; set; }
        public long ISMTPLAPTA_UpdatedBy { get; set; }
        public long? plannerStatus { get; set; }
        public bool flgapproved { get; set; }
        public bool? flgrejected { get; set; }
        public long extraflag { get; set; }

        public Array RTO_FOemplist { get; set; }
        public Array get_Assignedtasklist { get; set; }
        public Array get_client { get; set; }
        public Array get_Status { get; set; }
        public Array holidaylist { get; set; }
        public Array get_employeelist { get; set; }
        public Array get_department { get; set; }
        public Array get_taskdetails { get; set; }
        public Array get_effortdetails { get; set; }
        public Array get_plannerdetails { get; set; }
        public Array get_plannerdetails_daily  { get; set; }
        public Array get_plannercategory  { get; set; }
        public Array get_plannerApprovaldetails { get; set; }
        public Array get_Attdetails { get; set; }
        public Array get_taskproject { get; set; }
        public Array get_taskmodule { get; set; }
        public Array get_plannerlist { get; set; }
        public Array get_Memolist { get; set; }
        public Array get_plannermax { get; set; }
        public Array pendingfeedback { get; set; }
        public Array get_updatedplannerlist { get; set; }
        public Array get_extratasklist { get; set; }
        public Array get_institute { get; set; }
        public Array get_ApprovedPlannerDetail { get; set; }
        public Array deviceArray { get; set; }
        public Array plannerextension { get; set; }
        public Array completedcreatedbyme { get; set; }        
        public decimal? totaleffort { get; set; }

        public bool plannerextapproval { get; set; }
        public addTaskArrayDTO[] addTaskArray { get; set; }
        public addExtraTaskArrayDTO[] addExtraTaskArray { get; set; }

        public taskplannerArrayDTO[] taskplannerArray { get; set; }
        public approveplannerArrayDTO[] approveplannerArray { get; set; }
        public miarrayDTO[] miarray { get; set; }
        public emparrayDTO[] emparray { get; set; }

        public Emplist[] Emplist { get; set; }
        public catlistid[] catlistid { get; set; }      
        public long ISMMTCAT_Id { get; set; }
        public string ISMMTCAT_TaskCategoryName { get; set; }
        public decimal ISMMTCAT_TaskPercentage { get; set; }
        
        public string ISMMTCAT_EachTaskMaxDuration { get; set; }
        public string ISMMTCAT_DurationFlg { get; set; }
        public bool ISMMTCAT_CompulsoryFlg { get; set; }
        public Array RTO_VehcalList { get; set; }

        //Added by Praveen
        public Array get_leaveDetails { get; set; }
        public Array planner_Rejectlist { get; set; }
        public Array Cerfificatelist { get; set; }
    }

    public class Emplist 
    {
        public long HRME_Id { get; set; }

        public long ISMTPL_Id  { get; set; }

        public long MI_Id { get; set; }
        public decimal ISMTPL_TotalHrs { get; set; }

    }
    public class addTaskArrayDTO
    {
        public long ISMTCRASTO_Id { get; set; }
        public long ISMTCR_Id { get; set; }
        public string ISMTCRASTO_Remarks { get; set; }
        public DateTime? ISMTCRASTO_StartDate { get; set; }
        public DateTime? ISMTCRASTO_EndDate { get; set; }
        public decimal ISMTCRASTO_EffortInHrs { get; set; }
    }

    public class addExtraTaskArrayDTO
    {
        public long ISMTPLTA_Id { get; set; }
        public long ISMTPL_Id { get; set; }
        public long ISMTCR_Id { get; set; }
        public string ISMTPLTA_Remarks { get; set; }
        public DateTime? ISMTPLTA_StartDate { get; set; }
        public DateTime? ISMTPLTA_EndDate { get; set; }
        public decimal ISMTPLTA_EffortInHrs { get; set; }
        public DateTime? ISMTPLTA_FinishedDate { get; set; }
        public string ISMTPLTA_Status { get; set; }
        public bool ISMTPLTA_DeviationFlg { get; set; }
        public bool ISMTPLTA_ExtraFlg { get; set; }
        public decimal ISMTPLTA_TimeTakenInHrs { get; set; }
    }


    public class taskplannerArrayDTO
    {
        public long ISMTPLTA_Id { get; set; }
        public long ISMTPL_Id { get; set; }
        public long ISMTCR_Id { get; set; }
        public string ISMTPLTA_Remarks { get; set; }
        public DateTime? ISMTPLTA_StartDate { get; set; }
        public DateTime? ISMTPLTA_EndDate { get; set; }
        public decimal ISMTPLTA_EffortInHrs { get; set; }
        public DateTime? ISMTPLTA_FinishedDate { get; set; }
        public string ISMTPLTA_Status { get; set; }
        public bool ISMTPLTA_DeviationFlg { get; set; }
        public bool ISMTPLTA_ExtraFlg { get; set; }
        public decimal ISMTPLTA_TimeTakenInHrs { get; set; }
        public bool ISMTPLAPTA_RejectedFlg { get; set; }
        public bool ISMTPLAPTA_UnPlannedFlg { get; set; }
        public bool ISMTPLTA_ActiveFlg { get; set; }
        public string ISMTPLTA_PreviousTask { get; set; }
        public string ISMTPLTA_ApprovalFlg { get; set; }
    }

    public class approveplannerArrayDTO
    {
        public long ISMTPLAPTA_Id { get; set; }
        public long ISMTPLAP_Id { get; set; }
        public long ISMTCR_Id { get; set; }
        public string ISMTPLAPTA_Remarks { get; set; }
        public DateTime? ISMTPLAPTA_StartDate { get; set; }
        public DateTime? ISMTPLAPTA_EndDate { get; set; }
        public decimal ISMTPLAPTA_EffortInHrs { get; set; }
        public DateTime? ISMTPLAPTA_FinishedDate { get; set; }
        public string ISMTPLAPTA_Status { get; set; }
        public bool ISMTPLAPTA_DeviationFlg { get; set; }
        public bool ISMTPLAPTA_ExtraTaskFlg { get; set; }
        public decimal ISMTPLAPTA_TimeTakenInHrs { get; set; }
        public bool? ISMTPLAPTA_ApprovalFlg { get; set; }
        public bool? ISMTPLAPTA_RejectedFlg { get; set; }
        public bool ISMTPLAPTA_UnPlannedFlg { get; set; }
        public bool ISMTPLAPTA_ActiveFlg { get; set; }
        public long ISMTPLAPTA_CreatedBy { get; set; }
        public long ISMTPLAPTA_UpdatedBy { get; set; }
        public long? plannerStatus { get; set; }          
        public long ISMTPLTA_Id { get; set; }

        public long extraflag { get; set; }
        public long? PTSCount { get; set; }
        
    }
    public class miarrayDTO
    {
        public long MI_Id { get; set; }
        public string MI_Name { get; set; }
    }
    public class emparrayDTO
    {
        public long HRME_Id { get; set; }
        public long UserId { get; set; }
    }
    public class catlistid
    {
        public long? ISMMTCAT_Id { get; set; }
        
    }


}
