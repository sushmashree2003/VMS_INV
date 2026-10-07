using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DomainModel.Model.com.vapstech.IssueManager
{
    [Table("ISM_Weekly_Deviation")]
    public class ISM_Weekly_Deviation_DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ISMWD_Id { get; set; }
        public long HRME_Id { get; set; }
        public long ISMTPL_Id { get; set; }
        public DateTime ISMWD_StartDate { get; set; }
        public DateTime ISMWD_EndDate { get; set; }
        public string ISMWD_TotalEffort { get; set; }
        public string ISMWD_CompletedEffort { get; set; }
        public decimal ISMWD_Completed { get; set; }
        public decimal ISMWD_Deviation { get; set; }
    }
}
