using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.IssueManager
{
    [Table("ISM_DailyReport_Approval")]
    public class ISM_DailyReport_ApprovalDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ISMDRPTAPP_Id { get; set; }
        public long ISMDRPT_Id { get; set; }
        public long HRME_Id { get; set; }
        public string ISMDRPTAPP_Remarks { get; set; }
        public bool ISMDRPTAPP_ActiveFlg { get; set; }
        public DateTime? ISMDRPTAPP_CreatedDate { get; set; }
        public DateTime? ISMDRPTAPP_UpdatedDate { get; set; }
        public long ISMDRPTAPP_CreatedBy { get; set; }
        public long ISMDRPTAPP_UpdatedBy { get; set; }

    }
}
