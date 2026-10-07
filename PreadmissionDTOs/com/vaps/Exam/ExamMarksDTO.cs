using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.Exam
{
    public class ExamMarksDTO : CommonParamDTO
    {
        public int ESTM_Id { get; set; }
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long ASMCL_Id { get; set; }
        public long ASMS_Id { get; set; }
        public long AMST_Id { get; set; }
        public int EME_Id { get; set; }
        public long ISMS_Id { get; set; }
        public decimal ESTM_Marks { get; set; }
        public string ESTM_MarksGradeFlg { get; set; }
        public long Id { get; set; }
        public DateTime LoginDateTime { get; set; }
        public string IP4 { get; set; }
        public bool ESTM_ActiveFlg { get; set; }
        public string ESTM_Grade { get; set; }
        public string ESTM_Flg { get; set; }
        public Array ctlist { get; set; }
        public Array seclist { get; set; }
        public Array subjectlist { get; set; }
        public Array Acdlist { get; set; }

        public Array examlist { get; set; }

        // public Array studentList { get; set; }
        public Array studentList { get; set; }
        public string SubjectName { get; set; }
        public decimal? TotalMarks { get; set; }
        public decimal? MarksEnterFor { get; set; }
        public decimal? MinMarks { get; set; }

        //public decimal? obtainmarks { get; set; }

        public string obtainmarks { get; set; }
        public string obtainvaluesold { get; set; }

        public long? amaY_RollNo { get; set; }
        public string amsT_AdmNo { get; set; }
        public string studentname { get; set; }

        public marksDTO[] detailsList { get; set; }

        public string messagesaveupdate { get; set; }

        public string subMorGFlag { get; set; }

        public long EMGR_Id { get; set; }

        public Array gradname { get; set; }

        public string grade { get; set; }

        public string marksdeleteflag { get; set; }
        public long saveupdatecount { get; set; }
        public long lastdateentry { get; set; }

    }
    public class marksDTO
    {
        public long  amsT_Id { get; set; }
        public decimal? totalMarks { get; set; }
        public decimal? minMarks { get; set; }
        public string obtainmarks { get; set; }
    
    }
}
