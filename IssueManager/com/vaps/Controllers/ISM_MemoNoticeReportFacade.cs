using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IssueManager.com.vaps.Interfaces;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.IssueManager;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IssueManager.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class ISM_MemoNoticeReportFacade : Controller
    {
        public ISM_MemoNoticeReportInterface _objinter;

        public ISM_MemoNoticeReportFacade(ISM_MemoNoticeReportInterface parameter)
        {
            _objinter = parameter;
        }

        [Route("getdetails")]
        public Task<ISM_EMPLOYEE_MEMONOTICEDTO> getdetails([FromBody] ISM_EMPLOYEE_MEMONOTICEDTO data)
        {
            return _objinter.getdetails(data);
        }

        [Route("getMemoNoticeReport")]
        public ISM_EMPLOYEE_MEMONOTICEDTO getMemoNoticeReport([FromBody]ISM_EMPLOYEE_MEMONOTICEDTO data)
        {
            return _objinter.getMemoNoticeReport(data);
        }

        [Route("gettaskdetails")]
        public ISM_EMPLOYEE_MEMONOTICEDTO gettaskdetails([FromBody]ISM_EMPLOYEE_MEMONOTICEDTO data)
        {
            return _objinter.gettaskdetails(data);
        }

        [Route("getdetailsNew")]
        public Task<ISM_EMPLOYEE_MEMONOTICEDTO> getdetailsNew([FromBody] ISM_EMPLOYEE_MEMONOTICEDTO data)
        {
            return _objinter.getdetailsNew(data);
        }
        [Route("DeptselectiondetailsNew")]
        public Task<ISM_EMPLOYEE_MEMONOTICEDTO> DeptselectiondetailsNew([FromBody] ISM_EMPLOYEE_MEMONOTICEDTO data)
        {
            return _objinter.DeptselectiondetailsNew(data);
        }
        [Route("DesgselectiondetailsNew")]
        public Task<ISM_EMPLOYEE_MEMONOTICEDTO> DesgselectiondetailsNew([FromBody] ISM_EMPLOYEE_MEMONOTICEDTO data)
        {
            return _objinter.DesgselectiondetailsNew(data);
        }
        
    }
}
