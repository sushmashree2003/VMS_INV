using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.IssueManager;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.IssueManager;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.IssueManager
{
    [Route("api/[controller]")]
    public class UserMappingController : Controller
    {

        public UserMappingDelegate _objdel = new UserMappingDelegate();

        [Route("getdetails/{id:int}")]
        public Clients_DTO getdetails(int id)
        {
            Clients_DTO data = new Clients_DTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));

            return _objdel.getdetails(data);
        }

        [Route("saveClientMappingdata")]
        public Clients_DTO saveClientMappingdata([FromBody]Clients_DTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));

            return _objdel.saveClientMappingdata(data);
        }

        [Route("deactiveClientMappingdata")]
        public Clients_DTO deactiveClientMappingdata([FromBody]Clients_DTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));

            return _objdel.deactiveClientMappingdata(data);
        }

        [Route("editClientMappingdata")]
        public Clients_DTO editClientMappingdata([FromBody]Clients_DTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _objdel.editClientMappingdata(data);
        }

        [Route("get_MappedUserlist")]
        public Clients_DTO get_MappedUserlist([FromBody]Clients_DTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _objdel.get_MappedUserlist(data);
        }
        
    }
}
