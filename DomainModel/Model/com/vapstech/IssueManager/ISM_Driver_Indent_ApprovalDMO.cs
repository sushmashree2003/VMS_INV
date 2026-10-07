using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DomainModel.Model.com.vapstech.IssueManager
{
    [Table("ISM_Driver_Indent_Approval")]
    public class ISM_Driver_Indent_ApprovalDMO
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public long ISMDIA_Id { get; set; }
        public long ISMDIT_Id { get; set; }
        public long ISMDIT_PreparedByUserId { get; set; }
        public long ISMDIT_ApprovalUserId { get; set; }
        public long ISMDIT_ReceivedByUserId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string ISMDIT_Remark { get; set; }
        public string ISMDIT_PreparedByFlag { get; set; }
        public string ISMDIT_Status { get; set; }
    }
}
