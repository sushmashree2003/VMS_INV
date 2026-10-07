using PreadmissionDTOs.com.vaps.IssueManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IssueManager.com.vaps.Interfaces
{
    public interface ISM_DailyReport_CheckListInterface
    {
        ISM_DailyReport_CheckListDTO loaddata(ISM_DailyReport_CheckListDTO dto);
        ISM_DailyReport_CheckListDTO getdata(ISM_DailyReport_CheckListDTO dto);
    }
}
