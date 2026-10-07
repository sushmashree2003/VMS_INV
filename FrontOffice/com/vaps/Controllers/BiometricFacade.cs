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
    public class BiometricFacade : Controller
    {
        public BiometricInterface _bio;
        public BiometricFacade(BiometricInterface bio)
        {
            _bio = bio;
        }
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

        // POST api/values
        [Route("punchdata")]
        public FO_Emp_PunchDTO punchdata([FromBody] FO_Emp_PunchDTO data)
        {

            return _bio.punchdata(data);
            //return null;
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

        [Route("Latedata")]
        public FO_Emp_PunchDTO Latedata([FromBody] FO_Emp_PunchDTO data)
        {

            return _bio.Latedata(data);
            //return null;
        }

        [Route("LateInAbs_Email")]
        public FO_Emp_PunchDTO LateInAbs_Email([FromBody] FO_Emp_PunchDTO data)
        {
            return _bio.LateInAbs_Email(data);
            //return null;
        }

        [Route("EarlyOut_Email")]
        public FO_Emp_PunchDTO EarlyOut_Email([FromBody] FO_Emp_PunchDTO data)
        {
            return _bio.EarlyOut_Email(data);
            //return null;
        }

        [Route("Earlydata")]
        public FO_Emp_PunchDTO Earlydata([FromBody] FO_Emp_PunchDTO data)
        {

            return _bio.Earlydata(data);
            //return null;
        }
        [Route("vapsdata")]
        public FO_Biometric_VAPS_IEMapping_DTO vapsdata([FromBody] FO_Biometric_VAPS_IEMapping_DTO data)
        {

            return _bio.vapsdata(data);
            //return null;
        }

        [Route("punchdata_vaps")]
        public FO_Emp_PunchDTO punchdata_vaps([FromBody] FO_Emp_PunchDTO data)
        {

            return _bio.punchdata_vaps(data);
            //return null;
        }
        [Route("RFCardpunchdata")]
        public FO_Emp_PunchDTO RFCardpunchdata([FromBody]FO_Emp_PunchDTO data)
        {
            return _bio.RFCardpunchdata(data);
        }

        [Route("AutoAbsent")]
        public FO_Emp_PunchDTO AutoAbsent([FromBody] FO_Emp_PunchDTO data)
        {
            return _bio.AutoAbsent(data);
        }

        [Route("BioMeteric_TemperatureData")]
        public FO_Emp_PunchDTO BioMeteric_TemperatureData([FromBody] FO_Emp_PunchDTO data)
        {
            return _bio.BioMeteric_TemperatureData(data);
        }
        [Route("kaleyra")]
        public Task<FO_Emp_PunchDTO> kaleyra(FO_Emp_PunchDTO data)
        {
            return _bio.kaleyra(data);
        }
    }
}
