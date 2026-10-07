using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.IssueManager
{
    [Table("ISM_CallCenter_CLientFeedback")]
    public  class ISM_CallCenter_CLientFeedbackDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ICCF_ID { get; set; }
        public long ISMMCLT_Id { get; set; }
        public long ICCF_IE_HRMEID { get; set; }
        public string EndUser_No { get; set; }
        public string Mangement_No { get; set; }
        public string NodalOfficerRemark { get; set; }
        public string MangmentRemark { get; set; }
        public string ConfirmationRemark { get; set; }
        public DateTime ICCF_Date { get; set; }
        public long ICCF_Createby { get; set; }
        public long ICCF_updatedby { get; set; }
        public DateTime ICCF_createddate { get; set; }
        public DateTime ICCF_updateddate { get; set; }
        
        public bool ICCF_Activeflag { get; set; }
        public string ICCF_Flag { get; set; }
        public long ASMAY_Id { get; set; }
        public long IMM_Id { get; set; }
        public long IVRMM_Id { get; set; }
        public string ParentsRemarks { get; set; }
        public long ParentsNumber { get; set; }
        public string PCstatus { get; set; }

    }
}
