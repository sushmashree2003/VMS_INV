using PreadmissionDTOs.com.vaps.VMS.HRMS;

namespace Recruitment.com.vaps.Interfaces
{
    public interface OnlineTestInterface
    {
        #region  MASTER QUESTION 
        //  MASTER QUESTION 
        OnlineTestDTO getmastercompliexities(OnlineTestDTO data);
        OnlineTestDTO getmasterquestionloaddata(OnlineTestDTO data);
        OnlineTestDTO getqnspapertype(OnlineTestDTO data);
        OnlineTestDTO SaveMasterQuestionDetails(OnlineTestDTO data);
        OnlineTestDTO EditMasterQuestion(OnlineTestDTO data);
        OnlineTestDTO ViewMasterQuesDoc(OnlineTestDTO data);
        OnlineTestDTO DeactivateActivateDocument(OnlineTestDTO data);
        OnlineTestDTO ViewMasterQuesOptions(OnlineTestDTO data);
        OnlineTestDTO DeactivateActivateQuesOption(OnlineTestDTO data);
        OnlineTestDTO AddMoreOptions(OnlineTestDTO data);
        OnlineTestDTO DeactivateActivateQues(OnlineTestDTO data);
        OnlineTestDTO ViewUploadOptionFiles(OnlineTestDTO data);
        OnlineTestDTO DeactivateActivateOptionsDocument(OnlineTestDTO data);
        OnlineTestDTO loadcdata(OnlineTestDTO data);
        OnlineTestDTO DeactivateActivateComplexities(OnlineTestDTO data);
        OnlineTestDTO SaveMasterComplexity(OnlineTestDTO data);
        #endregion



    }
}
