using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.VMS.Legal
{
    [Table("LNC_Master_Court")]
    public class LNC_Master_CourtDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long MC_Id { get; set; }
        public long MCT_Id { get; set; }
        public string MC_Name { get; set; }
        public bool MC_ActiveFlag { get; set; }
        public DateTime MC_CreatedDate { get; set; }
        public DateTime MC_UpdatedDate { get; set; }
        public long? MC_CreatedBy { get; set; }
        public long? MC_UpdatedBy { get; set; }
    }
}
