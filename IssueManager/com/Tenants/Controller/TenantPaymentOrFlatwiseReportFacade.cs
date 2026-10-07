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
    public class TenantPaymentOrFlatwiseReportFacade : ControllerBase
    {
        public TenantPaymentOrFlatwiseReportInterface _objinter;

        public TenantPaymentOrFlatwiseReportFacade(TenantPaymentOrFlatwiseReportInterface parameter)
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

        [Route("buildingchange")]
        public TenantAgreementDTO buildingchange([FromBody] TenantAgreementDTO data)
        {
            return _objinter.buildingchange(data);
        }

        [Route("floorchange")]
        public TenantAgreementDTO floorchange([FromBody] TenantAgreementDTO data)
        {
            return _objinter.floorchange(data);
        }

        [Route("companychange")]
        public TenantAgreementDTO companychange([FromBody] TenantAgreementDTO data)
        {
            return _objinter.companychange(data);
        }
    }
}
