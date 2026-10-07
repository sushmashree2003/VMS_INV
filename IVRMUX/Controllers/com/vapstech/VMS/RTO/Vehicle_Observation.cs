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
    public class Vehicle_Observation : Controller
    {
        Vehicle_ObservationDelegate del = new Vehicle_ObservationDelegate();
        [Route("loaddata/{id:int}")]
        //VMS_FO_Vehicle_ObservationDTO
        public VMS_FO_Vehicle_ObservationDTO loaddata(int id)
        {
            VMS_FO_Vehicle_ObservationDTO data = new VMS_FO_Vehicle_ObservationDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.loaddata(data);
        }
        [HttpPost]
        [Route("savedata")]
        public VMS_FO_Vehicle_ObservationDTO savedata([FromBody]VMS_FO_Vehicle_ObservationDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));

            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));

            return del.savedata(data);
        }
        [Route("onchange")]
        public VMS_FO_Vehicle_ObservationDTO onchange([FromBody]VMS_FO_Vehicle_ObservationDTO data)
        {

            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));

            return del.onchange(data);
        }
        //savedataSource
        [Route("savedataSource")]
        public VMS_FO_Vehicle_ObservationDTO savedataSource([FromBody]VMS_FO_Vehicle_ObservationDTO data)
        {

            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));

            return del.savedataSource(data);
        }
        //clientDecative
        [Route("clientDecative")]
        public VMS_FO_Vehicle_ObservationDTO clientDecative([FromBody]VMS_FO_Vehicle_ObservationDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));

            return del.clientDecative(data);
        }
        //
        [Route("RecieptReport")]
        public VMS_FO_Vehicle_ObservationDTO RecieptReport([FromBody]VMS_FO_Vehicle_ObservationDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));

            return del.RecieptReport(data);
        }
    }
}
