using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.IssueManager
{
    public class ISM_DailyReport_ApprovalDTO
    {
        public long MI_Id { get; set; }
        public long ISMTCR_Id { get; set; }
        public long ISMDRPT_Id { get; set; }
        public long UserId { get; set; }
        public long ASMAY_Id { get; set; }
        public string Role_flag { get; set; }
        public string empname { get; set; }
        public string empdepartment { get; set; }
        public long IVRMRT_Id { get; set; }
        public bool roleflag { get; set; }
        public bool returnval { get; set; }
        public Array get_userEmplist { get; set; }
        public Array DepartmentList { get; set; }
        public Array Designation { get; set; }
        public Array getchecklistarray { get; set; }
        public Array getsaveddetails { get; set; }
        public Array empwise_statuslist { get; set; }
        public Array getdrnotsetreport { get; set; }
        public Array filldata { get; set; }
        public DepartmentDTO[] departmentlist { get; set; }
        public DesignationDTO[] designationlist { get; set; }
        public deptlist_Temp[] deptlist { get; set; }
        public desglist_Temp[] desglist { get; set; }
        public emplist_Temp[] emplist { get; set; }
        public selectedapprovaldetails[] selectedapprovaldetails { get; set; }
        public selectedemployeedetails[] selectedemployeedetails { get; set; }
        public DateTime Fromdate { get; set; }
        public DateTime previousdate { get; set; }
        public DateTime Todate { get; set; }
        public string ISMDRPTFL_FileName { get; set; }
        public string ISMDRPTFL_RefNo { get; set; }
        public string ISMDRPTFL_Remarks { get; set; }
        public string ISMDRPTFL_FilePath { get; set; }
        public string ISMMTCATCL_CheckListName { get; set; }
        public string employeename { get; set; }
        public string department { get; set; }
        public string desgination { get; set; }
        public string emailid { get; set; }
        public string gradename { get; set; }
        public string HRME_AppDownloadedDeviceId { get; set; }
        public string sound { get; set; }
        public long ISMMTCATCL_Id { get; set; }
        public long? HRMD_Id { get; set; }
        public long HRME_Id { get; set; }
        public string Concallmsg { get; set; }
        public Array get_taskdetails { get; set; }
        public Array get_taskresponse { get; set; }
        public Array get_taskattachments { get; set; }
        public Array get_responseattachments { get; set; }
        public Array notificationUser { get; set; }
        public Array deviceArray { get; set; }
        public Array get_unratedemployeelist { get; set; }
        public Array drnotapprovedmessage { get; set; }
        public Array monthlist { get; set; }
        public Array yearlist { get; set; }
        public long MonthId { get; set; }
        public long YearId { get; set; }
        public string TemplateName { get; set; }
        public string DRApprovalTypeFlag { get; set; }
        public Array gettaskwisecommentsdetails { get; set; }
        public decimal? ISMDRPTDWMAPP_ApprovedEffort { get; set; }
        public long ISMDRPTDWMAPP_ApprovedHRME_Id { get; set; }
        public string ISMDRPTDWMAPP_Remarks { get; set; }
        public DateTime ISMDRPTDWMAPP_CreatedDate { get; set; }
        public Array GetDaywisedetails { get; set; }
        //added by sanjeev
        public Array AdavanceList { get; set; }
        public Array ApplyList { get; set; }
        //added by praveen
        public Array get_leaveDetails { get; set; }
        public Array Cerfificatelist { get; set; }        
        public Array compoffdetails { get; set; }
        public Array NOCdetails { get; set; }
    }

    public class deptlist_Temp
    {
        public long HRMDC_ID { get; set; }
    }
    public class desglist_Temp
    {
        public long HRMDES_Id { get; set; }
    }
    public class emplist_Temp
    {
        public long HRME_Id { get; set; }
    }
    public class selectedapprovaldetails
    {
        public bool Approvedflag { get; set; }
        public decimal? Efforts { get; set; }
        public string Remarks { get; set; }
        public string ISMDRPT_TimeTakenInHrsmin { get; set; }
        public string Task_Response { get; set; }
        public long ISMDRPT_Id { get; set; }
        public long ISMTCR_Id { get; set; }
        public long HRME_Id { get; set; }
        public long ISMTPLTA_Id { get; set; }   
        public string ISMDRPT_Remarks { get; set; }
        public decimal? ISMDRPT_TimeTakenInHrs { get; set; }
        public decimal? ISMDRPT_ApprovedTime { get; set; }

        public bool ConCallApprovedflag { get; set; }
        public string ISMMTCAT_TaskCategoryName { get; set; }


    }

    public class selectedemployeedetails
    {
        public long HRME_Id { get; set; }
        public string employeename { get; set; }
        public string remarks { get; set; }
        public DateTime? fromdate { get; set; }
        public decimal? approvedefforts { get; set; }
        public selectedapprovaldetails[] selectedapprovaldetails { get; set; }
    }

}
