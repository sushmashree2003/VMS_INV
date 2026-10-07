using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.com.vapstech.IssueManager
{
    [Table("ISM_DailyReport_Daywise")]
    public class ISM_DailyReport_DaywiseDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ISMDRPTDW_Id { get; set; }
        public long MI_Id { get; set; }
        public long HRME_Id { get; set; }
        public long ISMTPL_Id { get; set; }
        public long ISMTPLAP_Id { get; set; }
        public string ISMDRPTDW_OverallComments { get; set; }
        public DateTime? ISMDRPTDW_Date { get; set; }
        public bool? ISMDRPTDW_BlockedDayDRFlg { get; set; }
        public bool? ISMDRPTDW_HalfDayDRFlg { get; set; }
        public bool? ISMDRPTDW_SalarySanctionFlg { get; set; }
        public bool? ISMDRPTDW_SanctionedHalfDayFlg { get; set; }
        public bool? ISMDRPTDW_ActiveFlg { get; set; }
        public bool? ISMDRPTDW_OrthersDateFlg { get; set; }
        public string ISMDRPTDW_OthersDateReason { get; set; }
        public DateTime? ISMDRPTDW_CreatedDate { get; set; }
        public DateTime? ISMDRPTDW_UpdatedDate { get; set; }
        public long ISMDRPTDW_CreatedBy { get; set; }
        public long ISMDRPTDW_UpdatedBy { get; set; }
        public decimal? ISMDRPTDW_ApprovedEffort { get; set; }
    }
}
