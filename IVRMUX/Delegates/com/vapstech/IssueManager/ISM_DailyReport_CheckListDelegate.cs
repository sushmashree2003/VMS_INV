using CommonLibrary;
using PreadmissionDTOs.com.vaps.IssueManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.IssueManager
{
  
    public class ISM_DailyReport_CheckListDelegate
    {
        CommonDelegate<ISM_DailyReport_CheckListDTO, ISM_DailyReport_CheckListDTO> COMM = new CommonDelegate<ISM_DailyReport_CheckListDTO, ISM_DailyReport_CheckListDTO>();
        public ISM_DailyReport_CheckListDTO loaddata(ISM_DailyReport_CheckListDTO data)
        {
            return COMM.POSTIssueManager(data, "ISM_DailyReport_CheckListFacade/loaddata/");
        }
        public ISM_DailyReport_CheckListDTO getdata(ISM_DailyReport_CheckListDTO data)
        {
            return COMM.POSTIssueManager(data, "ISM_DailyReport_CheckListFacade/getdata/");
        }

    }
}
