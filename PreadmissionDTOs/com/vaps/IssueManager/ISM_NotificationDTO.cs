using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.IssueManager
{
    public class ISM_NotificationDTO : CommonParamDTO
    {
        public string MI_Logo { get; set; }
        public string message { get; set; }
        public long UserId { get; set; }    
        public string Role_flag { get; set; }
        public string ISMBE_Reason  { get; set; }
        public long MI_Id { get; set; }
        public long ISMTCR_Id { get; set; }
        public long? ISMBE_Id { get; set; }
        public long? ISMEMN_ID { get; set; } 
        public long HRME_Id { get; set; }
        public DateTime? drdate { get; set; }
        public long ISMNO_Id { get; set; }       
        public string ISMNO_Notification { get; set; }
        public DateTime ISMNO_NoticationDate { get; set; }
        public string ISMNO_NotificationType { get; set; }
        public bool ISMNO_ReadFlg { get; set; }
        public bool ISMNO_MakeUnReadFlg { get; set; }
        public bool ISMNO_ActiveFlag { get; set; }
        public long ISMNO_CreatedBy { get; set; }
        public long ISMNO_UpdatedBy { get; set; }
        public Array get_unauthorizenotice { get; set; }
        public Array get_notifications { get; set; }     
        public Array get_notificationsdetails { get; set; }     
        public Array getemployeedetails { get; set; }     
        public Array get_dailyreportdetails { get; set; }              
        public Array get_dailyreportdeviation { get; set; }
        public Array get_plannerdata { get; set; }
        public Array get_plannerdetails { get; set; }

        public string employeename { get; set; }
        public DateTime? startdate { get; set; }
        public DateTime? FOEP_PunchDate { get; set; }
        public DateTime? HRMERT_Enddate { get; set; }
        public DateTime? enddate { get; set; }
        public string empcode { get; set; }
        public string EmployeeName { get; set; }
        public string MI_Name { get; set; }
        public string HRME_EmployeeCode { get; set; }
        public string HRMDES_DesignationName { get; set; }
        public string department { get; set; }
        public string pushtocken  { get; set; }
        public string servicekey   { get; set; }


    }
}
