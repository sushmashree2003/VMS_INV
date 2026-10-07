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
    public class HW_HeathCheckUpController : Controller
    {
        HW_HeathCheckUpDelegate _delg = new HW_HeathCheckUpDelegate();

        // GET: api/<controller>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [Route("OnLoadHWHealthCheckUp")]
        public HW_HeathCheckUpDTO OnLoadHWHealthCheckUp([FromBody] HW_HeathCheckUpDTO data)
        {
            if (data.OnLoadOrOnChange.Equals("OnLoad"))
            {
                data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            }
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));

            return _delg.OnLoadHWHealthCheckUp(data);
        }

        [Route("SaveHWHealthChekUp")]
        public HW_HeathCheckUpDTO SaveHWHealthChekUp([FromBody] HW_HeathCheckUpDTO data)
        {
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delg.SaveHWHealthChekUp(data);
        }

        [Route("welclosesavedata")]
        public HW_HeathCheckUpDTO welclosesavedata([FromBody] HW_HeathCheckUpDTO data)
        {
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delg.welclosesavedata(data);
        }

        //added by praveenkuamr Proposal Signoff
        [Route("ProposalSignOff")]
        public HW_HeathCheckUpDTO ProposalSignOff([FromBody] HW_HeathCheckUpDTO data)
        {
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delg.ProposalSignOff(data);
        }

        [Route("EditHealthCheckUp")]
        public HW_HeathCheckUpDTO EditHealthCheckUp([FromBody] HW_HeathCheckUpDTO data)
        { 
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delg.EditHealthCheckUp(data);
        }

        [Route("ViewHealthCheckUpFiles")]
        public HW_HeathCheckUpDTO ViewHealthCheckUpFiles([FromBody] HW_HeathCheckUpDTO data)
        { 
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delg.ViewHealthCheckUpFiles(data);
        }

        [Route("ActiveDeactiveHealthCheckUp")]
        public HW_HeathCheckUpDTO ActiveDeactiveHealthCheckUp([FromBody] HW_HeathCheckUpDTO data)
        { 
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delg.ActiveDeactiveHealthCheckUp(data);
        }

        [Route("DeleteHWHealthCheckUpFiles")]
        public HW_HeathCheckUpDTO DeleteHWHealthCheckUpFiles([FromBody] HW_HeathCheckUpDTO data)
        { 
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delg.DeleteHWHealthCheckUpFiles(data);
        }

        //Approval Process

        [Route("LoadHWHealthCheckUpdataApproval")]
        public HW_HeathCheckUpDTO LoadHWHealthCheckUpdataApproval([FromBody] HW_HeathCheckUpDTO data)
        {
            if (data.OnLoadOrOnChange.Equals("OnLoad"))
            {
                data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            }
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delg.LoadHWHealthCheckUpdataApproval(data);
        }

        [Route("SaveHWHealthChekUpApproval")]
        public HW_HeathCheckUpDTO SaveHWHealthChekUpApproval([FromBody] HW_HeathCheckUpDTO data)
        {             
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delg.SaveHWHealthChekUpApproval(data);
        }

        [Route("ViewHealthCheckUpComments")]
        public HW_HeathCheckUpDTO ViewHealthCheckUpComments([FromBody] HW_HeathCheckUpDTO data)
        {             
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delg.ViewHealthCheckUpComments(data);
        }

        //health checkup report
       // [Route("HealthCheckUpInstitution")]

        [Route("HealthCheckUpInstitution")]
        public HW_HeathCheckUpDTO HealthCheckUpInstitution([FromBody] HW_HeathCheckUpDTO data)
        {
            if (data.OnLoadOrOnChange.Equals("OnLoad"))
            {
                data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            }
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));

            return _delg.HealthCheckUpInstitution(data);
        }

       // [Route("getdetails/{id:int}")]
       //// [Route("getdetails")]
       // public HW_HeathCheckUpDTO getdetails( HW_HeathCheckUpDTO data)
       // {
       //     if (data.OnLoadOrOnChange.Equals("OnLoad"))
       //     {
       //         data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
       //     }
       //     data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
       //     return _delg.getdetails(data);
       // }


        [Route("getdetails")]
        public HW_HeathCheckUpDTO getdetails([FromBody] HW_HeathCheckUpDTO data)
        {
           
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delg.getdetails(data);
        }

        [Route("esculationmail")]
        public HW_HeathCheckUpDTO esculationmail([FromBody] HW_HeathCheckUpDTO data)
        {
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delg.esculationmail(data);
        }
    }
}
