using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.FrontOffice;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.FrontOffice;


namespace IVRMUX.Controllers.com.vapstech.FrontOffice
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class GeneralRequestProcessController : Controller
    {
        GeneralRequestProcessDelegate ad = new GeneralRequestProcessDelegate();

        /////punch request ////
        [Route("getpunch/{id:int}")]
        public FO_Emp_PunchDTO getonlineLeave(int id)
        {
            FO_Emp_PunchDTO lv = new FO_Emp_PunchDTO();
            lv.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            lv.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return ad.getpunch(lv);
        }

        [Route("emprequest")]
        public FO_Emp_PunchDTO emprequest([FromBody]FO_Emp_PunchDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return ad.emprequest(data);
        }

        [Route("grsavedetail")]
        public FO_Emp_PunchDTO grsavedetail([FromBody]FO_Emp_PunchDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return ad.grsavedetail(data);
        }
        //approval
        [Route("getalldetails/{id:int}")]
        public FO_Emp_PunchDTO getdetails(int id)
        {
            FO_Emp_PunchDTO data = new FO_Emp_PunchDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return ad.getdetails(data);
        }
        [Route("getstatus")]
        public FO_Emp_PunchDTO getstatus([FromBody] FO_Emp_PunchDTO data)
        {           
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return ad.getstatus(data);
        }
        [Route("approvaldata")]
        public FO_Emp_PunchDTO approvaldata([FromBody] FO_Emp_PunchDTO data)
        {           
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return ad.approvaldata(data);
        }

    }
}
