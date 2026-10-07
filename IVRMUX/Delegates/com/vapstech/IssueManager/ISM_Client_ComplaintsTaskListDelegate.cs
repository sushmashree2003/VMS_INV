using CommonLibrary;
using PreadmissionDTOs.com.vaps.IssueManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.IssueManager
{
    public class ISM_Client_ComplaintsTaskListDelegate
    {
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<ISM_ComplaintsTaskListDTO, ISM_ComplaintsTaskListDTO> COMMM = new CommonDelegate<ISM_ComplaintsTaskListDTO, ISM_ComplaintsTaskListDTO>();

        public ISM_ComplaintsTaskListDTO getdetails(ISM_ComplaintsTaskListDTO data)
        {
            return COMMM.POSTIssueManager(data, "ISM_Client_ComplaintsTaskListFacade/getdetails/");
        }
        public ISM_ComplaintsTaskListDTO gettaskdetails(ISM_ComplaintsTaskListDTO data)
        {
            return COMMM.POSTIssueManager(data, "ISM_Client_ComplaintsTaskListFacade/gettaskdetails");
        }  
        public ISM_ComplaintsTaskListDTO getsearchtask(ISM_ComplaintsTaskListDTO data)
        {
            return COMMM.POSTIssueManager(data, "ISM_Client_ComplaintsTaskListFacade/getsearchtask");
        }       

    }
}
