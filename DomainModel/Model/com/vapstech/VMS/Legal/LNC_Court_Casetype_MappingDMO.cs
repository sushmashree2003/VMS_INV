using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.VMS.Legal
{
    [Table("LNC_Court_Casetype_Mapping")]
    public class LNC_Court_Casetype_MappingDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long CCM_Id { get; set; }
        public string CCM_Court { get; set; }
        public string CCM_Case { get; set; }
        public bool CCM_ActiveFlag { get; set; }
        public DateTime CCM_CreatedDate { get; set; }
        public DateTime CCM_UpdatedDate { get; set; }
        public long? CCM_CreatedBy { get; set; }
        public long? CCM_UpdatedBY { get; set; }
    }
}
