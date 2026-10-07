using CommonLibrary;
using PreadmissionDTOs.com.vaps.VMS.HRMS;
using System;

namespace corewebapi18072016.Delegates.com.vapstech.VMS.OT
{
    public class OnlineTestDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<OnlineTestDTO, OnlineTestDTO> COMMM = new CommonDelegate<OnlineTestDTO, OnlineTestDTO>();

        #region  MASTER QUESTION 
        //  MASTER QUESTION 
        public OnlineTestDTO getmasterquestionloaddata(OnlineTestDTO data)
        {
            return COMMM.POSTVMS(data, "OnlineTestFacade/getmasterquestionloaddata");
        }
        public OnlineTestDTO getmastercompliexities(OnlineTestDTO data)
        {
            return COMMM.POSTVMS(data, "OnlineTestFacade/getmastercompliexities");
        }
        public OnlineTestDTO getqnspapertype(OnlineTestDTO data)
        {
            return COMMM.POSTVMS(data, "OnlineTestFacade/getqnspapertype");
        }
     
        public OnlineTestDTO SaveMasterQuestionDetails(OnlineTestDTO data)
        {
            return COMMM.POSTVMS(data, "OnlineTestFacade/SaveMasterQuestionDetails");
        }
        public OnlineTestDTO EditMasterQuestion(OnlineTestDTO data)
        {
            return COMMM.POSTVMS(data, "OnlineTestFacade/EditMasterQuestion");
        }
        public OnlineTestDTO ViewMasterQuesDoc(OnlineTestDTO data)
        {
            return COMMM.POSTVMS(data, "OnlineTestFacade/ViewMasterQuesDoc");
        }
        public OnlineTestDTO DeactivateActivateDocument(OnlineTestDTO data)
        {
            return COMMM.POSTVMS(data, "OnlineTestFacade/DeactivateActivateDocument");
        }
        public OnlineTestDTO ViewMasterQuesOptions(OnlineTestDTO data)
        {
            return COMMM.POSTVMS(data, "OnlineTestFacade/ViewMasterQuesOptions");
        }
        public OnlineTestDTO DeactivateActivateQuesOption(OnlineTestDTO data)
        {
            return COMMM.POSTVMS(data, "OnlineTestFacade/DeactivateActivateQuesOption");
        }
        public OnlineTestDTO AddMoreOptions(OnlineTestDTO data)
        {
            return COMMM.POSTVMS(data, "OnlineTestFacade/AddMoreOptions");
        }
        public OnlineTestDTO DeactivateActivateQues(OnlineTestDTO data)
        {
            return COMMM.POSTVMS(data, "OnlineTestFacade/DeactivateActivateQues");
        }
        public OnlineTestDTO ViewUploadOptionFiles(OnlineTestDTO data)
        {
            return COMMM.POSTVMS(data, "OnlineTestFacade/ViewUploadOptionFiles");
        }
        public OnlineTestDTO DeactivateActivateOptionsDocument(OnlineTestDTO data)
        {
            return COMMM.POSTVMS(data, "OnlineTestFacade/DeactivateActivateOptionsDocument");
        }
        public OnlineTestDTO loadcdata(OnlineTestDTO data)
        {
            return COMMM.POSTVMS(data, "OnlineTestFacade/loadcdata");
        }
        public OnlineTestDTO DeactivateActivateComplexities(OnlineTestDTO data)
        {
            return COMMM.POSTVMS(data, "OnlineTestFacade/DeactivateActivateComplexities");
        }
        public OnlineTestDTO SaveMasterComplexity(OnlineTestDTO data)
        {
            return COMMM.POSTVMS(data, "OnlineTestFacade/SaveMasterComplexity");
        }
        #endregion


 

    }
}
