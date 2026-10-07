using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.OnlineExam
{
    public class OnlineExamDTO
    {
        public long MI_Id { get; set; }

        public long userid { get; set; }
        public long ASMCL_Id { get; set; }
        public long PAMOEQ_Id { get; set; }
        public string PAMOEQ_Question { get; set; }
        public string PAMOEQOA_OptionCode { get; set; }
        public string PAMOEQOA_Option { get; set; }
        public Array getQuestion { get; set; }
        public Array savedanswer { get; set; }
        public Array getclass { get; set; }
        public Array getSubjects { get; set; }
        public long ISMS_Id { get; set; }
        public string ISMS_SubjectName { get; set; }
        public bool PAMOEQOA_AnswerFlag { get; set; }
        public SelectOptionDTO[] saveanswerlst { get; set; }
        public Array getQdetails { get; set; }
        public long PASTE_Id { get; set; }
        public long Amst_ID { get; set; }
        public string PASTE_TotalTime { get; set; }
        public Array result { get; set; }
        public class SelectOptionDTO
        {
            public long PAMOEQOA_Id { get; set; }
            public long QuizeQuastions { get; set; }
            public long PAMOEQ_Id { get; set; }
            public string q_name { get; set; }
            public test[] test1 { get; set; }
          
            public class test
            {
                public long? ISMS_Id { get; set; }
                public string ISMS_SubjectName { get; set; }
                public bool PAMOEQOA_AnswerFlag { get; set; }
                public long? Amst_ID { get; set; }
                public string PASTE_TotalTime { get; set; }
                public long? PASTE_TotalQnsAnswered { get; set; }
                public long? PASTE__TotalCorrectAns { get; set; }
                public long? PASTE_TotalMaxMarks { get; set; }
                public long? PASTE_TotalMarks { get; set; }
                public decimal? PASTE_Percentage { get; set; }
                public long? PASTE_Id { get; set; }
                public long PAMOEQOA_Id { get; set; }
                public long? MI_Id { get; set; }
                public long? userid { get; set; }
                public long ASMCL_Id { get; set; }
                public long PAMOEQ_Id { get; set; }
                public Array getQuestion { get; set; }
                public Array getclass { get; set; }
                public Array getSubjects { get; set; }
                public string PAMOEQ_Question { get; set; }
                public string PAMOEQOA_Option { get; set; }
                public string PAMOEQOA_OptionCode { get; set; }
                public SelectOptionDTO[] saveanswerlst { get; set; }

            }
         }

        public long PAMOEQOA_Id { get; set; }
      
    }
}