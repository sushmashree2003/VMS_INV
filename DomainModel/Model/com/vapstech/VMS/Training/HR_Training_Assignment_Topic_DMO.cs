using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.VMS.Training
{
    [Table("HR_Training_Assignment_Setting_Topic")]
    public class HR_Training_Assignment_Topic_DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long HRTRAST_Id { get; set; }
        public long HRMTT_Id { get; set; }
        public long HRTRAST_NoofQns { get; set; }
        public double HRTRAST_TotalMarks { get; set; }
        public string HRTRAST_TotalDuration { get; set; }
        public double HRTRAST_EachQnsMarks { get; set; }
        public string HRTRAST_EachQnsDuration { get; set; }
        public long HRTRAST_NoOfOptions { get; set; }
        public bool HRTRAST_ActiveFlg { get; set; }
        public long HRTRAST_CreatedBy { get; set; }
        public long HRTRAST_UpdatedBy { get; set; }
        public DateTime? HRTRAST_CreatedDate { get; set; }
        public DateTime? HRTRAST_UpdatedDate { get; set; }

    }
}
