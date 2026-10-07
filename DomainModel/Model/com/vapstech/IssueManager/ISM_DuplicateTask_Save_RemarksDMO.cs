using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.com.vapstech.IssueManager
{
    [Table("ISM_DuplicateTask_Save_Remarks")]
    public class ISM_DuplicateTask_Save_RemarksDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ISMDTSR_Id { get; set; }
        public long ISMTCR_Id { get; set; }
        public string ISMDTSR_Remarks { get; set; }
        public bool ISMDTSR_Activeflag { get; set; }
        public long CreatedBy { get; set; }
        public long UpdatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
