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
    public class FO_Vehicle_Ob_report : Controller
    {
        //FO_Vehicle_Ob_reportDelegate 
        FO_Vehicle_Ob_reportDelegate del = new FO_Vehicle_Ob_reportDelegate();
        [Route("loaddata1/{id:int}")]
        //VMS_Vehicle_ReportDTO
        public FO_Vehicle_Ob_reportDTO loaddata1(int id)
        {
            FO_Vehicle_Ob_reportDTO data = new FO_Vehicle_Ob_reportDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.loaddata1(data);
        }
        [HttpPost]
        [Route("savedata")]
        public FO_Vehicle_Ob_reportDTO savedata([FromBody]FO_Vehicle_Ob_reportDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));

            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));

            return del.savedata(data);
        }
        [Route("onchange")]
        public FO_Vehicle_Ob_reportDTO onchange([FromBody]FO_Vehicle_Ob_reportDTO data)
        {

            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));

            return del.onchange(data);
        }
        //savedataSource
        [Route("savedataSource")]
        public FO_Vehicle_Ob_reportDTO savedataSource([FromBody]FO_Vehicle_Ob_reportDTO data)
        {

            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));

            return del.savedataSource(data);
        }
        //clientDecative
        [Route("clientDecative")]
        public FO_Vehicle_Ob_reportDTO clientDecative([FromBody]FO_Vehicle_Ob_reportDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));

            return del.clientDecative(data);
        }
        //
        [Route("RecieptReport1")]
        public FO_Vehicle_Ob_reportDTO RecieptReport1([FromBody]FO_Vehicle_Ob_reportDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));

            return del.RecieptReport1(data);
        }
    }
}
