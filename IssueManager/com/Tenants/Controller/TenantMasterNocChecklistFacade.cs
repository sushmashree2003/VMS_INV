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
    public class TenantMasterNocChecklistFacade : ControllerBase
    {
        public TenantMasterNocChecklistInterface _interface;

        public TenantMasterNocChecklistFacade(TenantMasterNocChecklistInterface _interf)
        {
            _interface = _interf;
        }

        [Route("getloaddata")]
        public TenantMasterNocChecklistDTO getloaddata([FromBody] TenantMasterNocChecklistDTO data)
        {
            return _interface.getloaddata(data);
        }

        [Route("getamenity")]
        public TenantMasterNocChecklistDTO getamenity([FromBody] TenantMasterNocChecklistDTO data)
        {
            return _interface.getamenity(data);
        }

        [Route("savedetails")]
        public TenantMasterNocChecklistDTO savedetails([FromBody] TenantMasterNocChecklistDTO data)
        {
            return _interface.savedetails(data);
        }
        [Route("activedeactive")]
        public TenantMasterNocChecklistDTO activedeactive([FromBody] TenantMasterNocChecklistDTO data)
        {
            return _interface.activedeactive(data);
        }


    }
}
