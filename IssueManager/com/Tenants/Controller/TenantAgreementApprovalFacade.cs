using System;
using System.Collections.Generic;
using System.Linq;
using IssueManager.com.Tenants.Interface;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.IssueManager.Tenants;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IssueManager.com.Tenants.Facade
{
    [Route("api/[controller]")]
    public class TenantAgreementApprovalFacade : ControllerBase
    {
        public TenantAgreementApprovalInterface _interface;

        public TenantAgreementApprovalFacade(TenantAgreementApprovalInterface _inter)
        {
            _interface = _inter;
        }

        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [Route("loaddata")]
        public TenantAgreementApprovalDTO loaddata([FromBody] TenantAgreementApprovalDTO data)
        {
            return _interface.loaddata(data);
        }

        //[HttpGet]
        [Route("getdatas")]
        public TenantAgreementApprovalDTO getdatas([FromBody] TenantAgreementApprovalDTO data)
        {
            return _interface.getdatas(data);
        }

        [HttpPost]
        [Route("savedetails")]
        public TenantAgreementApprovalDTO savedetails([FromBody] TenantAgreementApprovalDTO data)
        {
            return _interface.savedetails(data);
        }

    }
}
