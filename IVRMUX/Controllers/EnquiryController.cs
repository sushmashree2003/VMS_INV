using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using corewebapi18072016.Delegates;
using DomainModel.Model;
using PreadmissionDTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]

    public class EnquiryController : Controller
    {
        EnquiryDelegate enqu = new EnquiryDelegate();

     

        [HttpPost]
        [Route("getalldetails")]
        public Enq Get([FromBody] Enq en)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            en.MI_Id = mid;

            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            en.Id = UserId;

            int ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            en.ASMAY_Id = ASMAY_Id;

            return enqu.getcountrydata(en);
            //return enq;
        }
        //[Route("getalldetails/{id:int}")]
        //public Enq getdetails(int id)
        //{
        //    return enqu.getalldetails(id);
        //}

        [HttpGet]
        [Route("getenquirycontroller/{id:int}")]
        public StateDTO Getstates(int id)
        {
            //int drpdata = en.countryid;
            //EnqDTO enq=new EnqDTO();
            return enqu.enqdatacountrydrp(id);
            //return enq;
        }

        [Route("getenquirystatecontroller/{id:int}")]
        public CityDTO getcity(int id)
        {
            return enqu.cityfill(id);
        }
        [Route("GetEnqdetails/{id:int}")]
        public Enq getdetail(int id)
        {
            //HttpContext.Session.SetString("enqid", id.ToString()); //Set
            // id = 12;
            return enqu.Editdetailss(id);
            //HttpContext.Session.SetString("institutionid","0"); //Set
        }

        [HttpPost]
        public Enq saveEnqdetail([FromBody] Enq en)
        {   
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            en.MI_Id = mid;

            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            en.Id = UserId;

            //int enqid = 0;
            //if (HttpContext.Session.GetString("enqid") != null)
            //{
            //    enqid = Convert.ToInt32(HttpContext.Session.GetString("enqid"));//Get
            //    en.PASE_Id = enqid;
            //    HttpContext.Session.Remove("enqid");
            //}
            return enqu.saveEnqdetails(en);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {

        }

        // DELETE api/values/5
        [HttpPost]
        [Route("DeleteDetails")]
        public Enq Delete([FromBody] Enq en)
        {
            return enqu.DeleteEnqrecord(en);
        }
        public Enq clear(int id)
        {
            return enqu.clearEnqdata(id);

        }
    }
}
