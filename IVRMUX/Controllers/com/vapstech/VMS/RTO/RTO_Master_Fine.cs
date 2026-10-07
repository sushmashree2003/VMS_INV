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
    public class RTO_Master_Fine : Controller
    {
        RTO_Master_FineDelegate del = new RTO_Master_FineDelegate();
        [Route("loaddata/{id:int}")]
        //RTO_Master_FineDTO
        public RTO_Master_FineDTO loaddata(int id)
        {
            RTO_Master_FineDTO data = new RTO_Master_FineDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.loaddata(data);
        }
        [HttpPost]
        [Route("savedata")]
        public VMS_Vehicle_KeyDTO savedata([FromBody]VMS_Vehicle_KeyDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));

            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
          
            return del.savedata(data);
        }
        [Route("onchange")]
        public VMS_Vehicle_KeyDTO onchange([FromBody]VMS_Vehicle_KeyDTO data)
        {

            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));

            return del.onchange(data);
        }
        //savedataSource
        [Route("savedataSource")]
        public RTO_Master_FineDTO savedataSource([FromBody]RTO_Master_FineDTO data)
        {

            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));

            return del.savedataSource(data);
        }
        //clientDecative
        [Route("clientDecative")]
        public RTO_Master_FineDTO clientDecative([FromBody]RTO_Master_FineDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));

            return del.clientDecative(data);
        }
        //
        [Route("RecieptReport")]
        public RTO_Master_FineDTO RecieptReport([FromBody]RTO_Master_FineDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));

            return del.RecieptReport(data);
        }
        //VehicalUpdate
        [Route("VehicalUpdate")]
        public VMS_Vehicle_KeyDTO VehicalUpdate([FromBody]VMS_Vehicle_KeyDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));

            return del.VehicalUpdate(data);
        }
    }
}
