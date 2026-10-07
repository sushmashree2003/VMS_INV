using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FMS.Com.Interface;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.FMS;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FMS.Com.Facade
{
    [Route("api/[controller]")]
    public class FMSApprovalFacade : Controller
    {
        public FMSApprovalInterface _interface;

        public FMSApprovalFacade(FMSApprovalInterface _inter)
        {
            _interface = _inter;
        }
        
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [Route("loaddata")]
        public FMSApprovalDTO loaddata([FromBody] FMSApprovalDTO data)
        {
            return _interface.loaddata(data);
        }

        //[HttpGet]
        [Route("getdatas")]
        public FMSApprovalDTO getdatas([FromBody] FMSApprovalDTO data)
        {
            return _interface.getdatas(data);
        }

        [HttpPost]
        [Route("savedetails")]
        public FMSApprovalDTO savedetails([FromBody] FMSApprovalDTO data)
        {
            return _interface.savedetails(data);
        }

    }
}
