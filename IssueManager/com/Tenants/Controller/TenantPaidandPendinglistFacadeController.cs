using IssueManager.com.Tenants.Interface;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.IssueManager.Tenants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IssueManager.com.Tenants.Controller
{
    [Route("api/[controller]")]
    public class TenantPaidandPendinglistFacadeController : ControllerBase
    {
        public TenantPaidandPendinglistInterface _interface;

        public TenantPaidandPendinglistFacadeController(TenantPaidandPendinglistInterface _interf)
        {
            _interface = _interf;
        }
        
        [Route("onloadgetdetails")]
        public TenantPaidandPendinglistDTO onloadgetdetails([FromBody] TenantPaidandPendinglistDTO data)
        {
            return _interface.getBasicData(data);
        }

        [Route("get_report")]
        public TenantPaidandPendinglistDTO get_report([FromBody] TenantPaidandPendinglistDTO data)
        {
            return _interface.get_report(data);
        }

        [Route("sendmail")]
        public TenantPaidandPendinglistDTO sendmail([FromBody] TenantPaidandPendinglistDTO data)
        {
            return _interface.sendmail(data);
        }

    }
}
