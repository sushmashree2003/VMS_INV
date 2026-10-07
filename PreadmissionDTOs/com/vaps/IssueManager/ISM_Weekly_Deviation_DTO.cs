using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.IssueManager
{
    public class ISM_Weekly_Deviation_DTO
    {
        public long ISMWD_Id { get; set; }
        public long HRME_Id { get; set; }
        public long ISMTPL_Id { get; set; }
        public DateTime ISMWD_StartDate { get; set; }
        public DateTime ISMWD_EndDate { get; set; }
        public long ISMWD_TotalEffort { get; set; }
        public long ISMWD_CompletedEffort { get; set; }
        public string ISMWD_Completed { get; set; }
        public string ISMWD_Deviation { get; set; }
        public Array deviation_ary { get; set; }
        public Array deviation_menomail { get; set; }

        public  List<ISM_Weekly_Deviation_DTO> deviation_list{ get; set; }
    }
}
