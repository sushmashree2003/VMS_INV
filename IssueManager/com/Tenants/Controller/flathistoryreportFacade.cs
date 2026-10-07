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
    public class flathistoryreportFacade : ControllerBase
    {
        public flathistoryreportInterface _interface;

        public flathistoryreportFacade(flathistoryreportInterface _interf)
        {
            _interface = _interf;
        }

        // GET: api/<controller>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        //Master Type
        [Route("OnLoadRatingMasterType")]
        public TenantAgreementDTO OnLoadRatingMasterType([FromBody] TenantAgreementDTO data)
        {
            return _interface.OnLoadRatingMasterType(data);
        }
//added by anvesha
        [Route("get_report")]
        public TenantAgreementDTO get_report([FromBody]TenantAgreementDTO data)
        {
            return _interface.get_report(data);
        }
        [Route("buildingchange")]
        public TenantAgreementDTO buildingchange([FromBody] TenantAgreementDTO data)
        {
            return _interface.buildingchange(data);
        }
        [Route("roomchange")]
        public TenantAgreementDTO roomchange([FromBody] TenantAgreementDTO data)
        {
            return _interface.roomchange(data);
        }

        //added by anvesha
        [Route("ViewHealthCheckUpFiles")]
        public TenantAgreementDTO ViewHealthCheckUpFiles([FromBody] TenantAgreementDTO data)
        {
            return _interface.ViewHealthCheckUpFiles(data);
        }

    }
}
