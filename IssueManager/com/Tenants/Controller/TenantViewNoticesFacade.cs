using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IssueManager.com.Tenants.Interface;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.IssueManager.Tenants;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IssueManager.com.Tenants.Controller
{
    [Route("api/[controller]")]
    public class TenantViewNoticesFacade :ControllerBase
    {
        public TenantViewNoticesInterface _objinter;

        public TenantViewNoticesFacade(TenantViewNoticesInterface parameter)
        {
            _objinter = parameter;
        }

        [Route("getdetails")]
        public Task<TenantAgreementDTO> getdetails([FromBody] TenantAgreementDTO data)
        {
            return _objinter.getdetails(data);
        }

        [Route("get_report")]
        public Task<TenantAgreementDTO> get_report([FromBody]TenantAgreementDTO data)
        {
            return _objinter.get_report(data);
        }
        [Route("ViewFiles")]
        public Task<TenantAgreementDTO> ViewFiles([FromBody]TenantAgreementDTO data)
        {
            return _objinter.ViewFiles(data);
        }

        [Route("get_NoticeBoardreport")]
        public Task<TenantAgreementDTO> get_NoticeBoardreport([FromBody]TenantAgreementDTO data)
        {
            return _objinter.get_NoticeBoardreport(data);
        }

    }
}
