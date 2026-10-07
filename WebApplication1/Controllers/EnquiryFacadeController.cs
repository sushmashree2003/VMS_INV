using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Interfaces;
using WebApplication1.Services;
using PreadmissionDTOs;
using Microsoft.Extensions.Logging;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    public class EnquiryFacadeController : Controller
    {
        public EnquiryInterface _enq;

        public EnquiryFacadeController(EnquiryInterface enqui)
        {
            _enq = enqui;
        }

        // GET api/values/5
        [HttpGet]
        [Route("getenquirycontroller/{id:int}")]
        //[Route("getenquirycontroller")]
        public StateDTO Getcountrydata(int id)
        {
            return _enq.enqdrpcountrydata(id);
        }
        [Route("getenquirystatecontroller/{id:int}")]
        //[Route("getenquirycontroller")]
        public CityDTO getcity(int id)
        {
            return _enq.getcity(id);
        }
        [Route("GetEnqdetails/{id:int}")]
        //[Route("getenquirycontroller")]
        public Enq Editenqdetails(int id)
        {
            // id = 12;
            return _enq.EditDetails(id);
        }
        //[Route("getalldetails/{id:int}")]
        //public Enq Getalldetails(Enq en)
        //{
        //    // id = 12;
        //    return _enq.GetAllDetais(en);
        //}

        // POST api/values
        [HttpPost]
        public async Task<Enq> Post([FromBody] Enq enquiry)
        {
            return await _enq.saveEnqdata(enquiry);
        }

        [Route("loaddata")]
        public Enq Getdata([FromBody] Enq en)
        {
            return _enq.countrydrp(en);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public string Put([FromBody]EnqDTO enw)
        {
            return "success";
        }

        // DELETE api/values/5
        // [HttpDelete("{id}")]
        public Enq clear(int id)
        {
            return _enq.clearEnqdata(id);
        }

        [HttpPost]
        [Route("DeleteDetails")]
        public Enq Deleterec([FromBody] Enq enw)
        {
            return _enq.DeleteEnqDetails(enw);
        }
    }
}
