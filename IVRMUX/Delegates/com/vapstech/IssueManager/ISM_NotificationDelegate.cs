using CommonLibrary;
using PreadmissionDTOs.com.vaps.IssueManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.IssueManager
{
    public class ISM_NotificationDelegate
    {
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<ISM_NotificationDTO, ISM_NotificationDTO> COMMM = new CommonDelegate<ISM_NotificationDTO, ISM_NotificationDTO>();

        public ISM_NotificationDTO getdetails(ISM_NotificationDTO data)
        {
            return COMMM.POSTIssueManager(data, "ISM_NotificationFacade/getdetails/");
        }
        public ISM_NotificationDTO getnotificationdetails(ISM_NotificationDTO data)
        {
            return COMMM.POSTIssueManager(data, "ISM_NotificationFacade/getnotificationdetails");
        }
        public ISM_NotificationDTO pushnotificationdetails(ISM_NotificationDTO data)
        {
            return COMMM.POSTIssueManager(data, "ISM_NotificationFacade/pushnotificationdetails");
        }
    

    }
}
