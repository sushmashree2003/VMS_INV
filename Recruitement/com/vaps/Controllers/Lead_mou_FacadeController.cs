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
    public class Lead_mou_FacadeController : Controller
    {

        public Lead_mou_Interface _ads;

        public Lead_mou_FacadeController(Lead_mou_Interface adstu)
        {
            _ads = adstu;
        }

        
        [Route("OnloadGetDetails")]
        public Lead_Target_MappingDTO OnloadGetDetails([FromBody] Lead_Target_MappingDTO dto)
        {
            return _ads.getBasicData(dto);
        }

        [Route("lead_product")]
        public Lead_Target_MappingDTO lead_product([FromBody]Lead_Target_MappingDTO dto)
        {
            return _ads.lead_product(dto);
        }

        [Route("savedata")]
        public Lead_Target_MappingDTO savedata([FromBody]Lead_Target_MappingDTO dto)
        {
            return _ads.savedata(dto);
        }

        [Route("MOUViewdata")]
        public Lead_Target_MappingDTO MOUViewdata([FromBody]Lead_Target_MappingDTO dto)
        {
            return _ads.MOUViewdata(dto);
        }

        [Route("sendmail")]
        public Lead_Target_MappingDTO sendmail([FromBody]Lead_Target_MappingDTO dto)
        {
            return _ads.sendmail(dto);
        }


        
    }
}
