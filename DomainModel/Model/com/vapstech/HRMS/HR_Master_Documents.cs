using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.HRMS
{
    [Table("HR_Master_Documents")]
    public class HR_Master_Documents
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long HRMDOC_Id { get; set; }
        public long MI_Id { get; set; }
        public string HRMDOC_DocumentName { get; set; }
        public string HRMDOC_Description { get; set; }
        public string HRMDOC_Doctype { get; set; }
        public bool HRMDOC_FLAG { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public long? CreatedBy { get; set; }
        public long? UpdatedBy { get; set; }

        
    }
}
