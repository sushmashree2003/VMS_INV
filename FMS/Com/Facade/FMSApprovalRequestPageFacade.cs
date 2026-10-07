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
    public class FMSApprovalRequestPageFacade : Controller
    {
        public FMSApprovalRequestPageInterface _interface;

        public FMSApprovalRequestPageFacade(FMSApprovalRequestPageInterface _inter)
        {
            _interface = _inter;
        }
        // GET: api/<controller>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [Route("loaddata")]
        public FMSApprovalRequestPageDTO loaddata([FromBody] FMSApprovalRequestPageDTO data)
        {
            return _interface.loaddata(data);
        }

        [HttpPost]
        [Route("savedetails")]
        public FMSApprovalRequestPageDTO savedetails([FromBody] FMSApprovalRequestPageDTO data)
        {
            return _interface.savedetails(data);
        }
        [Route("editdetails")]
        public FMSApprovalRequestPageDTO editdetails([FromBody] FMSApprovalRequestPageDTO data)
        {
            return _interface.editdetails(data);
        }
        [Route("deactive")]
        public FMSApprovalRequestPageDTO deactive([FromBody] FMSApprovalRequestPageDTO data)
        {
            return _interface.deactive(data);
        }

        [Route("viewuploadflies")]
        public FMSApprovalRequestPageDTO viewuploadflies([FromBody] FMSApprovalRequestPageDTO data)
        {
            return _interface.viewuploadflies(data);
        }

        [HttpPost]
        [Route("Updatedetails")]
        public FMSApprovalRequestPageDTO Updatedetails([FromBody] FMSApprovalRequestPageDTO data)
        {
            return _interface.Updatedetails(data);
        }

    }
}
