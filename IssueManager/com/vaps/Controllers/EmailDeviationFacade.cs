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
    public class EmailDeviationFacade : Controller
    {


        public EmailDeviationInterface _objinter;

        public EmailDeviationFacade(EmailDeviationInterface parameter)
        {
            _objinter = parameter;
        }

        [Route("sendeach")]
        public EmailDeviationDTO sendeach([FromBody] EmailDeviationDTO data)
        {
            return _objinter.sendeach(data);
        }

        [Route("updatepriority")]
        public EmailDeviationDTO updatepriority([FromBody] EmailDeviationDTO data)
        {
            return _objinter.updatepriority(data);
        }
        [Route("blockdailyreport")]
        public EmailDeviationDTO blockdailyreport([FromBody] EmailDeviationDTO data)
        {
            return _objinter.blockdailyreport(data);
        }

        [Route("makercheckerverification")]
        public EmailDeviationDTO makercheckerverification([FromBody] EmailDeviationDTO data)
        {
            return _objinter.makercheckerverification(data);
        }
        [Route("Sendnotassignedtask")]
        public EmailDeviationDTO Sendnotassignedtask([FromBody] EmailDeviationDTO data)
        {
            return _objinter.Sendnotassignedtask(data);
        }
        [Route("Sendnotassignedtask_Critical")]
        public EmailDeviationDTO Sendnotassignedtask_Critical([FromBody] EmailDeviationDTO data)
        {
            return _objinter.Sendnotassignedtask_Critical(data);
        }
        [Route("senddepartmentapproval")]
        public EmailDeviationDTO senddepartmentapproval([FromBody] EmailDeviationDTO data)
        {
            return _objinter.senddepartmentapproval(data);
        }
    }
}
