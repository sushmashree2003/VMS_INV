using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.VMS.HRMS
{
    public class OnlineTestCandidateDTO
    {
        public long HRCD_Id { get; set; }
        public long OTCANDEX_Id { get; set; }
        public long OTMOEQ_Id { get; set; }
        public long MI_Id { get; set; }
        public string OTMOEQF_FileName { get; set; }
        public string OTMOEQF_FilePath { get; set; }
        public int totalquestion { get; set; }
        public DateTime? fromdate { get; set; }
        public DateTime? todate { get; set; }
        public long roleid { get; set; }
        public long Userid { get; set; }
        public int countoption { get; set; }
        public long HRMP_Id { get; set; }
        public long OTQPTYP_Id { get; set; }
      
        public string type { get; set; }
        public string OTMOEQ_Question { get; set; }
        public string rolename { get; set; }
        public string HRMP_Position { get; set; }
        public string OTCANDEX_TotalTime { get; set; }
        public string OTCANDEXSANS_Answer { get; set; }
        public string User_Name { get; set; }
        public string OTQPTYP_QuestionPaperName { get; set; }
        public string OTQPTYP_QuestionPaperDesc { get; set; }
        public bool OTMOEQ_SubjectiveFlg { get; set; }
        public bool OTMOEQ_ActiveFlg { get; set; }
        public bool returnval { get; set; }
        public bool OTMOEQ_AnswerUploadFlag { get; set; }
        public string OTMOEQ_Answer { get; set; }
        public decimal maxmarks { get; set; }
        public decimal OTMOEQ_Marks { get; set; }
        public string OTMOEQ_QuestionDesc { get; set; }
        public string OTMOEQOAF_FileName { get; set; }
        public string OTMOEQOAF_FilePath { get; set; }
        public Array result { get; set; }
        public Array getoptionwisefiles { get; set; }
        public Array getquestiondoclist { get; set; }
        public Array milist { get; set; }
        public Array getcandidatequesansdetails { get; set; }
        public Array getcandidatequesansdetailsobj { get; set; }
        public Array getcandidatedetails { get; set; }
        public Array papertypelist { get; set; }
        public Array editlist { get; set; }
        public Array getsavedanswer { get; set; }
        public Array getallexamdetails { get; set; }
        public Array getexamquestionlist { get; set; }
        public Array gettodaysexamdetails { get; set; }
        public Array geteditmasterquestion { get; set; }
        public Array getquestionoptionlist { get; set; }
        public Array getMasterQuestiondetails { get; set; }
        public Array positionlist { get; set; }
        public Array getviedocarray { get; set; }
        public Array getSavedOptions { get; set; }
        public Array getViewSavedOptions { get; set; }
        public long? OTMOEQOA_Id { get; set; }
      
        public string message { get; set; }
        public string OTMOEQOA_Option { get; set; }
        public string OTMOEQOA_OptionCode { get; set; }
        public bool OTMOEQOA_AnswerFlag { get; set; }
        public string OTMOEQOA_AnswerDesc { get; set; }
        public DateTime OTMOEQ_CreatedDate { get; set; }
        public tempoptionsDTO[] tempoptionsDTO { get; set; }
        public saveanswerlsttemp[] saveanswerlsttemp { get; set; }
        public LP_OnlineFinalDetails[] LP_OnlineFinalDetails { get; set; }
        public exmids[] exmids { get; set; }
        public savedetails[] savedetails { get; set; }

        public tempansfilesdto[] tempoptionsfiles { get; set; }


    }

    public class tempansfilesdto
    {
        public long OTCANDEX_Id { get; set; }
        public string OTCANDEXAS_AnswerSheetFile { get; set; }
        public string OTCANDEXAS_AnswerSheetPath { get; set; }
        public long OTMOEQ_Id { get; set; }


    }

    public class saveanswerlsttemp
    {
        public long OTMOEQ_Id { get; set; }
        public long OTMOEQOA_Id { get; set; }
        public long QuizeQuastions { get; set; }
        public string OTMOEQ_Question { get; set; }
        public bool OTMOEQOA_AnswerFlag { get; set; }
    }
    public class LP_OnlineFinalDetails
    {
        public long OTMOEQ_Id { get; set; }
        public long? QuizeQuastions { get; set; }
        public string OTMOEQ_Question { get; set; }
        public bool OTMOEQOA_AnswerFlag { get; set; }
        public bool OTMOEQ_SubjectiveFlg { get; set; }
        public string answer { get; set; }
        public tempansfilesdto[] tempoptionsfiles { get; set; }
    }
    public class savedetails
    {
        public long OTQPTYP_Id { get; set; }
        public decimal? OTCANDEXSANS_Marks { get; set; }
        public decimal? OTMOEQ_Marks { get; set; }
        public long OTCANDEX_Id { get; set; }
        public long OTMOEQ_Id { get; set; }
    }
    public class exmids {

        public long OTCANDEX_Id { get; set; }

    }
}