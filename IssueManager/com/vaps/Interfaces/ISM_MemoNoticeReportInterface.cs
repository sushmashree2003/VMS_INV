using PreadmissionDTOs.com.vaps.IssueManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IssueManager.com.vaps.Interfaces
{
    public interface ISM_MemoNoticeReportInterface
    {
        Task<ISM_EMPLOYEE_MEMONOTICEDTO> getdetails(ISM_EMPLOYEE_MEMONOTICEDTO data);
        ISM_EMPLOYEE_MEMONOTICEDTO getMemoNoticeReport(ISM_EMPLOYEE_MEMONOTICEDTO data);
        ISM_EMPLOYEE_MEMONOTICEDTO gettaskdetails(ISM_EMPLOYEE_MEMONOTICEDTO data);

        Task<ISM_EMPLOYEE_MEMONOTICEDTO> getdetailsNew(ISM_EMPLOYEE_MEMONOTICEDTO data);
        Task<ISM_EMPLOYEE_MEMONOTICEDTO> DeptselectiondetailsNew(ISM_EMPLOYEE_MEMONOTICEDTO data);
        Task<ISM_EMPLOYEE_MEMONOTICEDTO> DesgselectiondetailsNew(ISM_EMPLOYEE_MEMONOTICEDTO data);
    }
}
