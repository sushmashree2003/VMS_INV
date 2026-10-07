using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.OnlineExam
{
    public class MasterQuestionDTO
    {
        public long MI_Id { get; set; }
        public string returnduplicatestatus { get; set; }
        public bool returnval { get; set; }
        public bool already_cnt { get; set; }
        public string message { get; set; }

        //---------------------------------------------------
        public long ASMCL_Id { get; set; }
        public long PAMOEQ_Id { get; set; }
        public long ISMS_Id { get; set; }
        public string PAMOEQ_Question { get; set; }
        public decimal PAMOEQ_Marks { get; set; }
        public string PAMOEQ_QuestionDesc { get; set; }
        public long PAMOEQOA_Id { get; set; } 
        public string PAMOEQOA_Option { get; set; }
        public string PAMOEQOA_OptionCode { get; set; }
        public bool PAMOEQOA_AnswerFlag { get; set; }
        public string OptionType { get; set; }

        public Array getQuestiondetails { get; set; }
        public Array getQdetails { get; set; }
        public Array getclass { get; set; }
        public Array getSubjects { get; set; }
        public Array getAnsOptions { get; set; }
        public Array editQus { get; set; }
        public Array getFQuestiondetails { get; set; }
        public Array getFQOptiondetails { get; set; }
        public Array getoptiondetails { get; set; }

        public Array result { get; set; }
        public string ASMCL_ClassName { get; set; }
        public long seleted_Qus { get; set; }
        public SelectOptionDTO[] seleted_Ans { get; set; }

        public class SelectOptionDTO
        {
           // public long PAMOEQOA_Id { get; set; }
            public long PAMOEQ_Id { get; set; }
            public string pamoeaO_Options { get; set; }
            public string PAMOEQOA_Option { get; set; }
            public string PAMOEQOA_OptionCode { get; set; }
            public bool PAMOEQOA_AnswerFlag { get; set; }
        }

        public long PAMOES_Id { get; set; }

        public long Noofques { get; set; }
        public long totmrks { get; set; }
        public string totdur { get; set; }
        public long echmrkques { get; set; }
        public string echquesdur { get; set; }
        public long noopt { get; set; }
       public long PAMOEQC_Id { get; set; }


    }
}