using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.FrontOffice;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers.com.vapstech.FrontOffice
{
    [Route("api/[controller]")]
    public class SecurityDetailsController : Controller
    {
        SecurityDetailsDelegate del = new SecurityDetailsDelegate();

        
        [Route("getalldetails")]
        public SecurityDetailsDTO getalldetails([FromBody] SecurityDetailsDTO data)
        {           
            return del.getalldetails(data);
        }
        [Route("getdata")]
        public SecurityDetailsDTO getdata([FromBody]SecurityDetailsDTO data)
        {       
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.getdata(data);
        }
        [Route("edittab1")]
        public SecurityDetailsDTO edittab1([FromBody] SecurityDetailsDTO data)
        {
           
            return del.edittab1(data);
        }
        [Route("deactive")]
        public SecurityDetailsDTO deactive([FromBody]SecurityDetailsDTO data)
        {
           
            return del.deactive(data);
        }
        [Route("Get_StateData")]
        public SecurityDetailsDTO Get_StateData([FromBody]SecurityDetailsDTO data)
        {
            
            return del.Get_StateData(data);
        }
    }
}
