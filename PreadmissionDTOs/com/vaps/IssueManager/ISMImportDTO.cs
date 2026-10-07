using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.IssueManager
{
    public class ISMImportDTO : CommonParamDTO
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
        public string Role_flag { get; set; }
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
        public string HRME_EmployeeFirstName { get; set; }
        public string HRME_EmployeeMiddleName { get; set; }
        public string HRME_EmployeeLastName { get; set; }
        public string HRMD_DepartmentName { get; set; }
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
        public string ISMTCR_Desc { get; set; }
        public string ISMTCR_Status { get; set; }
        public bool ISMTCR_ReOpenFlg { get; set; }
        public DateTime? ISMTCR_ReOpenDate { get; set; }
        public bool ISMTCR_ActiveFlg { get; set; }

        public long ISMTCRASTO_Id { get; set; }
        public DateTime ISMTCRASTO_AssignedDate { get; set; }
        public long ISMTCRASTO_AssignedBy { get; set; }
        public string ISMTCRASTO_Remarks { get; set; }
        public DateTime? ISMTCRASTO_StartDate { get; set; }
        public DateTime? ISMTCRASTO_EndDate { get; set; }
        public long ISMTCRASTO_EffortInHrs { get; set; }
        public bool ISMTCRASTO_ActiveFlg { get; set; }
        public string assignedby { get; set; }
        public int suscnt { get; set; }
        public int duplicate  { get; set; }
        public int failcnt { get; set; }
        public string approvedby { get; set; }
        public string ISMTPLTA_EffortInHrsnew { get; set; }

        public long ISMTPLTA_Id { get; set; }
        public long ISMTPL_Id { get; set; }
        public string ISMTPLTA_Remarks { get; set; }
        public DateTime? ISMTPLTA_StartDate { get; set; }
        public DateTime? ISMTPLTA_EndDate { get; set; }
        public long ISMTPLTA_EffortInHrs { get; set; }
        public DateTime? ISMTPLTA_FinishedDate { get; set; }
        public string ISMTPLTA_Status { get; set; }
        public string ISMTPLTA_Statusnew { get; set; }
        public string taskStatus { get; set; }
        public bool ISMTPLTA_DeviationFlg { get; set; }
        public bool ISMTPLTA_ExtraFlg { get; set; }
        public long ISMTPLTA_TimeTakenInHrs { get; set; }

        public bool ISMTPLTA_ActiveFlg { get; set; }
        public long ISMTPLTA_CreatedBy { get; set; }
        public long ISMTPLTA_UpdatedBy { get; set; }

        public long ISMTPL_PlannedBy { get; set; }
        public string ISMTPL_PlannerName { get; set; }
        public string ISMTPL_Remarks { get; set; }
        public DateTime? ISMTPL_StartDate { get; set; }
        public DateTime? ISMTPL_EndDate { get; set; }
        public long ISMTPL_TotalHrs { get; set; }
        public bool ISMTPL_ApprovalFlg { get; set; }
        public long ISMTPL_ApprovedBy { get; set; }
        public bool ISMTPL_ActiveFlg { get; set; }
        public long ISMTPL_CreatedBy { get; set; }
        public long ISMTPL_UpdatedBy { get; set; }
        public DateTime plannerMaxdate { get; set; }


        public long ISMTPLAP_Id { get; set; }
        public string ISMTPLAP_Remarks { get; set; }
        public bool ISMTPLAP_ActiveFlg { get; set; }
        public long ISMTPLAP_CreatedBy { get; set; }
        public long ISMTPLAP_UpdatedBy { get; set; }


        public long ISMTPLAPTA_Id { get; set; }
        public string ISMTPLAPTA_Remarks { get; set; }
        public DateTime? ISMTPLAPTA_StartDate { get; set; }
        public DateTime? ISMTPLAPTA_EndDate { get; set; }
        public long ISMTPLAPTA_EffortInHrs { get; set; }
        public DateTime? ISMTPLAPTA_FinishedDate { get; set; }
        public string ISMTPLAPTA_Status { get; set; }
        public bool ISMTPLAPTA_DeviationFlg { get; set; }
        public bool ISMTPLAPTA_ExtraTaskFlg { get; set; }
        public long ISMTPLAPTA_TimeTakenInHrs { get; set; }
        public bool? ISMTPLAPTA_RejectedFlg { get; set; }
        public bool? ISMTPLAPTA_ApprovalFlg { get; set; }
        public bool ISMTPLAPTA_UnPlannedFlg { get; set; }
        public bool ISMTPLAPTA_ActiveFlg { get; set; }
        public long ISMTPLAPTA_CreatedBy { get; set; }
        public long ISMTPLAPTA_UpdatedBy { get; set; }
        public string plannerStatus { get; set; }
        public bool? flgapproved { get; set; }
        public bool? flgrejected { get; set; }

        //adarsh
        public long ISMMTCAT_Id { get; set; }
        public taskemployeeDTO[] taskEmpArray { get; set; }
        public string TaskDay { get; set; }
        public string periodicity { get; set; }
        public DateTime? startdate { get; set; }
        public DateTime? enddate { get; set; }
        public string remarks { get; set; }
        public decimal effortinhrs { get; set; }
        public string TimeRequiredFlg { get; set; }
        public DateTime? Yearlydate { get; set; }
        public long ISMTAPL_Id { get; set; }
        public MulttaskDepartments[] MulttaskDepartments { get; set; }

        public attachmentArray1[] attachmentArray { get; set; }
        public Array getyearplantasklist { get; set; }
        public Array getyearplangridtasklist { get; set; }
        public long ISMMIMPPL_Id { get; set; }

        public DateTime Fromdate { get; set; }
        public DateTime Todate { get; set; }
        public string approvalflg { get; set; }
        public Array get_Assignedtasklist { get; set; }
        public Array get_client { get; set; }
        public Array get_Status { get; set; }
        public Array holidaylist { get; set; }
        public Array get_employeelist { get; set; }
        public Array get_department { get; set; }
        public Array get_taskdetails { get; set; }
        public Array get_effortdetails { get; set; }
        public Array get_plannerdetails { get; set; }
        public Array get_plannerApprovaldetails { get; set; }
        public Array get_Attdetails { get; set; }
        public Array get_taskproject { get; set; }
        public Array get_taskmodule { get; set; }
        public Array get_plannerlist { get; set; }
        public Array get_extratasklist { get; set; }
       // public importdto[] addTaskArray { get; set; }
        public importdto[] addtaskcontent { get; set; }
        public importip[] plannercontent { get; set; }
        public advplnannerdto[] advimppln { get; set; }
        public addExtraTaskArrayDTO[] addExtraTaskArray { get; set; }

        public taskplannerArrayDTO[] taskplannerArray { get; set; }
        public approveplannerArrayDTO[] approveplannerArray { get; set; }

        public yearplannerapproval[] yearplannerapproval { get; set; }



        public swaptasklist[] swaptasklist { get; set; }

    }
    public class importdto
    {
        public string Department { get; set; }
        public string Project { get; set; }
        public string Module { get; set; }
        public string Task_Title { get; set; } 
        public string Priority { get; set; } 
        public string Task_Description { get; set; }
        public string Issue_Type_Activity { get; set; }
    //public string Start_Date { get; set; }
    public string Client { get; set; }
 //public string End_Date { get; set; }
public string  Effort_in_hrs { get; set; } 
public string Remarks { get; set; }
public string Assigned_By_empcode { get; set; }
public string  Assigned_to_empcode { get; set; } 
public string Assigned_By_empcmp { get; set; } 
public string Assigned_to_empcmp { get; set; } 
public string StartDay { get; set; } 
public string StartMonth { get; set; } 
public string StratYear { get; set; } 
public string EndDay { get; set; } 
public string EndMonth { get; set; } 
public string EndYear { get; set; } 

    }
    public class importip
    {
        public string PlannerName { get; set; }
        public string Module { get; set; }
        public string Activity { get; set; }
        public string Remark { get; set; } 
        public string Periodicity { get; set; } 
        public string TimeDuration { get; set; }
        public string DurationType { get; set; }
   

    }

    public class advplnannerdto
    {
        public string Department { get; set; }
        public string Client { get; set; }
        public string Project { get; set; }
        public string Module { get; set; }
        public string TaskTitle { get; set; }
        public string Priority { get; set; }
        public string  Category { get; set; }
        public string TaskDescription { get; set; }
        public string IssueType { get; set; }
        public string Remarks { get; set; }
        public string AssignedByCompanyCode { get; set; }
        public string AssignedByEmployeeCode { get; set; }
        public string AssignedToCompanyCode { get; set; }
        public string  AssignedToEmployeeCode { get; set; }
        public string StartDay { get; set; }
        public string StartMonth { get; set; }
        public string StartYear { get; set; }
        public string EndDay { get; set; }
        public string EndMonth { get; set; }
        public string EndYear { get; set; }
        public string Periodicity { get; set; }
        public string TaskDay { get; set; }
        public string EffortType { get; set; }
        public string Effort { get; set; }
        public string YearlyTaskDay { get; set; }
        public string YearlyTaskMonth { get; set; }
        public string YearlyTaskYear { get; set; }


    }


    public class yearplannerapproval
    {
        public long ISMTAPL_Id { get; set; }

    }

    public class swaptasklist
    {

        public long ISMTCR_Id { get; set; }
        public long ISMTAPL_Id { get; set; }
        public DateTime? swap_start_date { get; set; }
        public DateTime? swap_end_date { get; set; }
        public long ISMMIMPPL_Id { get; set; }

    }


}
