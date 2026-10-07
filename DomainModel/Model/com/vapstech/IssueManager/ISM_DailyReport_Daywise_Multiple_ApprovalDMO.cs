using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.IssueManager
{
    [Table("ISM_DailyReport_Daywise_Multiple_Approval")]
    public class ISM_DailyReport_Daywise_Multiple_ApprovalDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long ISMDRPTDWMAPP_Id { get; set; }
        public long ISMDRPTDW_Id { get; set; }
        public decimal? ISMDRPTDWMAPP_ApprovedEffort { get; set; }
        public long ISMDRPTDWMAPP_ApprovedHRME_Id { get; set; }
        public string ISMDRPTDWMAPP_Remarks { get; set; }
        public bool ISMDRPTDWMAPP_ActiveFlg { get; set; }
        public DateTime ISMDRPTDWMAPP_CreatedDate { get; set; }
        public DateTime ISMDRPTDWMAPP_UpdatedDate { get; set; }
        public long ISMDRPTDWMAPP_CreatedBy { get; set; }
        public long ISMDRPTDWMAPP_UpdatedBy { get; set; }
         
    }
}
