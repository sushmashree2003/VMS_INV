using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IssueManager.com.RTO.Interfaces;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.VMS.RTO;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IssueManager.com.RTO.Controllers
{
    [Route("api/[controller]")]
    public class RTO_Master_FineFacade : Controller
    {
        public RTO_Master_FineInterface inter;
        public RTO_Master_FineFacade(RTO_Master_FineInterface s)
        {
            inter = s;
        }
        [Route("loaddata")]
        public RTO_Master_FineDTO loaddata([FromBody] RTO_Master_FineDTO data)
        {
            return inter.loaddata(data);
        }
        [Route("savedata")]
        public VMS_Vehicle_KeyDTO savedata([FromBody] VMS_Vehicle_KeyDTO data)
        {
            return inter.savedata(data);
        }
        //onchange
        [Route("onchange")]
        public VMS_Vehicle_KeyDTO onchange([FromBody] VMS_Vehicle_KeyDTO data)
        {
            return inter.onchange(data);
        }

        //savedataSource
        [Route("savedataSource")]
        public RTO_Master_FineDTO savedataSource([FromBody] RTO_Master_FineDTO data)
        {
            return inter.savedataSource(data);
        }
        //clientDecative
        [Route("clientDecative")]
        public RTO_Master_FineDTO clientDecative([FromBody] RTO_Master_FineDTO data)
        {
            return inter.clientDecative(data);
        }
        //RecieptReport
        [Route("RecieptReport")]
        public RTO_Master_FineDTO RecieptReport([FromBody] RTO_Master_FineDTO data)
        {
            return inter.RecieptReport(data);
        }
        //VehicalUpdate
        [Route("VehicalUpdate")]
        public VMS_Vehicle_KeyDTO VehicalUpdate([FromBody] VMS_Vehicle_KeyDTO data)
        {
            return inter.VehicalUpdate(data);
        }
    }
}
