using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.IssueManager.QRcode_Activity
{

    [Table("HR_Master_Area_Activity_Staff_Mapping")]
    public class HRMasterAreaStaffMappingDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long HRMARACTSTMAP_Id { get; set; }
        public long HRMARACT_Id { get; set; }
        public long User_Id { get; set; }
        public DateTime? HRMARACTSTMAP_Date { get; set; }
        public bool HRMARACTSTMAP_ActiveFlag { get; set; }
        public DateTime? HRMARACTSTMAP_CreatedDate { get; set; }
        public DateTime? HRMARACTSTMAP_UpdatedDate { get; set; }
        public long HRMARACTSTMAP_CreatedBy { get; set; }
        public long HRMARACTSTMAP_UpdatedBy { get; set; }
    }

}

