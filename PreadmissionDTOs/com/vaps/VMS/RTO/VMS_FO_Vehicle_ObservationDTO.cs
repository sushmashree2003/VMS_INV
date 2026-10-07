using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.VMS.RTO
{
   public class VMS_FO_Vehicle_ObservationDTO
    {
        public long MI_Id { get; set; }
        public long UserId { get; set; }
        public string returnval { get; set; }
        public long VMSFOVOB_Id { get; set; }       
        public long HRME_Id { get; set; }
        public long TRMV_Id { get; set; }
        public DateTime? VMSFOVOB_DateTime { get; set; }
        public bool VMSFOVOB_AnyDamageFlg { get; set; }
        public string VMSFOVOB_DamageDescription { get; set; }
        public bool VMSFOVOB_ActiveFlag { get; set; }
        public Array ObserVationList { get; set; }
        public Array emplist { get; set; }
        public Array vehicalArray { get; set; }
        public Array editlist { get; set; }
        public Array filist { get; set; }
        public Observation_Files[] Observation_Fileslist { get; set; }
        public string Template { get; set; }
        public string CreatedBy { get; set; }
    }
    public class Observation_Files
    {
       public string VMSFOVOBFL_FileName { get; set; }
        public string VMSFOVOBFL_FilePath { get; set; }
        public string VMSFOVOBFL_Filedesc { get; set; }
    }
}
