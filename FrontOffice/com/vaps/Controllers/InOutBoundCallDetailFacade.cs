using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FrontOfficeHub.com.vaps.Interfaces;
using PreadmissionDTOs.com.vaps.FrontOffice;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace FrontOfficeHub.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class InOutBoundCallDetailFacade : Controller
    {
        public InOutBoundCallDetailInterface _org;
        public InOutBoundCallDetailFacade(InOutBoundCallDetailInterface maspag)
        {
            _org = maspag;
        }

        [HttpPost]
        [Route("getalldetails")]
        public ISM_Inbound_Outbound_Call_DetailsDTO Getdet([FromBody] ISM_Inbound_Outbound_Call_DetailsDTO data)
        {
            return _org.getdata(data);
        }
        [Route("report")]
        public ISM_Inbound_Outbound_Call_DetailsDTO report([FromBody] ISM_Inbound_Outbound_Call_DetailsDTO data)
        {
            return _org.report(data);
        }
        [Route("savedata")]
        public ISM_Inbound_Outbound_Call_DetailsDTO savedata([FromBody] ISM_Inbound_Outbound_Call_DetailsDTO data)
        {
            return _org.savedata(data);
        }

        [Route("getdetails/{id:int}")]
        public ISM_Inbound_Outbound_Call_DetailsDTO getdetails(int id)
        {
            return _org.getdetails(id);
        }

        [Route("deletedata/{id:int}")]
        public ISM_Inbound_Outbound_Call_DetailsDTO deletedata(int id)
        {
            return _org.deletedata(id);
        }
    }
}
