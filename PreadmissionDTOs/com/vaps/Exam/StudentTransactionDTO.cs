using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.Exam
{
    public class StudentTransactionDTO : CommonParamDTO
    {
        //Exm_CCE_SKILLS_Transaction
        public int ECST_Id { get; set; }
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long ASMCL_Id { get; set; }
        public long ASMS_Id { get; set; }
        public long AMST_Id { get; set; }
        public long ECS_Id { get; set; }
        public long ECSA_Id { get; set; }
        public int ECT_Id { get; set; }
        public decimal? ECST_Score { get; set; }
        public int EMGR_Id { get; set; }
        public bool ECST_ActiveFlag { get; set; }

        //Exm_CCE_Activities_Transaction
        public int ECSACTT_Id { get; set; }

        public int ECACT_Id { get; set; }
        public int ECACTA_Id { get; set; }

        public decimal? ECSACTT_Score { get; set; }

        public bool ECSACTT_ActiveFlag { get; set; }
        //Common members.
        public Array academicYear { get; set; }
        public Array classList { get; set; }
        public Array sectionList { get; set; }
        public Array examTerms { get; set; }
        public Array skillsList { get; set; }
        public Array activitiesList { get; set; }
        public Array studentList { get; set; }
        public Array areasList { get; set; }
        public StudentTransactionDTO[] scoreList { get; set; } 
        public string studentName { get; set; }
        public string regNumber { get; set; }
        public long rollNumber { get; set; }
        public string areaName { get; set; }
        public string Skills { get; set; }
        public StudentTransactionDTO[] SelectedStudentScore { get; set; }
        public string returnVal { get; set; }
    }
}
