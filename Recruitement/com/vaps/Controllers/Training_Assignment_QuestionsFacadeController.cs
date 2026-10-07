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
    public class Training_Assignment_QuestionsFacadeController : Controller
    {
        public Training_Assignment_QuestionsInterface _flr;


        public Training_Assignment_QuestionsFacadeController(Training_Assignment_QuestionsInterface ct)
        {
            _flr = ct;
        }
        // GET: api/<controller>
        [Route("getdata_question")]
        public HR_Training_Assignment_Questions_DTO getdata_question([FromBody] HR_Training_Assignment_Questions_DTO dto)
        {
            return _flr.getdata_question(dto);
        }

        [Route("SaveEdit_question")]
        public HR_Training_Assignment_Questions_DTO SaveEdit_question([FromBody] HR_Training_Assignment_Questions_DTO dTO)
        {
            return _flr.SaveEdit_question(dTO);
        }

        [Route("details_question")]
        public HR_Training_Assignment_Questions_DTO details_question([FromBody]HR_Training_Assignment_Questions_DTO id)
        {
            return _flr.details_question(id);
        }

        [HttpPost]
        [Route("deactivate_question")]
        public HR_Training_Assignment_Questions_DTO deactivate_question([FromBody] HR_Training_Assignment_Questions_DTO dTO)
        {
            return _flr.deactivate_question(dTO);
        }


    }
}
