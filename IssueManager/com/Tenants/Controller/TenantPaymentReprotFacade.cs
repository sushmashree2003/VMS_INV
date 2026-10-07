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
    public class TenantPaymentReprotFacade :ControllerBase
    {
        public TenantPaymentReprotInterface _objinter;

        public TenantPaymentReprotFacade(TenantPaymentReprotInterface parameter)
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

        [Route("institutionchange")]
        public TenantAgreementDTO institutionchange([FromBody] TenantAgreementDTO data)
        {
            return _objinter.institutionchange(data);
        }
        [Route("buildingchange")]
        public TenantAgreementDTO buildingchange([FromBody] TenantAgreementDTO data)
        {
            return _objinter.buildingchange(data);
        }

        [Route("Floorchange")]
        public TenantAgreementDTO Floorchange([FromBody] TenantAgreementDTO data)
        {
            return _objinter.Floorchange(data);
        }
        [Route("TenantDetails")]
        public TenantAgreementDTO TenantDetails([FromBody] TenantAgreementDTO data)
        {
            return _objinter.TenantDetails(data);
        }
        [Route("TenantConsolidatedReport")]
        public TenantAgreementDTO TenantConsolidatedReport([FromBody] TenantAgreementDTO data)
        {
            return _objinter.TenantConsolidatedReport(data);
        }
        [Route("TenantMonthlyPaymentDetails")]
        public TenantAgreementDTO TenantMonthlyPaymentDetails([FromBody] TenantAgreementDTO data)
        {
            return _objinter.TenantMonthlyPaymentDetails(data);
        }

    }
}
