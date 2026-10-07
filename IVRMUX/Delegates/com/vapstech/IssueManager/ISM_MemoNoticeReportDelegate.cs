using CommonLibrary;
using PreadmissionDTOs.com.vaps.IssueManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.IssueManager
{
    public class ISM_MemoNoticeReportDelegate
    {
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<ISM_EMPLOYEE_MEMONOTICEDTO, ISM_EMPLOYEE_MEMONOTICEDTO> COMMM = new CommonDelegate<ISM_EMPLOYEE_MEMONOTICEDTO, ISM_EMPLOYEE_MEMONOTICEDTO>();

        public ISM_EMPLOYEE_MEMONOTICEDTO getdetails(ISM_EMPLOYEE_MEMONOTICEDTO data)
        {
            return COMMM.POSTIssueManager(data, "ISM_MemoNoticeReportFacade/getdetails/");
        }
        public ISM_EMPLOYEE_MEMONOTICEDTO getMemoNoticeReport(ISM_EMPLOYEE_MEMONOTICEDTO data)
        {
            return COMMM.POSTIssueManager(data, "ISM_MemoNoticeReportFacade/getMemoNoticeReport");
        }

        public ISM_EMPLOYEE_MEMONOTICEDTO gettaskdetails(ISM_EMPLOYEE_MEMONOTICEDTO data)
        {
            return COMMM.POSTIssueManager(data, "ISM_MemoNoticeReportFacade/gettaskdetails");
        }
        public ISM_EMPLOYEE_MEMONOTICEDTO getdetailsNew(ISM_EMPLOYEE_MEMONOTICEDTO data)
        {
            return COMMM.POSTIssueManager(data, "ISM_MemoNoticeReportFacade/getdetailsNew/");
        }
        public ISM_EMPLOYEE_MEMONOTICEDTO DeptselectiondetailsNew(ISM_EMPLOYEE_MEMONOTICEDTO data)
        {
            return COMMM.POSTIssueManager(data, "ISM_MemoNoticeReportFacade/DeptselectiondetailsNew/");
        }
        
        public ISM_EMPLOYEE_MEMONOTICEDTO DesgselectiondetailsNew(ISM_EMPLOYEE_MEMONOTICEDTO data)
        {
            return COMMM.POSTIssueManager(data, "ISM_MemoNoticeReportFacade/DesgselectiondetailsNew/");
        }
    }
}
