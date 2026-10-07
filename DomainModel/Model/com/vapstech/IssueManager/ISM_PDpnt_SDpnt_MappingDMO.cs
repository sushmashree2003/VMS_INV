using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;


namespace DomainModel.Model.com.vapstech.IssueManager
{
    [Table("ISM_PDpnt_SDpnt_Mapping")]
    public class ISM_PDpnt_SDpnt_MappingDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long ISMPDSDMAP_Id { get; set; }
        public long MI_Id { get; set; }
        public long HRMD_Id { get; set; }
        public long ISMPDSDMAP_SMI_Id { get; set; }
        public long ISMPDSDMAP_SHRMD_Id { get; set; }
        public bool ISMPDSDMAP_ActiveFlag { get; set; }
        public long ISMPDSDMAP_CreatedBy { get; set; }
        public long ISMPDSDMAP_UpdatedBy { get; set; }
        public DateTime? ISMPDSDMAP_CreatedDate { get; set; }
        public DateTime? ISMPDSDMAP_UpdatedDate { get; set; }
    }
}
