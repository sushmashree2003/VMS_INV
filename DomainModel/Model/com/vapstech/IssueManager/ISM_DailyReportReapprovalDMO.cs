using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.IssueManager
{
    [Table("ISM_DailyReport_Reapproval")]
    public  class ISM_DailyReportReapprovalDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
       public long ISMDRPTR_Id { get; set; }
        public long ISMDRPT_Id { get; set; }
        public long MI_Id { get; set; }
        public long HRME_Id { get; set; }
        public long ISMTPL_Id { get; set; }
        public long ISMTPLAP_Id { get; set; }
        public long? ISMTPLTA_Id { get; set; }
        public long ISMTCR_Id { get; set; }
        public DateTime? ISMDRPTR_Date { get; set; }
        public string ISMDRPTR_Remarks { get; set; }
        public decimal? ISMDRPTR_TimeTakenInHrs { get; set; }
        public string ISMDRPTR_Status { get; set; }
        public bool ISMDRPTR_DeviationFlg { get; set; }
        public bool ISMDRPTR_ExtraFlg { get; set; }
        public bool ISMDRPTR_ActiveFlg { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public long ISMDRPTR_CreatedBy { get; set; }
        public long ISMDRPTR_UpdatedBy { get; set; }
        public bool? ISMDRPTR_OrdersDateFlg { get; set; }
        public bool? ISMDRPTR_OthersDateReason { get; set; }
        public string ISMDRPTR_BlockedFlag { get; set; }
        public string ISMDRPTR_JeotaggedPhoto { get; set; }
        public string ISMDRPTR_JeotaggedPhotoFath { get; set; }
        public string ISMDRPTR_LocationLatLogValue { get; set; }
        public string ISMDRPTR_LocationName { get; set; }
        public bool? ISMDRPTR_HalfDayFlag { get; set; }
        public string ISMDRPTR_TimeTakenFlg { get; set; }
        public bool? ISMDRPTR_ApprovedFlg { get; set; }
        public decimal? ISMDRPTR_ApprovedTime { get; set; }
        public string ISMDRPTR_ApprovedTimeFlg { get; set; }
        public long? ISMDR_Id { get; set; }

        public bool? ISMDRPT_CompoffApplicableflag { get; set; }
    }
}
