using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.IssueManager.DriversChart
{
    [Table("VMS_Master_Driver_Shifts")]
    public class MasterDriverShiftsDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long VMSMDSH_Id { get; set; }
        public long MI_Id { get; set; }
        public string VMSMDSH_ShiftName { get; set; }
        public string VMSMDSH_ShiftStartTime { get; set; }
        public string VMSMDSH_ShiftEndTime { get; set; }
        public bool VMSMDSH_OnlyOnSpecificDayFlg { get; set; }
        public string VMSMDSH_Remarks { get; set; }
        public bool VMSMDSH_ActiveFlag { get; set; }

        public DateTime VMSMDSH_CreatedDate { get; set; }
        public DateTime VMSMDSH_UpdatedDate { get; set; }
        public long VMSMDSH_CreatedBy { get; set; }
        public long VMSMDSH_UpdatedBy { get; set; }

    }
}
