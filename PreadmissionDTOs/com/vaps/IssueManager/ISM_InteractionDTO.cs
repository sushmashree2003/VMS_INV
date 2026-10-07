using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.IssueManager
{
    public class ISM_InteractionDTO : CommonParamDTO
    {
        public Master_NumberingDTO transnumbconfigurationsettingsss { get; set; }
        public string returnduplicatestatus { get; set; }
        public bool returnval { get; set; }
        public bool already_cnt { get; set; }
        public string message { get; set; }
        public long ISMNO_Id { get; set; }
        public long MI_Id { get; set; }
        public long notifycount { get; set; }
        public long MI_Id_r { get; set; }
        public long ASMAY_Id { get; set; }
        public long UserId { get; set; }
        public string Auto_id { get; set; }
        public string Role_flag { get; set; }
        public string roletype { get; set; }
        public long employee_Id { get; set; }
        public long IVRMRT_Id { get; set; }
        public string sound { get; set; }

        public long ISMINTR_Id { get; set; }
        public string ISMINTR_InteractionId { get; set; }
        public long ISMINTR_ComposedByHRME_Id { get; set; }
        public string ISMINTR_GroupOrIndFlg { get; set; }
        public string ISMINTR_Subject { get; set; }
        public DateTime ISMINTR_DateTime { get; set; }
        public string ISMINTR_Interaction { get; set; }
        public bool ISMINTR_ActiveFlag { get; set; }
        public long ISMINTR_CreatedBy { get; set; }
        public long ISMINTR_UpdatedBy { get; set; }

        public long ISMINTRD_Id { get; set; }
        public long ISMINTRD_ToId { get; set; }
        public long ISMINTRD_ComposedById { get; set; }
        public string ISMINTRD_Interaction { get; set; }
        public DateTime ISMINTRD_DateTime { get; set; }
        public int ISMINTRD_InteractionOrder { get; set; }
        public bool ISMINTRD_ActiveFlag { get; set; }
        public long ISMINTRD_CreatedBy { get; set; }
        public long ISMINTRD_UpdatedBy { get; set; }

        public Array get_toEmplist { get; set; }             
        public Array get_inboxMessages { get; set; }             
        public Array get_messageDetail { get; set; }             
        public Array get_replyMessages { get; set; }             
        public arraymessageDMO[] arraymessage { get; set; }      
        public arrayreplyDMO[] arrayreply { get; set; }   
        public feedbackdt1[] feedbackdtl { get; set; }   
        
        //feedback start
        public long inbcount { get; set; }
        public long ISMTCR_Id { get; set; }
        public long ISMDRF_RCV_HRME_Id { get; set; }
        public long ISMDRF_Send_HRME_Id { get; set; }
        public long ISMDRF_Id { get; set; }
        public string sender_empname { get; set; }
        public string HRMD_DepartmentName { get; set; }
        public string ISMDRF_FeedBack { get; set; }
        public Array get_feedbacklist { get; set; }
        public Array feedbackcount { get; set; }
        public Array get_feedbackdetails { get; set; }
        public Array get_feedback { get; set; }
        public Array from_emp_details { get; set; }
        public string f_return { get; set; }
        // feedback end
    }
    public class arraymessageDMO
    {
        public long HRME_Id { get; set; }
        public string employeeName { get; set; }
    }
    public class arrayreplyDMO
    {      
        public long ISMINTRD_ToId { get; set; }       
    }
    public class feedbackdt1
    {
        public long ISMDRF_RCV_HRME_Id { get; set; }
        public long ISMTCR_Id { get; set; }
        public long ISMDRF_Id { get; set; }
        public long MI_Id_r { get; set; }
        public string ISMDRF_FeedBack { get; set; }
        public DateTime? ISMDRF_Feedback_DR_Date { get; set; }
    }
}
