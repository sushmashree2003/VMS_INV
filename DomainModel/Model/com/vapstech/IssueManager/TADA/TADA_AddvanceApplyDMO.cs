using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.IssueManager.TADA
{
    [Table("VMS_TADA_Advance_Apply")]
    public class TADA_AddvanceApplyDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long VTADAAA_Id { get; set; }
        public long MI_Id { get; set; }
        public long HRME_Id { get; set; }
        public DateTime? VTADAAA_AppliedDate { get; set; }
        public DateTime? VTADAAA_FromDate { get; set; }
        public DateTime? VTADAAA_ToDate { get; set; }
        public long VTADAAA_ClientId { get; set; }

        public string VTADAAA_ToAddress { get; set; }
        public string VTADAAA_Remarks { get; set; }
        public decimal VTADAAA_TotalAppliedAmount { get; set; }
        public decimal VTADAAA_TotalSactionedAmount { get; set; }
        public decimal VTADAAA_TotalPaidAmount { get; set; }
        
        public string VTADAAA_StatusFlg { get; set; }
        public bool VTADAAA_ActiveFlg { get; set; }
        public long VTADAAA_CreatedBy { get; set; }
        public long VTADAAA_UpdatedBy { get; set; }
        public DateTime? VTADAAA_CreatedDate { get; set; }
        public DateTime? VTADAAA_UpdatedDate { get; set; }
        public long IVRMMCT_Id { get; set; }
        public List<TADA_AddvanceApplyDetailsDMO> TADA_AddvanceApplyDetailsDMO { get; set; }
        public string VTADAAA_DepartureTime { get; set; }
        public string VTADAAA_ArrivalTime { get; set; }
        public string VTADAAA_ClientMultiple { get; set; }
        public List<VMS_TADDA_MultipleClient_FilesDMO> VMS_TADDA_MultipleClient_FilesDMO { get; set; }
        public List<VMS_TADA_Advance_Apply_CityDMO> VMS_TADA_Advance_Apply_City { get; set; }
        public string IER_ID { get; set; }
    }
}
