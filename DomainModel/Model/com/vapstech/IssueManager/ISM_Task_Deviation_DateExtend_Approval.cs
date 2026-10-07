using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.IssueManager
{
    [Table("ISM_Task_Deviation_DateExtend_Approval")]
    public  class ISM_Task_Deviation_DateExtend_Approval
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ISMTDEA_Id { get; set; }
        public long ISMTDE_Id { get; set; }
        public long ISMTDEA_ApprovedBy { get; set; }
        public long ISMTDEA_Sanctionlevel { get; set; }
        public string ISMTDEA_Remarks { get; set; }
        public long ISMTDEA_FinalFlag { get; set; }
        public bool ISMTDEA_ActiveFlag { get; set; }
        public DateTime ISMTDEA_CreatedDate { get; set; }
        public DateTime ISMTDEA_UpdatedDate { get; set; }
        public long ISMTDEA_CreatedBy { get; set; }
        public long ISMTDEA_UpdatedBy { get; set; }
    }
}
