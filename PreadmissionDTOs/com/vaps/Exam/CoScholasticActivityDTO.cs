using Microsoft.AspNetCore.Http;
using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.Exam
{
    public class CoScholasticActivityDTO
    {

        public string returnduplicatestatus { get; set; }
        public bool returnval { get; set; }
        public bool already_cnt { get; set; }


        public int ECACT_Id { get; set; }
        public long MI_Id { get; set; }
        public string ECACT_SkillName { get; set; }
        public string ECACT_SkillCode { get; set; }
        public bool ECACT_ActiveFlag { get; set; }
        public Array editlist { get; set; }
        public Array gridlist { get; set; }

        //--------------EXM_CCE_Activities_AREA
        public int ECACTA_Id { get; set; }
        public string ECACTA_SkillArea { get; set; }
        public int ECACTA_SkillOrder { get; set; }
        public bool ECACTA_ActiveFlag { get; set; }
        public Array editlist1 { get; set; }
        public Array gridlist1 { get; set; }




    }
}
