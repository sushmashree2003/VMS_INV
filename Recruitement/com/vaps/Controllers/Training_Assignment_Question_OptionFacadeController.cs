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
    public class Training_Assignment_Question_OptionFacadeController : Controller
    {
        public Training_Assignment_Question_OptionInterface _flr;


        public Training_Assignment_Question_OptionFacadeController(Training_Assignment_Question_OptionInterface ct)
        {
            _flr = ct;
        }
        [Route("getdata_TAQO")]
        public HR_Training_Assignment_Question_Option_DTO getdata_TAQO([FromBody] HR_Training_Assignment_Question_Option_DTO dto)
        {
            return _flr.getdata_TAQO(dto);
        }
        [Route("SaveEdit_TAQO")]
        public HR_Training_Assignment_Question_Option_DTO SaveEdit_TAQO([FromBody] HR_Training_Assignment_Question_Option_DTO dto)
        {
            return _flr.SaveEdit_TAQO(dto);
        }
        [HttpPost]
        [Route("deactivate_TAQO")]
        public HR_Training_Assignment_Question_Option_DTO deactivate_TAQO([FromBody] HR_Training_Assignment_Question_Option_DTO dTO)
        {
            return _flr.deactivate_TAQO(dTO);
        }

        [Route("details_TAQO")]
        public HR_Training_Assignment_Question_Option_DTO details_TAQO([FromBody]HR_Training_Assignment_Question_Option_DTO id)
        {
            return _flr.details_TAQO(id);
        }
        [Route("option_view_TAQO")]
        public HR_Training_Assignment_Question_Option_DTO option_view_TAQO([FromBody]HR_Training_Assignment_Question_Option_DTO id)
        {
            return _flr.option_view_TAQO(id);
        }

    }
}
