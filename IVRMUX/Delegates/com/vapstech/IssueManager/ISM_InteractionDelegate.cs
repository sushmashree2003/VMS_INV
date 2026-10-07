using CommonLibrary;
using PreadmissionDTOs.com.vaps.IssueManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.IssueManager
{
    public class ISM_InteractionDelegate
    {
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<ISM_InteractionDTO, ISM_InteractionDTO> COMMM = new CommonDelegate<ISM_InteractionDTO, ISM_InteractionDTO>();

        public ISM_InteractionDTO getdetails(ISM_InteractionDTO data)
        {
            return COMMM.POSTIssueManager(data, "ISM_InteractionFacade/getdetails/");
        }     
        public ISM_InteractionDTO savedata(ISM_InteractionDTO data)
        {
            return COMMM.POSTIssueManager(data, "ISM_InteractionFacade/savedata");
        }
        public ISM_InteractionDTO get_replydetails(ISM_InteractionDTO data)
        {
            return COMMM.POSTIssueManager(data, "ISM_InteractionFacade/get_replydetails");
        }     
         public ISM_InteractionDTO savereply(ISM_InteractionDTO data)
        {
            return COMMM.POSTIssueManager(data, "ISM_InteractionFacade/savereply");
        }     

        //=================feedback==================
          public ISM_InteractionDTO getfeedbac(ISM_InteractionDTO data)
        {
            return COMMM.POSTIssueManager(data, "ISM_InteractionFacade/getfeedbac");
        }
        public ISM_InteractionDTO feedback_msg_details(ISM_InteractionDTO data)
        {
            return COMMM.POSTIssueManager(data, "ISM_InteractionFacade/feedback_msg_details");
        }
        public async Task<ISM_InteractionDTO> desktopnotification(ISM_InteractionDTO data)
        {
            return COMMM.POSTIssueManager(data, "ISM_InteractionFacade/desktopnotification");
        }     
         public ISM_InteractionDTO Sendfeedback_tfr(ISM_InteractionDTO data)
        {
            return COMMM.POSTIssueManager(data, "ISM_InteractionFacade/Sendfeedback_tfr");
        }     
         public ISM_InteractionDTO getViewfeedback(ISM_InteractionDTO data)
        {
            return COMMM.POSTIssueManager(data, "ISM_InteractionFacade/getViewfeedback");
        }

        //=================Notification==================
        public ISM_InteractionDTO getnotification(ISM_InteractionDTO data)
        {
            return COMMM.POSTIssueManager(data, "ISM_InteractionFacade/getnotification");
        }
        public ISM_InteractionDTO clicknotification(ISM_InteractionDTO data)
        {
            return COMMM.POSTIssueManager(data, "ISM_InteractionFacade/clicknotification");
        }


    }
}
