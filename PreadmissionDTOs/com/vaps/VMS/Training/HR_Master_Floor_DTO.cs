using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;

namespace PreadmissionDTOs.com.vaps.VMS.Training
{
    public class HR_Master_Floor_DTO : CommonParamDTO
    {
        public long HRMF_Id { get; set; }
        public long MI_Id { get; set; }
        public long HRMB_Id { get; set; }
        public string HRMF_FloorName { get; set; }
        public bool HRMF_ActiveFlag { get; set; }
        public long HRMF_CreatedBy { get; set; }
        public long HRMF_UpdatedBy { get; set; }
        public long userId { get; set; }
        public bool returnval { get; set; }
        public string returnvalue { get; set; }
        public string HRMB_BuildingName { get; set; }
        public Array floor_list { get; set; }
        public Array building_list { get; set; }
       



    }
}