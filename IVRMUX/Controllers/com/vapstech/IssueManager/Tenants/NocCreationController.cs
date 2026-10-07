using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.IssueManager.Tenants;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.IssueManager.Tenants;

namespace IVRMUX.Controllers.com.vapstech.IssueManager.Tenants
{
    [Route("api/[controller]")]
    public class NocCreationController : Controller
    {

        NocCreationDelegate _delegate = new NocCreationDelegate();

        [HttpGet]

        [Route("getloaddata/{id:int}")]
        public NocCreationDTO getloaddata(int id)
        {
            NocCreationDTO data = new NocCreationDTO();
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.getloaddata(data);
        }


        //post
        [HttpPost]
        [Route("getfloors")]
        public NocCreationDTO getfloors([FromBody] NocCreationDTO data)
        {

            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.getfloors(data);
        }

        //post
        [HttpPost]
        [Route("getAreaName")]
        public NocCreationDTO getAreaName([FromBody] NocCreationDTO data)
        {

            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.getAreaName(data);
        }


        //post
        [HttpPost]
        [Route("getActivityName")]
        public NocCreationDTO getActivityName([FromBody] NocCreationDTO data)
        {

            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.getActivityName(data);
        }
        [HttpPost]
        [Route("gettenantname")]
        public NocCreationDTO gettenantname([FromBody] NocCreationDTO data)
        {

            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.gettenantname(data);
        }
        
       //post
       [HttpPost]
        [Route("getUsername")]
        public NocCreationDTO getUsername([FromBody] NocCreationDTO data)
        {
            return _delegate.getUsername(data);
        }

        [HttpPost]
        [Route("savedetailsStaffMapping")]
        public NocCreationDTO savedetailsStaffMapping([FromBody] NocCreationDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.User_Id = Convert.ToInt64(HttpContext.Session.GetInt32("userId"));
            return _delegate.savedetailsStaffMapping(data);
        }

        [HttpPost]
        [Route("deactive")]
        public NocCreationDTO deactive([FromBody] NocCreationDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.deactive(data);
        }


        //[HttpPost]
        //[Route("edit")]
        //public NocCreationDTO edit([FromBody] NocCreationDTO data)
        //{
        //    data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
        //    data.User_Id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
        //    return _delegate.edit(data);
        //}

    }
}