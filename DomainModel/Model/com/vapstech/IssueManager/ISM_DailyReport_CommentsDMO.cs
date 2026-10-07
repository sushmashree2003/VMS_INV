using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.IssueManager
{
    [Table("ISM_DailyReport_Comments")]
    public class ISM_DailyReport_CommentsDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ISMDRPTCOMT_Id { get; set; }
        public long ISMDRPT_Id { get; set; }
        public long HRME_Id { get; set; }
        public string ISMDRPTCOMT_Comments { get; set; }
        public bool ISMDRPTCOMT_ActiveFlg { get; set; }
        public DateTime ISMDRPTCOMT_CreatedDate { get; set; }
        public DateTime ISMDRPTCOMT_UpdatedDate { get; set; }
        public long ISMDRPTCOMT_CreatedBy { get; set; }
        public long ISMDRPTCOMT_UpdatedBy { get; set; }
    }
}
