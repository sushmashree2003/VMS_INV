using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FrontOffice.com.vaps.Interfaces;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.FrontOffice;
using PreadmissionDTOs.com.vaps.IssueManager.PettyCash;
using PreadmissionDTOs.com.vaps.VMS.Training;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FrontOffice.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class clientIEmappingFacadeController : Controller
    {
        public clientIEmappingInterface _interface;

        public clientIEmappingFacadeController(clientIEmappingInterface _inter)
        {
            _interface = _inter;
        }
        [Route("onloaddata")]
        public clientIEmappingDTO onloaddata([FromBody] clientIEmappingDTO data)
        {
            return _interface.onloaddata(data);
        }

        [Route("saverecord")]
        public clientIEmappingDTO saverecord([FromBody] clientIEmappingDTO data)
        {
            return _interface.saverecord(data);
        }

        [Route("deactive")]
        public clientIEmappingDTO deactive([FromBody] clientIEmappingDTO data)
        {
            return _interface.deactive(data);
        }

        [Route("validatedata")]
        public clientIEmappingDTO validatedata([FromBody]clientIEmappingDTO data)
        {
            return _interface.validateData(data);
        }
    }
}
