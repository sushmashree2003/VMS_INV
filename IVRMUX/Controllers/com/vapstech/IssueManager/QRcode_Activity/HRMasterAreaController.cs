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
    public class HRMasterAreaController : Controller
    {

        HRMasterAreaDelegate _delegate = new HRMasterAreaDelegate();

        [HttpGet]

        [Route("getloaddata/{id:int}")]
        public HRMasterAreaDTO getloaddata(int id)
        {
            HRMasterAreaDTO data = new HRMasterAreaDTO();
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.getloaddata(data);
        }

        //post
        [HttpPost]
        [Route("getfloors")]
        public HRMasterAreaDTO getfloors([FromBody] HRMasterAreaDTO data)
        {
            
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.getfloors(data);
        }

        //post
        [HttpPost]
        [Route("getAreaName")]
        public HRMasterAreaDTO getAreaName([FromBody] HRMasterAreaDTO data)
        {

            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.getAreaName(data);
        }


        //post
        [HttpPost]
        [Route("getActivityName")]
        public HRMasterAreaDTO getActivityName([FromBody] HRMasterAreaDTO data)
        {

            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.getActivityName(data);
        }




        [HttpPost]
        [Route("savedetailsAREA")]
        public HRMasterAreaDTO savedetailsAREA([FromBody] HRMasterAreaDTO data)
        {


            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.savedetailsAREA(data);
        }


        [HttpPost]
        [Route("savedetailsAREAActivity")]
        public HRMasterAreaDTO savedetailsAREAActivity([FromBody] HRMasterAreaDTO data)
        {

            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.savedetailsAREAActivity(data);
        }


        [HttpPost]
        [Route("savedetailsAREAActivityTime")]
        public HRMasterAreaDTO savedetailsAREAActivityTime([FromBody] HRMasterAreaDTO data)
        {

            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.savedetailsAREAActivityTime(data);
        }

        [Route("deactiveArea")]
        public HRMasterAreaDTO deactiveArea([FromBody] HRMasterAreaDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.deactiveArea(data);
        }

        [Route("deactiveAreaActivity")]
        public HRMasterAreaDTO deactiveAreaActivity([FromBody] HRMasterAreaDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.deactiveAreaActivity(data);
        }

        [Route("deactiveAreaActivityTime")]
        public HRMasterAreaDTO deactiveAreaActivityTime([FromBody] HRMasterAreaDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.deactiveAreaActivityTime(data);
        }
        [Route("FloorQRCODE")]
        public HRMasterAreaDTO FloorQRCODE([FromBody]HRMasterAreaDTO dto)
        {
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
        
            return _delegate.FloorQRCODE(dto);
        }
        [HttpPost]
        [Route("edit")]
        public HRMasterAreaDTO edit([FromBody]HRMasterAreaDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
         
            return _delegate.edit(data);
        }

    }
}