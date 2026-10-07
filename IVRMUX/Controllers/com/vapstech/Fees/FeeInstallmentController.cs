using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using corewebapi18072016.Delegates;
using Microsoft.AspNetCore.Http;
using PreadmissionDTOs.com.vaps.Fees;


// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class FeeInstallmentController : Controller
    {
        FeeInstallmentDelegate FID = new FeeInstallmentDelegate();
        // FID = new FeeInstallmentDelegate();
        // GET: api/values
        [HttpGet]

        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [Route("getalldetails/{id:int}")]
        public FeeInstallmentDTO Get([FromQuery] int id)
        {
            FeeInstallmentDTO data = new FeeInstallmentDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return FID.getdetails(data);
        }

        [Route("savedetail")]
        public FeeInstallmentDTO savedetail([FromBody] FeeInstallmentDTO Grouppage)
        {
            Grouppage.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return FID.savedetails(Grouppage);
        }

        [Route("getdetails/{id:int}")]
        public FeeInstallmentDTO getdetail(int id)
        {
            return FID.getpagedetails(id);
        }

        [Route("deactivate")]
        public FeeInstallmentDTO deactvate([FromBody] FeeInstallmentDTO id)
        {
            return FID.deactivateAcademicYear(id);
        }
    }
}