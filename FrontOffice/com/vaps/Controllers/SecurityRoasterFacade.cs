using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FrontOfficeHub.com.vaps.Interfaces;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.FrontOffice;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FrontOfficeHub.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class SecurityRoasterFacade : Controller
    {
        public SecurityRoasterInterface _org;

        public SecurityRoasterFacade(SecurityRoasterInterface orga)
        {
            _org = orga;
        }


        [Route("getalldetails")]
        public SecurityRoasterDTO getalldetails([FromBody] SecurityRoasterDTO data)
        {
            return _org.getalldetails(data);
        }
        [Route("getdata")]
        public SecurityRoasterDTO getdata([FromBody] SecurityRoasterDTO data)
        {
            return _org.getdata(data);
        }
        [Route("edittab1")]
        public SecurityRoasterDTO edittab1([FromBody]  SecurityRoasterDTO data)
        {
            return _org.edittab1(data);
        }
        [Route("deactive")]
        public SecurityRoasterDTO deactive([FromBody] SecurityRoasterDTO data)
        {
            return _org.deactive(data);
        }
        [Route("report")]
        public SecurityRoasterDTO report([FromBody] SecurityRoasterDTO data)
        {
            return _org.report(data);
        }

        [Route("Binddata")]
        public SecurityRoasterDTO Binddata([FromBody] SecurityRoasterDTO data)
        {
            return _org.Binddata(data);
        }
        
    }
}
