using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DomainModel.Model.com.vapstech.IssueManager
{
    [Table("VMS_EmpWiseMonthEndRemarks")]
    public class VMS_EmpWiseMonthEndRemarksDMO
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]        
        public long VMSEWMER_Id { get; set; }
        public long HRME_Id { get; set; }
        public long VMSMMEDPT_Id { get; set; }
        public string VMSEWMER_Remarks { get; set; }
        public long VMSEWMER_Month { get; set; }
        public long VMSEWMER_Year { get; set; }
        public string VMSEWMER_Value { get; set; }
        public long CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
