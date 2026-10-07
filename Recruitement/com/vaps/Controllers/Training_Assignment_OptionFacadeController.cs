using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.VMS.Training;
using Recruitment.com.vaps.Interfaces;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Recruitment.com.vaps.Controllers
{
     [Route("api/[controller]")]
     public class Training_Assignment_OptionFacadeController : Controller
     {
        public Training_Assignment_OptionInterface _flr;


        public Training_Assignment_OptionFacadeController(Training_Assignment_OptionInterface ct)
        {
            _flr = ct;
        }

        [Route("getdata_MFO")]
        public HR_Training_Assignment_Option_DTO getdata_MFO([FromBody] HR_Training_Assignment_Option_DTO dto)
        {
            return _flr.getdata_MFO(dto);
        }

        [Route("SaveEdit_MFO")]
        public HR_Training_Assignment_Option_DTO SaveEdit_MFO([FromBody] HR_Training_Assignment_Option_DTO dTO)
        {
            return _flr.SaveEdit_MFO(dTO);
        }

        [HttpPost]
        [Route("deactivate_MFO")]
        public HR_Training_Assignment_Option_DTO deactivate_MFO([FromBody] HR_Training_Assignment_Option_DTO dTO)
        {
            return _flr.deactivate_MFO(dTO);
        }

        [Route("details_MFO")]
        public HR_Training_Assignment_Option_DTO details_MFO([FromBody]HR_Training_Assignment_Option_DTO id)
        {
            return _flr.details_MFO(id);
        }








    }
}
