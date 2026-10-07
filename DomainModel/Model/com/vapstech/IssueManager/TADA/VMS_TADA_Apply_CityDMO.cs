using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.IssueManager.TADA
{
    [Table("VMS_TADA_Apply_City")]
    public class VMS_TADA_Apply_CityDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long VTADAAC_Id { get; set; }
        public long VTADAA_Id { get; set; }
        public string VTADAAC_FromState { get; set; }
        public string VTADAAC_ToState { get; set; }
        public long VTADAAC_CreatedBy { get; set; }
        public long VTADAAC_UpdatedBy { get; set; }
        public DateTime? VTADAAC_CreatedDate  { get; set; }
        public DateTime? VTADAAC_UpdatedDate { get; set; }
        public decimal? VTADAAC_Amount { get; set; }
        public long HRMDES_Id { get; set; }
        public long VTADAMT_Id { get; set; }
        public long VTADAMD_Id { get; set; }
        public long IVRMMCT_Id { get; set; }


    }
}
