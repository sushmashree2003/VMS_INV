using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.VMS.HRMS
{
    public class OnlineTestDTO
    {
        public long OTMOEQ_Id { get; set; }
        public long OTMOEQOAF_Id { get; set; }
        public long OTMOEQF_Id { get; set; }
        public long MI_Id { get; set; }
        public long roleid { get; set; }
        public bool OTMOEQOA_ActiveFlg { get; set; }
        public bool OTMOEQ_AnswerUploadFlag { get; set; }
        public long Userid { get; set; }
        public long? LPMCOMP_Id { get; set; }
        public int countoptionfiles { get; set; }
        public int countdoc { get; set; }
        public int countoption { get; set; }
        public long HRMP_Id { get; set; }
        public long OTQPTYP_Id { get; set; }
        public string LPMCOMP_ComplexityName { get; set; }
        public string LPMCOMP_ComplexityDesc { get; set; }
        public string OTMOEQ_Question { get; set; }
        public string HRMP_Position { get; set; }
        public string OTQPTYP_QuestionPaperName { get; set; }
        public bool OTMOEQ_SubjectiveFlg { get; set; }
        public bool OTMOEQ_ActiveFlg { get; set; }
        public bool returnval { get; set; }
        public string OTMOEQ_Answer { get; set; }
        public decimal OTMOEQ_Marks { get; set; }
        public string OTMOEQ_QuestionDesc { get; set; }
        public Array papertypelist { get; set; }
        public Array getMasterComplexitiesdetails { get; set; }
        public Array editlist { get; set; }
        public Array geteditdocuments { get; set; }
        public Array milist { get; set; }
        public Array geteditmasterquestion { get; set; }
        public Array getMasterQuestiondetails { get; set; }
        public Array getarratcomplexities { get; set; }
        public Array positionlist { get; set; }
        public Array getViewSavedOptionsFiles { get; set; }
        public Array getviedocarray { get; set; }
        public Array getSavedOptions { get; set; }
        public Array getViewSavedOptions { get; set; }
        public long OTMOEQOA_Id { get; set; }
      
        public string message { get; set; }
        public string OTMOEQOA_Option { get; set; }
        public string OTMOEQOA_OptionCode { get; set; }
        public bool OTMOEQOA_AnswerFlag { get; set; }
        public string OTMOEQOA_AnswerDesc { get; set; }
        public DateTime OTMOEQ_CreatedDate { get; set; }
        public tempoptionsDTO[] tempoptionsDTO { get; set; }
        public qstionfilesDTO[] tempfilesDTO { get; set; }
 


    }


    public class tempoptionsDTO
    {
        public long OTMOEQOA_Id { get; set; }
        public string OTMOEQOA_Option { get; set; }
        public string OTMOEQOA_OptionCode { get; set; }
        public bool OTMOEQOA_AnswerFlag { get; set; }
        public string OTMOEQOA_AnswerDesc { get; set; }
        public tempoptionsfilesot[] tempoptionsfiles { get; set; }
    }


    public class qstionfilesDTO
    {
        public long OTMOEQF_Id { get; set; }
        public string OTMOEQF_FileName { get; set; }
        public string OTMOEQF_FilePath { get; set; }
    }



    public class tempoptionsfilesot
    {
        public long OTMOEQOAF_Id { get; set; }
        public string OTMOEQOAF_FileName { get; set; }
        public string OTMOEQOAF_FilePath { get; set; }
        public bool? OTMOEQOAF_ActiveFlag { get; set; }
    }
}