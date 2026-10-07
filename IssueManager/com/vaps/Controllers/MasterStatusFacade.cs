using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IssueManager.com.vaps.Interfaces;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.IssueManager;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IssueManager.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class MasterStatusFacade : Controller
    {

        public MasterStatusInterface _objinter;

        public MasterStatusFacade(MasterStatusInterface parameter)
        {
            _objinter = parameter;
        }

        [Route("getdetails")]
        public MasterStatus_DTO getdetails([FromBody] MasterStatus_DTO dTO)
        {
            return _objinter.getdetails(dTO);
        }

        [Route("saverecord")]
        public MasterStatus_DTO saverecord([FromBody]MasterStatus_DTO value)
        {
            return _objinter.saverecord(value);
        }

        [Route("deactiveY")]
        public MasterStatus_DTO deactiveY([FromBody]MasterStatus_DTO value)
        {
            return _objinter.deactiveY(value);
        }
        [Route("save_ack")]
        public MasterStatus_DTO save_ack([FromBody]MasterStatus_DTO value)
        {
            return _objinter.save_ack(value);
        }




    }
}
