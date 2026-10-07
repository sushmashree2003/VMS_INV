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
    public class ISM_UserEmployeeMappingController : Controller
    {

        public ISM_UserEmployeeMappingDelegate _objdel = new ISM_UserEmployeeMappingDelegate();

        [Route("getdetails/{id:int}")]
        public Clients_DTO getdetails(int id)
        {
            Clients_DTO data = new Clients_DTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));

            return _objdel.getdetails(data);
        }

        [Route("getemployeelist")]
        public Clients_DTO getemployeelist([FromBody]Clients_DTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _objdel.getemployeelist(data);
        }
         

         [Route("savedata")]
        public Clients_DTO savedata([FromBody]Clients_DTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _objdel.savedata(data);
        }
          [Route("deactive")]
        public Clients_DTO deactive([FromBody]Clients_DTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _objdel.deactive(data);
        }

        [Route("updateorder")]
        public Clients_DTO updateorder([FromBody]Clients_DTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _objdel.updateorder(data);
        }

        [Route("getdetailsNew")]
        public Clients_DTO getdetailsNew([FromBody]Clients_DTO data)
        {
            
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));

            return _objdel.getdetailsNew(data);
        }
     

        [Route("getUserEmpMappedReport")]
        public Clients_DTO getUserEmpMappedReport([FromBody]Clients_DTO data)
        {

            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));

            return _objdel.getUserEmpMappedReport(data);
        }
    }
}
