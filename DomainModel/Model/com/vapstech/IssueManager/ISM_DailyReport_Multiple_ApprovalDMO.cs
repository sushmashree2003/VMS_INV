using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.IssueManager
{
    [Table("ISM_DailyReport_Multiple_Approval")]
    public class ISM_DailyReport_Multiple_ApprovalDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ISMDRPTMAPP_Id { get; set; }
        public long ISMDRPT_Id { get; set; }
        public long HRME_Id { get; set; }
        public string ISMDRPTMAPP_Remarks { get; set; }
        public decimal? ISMDRPTMAPP_ApprovedEfforts { get; set; }
        public bool ISMDRPTMAPP_ApprovedFlag { get; set; }
        public bool ISMDRPTMAPP_ActiveFlg { get; set; }
        public DateTime? ISMDRPTMAPP_CreatedDate { get; set; }
        public DateTime? ISMDRPTMAPP_UpdatedDate { get; set; }
        public long ISMDRPTMAPP_CreatedBy { get; set; }
        public long ISMDRPTMAPP_UpdatedBy { get; set; }
    }
}
