using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.IssueManager.QRcode_Activity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.IssueManager.QRcode_Activity;

namespace IVRMUX.Controllers.com.vapstech.IssueManager.QRcode_Activity
{
    [Route("api/[controller]")]
    public class HRMasterAreaStaffMappingController : Controller
    {

        HRMasterAreaStaffMappingDelegate _delegate = new HRMasterAreaStaffMappingDelegate();

        [HttpGet]

        [Route("getloaddata/{id:int}")]
        public HRMasterAreaStaffMappingDTO getloaddata(int id)
        {
            HRMasterAreaStaffMappingDTO data = new HRMasterAreaStaffMappingDTO();
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.getloaddata(data);
        }


        //post
        [HttpPost]
        [Route("getfloors")]
        public HRMasterAreaStaffMappingDTO getfloors([FromBody] HRMasterAreaStaffMappingDTO data)
        {

            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.getfloors(data);
        }

        //post
        [HttpPost]
        [Route("getAreaName")]
        public HRMasterAreaStaffMappingDTO getAreaName([FromBody] HRMasterAreaStaffMappingDTO data)
        {

            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.getAreaName(data);
        }


        //post
        [HttpPost]
        [Route("getActivityName")]
        public HRMasterAreaStaffMappingDTO getActivityName([FromBody] HRMasterAreaStaffMappingDTO data)
        {

            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.getActivityName(data);
        }
        //post
        [HttpPost]
        [Route("getEmployeeName")]
        public HRMasterAreaStaffMappingDTO getEmployeeName([FromBody] HRMasterAreaStaffMappingDTO data)
        {

            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.getEmployeeName(data);
        }


        //post
        [HttpPost]
        [Route("getUsername")]
        public HRMasterAreaStaffMappingDTO getUsername([FromBody] HRMasterAreaStaffMappingDTO data)
        {  return _delegate.getUsername(data);
        }



        [HttpPost]
        [Route("savedetailsStaffMapping")]
        public HRMasterAreaStaffMappingDTO savedetailsStaffMapping([FromBody] HRMasterAreaStaffMappingDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.savedetailsStaffMapping(data);
        }


        [HttpPost]
        [Route("deactive")]
        public HRMasterAreaStaffMappingDTO deactive([FromBody] HRMasterAreaStaffMappingDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.deactive(data);
        }


        [HttpPost]
        [Route("edit")]
        public HRMasterAreaStaffMappingDTO edit([FromBody] HRMasterAreaStaffMappingDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.User_Id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delegate.edit(data);
        }

    }
}