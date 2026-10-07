using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using corewebapi18072016.Delegates.com.vapstech.College.Admission;
using PreadmissionDTOs.com.vaps.College.Admission;
using Microsoft.AspNetCore.Http;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers.com.vapstech.College.Admission
{
    [Route("api/[controller]")]
    public class ClgSeatDistributionController : Controller
    {

        ClgSeatDistributionDelegate _Seat = new ClgSeatDistributionDelegate();

        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        
        [Route("getalldetails/{id:int}")]
        public ClgSeatDistributionDTO getalldetails (int id)
        {
            ClgSeatDistributionDTO data = new ClgSeatDistributionDTO();
            data.MI_Id= Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _Seat.getalldetails(data);
        }


        [Route("getCoursedata")]
        public ClgSeatDistributionDTO getCoursedata([FromBody]ClgSeatDistributionDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));           
            return _Seat.getCoursedata(data);
        }
        [Route("getBranchdata")]
        public ClgSeatDistributionDTO getBranchdata([FromBody]ClgSeatDistributionDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _Seat.getBranchdata(data);
        }
        [Route("getSemesterdata")]
        public ClgSeatDistributionDTO getSemesterdata([FromBody]ClgSeatDistributionDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _Seat.getSemesterdata(data);
        }

        [Route("get_Category")]
        public ClgSeatDistributionDTO get_Category([FromBody] ClgSeatDistributionDTO data)
        {
            
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _Seat.get_Category(data);
        }
        [Route("get_Seattotal/{id:int}")]
        public ClgSeatDistributionDTO get_Seattotal(int id)
        {
            ClgSeatDistributionDTO data = new ClgSeatDistributionDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _Seat.get_Seattotal(data);
        }
        [Route("savedata")]
        public ClgSeatDistributionDTO savedata([FromBody]ClgSeatDistributionDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _Seat.savedata(data);
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
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
