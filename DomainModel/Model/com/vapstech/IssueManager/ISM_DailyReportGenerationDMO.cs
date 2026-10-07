using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.com.vapstech.IssueManager
{
    [Table("ISM_DailyReport")]
    public class ISM_DailyReportGenerationDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ISMDRPT_Id { get; set; }
        public long MI_Id { get; set; }
        public long HRME_Id { get; set; }
        public long ISMTPL_Id { get; set; }
        public long ISMTPLAP_Id { get; set; }
        public long? ISMTPLTA_Id { get; set; }
        public long ISMTCR_Id { get; set; }
        public DateTime? ISMDRPT_Date { get; set; }
        public string ISMDRPT_Remarks { get; set; }
        public decimal? ISMDRPT_TimeTakenInHrs { get; set; }
        public string ISMDRPT_Status { get; set; }
        public bool ISMDRPT_DeviationFlg { get; set; }
        public bool ISMDRPT_ExtraFlg { get; set; }
        public bool ISMDRPT_ActiveFlg { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public long ISMDRPT_CreatedBy { get; set; }
        public long ISMDRPT_UpdatedBy { get; set; }
        public bool? ISMDRPT_BlockedFlag { get; set; }
        public bool? ISMDRPT_OrdersDateFlg { get; set; }
        public string ISMDRPT_OthersDateReason { get; set; }
        public string ISMDRPT_JeotaggedPhoto { get; set; }
        public string ISMDRPT_JeotaggedPhotoFath { get; set; }
        public string ISMDRPT_LocationLatLogValue { get; set; }
        public string ISMDRPT_LocationName { get; set; }
        public bool? ISMDRPT_HalfDayFlag { get; set; }
        public string ISMDRPT_TimeTakenFlg { get; set; }
        public bool? ISMDRPT_ApprovedFlg { get; set; }
        public decimal? ISMDRPT_ApprovedTime { get; set; }
        public string ISMDRPT_ApprovedTimeFlg { get; set; }
        public long? ISMDR_Id { get; set; }
        public bool? ISMDRPT_CompoffApplicableflag { get; set; }
        public List<ISM_DailyReport_CommentsDMO> ISM_DailyReport_CommentsDMO { get; set; }
        public List<ISM_DailyReport_FilesDMO> ISM_DailyReport_FilesDMO { get; set; }
        public List<ISM_DailyReportReapprovalDMO> ISM_DailyReportReapprovalDMO { get; set; }

    }
}
