using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.FMS
{
    [Table("FMS_ReferenceNo_Configuration")]
    public class FMS_ReferenceNo_ConfigurationDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long FMSRNCON_Id { get; set; }
        public long MI_Id { get; set; }
        public string FMSRNCON_Criterias { get; set; }
        public int FMSRNCON_CriteriaOrder { get; set; }
        public bool FMSRNCON_ActiveFlg { get; set; }
        public DateTime? FMSRNCON_CreatedDate { get; set; }
        public DateTime? FMSRNCON_UpdatedDate { get; set; }
        public long? FMSRNCON_CreatedBy { get; set; }
        public long? FMSRNCON_UpdatedBy { get; set; }
    }
}
