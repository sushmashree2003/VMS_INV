using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.FMS
{
    [Table("FMS_Correspondence_SalesLead")]
    public class FMS_Correspondence_SalesLeadDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long FMSCORSL_Id { get; set; }
        public long FMSCOR_Id { get; set; }
        public long ISMSLE_Id { get; set; }
        public bool FMSCORSL_ActiveFlg { get; set; }    
        public DateTime? FMSCORSL_CreatedDate { get; set; }
        public DateTime? FMSCORSL_UpdatedDate { get; set; }
        public long? FMSCORSL_CreatedBy { get; set; }
        public long? FMSCORSL_UpdatedBy { get; set; }
        public long? ISMSBOM_Id { get; set; }
    }
}
