using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FrontOffice.com.vaps.Interfaces;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.FrontOffice;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FrontOffice.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class GeneralRequestProcessFacadeController : Controller
    {
        public GeneralRequestProcessInterface _org;

        public GeneralRequestProcessFacadeController(GeneralRequestProcessInterface maspag)
        {
            _org = maspag;
        }


        /////punch request ////
        [HttpPost]
        [Route("getpunch")]
        public FO_Emp_PunchDTO getpunch([FromBody] FO_Emp_PunchDTO data)
        {
            return _org.getpunch(data);
        }
        [Route("emprequest")]
        public FO_Emp_PunchDTO emprequest([FromBody] FO_Emp_PunchDTO data)
        {
            return _org.emprequest(data);
        }

        [Route("grsavedetail")]
        public FO_Emp_PunchDTO grsavedetail([FromBody] FO_Emp_PunchDTO data)
        {
            return _org.grsavedetail(data);
        }
        //approval
        [HttpPost]
        [Route("getalldetails")]
        public FO_Emp_PunchDTO Getdet([FromBody] FO_Emp_PunchDTO data)
        {
            return _org.getdata(data);
        }
        [HttpPost]
        [Route("getstatus")]
        public FO_Emp_PunchDTO getstatus([FromBody] FO_Emp_PunchDTO data)
        {
            return _org.getstatus(data);
        }
        [HttpPost]
        [Route("approvaldata")]
        public FO_Emp_PunchDTO approvaldata([FromBody] FO_Emp_PunchDTO data)
        {
            return _org.approvaldata(data);
        }

    }
}
