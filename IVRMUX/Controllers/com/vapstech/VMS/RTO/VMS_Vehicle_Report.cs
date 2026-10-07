using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.VMS.RTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.VMS.RTO;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.VMS.RTO
{
    [Route("api/[controller]")]
    public class VMS_Vehicle_Report : Controller
    {
        VMS_Vehicle_ReportDelegate del = new VMS_Vehicle_ReportDelegate();
        [Route("loaddata/{id:int}")]
        //VMS_Vehicle_ReportDTO
        public VMS_Vehicle_ReportDTO loaddata(int id)
        {
            VMS_Vehicle_ReportDTO data = new VMS_Vehicle_ReportDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.loaddata(data);
        }
        [HttpPost]
        [Route("savedata")]
        public VMS_Vehicle_ReportDTO savedata([FromBody]VMS_Vehicle_ReportDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));

            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));

            return del.savedata(data);
        }
        [Route("onchange")]
        public VMS_Vehicle_ReportDTO onchange([FromBody]VMS_Vehicle_ReportDTO data)
        {

            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));

            return del.onchange(data);
        }
        //savedataSource
        [Route("savedataSource")]
        public VMS_Vehicle_ReportDTO savedataSource([FromBody]VMS_Vehicle_ReportDTO data)
        {

            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));

            return del.savedataSource(data);
        }
        //clientDecative
        [Route("clientDecative")]
        public VMS_Vehicle_ReportDTO clientDecative([FromBody]VMS_Vehicle_ReportDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));

            return del.clientDecative(data);
        }
        //
        [Route("RecieptReport")]
        public VMS_Vehicle_ReportDTO RecieptReport([FromBody]VMS_Vehicle_ReportDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));

            return del.RecieptReport(data);
        }
    }
}
