using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.IssueManager
{
    [Table("ISM_Task_Regenarate_Remarks")]

    public class ISM_Task_Regenarate_RemarksDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long ISMTRR_Id { get; set; }
        public long HRME_Id { get; set; }
        public long MI_Id { get; set; }
        public DateTime ISMTRR_RemarksDate { get; set; }
        public string ISMTRR_Remarks { get; set; }
        public DateTime ISMTRR_CreatedDate { get; set; }
        public DateTime ISMTRR_UpdateDate { get; set; }
        public long ISMTRR_CreatedBy { get; set; }
        public long ISMTRR_UpdatedBy { get; set; }
    }
}
