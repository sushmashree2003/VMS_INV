using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.IssueManager
{
    public class ISM_ExtensionDTO : CommonParamDTO
    {    
        public string returnduplicatestatus { get; set; }
        public bool returnval { get; set; }
        public bool already_cnt { get; set; }
        public string message { get; set; }
        public long UserId { get; set; }       
        public string Role_flag { get; set; }
        public string roletype { get; set; }
        public long IVRMRT_Id { get; set; }
        public string sound { get; set; }
        public long MI_Id { get; set; }

        //===================================== Daily Report Extension
        public long ISMODE_Id { get; set; }
        public long HRME_Id { get; set; }
        public string ISMODE_Reason { get; set; }
        public string ISMODE_Noofdays { get; set; }
        public DateTime ISMODE_Date { get; set; }
        public bool ISMODE_ActiveFlg { get; set; }
        public bool returnvalnot { get; set; }
        public long ISMODE_CreatedBy { get; set; }
        public long ISMODE_UpdatedBy { get; set; }

        public bool email_chk { get; set; }
        public bool notifi_chk { get; set; }
        public int scnt { get; set; }
        public int fcnt { get; set; }
        public string ret { get; set; }
        public string emailStatus { get; set; }

        //=================================== Planner Extension
        public long ISMPLE_Id { get; set; }     
        public DateTime ISMPLE_FromDate { get; set; }
        public DateTime ISMPLE_ToDate { get; set; }
        public string ISMPLE_Reason { get; set; }
        public bool ISMPLE_ActiveFlg { get; set; }
        public long ISMPLE_CreatedBy { get; set; }
        public long ISMPLE_UpdatedBy { get; set; }

        public long IVRM_Month_Id { get; set; }
        public string IVRM_Month_Name { get; set; }
        public string ExtensionType { get; set; }
        public string employeename { get; set; }
        public string byemployeename  { get; set; }
        public string HRME_EmployeeCode { get; set; }

        public string empname { get; set; }
        public Array get_Emplist { get; set; }    
        public Array get_month { get; set; }    
        public Array get_totalotherdays { get; set; }    
        public Array get_DailyExtension { get; set; }    
        public Array get_PlannerExtension { get; set; }
        public Array tADA_Extension { get; set; }
        public Array tAADA_Extension { get; set; }
        

        //=================================== TADA Extension
        public long TADAPLE_Id { get; set; }
        public DateTime TADAPLE_FromDate { get; set; }
        public DateTime TADAPLE_ToDate { get; set; }
        public string TADAPLE_Reason { get; set; }
        public bool TADAPLE_ActiveFlag { get; set; }
        public string TADAPLE_Statusflag { get; set; }
        public long TADAPLE_CreatedBy { get; set; }
        public long TADAPLE_UpdatedBy { get; set; }
    }
}
