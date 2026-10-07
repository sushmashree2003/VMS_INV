using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs
{
    public class MasterSubjectAllMDTO : CommonParamDTO
    {
        public bool already_cnt { get; set; }

        public long ISMS_Id { get; set; }
        public long MI_Id { get; set; }
        public string ISMS_SubjectName { get; set; }
        public string ISMS_IVRSSubjectName { get; set; }
        public string ISMS_SubjectCode { get; set; }
        public decimal? ISMS_Max_Marks { get; set; }
        public decimal? ISMS_Min_Marks { get; set; }
        public long? ISMS_ExamFlag { get; set; }
        public long? ISMS_PreadmFlag { get; set; }
        public long? ISMS_SubjectFlag { get; set; }
        public long? ISMS_BatchAppl { get; set; }
        public long? ISMS_ActiveFlag { get; set; }
        public long? ISMS_OrderFlag { get; set; }
        public bool ISMS_TTFlag { get; set; }
        public bool ISMS_AttendanceFlag { get; set; }
        public int? ISMS_LanguageFlg { get; set; }
        public int? ISMS_AtExtraFeeFlg { get; set; }
        public Array subject_m_list { get; set; }
        public Array subject_m_listOrder { get; set; }
        public string returnduplicatestatus { get; set; }
        public string returnvaluetype { get; set; }
        public bool returnval { get; set; }
        public Array edit_m_subject { get; set; }

        public long PAMS_Id { get; set; }
     
        public string PAMS_SubjectName { get; set; }
        public string PAMS_SubjectCode { get; set; }
        public decimal PAMS_MaxMarks { get; set; }
        public decimal PAMS_MinMarks { get; set; }
        public string PAMS_SubjectFlag { get; set; }
        public int PAMS_ActiveFlag { get; set; }
        public Array MasterSubjectData { get; set; }
        public string msg { get; set; }
        public int count { get; set; }
        public subject_orderDTO[] subjectDTO { get; set; }
        public string retrunMsg { get; set; }

    }
    public class subject_orderDTO:CommonParamDTO
    {
        public long ISMS_Id { get; set; }
        public long MI_Id { get; set; }
        public string ISMS_SubjectName { get; set; }
        public string ISMS_SubjectCode { get; set; }
        public decimal? ISMS_Max_Marks { get; set; }
        public decimal? ISMS_Min_Marks { get; set; }
        public long ISMS_ExamFlag { get; set; }
        public long ISMS_PreadmFlag { get; set; }
        public long ISMS_SubjectFlag { get; set; }
        public long ISMS_BatchAppl { get; set; }
        public long ISMS_ActiveFlag { get; set; }
        public long ISMS_OrderFlag { get; set; }
        public bool ISMS_TTFlag { get; set; }
        public bool ISMS_AttendanceFlag { get; set; }
        public int ISMS_LanguageFlg { get; set; }
        public int ISMS_AtExtraFeeFlg { get; set; }
    }

}
