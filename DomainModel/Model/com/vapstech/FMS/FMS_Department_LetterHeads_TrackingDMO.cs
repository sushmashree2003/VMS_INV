using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;


namespace DomainModel.Model.com.vapstech.FMS
{
    [Table("FMS_Department_LetterHeads_Tracking")]
    public class FMS_Department_LetterHeads_TrackingDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long FMSDLHT_Id { get; set; }
        public long MI_Id { get; set; }
        public long HRMD_Id { get; set; }
        public string FMSDLHT_LetterheadNo { get; set; }
        public DateTime? FMSDLHT_Date { get; set; }
        public bool? FMSDLHT_CancelledFlag { get; set; }
        public string FMSDLHT_Reason { get; set; }
        public long? FMSDLHT_CreatedBy { get; set; }
        public long? FMSDLHT_UpdatedBy { get; set; }
        public DateTime? FMSDLHT_CreatedDate { get; set; }
        public DateTime? FMSDLHT_UpdatedDate { get; set; }
       
    }
}
