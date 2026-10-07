using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FrontOfficeHub.com.vaps.Interfaces;
using PreadmissionDTOs.com.vaps.FrontOffice;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace FrontOfficeHub.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class SecurityDetailsFacade : Controller
    {
        public SecurityDetailsInterface _org;
        public SecurityDetailsFacade(SecurityDetailsInterface maspag)
        {
            _org = maspag;
        }

        [Route("getalldetails")]
        public SecurityDetailsDTO getalldetails([FromBody] SecurityDetailsDTO data)
        {
            return _org.getalldetails(data);
        }
        [Route("getdata")]
        public SecurityDetailsDTO getdata([FromBody] SecurityDetailsDTO data)
        {
            return _org.getdata(data);
        }
        [Route("edittab1")]
        public SecurityDetailsDTO edittab1([FromBody]  SecurityDetailsDTO data)
        {
            return _org.edittab1(data);
        }
        [Route("deactive")]
        public SecurityDetailsDTO deactive([FromBody] SecurityDetailsDTO data)
        {
            return _org.deactive(data);
        }
        [Route("Get_StateData")]
        public SecurityDetailsDTO Get_StateData([FromBody] SecurityDetailsDTO data)
        {
            return _org.Get_StateData(data);
        }
    }
}
