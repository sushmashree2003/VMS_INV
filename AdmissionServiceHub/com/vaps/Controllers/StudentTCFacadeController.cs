using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.admission;
using AdmissionServiceHub.com.vaps.Interfaces;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace AdmissionServiceHub.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class StudentTCFacadeController : Controller
    {
        public StudentTCInterface _IAtt;
        public StudentTCFacadeController(StudentTCInterface enqui)
        {
            _IAtt = enqui;
        }
        // GET: api/values
        [HttpPost]

        [Route("getactiveDetails")]
        public StudentTCDTO getactiveDetails([FromBody]StudentTCDTO tc)
        {
            return _IAtt.gettcDetails(tc);
        }
        [Route("getStatusDetails")]
        public StudentTCDTO getStatusDetails([FromBody]StudentTCDTO tc)
        {
            return _IAtt.getStatusDetails(tc);
        }

        [Route("savedetails")]

        public Task<StudentTCDTO> Post([FromBody]StudentTCDTO stu_tc)
        {

            return _IAtt.saveTcdet(stu_tc);

        }


        [Route("chk_tc_dup")]

        public StudentTCDTO chk_tc_dup([FromBody]StudentTCDTO stu_tc)
        {

            return _IAtt.chk_tc_dup(stu_tc);

        }

        [Route("getstudent_name_list")]

        public async Task<StudentTCDTO> getstudent_name_list([FromBody]StudentTCDTO name_list)
        {

            return await _IAtt.getstudent_name_list(name_list);

        }


        [Route("getstudenttcdata")]
        public async Task<StudentTCDTO> getstudenttcdata([FromBody]StudentTCDTO MIID)
        {
            return await _IAtt.GetStudentInitialData(MIID);
        }


        [Route("gettcDetails")]
        public StudentTCDTO gettcDetails([FromBody]StudentTCDTO tc)
        {
            return _IAtt.gettcDetails(tc);
        }

        [Route("searchfilter")]
        public StudentTCDTO searchfilter([FromBody]StudentTCDTO data)
        {
            return _IAtt.searchfilter(data);
        }
        //public StudentTCDTO otherdetails([FromBody]StudentTCDTO stu_tc)
        //{

        //    return _IAtt.otherdet(stu_tc);

        //}


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
