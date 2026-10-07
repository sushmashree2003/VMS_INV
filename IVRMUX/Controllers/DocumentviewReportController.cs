using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using corewebapi18072016.Delegates;
using PreadmissionDTOs;
using Microsoft.AspNetCore.Http;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class DocumentviewReportController : Controller
    {
        DocumentViewDelegate objdelegate = new DocumentViewDelegate();
        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
      
        [HttpGet]
        [Route("getdetails/{id:int}")]
        public DocumentViewDTO Get(int id)
        {
            DocumentViewDTO data = new DocumentViewDTO();
            data.mi_id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return objdelegate.getdetails(data);
        }
        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }
        [HttpPost]
        // Added on 19-9-2016
        [Route("getdpforreg")]
        public DocumentViewDTO getDpData([FromBody] DocumentViewDTO data)
        {

            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.mi_id = mid;

            ////int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            ////ctry.Id = UserId;

            ////int ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            ////ctry.ASMAY_Id = ASMAY_Id;

            //return sad.getIndependentDropDowns(ctry);
            return objdelegate.getDpData(data);
        }
        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
