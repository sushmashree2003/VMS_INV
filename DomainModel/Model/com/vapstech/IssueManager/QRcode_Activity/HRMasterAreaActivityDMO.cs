using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.IssueManager.QRcode_Activity
{

    [Table("HR_Master_Area_Activity")]
    public class HRMasterAreaActivityDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long HRMARACT_Id { get; set; }
        public long HRMAR_Id{get;set;}
        public string HRMARACT_ActivityName { get; set; }
        public string HRMARACT_ActivityDesc { get; set; }
        public bool HRMARACT_ActiveFlag { get; set; }
        public DateTime? HRMARACT_CreatedDate { get; set; }
        public DateTime? HRMARACT_UpdatedDate { get; set; }
        public long HRMARACT_CreatedBy { get; set; }
        public long HRMARACT_UpdatedBy { get; set; }
        
    }
}
