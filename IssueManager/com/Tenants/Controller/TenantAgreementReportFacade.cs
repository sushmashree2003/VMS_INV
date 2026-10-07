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
    public class TenantAgreementReportFacade :ControllerBase
    {
        public TenantAgreementReportInterface _objinter;

        public TenantAgreementReportFacade(TenantAgreementReportInterface parameter)
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
       
        [Route("Floorchange")]
        public TenantAgreementDTO Floorchange([FromBody] TenantAgreementDTO data)
        {
            return _objinter.Floorchange(data);
        }
        [Route("getallofflinedetails")]
        public TenantAgreementDTO getallofflinedetails([FromBody] TenantAgreementDTO data)
        {
            return _objinter.getallofflinedetails(data);
        }

        [Route("SaveElectricalbill")]
        public TenantAgreementDTO SaveElectricalbill([FromBody] TenantAgreementDTO data)
        {
            return _objinter.SaveElectricalbill(data);
        }

        [Route("editdetails")]
        public TenantAgreementDTO editdetails([FromBody] TenantAgreementDTO data)
        {
            return _objinter.editdetails(data);
        }

        [Route("deactive")]
        public TenantAgreementDTO deactive([FromBody] TenantAgreementDTO data)
        {
            return _objinter.deactive(data);
        }

        [Route("sendmail")]
        public TenantAgreementDTO sendmail([FromBody] TenantAgreementDTO data)
        {
            return _objinter.sendmail(data);
        }
        [Route("Saveallbilltype")]
        public TenantAgreementDTO Saveallbilltype([FromBody] TenantAgreementDTO data)
        {
            return _objinter.Saveallbilltype(data);
        }
        // Fine Caluclation
        [Route("loadtenants")]
        public TenantAgreementDTO loadtenants([FromBody] TenantAgreementDTO data)
        {
            return _objinter.loadtenants(data);
        }
        [Route("getTenantAmountDetails")]
        public TenantAgreementDTO getTenantAmountDetails([FromBody] TenantAgreementDTO data)
        {
            return _objinter.getTenantAmountDetails(data);
        }
        [Route("Savefine")]
        public TenantAgreementDTO Savefine([FromBody] TenantAgreementDTO data)
        {
            return _objinter.Savefine(data);
        }
    }
}
