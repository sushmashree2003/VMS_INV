using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs
{
    public class OralTestScheduleDTO : CommonParamDTO
    {

        public long PAOTS_Id { get; set; }
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long IVRMSTAUL_Id { get; set; }
        public DateTime? PAOTS_EntryDate { get; set; }
        public string PAOTS_ScheduleName { get; set; }
        public DateTime? PAOTS_ScheduleDate { get; set; }
        public string PAOTS_ScheduleTime { get; set; }
        public string PAOTS_ScheduleTimeTo { get; set; }
        public string PAOTS_AM_PM { get; set; }
        public string PAOTS_Remarks { get; set; }
        public DateTime? PAOTS_LB_FT { get; set; }
        public DateTime? PAOTS_LB_TT { get; set; }
        public int PAOTS_TimePeriod { get; set; }
        public bool PAOTS_TPFlag { get; set; }

        public bool autoschedule { get; set; }
        public int PAOTS_Strength { get; set; }

        public long PASR_Id { get; set; }

        public Array OralTestSchedule { get; set; }

        public Array SelectedStudentDetails { get; set; }

        public List<StudentDetailsDTO> SelectedStudentData { get; set; }

        public List<StudentDetailsDTO> SelectedStudentDataForEdit { get; set; }

        public string PAOTS_Superviser { get; set; }

        public string PAOTS_Skills { get; set; }

        public string returnvalue { get; set; }

        public long pamsT_Id { get; set; }

        public long userid { get; set; }
         
    }
}
