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
    public class TenantWaterBillFacade : ControllerBase
    {
        public TenantWaterBillInterface _flr;

        public TenantWaterBillFacade(TenantWaterBillInterface ct)
        {
            _flr = ct;
        }
        //----------------------------Building-----------------------------
        [Route("loaddata")]
        public TenantWaterBillDTO loaddata([FromBody] TenantWaterBillDTO id)
        {
            return _flr.loaddata(id);
        }
       

        [HttpPost]
        [Route("savetrainerfeedback")]
        public TenantWaterBillDTO savetrainerfeedback([FromBody] TenantWaterBillDTO dTO)
        {
            return _flr.savetrainerfeedback(dTO);
        }

        [Route("deactive")]
        public TenantWaterBillDTO deactive([FromBody]TenantWaterBillDTO data)
        {
            return _flr.deactive(data);
        }
        [Route("viewroomwisewaterbill")]
        public TenantWaterBillDTO viewroomwisewaterbill([FromBody]TenantWaterBillDTO data)
        {
            return _flr.viewroomwisewaterbill(data);
        }

    }
}
