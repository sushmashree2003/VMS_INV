using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DomainModel.Model.com.vapstech.IssueManager
{
    [Table("VMS_Master_MonthEndDepartment")]
    public class VMS_Master_MonthEndDepartmentDMO
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public long VMSMMEDPT_Id { get; set; }
        public long MI_Id { get; set; }
        public long HRMDC_Id { get; set; }
        public string VMSMMEDPT_Description { get; set; }
        public bool VMSMMEDPT_ActiveFlag { get; set; }
        public bool? VMSMMEDPT_ManualFlag { get; set; }
        public DateTime VMSMMEDPT_CreatedDate { get; set; }
        public DateTime VMSMMEDPT_UpdatedDate { get; set; }
    }
}
