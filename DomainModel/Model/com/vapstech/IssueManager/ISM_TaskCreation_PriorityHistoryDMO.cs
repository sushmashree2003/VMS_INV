using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.IssueManager
{
    [Table("ISM_TaskCreation_PriorityHistory")]
    public class ISM_TaskCreation_PriorityHistoryDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ISMTCRPH_Id { get; set; }
        public long ISMTCR_Id { get; set; }
        public long HRMPR_Id { get; set; }
        public string ISMTCRPH_Remarks { get; set; }
        public bool ISMTCRPH_ActiveFlg { get; set; }
        public DateTime? ISMTCRPH_CreatedDate { get; set; }
        public DateTime? ISMTCRPH_UpdatedDate { get; set; }
        public long ISMTCRPH_CreatedBy { get; set; }
        public long ISMTCRPH_UpdatedBy { get; set; }
    }
}
