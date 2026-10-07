using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.FMS
{
    [Table("FMS_File_Request")]
    public class FMSApprovalRequestPageDMO
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long FMSFLREQ_Id { get; set; }
        public long MI_Id { get; set; }
        public long HRME_Id { get; set; }
        public long FMSMPFN_Id { get; set; }
        public DateTime FMSFLREQ_RequestedDateTime { get; set; }
        public long FMSFLREQ_RequiredDays { get; set; }
        public bool FMSFLREQ_ApprovedFlg { get; set; }
        public long? FMSFLREQ_ApprovedDays { get; set; }
        public long? FMSFLREQ_HandedOverBy { get; set; }
        public bool? FMSFLREQ_ReturnedFlg { get; set; }
        public DateTime? FMSFLREQ_ReturnedDateTime { get; set; }
        public string FMSFLREQ_ReturnRemarks { get; set; }
        public long? FMSFLREQ_ReceivedBy { get; set; }
        public bool? FMSFLREQ_ActiveFlg { get; set; }
        public DateTime? FMSFLREQ_CreatedDate { get; set; }
        public DateTime? FMSFLREQ_UpdatedDate { get; set; }
        public long? FMSFLREQ_CreatedBy { get; set; }
        public long? FMSFLREQ_UpdatedBy { get; set; }
        public DateTime FMSFLREQ_RequestedFromDate { get; set; }
        public DateTime FMSFLREQ_RequestedToDate { get; set; }


    }
}
