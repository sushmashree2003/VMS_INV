using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.IssueManager.TADA
{
    [Table("ISM_Mst_EmpCategory_Mapping")]
    public  class ISM_Mst_EmpCategory_MappingDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long IMECM_id { get; set; }
        public long MI_Id { get; set; }
        public long IMECM_EmpId { get; set; }
        public long IMRC_CategoryId { get; set; }
        public decimal IMRC_Percentage { get; set; }
        public bool IMECM_ActiveFlag { get; set; }
        public long? IMECM_Createdby { get; set; }
        public DateTime? IMECM_CreatedDate { get; set; }
        public long? IMECM_Updateby { get; set; }
        public long? IMECM_MinTarget { get; set; }
        public long? IMECM_MaxTarget { get; set; }
        public DateTime? IMECM_UpdateDate { get; set; }
    }
}
