using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.VMS.Sales
{
    [Table("ISM_CA_StudentStrength_Mapping")]
    public class ISM_CA_StudentStrength_MappingDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ICSSM_ID { get; set; }
        public int ICSSM_Strenth { get; set; }
        public decimal ICSSM_PrecentageFrom { get; set; }
        public decimal ICSSM_PrecentageTo { get; set; }
        public bool ICSSM_Active { get; set; }
        public long MI_ID { get; set; }
        public long ICSSM_Createdby { get; set; }
        public long ICSSM_Updatedby { get; set; }
        public DateTime? ICSSM_CreatedOn { get; set; }
        public DateTime? ICSSM_UpdateOn { get; set; }


    }
}
