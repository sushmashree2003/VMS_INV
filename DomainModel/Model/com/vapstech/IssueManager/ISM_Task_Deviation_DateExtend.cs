using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.IssueManager
{
    [Table("ISM_Task_Deviation_DateExtend")]
    public  class ISM_Task_Deviation_DateExtend
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ISMTDE_Id { get; set; }
        public long ISMTCR_Id { get; set; }
        public long HRME_Id { get; set; }
        public DateTime ISMTDE_TaskStartDate { get; set; }
        public DateTime ISMTDE_TaskEndDate { get; set; }
        public DateTime? ISMTDE_TaskExtendDate { get; set; } 
        public string ISMTDE_Status { get; set; }
        public string ISMTDE_Remarks { get; set; }
        public long ISMTDE_Sanctionlevel { get; set; }
        public bool ISMTDE_ActiveFlag { get; set; }
        public DateTime ISMTDE_CreatedDate { get; set; }
        public DateTime ISMTDE_UpdatedDate { get; set; }
        public long ISMTDE_CreatedBy { get; set; }
        public long ISMTDE_UpdatedBy { get; set; }

    }
}
