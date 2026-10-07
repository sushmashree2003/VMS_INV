using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.IssueManager
{
    public class ISM_ComplaintsTaskListDTO : CommonParamDTO
    {
        public Master_NumberingDTO transnumbconfigurationsettingsss { get; set; }
        public string returnduplicatestatus { get; set; }
        public bool returnval { get; set; }
        public bool transfer  { get; set; }
        public bool already_cnt { get; set; }
        public bool taskeditable { get; set; }
        public bool assigntaskeditable { get; set; }
        public string message { get; set; }
        public long UserId { get; set; }
        public string Auto_id { get; set; }
        public string HRMEM_EmailId { get; set; }
        public string Role_flag { get; set; }
        public string roletype { get; set; }

        public long HRMDC_ID { get; set; }
        public string HRMDC_Name { get; set; }
        public int HRMDC_Order { get; set; }

        public ISM_ComplaintsTaskListDTO[] savegroup { get; set; }
        public long IVRMRT_Id { get; set; }
        public string sound { get; set; }
        public Array arrlistchk { get; set; }
        public DateTime localIndianTime { get; set; }

        public long ISMMAC_Id { get; set; }
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public string ISMMAC_ActivityName { get; set; }
        public long HRMD_Id { get; set; }
        public long ISMMPR_Id { get; set; }
        public string ISMMPR_ProjectName { get; set; }

        public long HRME_Id { get; set; }
        public string HRME_EmployeeFirstName { get; set; }
        public string HRME_EmployeeMiddleName { get; set; }
        public string HRME_EmployeeLastName { get; set; }

        public string HRMD_DepartmentName { get; set; }
        public long IVRMM_Id { get; set; }
        public string IVRMM_ModuleName { get; set; }
        public string IVRMM_ModuleDesc { get; set; }
        public long ISMMMD_Id { get; set; }
        public long ISMMMD_ModuleHeadId { get; set; }
        public long ISMMCLT_Id { get; set; }
        public long ISMCIM_IEList { get; set; }
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

        public long taskCount { get; set; }
        public long openCount { get; set; }
        public long closedCount { get; set; }
        public long completedCount { get; set; }
        public long inprogressCount { get; set; }

        public long ISMTCRASTO_Id { get; set; }
        public DateTime ISMTCRASTO_AssignedDate { get; set; }
        public long ISMTCRASTO_AssignedBy { get; set; }
        public string ISMTCRASTO_Remarks { get; set; }
        public DateTime? ISMTCRASTO_StartDate { get; set; }
        public DateTime? ISMTCRASTO_EndDate { get; set; }
        public decimal ISMTCRASTO_EffortInHrs { get; set; }
        public bool ISMTCRASTO_ActiveFlg { get; set; }

        public long ISMTCRTRTO_Id { get; set; }
        public DateTime ISMTCRTRTO_TransferredDate { get; set; }
        public long ISMTCRTRTO_TransferredBy { get; set; }
        public string ISMTCRTRTO_Remarks { get; set; }
        public DateTime? ISMTCRTRTO_StartDate { get; set; }
        public DateTime? ISMTCRTRTO_EndDate { get; set; }
        public decimal ISMTCRTRTO_EffortInHrs { get; set; }
        public bool ISMTCRTRTO_ActiveFlg { get; set; }
        public long ISMTCRTRTO_CreatedBy { get; set; }
        public long ISMTCRTRTO_UpdatedBy { get; set; }

        public long ISMTCRRES_Id { get; set; }
        public DateTime ISMTCRRES_ResponseDate { get; set; }
        public string ISMTCRRES_Response { get; set; }
        public string ISMTCRRES_ResponseAttachment { get; set; }
        public bool ISMTCRRES_ActiveFlg { get; set; }
        public long ISMTCRRES_CreatedBy { get; set; }
        public long ISMTCRRES_UpdatedBy { get; set; }
        public string ISMMISTS_StatusName { get; set; }
        public string typeFlg { get; set; }
        public string countflag { get; set; }

        public string durationFlg { get; set; }
        public decimal eachTaskMaxDuration { get; set; }
        public string taskdescription { get; set; }
        public bool delettask { get; set; }

        public long byMecount { get; set; }
        public long toMecount { get; set; }
        public long Mycountonly  { get; set; }
        public Array get_priority { get; set; }
        public long transferMecount { get; set; }
        public long transferMecountonly { get; set; }
        public long transferbycount { get; set; }
        public Array totaltaskcount { get; set; }
        public string HRME_AppDownloadedDeviceId { get; set; }
        public long HRME_To { get; set; }

        public string[] attachmentArray { get; set; }
        public Array get_comptasklist { get; set; }
        public Array getmy_comptasklist { get; set; }
        public Array Rejected_comptasklist { get; set; }
        public Array assigntask_ids { get; set; }
        public Array get_client { get; set; }
        public Array get_Status { get; set; }
        public Array get_employeelist { get; set; }
        public Array get_department { get; set; }
        public Array get_userEmplist { get; set; }
        public Array get_taskdetails { get; set; }
        public Array get_taskattachments { get; set; }
        public Array get_responseattachments { get; set; }
        public Array Temparray { get; set; }
        public Array Temparray2 { get; set; }
        public Array get_taskresponse { get; set; }
        public Array get_Attdetails { get; set; }
        public Array get_taskproject { get; set; }
        public Array get_taskmodule { get; set; }
        public Array get_taskcount { get; set; }
        public Array get_pendingcount { get; set; }
        public Array get_tasks { get; set; }
        public Array completedcreatedbyme { get; set; }
        public Array get_tasks_completed { get; set; }
        public Array deviceArray { get; set; }
      
        public plannerArrayDTO[] plannerArray { get; set; }
        public mutlitaskstatusupdatelist[] savemultipletaskupdatearray  { get; set; }
        public taskEmpArrayDTO[] taskEmpArray { get; set; }
        public transferArrayDTO[] transferArray { get; set; }
        public arrayuserEmpDTO[] arrayuserEmp { get; set; }
        public arrayClientDTO[] arrayClient { get; set; }
        public arrayStatusDTO[] arrayStatus { get; set; }
        public hrme_list1[] hrme_list { get; set; }

        //********************************************************* Client 
        public Array get_clientcomptasklist { get; set; }
        public Array get_globalsearchtask { get; set; }
        public Array transfer_plannerUpdate  { get; set; }
        #region Pooja
        public string flag { get; set; }
        public string ISMTCR_Ids { get; set; }
        #endregion
    }
    public class plannerArrayDTO
    {
        public long ISMTCR_Id { get; set; }
        public string ISMTCRASTO_Remarks { get; set; }
        public DateTime? ISMTCRASTO_StartDate { get; set; }
        public DateTime? ISMTCRASTO_EndDate { get; set; }
        public decimal ISMTCRASTO_EffortInHrs { get; set; }
    }
    public class mutlitaskstatusupdatelist
    {
        public long ISMTCR_Id { get; set; }
        public string ISMTCRRES_Response { get; set; }
        public string ISMMISTS_StatusName { get; set; }
        public string ISMTCR_Desc { get; set; }
    }
    public class taskEmpArrayDTO
    {
        public long ISMTCR_Id { get; set; }
        public long HRME_Id { get; set; }
        public long? HRMPR_Id { get; set; }
    }
    public class transferArrayDTO
    {
        public long ISMTCRTRTO_Id { get; set; }
        public long ISMTCR_Id { get; set; }
        public long HRMD_Id { get; set; }
        public long HRME_Id { get; set; }
        public DateTime ISMTCRTRTO_TransferredDate { get; set; }
        public long ISMTCRTRTO_TransferredBy { get; set; }
        public string ISMTCRTRTO_Remarks { get; set; }
        public DateTime? ISMTCRTRTO_StartDate { get; set; }
        public DateTime? ISMTCRTRTO_EndDate { get; set; }
        public decimal ISMTCRTRTO_EffortInHrs { get; set; }
    }
    public class arrayuserEmpDTO
    {
        public long HRME_Id { get; set; }
        public long UserId { get; set; }

    }
    public class arrayClientDTO
    {
        public long ISMMCLT_Id { get; set; }
        public long UserId { get; set; }

    }
    public class arrayStatusDTO
    {
        public long ISMMISTS_Id { get; set; }  
        public string ISMMISTS_StatusName { get; set; }
    }
    public class hrme_list1
    {
        public long HRME_Id { get; set; }
    }
}
