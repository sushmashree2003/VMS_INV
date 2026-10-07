using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.IssueManager.QRcode_Activity
{

    [Table("HR_Master_Area_Activity_Time")]
    public class HRMasterAreaActivityTimeDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long HRMARACTT_Id { get; set; }
        public long HRMARACT_Id { get;set;}
        public string HRMARACTT_Time { get; set; }
        public long HRMARACTT_ActivityDuration { get; set; }
        public bool HRMARACTT_ActiveFlag { get; set; }
        public DateTime? HRMARACTT_CreatedDate { get; set; }
        public DateTime? HRMARACTT_UpdatedDate { get; set; }
        public long HRMARACTT_CreatedBy { get; set; }
        public long HRMARACTT_UpdatedBy { get; set; }


    }
}
