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
    public class RTO_VoilationReportController : Controller
    {
        //FO_Vehicle_Ob_reportDelegate 
        RTO_VoilationReportDelegate del = new RTO_VoilationReportDelegate();
        [Route("loaddata1/{id:int}")]
        //VMS_Vehicle_ReportDTO
        public RTO_VoilationReportDTO loaddata1(int id)
        {
            RTO_VoilationReportDTO data = new RTO_VoilationReportDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.loaddata1(data);
        }
        [HttpPost]
        [Route("savedata")]
        public RTO_VoilationReportDTO savedata([FromBody]RTO_VoilationReportDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));

            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));

            return del.savedata(data);
        }
        [Route("onchange")]
        public RTO_VoilationReportDTO onchange([FromBody]RTO_VoilationReportDTO data)
        {

            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));

            return del.onchange(data);
        }
        //savedataSource
        [Route("savedataSource")]
        public RTO_VoilationReportDTO savedataSource([FromBody]RTO_VoilationReportDTO data)
        {

            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));

            return del.savedataSource(data);
        }
        //clientDecative
        [Route("clientDecative")]
        public RTO_VoilationReportDTO clientDecative([FromBody]RTO_VoilationReportDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));

            return del.clientDecative(data);
        }
        //
        [Route("RecieptReport1")]
        public RTO_VoilationReportDTO RecieptReport1([FromBody]RTO_VoilationReportDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));

            return del.RecieptReport1(data);
        }
    }
}