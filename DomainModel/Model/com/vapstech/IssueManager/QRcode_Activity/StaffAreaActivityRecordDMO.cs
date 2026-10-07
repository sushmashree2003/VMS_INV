using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.IssueManager.QRcode_Activity
{

    [Table("HR_Staff_Area_Activity_Record")]
    public class StaffAreaActivityRecordDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long HRMARACT_Id { get; set; }
        public long MI_Id { get; set; }
        public long User_Id { get; set; }
        public long HRMAR_Id { get; set; }

        public long HRSTARACTRCD_Id { get; set; }
        public DateTime? HRSTARACTRCD_Date { get; set; }
        public string HRSTARACTRCD_Time { get; set; }
        public bool HRSTARACTRCD_ActiveFlag { get; set; }
        public DateTime? HRSTARACTRCD_CreatedDate { get; set; }
        public DateTime? HRSTARACTRCD_UpdatedDate { get; set; }
        public long HRSTARACTRCD_CreatedBy { get; set; }
        public long HRSTARACTRCD_UpdatedBy { get; set; }

    }

}

