using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.IssueManager.TADA
{
    [Table("ISM_Mst_EmpRegion_Mapping")]
    public class ISM_Mst_EmpRegion_MappingDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long IMER_Id { get; set; }
        public long MI_Id { get; set; }
        public long IMER_EmpId { get; set; }
        public long IVRMMS_Id { get; set; }
        public bool IMER_ActiveFlag { get; set; }
        public long IMER_Createdby { get; set; }
        public DateTime? IMER_CreatedDate { get; set; }
        public long IMER_Updateby { get; set; }
        public DateTime? IMER_UpdateDate { get; set; }

        public long? IMER_ApplicableRegion { get; set; }
    }

}
