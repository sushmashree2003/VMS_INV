using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.IssueManager.TADA
{
    [Table("VMS_TADA_Advance_Apply_City")]
    public  class VMS_TADA_Advance_Apply_CityDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long VTADAAAC_Id { get; set; }
        public long VTADAAA_Id { get; set; }
        public string VTADAAAC_FromState { get; set; }
        public string VTADAAAC_ToState { get; set; }
        public decimal VTADAAAC_Amount { get; set; }       
        public long VTADAAAC_CreatedBy { get; set; }
        public long VTADAAAC_UpdatedBy { get; set; }
        public DateTime VTADAAAC_CreatedDate { get; set; }
        public DateTime VTADAAAC_UpdatedDate { get; set; }
        public long? HRMDES_Id { get; set; }
        public long VTADAMT_Id { get; set; }
        public long VTADAMD_Id { get; set; }
        public long IVRMMCT_Id { get; set; }
    }
}
