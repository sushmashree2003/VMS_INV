using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using corewebapi18072016.Delegates;
using DomainModel.Model;
using PreadmissionDTOs;
using PreadmissionDTOs.com.vaps.admission;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Cors;

namespace corewebapi18072016.Controllers
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class studentbirthdayController: Controller
    {
        studentbirthdaydelegate st = new studentbirthdaydelegate();

       

         [HttpPost]
        [Route("getdetails")] 
        public studentbirthdayreportDTO getdetails([FromBody]studentbirthdayreportDTO MMD)
        {

            MMD.MI_ID = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return st.getdetails(MMD);
        }
        [Route("getdetailsadd")]
        public studentbirthdayreportDTO getdetailsadd([FromBody]studentbirthdayreportDTO MMD)
        {

            MMD.MI_ID = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return st.getdetailsadd(MMD);
        }
        [Route("getYear/{id:int}")]
        public studentbirthdayreportDTO getYear(int id)
        {
            id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return st.getYear(id);
        }



        [Route("ExportToExcle/")]
        public string ExportToExcle([FromBody] studentbirthdayreportDTO MMD)
        {
            return st.ExportToExcle(MMD);
        }












    }




}
