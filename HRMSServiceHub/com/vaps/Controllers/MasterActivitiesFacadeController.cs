using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HRMSServiceHub.com.vaps.Interfaces;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.FrontOffice;
using PreadmissionDTOs.com.vaps.HRMS;
using PreadmissionDTOs.com.vaps.IssueManager.PettyCash;
using PreadmissionDTOs.com.vaps.VMS.Training;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HRMSServiceHub.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class MasterActivitiesFacadeController : Controller
    {
        public MasterActivitiesInterface _interface;

        public MasterActivitiesFacadeController(MasterActivitiesInterface _inter)
        {
            _interface = _inter;
        }
        [Route("onloaddata")]
        public MasterActivitiesDTO onloaddata([FromBody] MasterActivitiesDTO data)
        {
            return _interface.onloaddata(data);
        }

        [Route("saverecord")]
        public MasterActivitiesDTO saverecord([FromBody] MasterActivitiesDTO data)
        {
            return _interface.saverecord(data);
        }

        [Route("deactiveY")]
        public MasterActivitiesDTO deactiveY([FromBody] MasterActivitiesDTO data)
        {
            return _interface.deactiveY(data);
        }
    }
}
