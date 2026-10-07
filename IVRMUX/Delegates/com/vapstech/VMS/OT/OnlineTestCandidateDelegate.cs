using CommonLibrary;
using PreadmissionDTOs.com.vaps.VMS.HRMS;
using System;

namespace corewebapi18072016.Delegates.com.vapstech.VMS.OT
{
    public class OnlineTestCandidateDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<OnlineTestCandidateDTO, OnlineTestCandidateDTO> COMMM = new CommonDelegate<OnlineTestCandidateDTO, OnlineTestCandidateDTO>();
        
        #region EXAM
        public OnlineTestCandidateDTO loadExamdata(OnlineTestCandidateDTO data)
        {
            return COMMM.POSTVMS(data, "OnlineTestCandidateFacade/loadExamdata");
        }
         public OnlineTestCandidateDTO getQuestion(OnlineTestCandidateDTO data)
        {
            return COMMM.POSTVMS(data, "OnlineTestCandidateFacade/getQuestion");
        }
         public OnlineTestCandidateDTO Saveanswer(OnlineTestCandidateDTO data)
        {
            return COMMM.POSTVMS(data, "OnlineTestCandidateFacade/Saveanswer");
        }
        public OnlineTestCandidateDTO submitexam(OnlineTestCandidateDTO data)
        {
            return COMMM.POSTVMS(data, "OnlineTestCandidateFacade/submitexam");
        }

        #endregion

        #region PAPER CORRECTION
        public OnlineTestCandidateDTO loadcorrection(OnlineTestCandidateDTO data)
        {
            return COMMM.POSTVMS(data, "OnlineTestCandidateFacade/loadcorrection");
        }

        public OnlineTestCandidateDTO GetSearchDetails(OnlineTestCandidateDTO data)
        {
            return COMMM.POSTVMS(data, "OnlineTestCandidateFacade/GetSearchDetails");
        }
        public OnlineTestCandidateDTO oncmpchange(OnlineTestCandidateDTO data)
        {
            return COMMM.POSTVMS(data, "OnlineTestCandidateFacade/oncmpchange");
        }
        public OnlineTestCandidateDTO ViewQuestionanswers(OnlineTestCandidateDTO data)
        {
            return COMMM.POSTVMS(data, "OnlineTestCandidateFacade/ViewQuestionanswers");
        }
        public OnlineTestCandidateDTO SaveSubjectiveMarks(OnlineTestCandidateDTO data)
        {
            return COMMM.POSTVMS(data, "OnlineTestCandidateFacade/SaveSubjectiveMarks");
        }
        #endregion


        #region REPORT

        public OnlineTestCandidateDTO loadDetailsRpt(OnlineTestCandidateDTO data)
        {
            return COMMM.POSTVMS(data, "OnlineTestCandidateFacade/loadDetailsRpt");
        }

        public OnlineTestCandidateDTO GetReport(OnlineTestCandidateDTO data)
        {
            return COMMM.POSTVMS(data, "OnlineTestCandidateFacade/GetReport");
        }
        public OnlineTestCandidateDTO ViewResultRpt(OnlineTestCandidateDTO data)
        {
            return COMMM.POSTVMS(data, "OnlineTestCandidateFacade/ViewResultRpt");
        }
        #endregion



        
    }
}
