using PreadmissionDTOs.com.vaps.VMS.HRMS;

namespace Recruitment.com.vaps.Interfaces
{
    public interface OnlineTestCandidateInterface
    {
        #region  EXAM
      
        OnlineTestCandidateDTO loadExamdata(OnlineTestCandidateDTO data);
        OnlineTestCandidateDTO getQuestion(OnlineTestCandidateDTO data);
        OnlineTestCandidateDTO Saveanswer(OnlineTestCandidateDTO data);
        OnlineTestCandidateDTO submitexam(OnlineTestCandidateDTO data);

        #endregion

        #region PAPER CORRECTION

        OnlineTestCandidateDTO loadcorrection(OnlineTestCandidateDTO data);
        OnlineTestCandidateDTO GetSearchDetails(OnlineTestCandidateDTO data);
        OnlineTestCandidateDTO oncmpchange(OnlineTestCandidateDTO data);
        OnlineTestCandidateDTO ViewQuestionanswers(OnlineTestCandidateDTO data);
        OnlineTestCandidateDTO SaveSubjectiveMarks(OnlineTestCandidateDTO data);

        #endregion

        #region REPORT
        OnlineTestCandidateDTO loadDetailsRpt(OnlineTestCandidateDTO data);
        OnlineTestCandidateDTO GetReport(OnlineTestCandidateDTO data);
        OnlineTestCandidateDTO ViewResultRpt(OnlineTestCandidateDTO data);
        
        #endregion

    }
}
