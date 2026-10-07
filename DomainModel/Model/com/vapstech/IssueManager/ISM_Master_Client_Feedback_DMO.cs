using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.com.vapstech.IssueManager
{
    [Table("ISM_Master_Client_Feedback")]
    public class ISM_Master_Client_FeedbackDMO 
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ISMMCF_Id { get; set; }
        public long ISMMCLT_Id { get; set; }
        public DateTime? ISMMCF_Date { get; set; }
        public string ISMMCF_Time { get; set; }
        public string ISMMCF_Remarks { get; set; }
        public DateTime? ISMMCF_CreatedDate { get; set; }
        public DateTime? ISMMCF_UpdatedDate { get; set; }
        public long ISMMCF_CreatedBy { get; set; }
        public long ISMMCF_UpdatedBy { get; set; }

        public bool ISMMCF_Negative { get; set; }

    }
}
