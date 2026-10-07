using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.FMS
{
    public class FMSApprovalRequestPageDTO
    {
        public string returnduplicatestatus { get; set; }
        public bool returnval { get; set; }
        public bool already_cnt { get; set; }
        public string message { get; set; }
        //----------------
        public long FMSFLREQ_Id { get; set; }
        public long MI_Id { get; set; }
        public long HRME_Id { get; set; }
        public long requestiddata { get; set; }
        public long FMSMPFN_Id { get; set; }
        public DateTime FMSFLREQ_RequestedDateTime { get; set; }
        public long FMSFLREQ_RequiredDays { get; set; }
        public bool? FMSFLREQ_ApprovedFlg { get; set; }
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
        public string duplicate { get; set; }
        //----------------
        //public long MI_Id { get; set; }
        public Array filename { get; set; }
        public Array get_tax { get; set; }
        public string filenames { get; set; }
        public long requireddays { get; set; }
        public long UserId { get; set; }
        public Array editdata { get; set; }
        public Array hrmetable { get; set; }
        public long registration { get; set; }
        public long iddata { get; set; }
        public string FMSMPFN_PhysicalFileName { get; set; }
    }
}
