using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.Inventory.Master;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Inventory;

namespace IVRMUX.Controllers.com.vapstech.Inventory.Master
{
    [Route("api/[controller]")]
    public class INV_LocationReportController : Controller
    {
        INV_LocationReportDelegate _delegate = new INV_LocationReportDelegate();
        //[HttpGet]
        //[Route("getloaddata/{id:int}")]
        //public INV_LocationReportDTO getloaddata(int id)
        //{
        //    INV_LocationReportDTO data = new INV_LocationReportDTO();
        //    data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
        //    data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
        //    return _delegate.getloaddata(data);
        //}
       
        [Route("getloaddata")]
        public INV_LocationReportDTO getloaddata([FromBody] INV_LocationReportDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delegate.getloaddata(data);
        }
        [Route("onreport")]
        public INV_LocationReportDTO onreport([FromBody] INV_LocationReportDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delegate.onreport(data);
        }
        //onchangeloaddata
        [Route("onchangeloaddata")]
        public INV_LocationReportDTO onchangeloaddata([FromBody] INV_LocationReportDTO data)
        {
            
            return _delegate.onchangeloaddata(data);
        }
    }
}