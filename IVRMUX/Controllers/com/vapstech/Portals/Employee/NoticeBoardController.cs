using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Portals.Employee;
using Microsoft.AspNetCore.Http;
using corewebapi18072016.Delegates.com.vapstech.Portals.Employee;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers.com.vapstech.Portals.Employee
{
    [Route("api/[controller]")]
    public class NoticeBoardController : Controller 
    {
        // GET: api/values
        NoticeBoardDelegats _notic = new NoticeBoardDelegats();
        // GET: api/values
        [Route("savedetail")]
        public IVRM_NoticeBoardDTO savedetail([FromBody]IVRM_NoticeBoardDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));         
           
            return _notic.savedetail(data);
        }

        [Route("Getdetails/{id:int}")]
        public IVRM_NoticeBoardDTO Getdetails(int id)
        {
            IVRM_NoticeBoardDTO obj = new IVRM_NoticeBoardDTO();    
            obj.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));         
            return _notic.Getdetails(obj);
        }
        [Route("deactivate")]
        public IVRM_NoticeBoardDTO deactivate([FromBody]IVRM_NoticeBoardDTO data)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;
            return _notic.deactivate(data);
        }
        [Route("editdetails")]
        public IVRM_NoticeBoardDTO editdetails([FromBody]IVRM_NoticeBoardDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
         
            return _notic.editdetails(data);
        }
        



    }
}
