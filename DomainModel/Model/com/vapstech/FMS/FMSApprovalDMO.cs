using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.FMS
{
    [Table("FMS_File_Request_Approve")]
    public class FMSApprovalDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long FMSFLREQAPP_Id { get; set; }
        public long FMSFLREQ_Id { get; set; }
        public long HRME_Id { get; set; }
        public bool FMSFLREQAPP_ApprovedFlg { get; set; }
        public long FMSFLREQAPP_ApprovedDays { get; set; }
        public string FMSFLREQAPP_ApproverRemarks { get; set; }
        public bool FMSFLREQAPP_ActiveFlg { get; set; }
        public DateTime FMSFLREQAPP_CreatedDate { get; set; }
        public DateTime FMSFLREQAPP_UpdatedDate { get; set; }
        public long FMSFLREQAPP_CreatedBy { get; set; }
        public long FMSFLREQAPP_UpdatedBy { get; set; }

    }
}
