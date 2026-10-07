using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.IssueManager.DriversChart
{
    [Table("VMS_Driver_Chart")]
    public class DriverChartGenerationDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long VMSDRCHT_Id { get; set; }
        public long MI_Id { get; set; }
        public long TRMD_Id { get; set; }
        public long VMSMDSH_Id { get; set; }
        public DateTime VMSDRCHT_FromDate { get; set; }
        public DateTime VMSDRCHT_ToDate { get; set; }
        public bool VMSDRCHT_SundayWorkingFlg { get; set; }
        public bool VMSDRCHT_SaturdayOffFlg { get; set; }

        public string VMSDRCHT_Remarks { get; set; }
        public bool VMSDRCHT_ActiveFlag { get; set; }

        public DateTime VMSDRCHT_CreatedDate { get; set; }
        public DateTime VMSDRCHT_UpdatedDate { get; set; }
            
        public long VMSDRCHT_CreatedBy { get; set; }
        public long VMSDRCHT_UpdatedBy { get; set; }
    }
}
