using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.IssueManager.DriversChart
{
   public class DriverChartGenerationDTO
    {

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
        public long UserId { get; set; }

        public Array master_shift { get; set; }
        public Array master_employee { get; set; }
        public bool returnval { get; set; }
        public bool duplicate { get; set; }
        //Akash
        public Array griddata { get; set; }


    }
}
