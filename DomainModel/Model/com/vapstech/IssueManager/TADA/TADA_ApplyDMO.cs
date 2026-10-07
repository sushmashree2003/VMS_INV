using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.IssueManager.TADA
{
    [Table("VMS_TADA_Apply")]
    public class TADA_ApplyDMO
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long VTADAA_Id { get; set; }
        public long MI_Id { get; set; }      
        public long ? VTADAAA_Id { get; set; }
        public long HRME_Id { get; set; }
        public DateTime? VTADAA_AppliedDate { get; set; }
        public DateTime? VTADAA_FromDate { get; set; }
        public DateTime? VTADAA_ToDate { get; set; }
        public long VTADAA_ClientId { get; set; }
        public string VTADAA_ToAddress { get; set; }
        public string VTADAA_Remarks { get; set; }
        public decimal VTADAA_TotalAppliedAmount { get; set; }
        public decimal VTADAA_TotalSactionedAmount { get; set; }
        public decimal VTADAA_TotalPaidAmount { get; set; }
        public string VTADAA_StatusFlg { get; set; }
        public bool VTADAA_ActiveFlg { get; set; }
        public long VTADAA_CreatedBy { get; set; }
        public long VTADAA_UpdatedBy { get; set; }
        public DateTime? VTADAA_CreatedDate { get; set; }
        public DateTime? VTADAA_UpdatedDate { get; set; }
        //public string VTADAA_FileName { get; set; }
        //public string VTADAA_FilePath { get; set; }
        public long IVRMMCT_Id { get; set; }
        public string VTADAA_DepartureTime { get; set; }
        public string VTADAA_ArrivalTime { get; set; }
        public string VTADAAA_ClientMultiple { get; set; }
        public string VTADAA_BalanceHRMS { get; set; }
        public bool? VTADAA_Finaldocument { get; set; }
        public bool? VTADAA_ApprovedFlg { get; set; }
        public string IER_ID { get; set; }     
        public string VTADAA_BalanceConcal { get; set; }
        public List<TADA_ApplyDetailsDMO> TADA_ApplyDetailsDMO { get; set; }
        public List<VMS_TADAApply_FilesDMO> VMS_TADAApply_FilesDMO { get; set; }
        public List<VMS_TADA_MultipleClient_FilesDMO> VMS_TADA_MultipleClient_FilesDMO { get; set; }
        public List<VMS_TADA_Apply_CityDMO> VMS_TADA_Apply_CityDMO { get; set; }
        
    }
}