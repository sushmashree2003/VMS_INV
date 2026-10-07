using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.VMS.HRMS;
using Recruitment.com.vaps.Interfaces;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Recruitment.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class Lead_Target_MappingFacadenewController : Controller
    {

        public Lead_Target_MappingnewInterface _ads;

        public Lead_Target_MappingFacadenewController(Lead_Target_MappingnewInterface adstu)
        {
            _ads = adstu;
        }

        // GET: api/values
        //[HttpGet]
        [Route("OnloadGetDetails")]
        public Lead_Target_MappingnewDTO OnloadGetDetails([FromBody] Lead_Target_MappingnewDTO dto)
        {
            return _ads.getBasicData(dto);
        }

        [Route("get_desig")]
        public Lead_Target_MappingnewDTO get_desig([FromBody] Lead_Target_MappingnewDTO dto)
        {
            return _ads.get_desig(dto);
        }


        [Route("get_employee")]
        public Lead_Target_MappingnewDTO get_employee([FromBody] Lead_Target_MappingnewDTO dto)
        {
            return _ads.get_employee(dto);
        }
        [Route("getdetails")]
        public Lead_Target_MappingnewDTO getdetails([FromBody] Lead_Target_MappingnewDTO dto)
        {
            return _ads.getdetails(dto);
        }
        [Route("getcatdetails")]
        public Lead_Target_MappingnewDTO getcatdetails([FromBody] Lead_Target_MappingnewDTO dto)
        {
            return _ads.getcatdetails(dto);
        }
        [Route("SaveTarget")]
        public Lead_Target_MappingnewDTO SaveTarget([FromBody] Lead_Target_MappingnewDTO dto)
        {
            return _ads.SaveTarget(dto);
        }
        [Route("Edit")]
        public Lead_Target_MappingnewDTO Edit([FromBody] Lead_Target_MappingnewDTO dto)
        {
            return _ads.Edit(dto);
        }
    }
}
