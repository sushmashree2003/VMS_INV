using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.FrontOffice
{
    public class SecurityRoasterDTO
    {
        public long ISMSR_Id { get; set; }
        public long ISMSD_Id { get; set; }
        public long MI_Id { get; set; }
        public DateTime ISMSR_Date { get; set; }
        public DateTime fromdate { get; set; }
        public DateTime todate { get; set; }
        public long HRMF_Id { get; set; }
        public string ISMSR_From { get; set; }
        public string ISMSR_To { get; set; }
        public string ISMSR_Status { get; set; }
        public long ISMSR_Userid { get; set; }
        public string ISMSR_Remarks { get; set; }
        public bool ISMSR_ActiveFlag { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public long ISMSR_CreatedBy { get; set; }
        public long ISMSR_UpdatedBy { get; set; }
        public long UserId { get; set; }
        public bool duplicate { get; set; }
        public string msg { get; set; }
        public bool ret { get; set; }
        public Array Editlist { get; set; }
        public Array alldata { get; set; }
        public Array Binddata { get; set; }
        public Array floorlist { get; set; }
        public Array namelist { get; set; }
        public Array report { get; set; }
        public string ISMSD_FirstName { get; set; }
        public string name { get; set; }
        public string floorname { get; set; }
        public string HRMF_FloorName { get; set; }
        public string ISMSD_Designation { get; set; } 
        public long[] flrlist { get; set; }
    }
}
