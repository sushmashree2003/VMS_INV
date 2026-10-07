using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.VMS.Training;
using Recruitment.com.vaps.Interfaces;

namespace Recruitment.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class Training_FeedbackFacadeController : Controller
    {
        public Training_Feedback_Interface _flr;

        public Training_FeedbackFacadeController(Training_Feedback_Interface ct)
        {
            _flr = ct;
        }
        //----------------------------Building-----------------------------
        [Route("loaddata")]
        public HR_Training_Feedback_DTO loaddata([FromBody] HR_Training_Feedback_DTO id)
        {
            return _flr.loaddata(id);
        }
        [Route("getQuestions")]
        public HR_Training_Feedback_DTO getQuestions([FromBody] HR_Training_Feedback_DTO  dto)
        {
            return _flr.getQuestions(dto);
        }

        [Route("getFeedbackTrainer")]
        public HR_Training_Feedback_DTO getFeedbackTrainer([FromBody] HR_Training_Feedback_DTO dto)
        {
            return _flr.getFeedbackTrainer(dto);
        }

        [HttpPost]
        [Route("savetrainerfeedback")]
        public HR_Training_Feedback_DTO savetrainerfeedback([FromBody] HR_Training_Feedback_DTO dTO)
        {
            return _flr.savetrainerfeedback(dTO);
        }

        [HttpPost]
        [Route("savetraineefeedback")]
        public HR_Training_Feedback_DTO savetraineefeedback([FromBody] HR_Training_Feedback_DTO dTO)
        {
            return _flr.savetraineefeedback(dTO);
        }

        [HttpPost]
        [Route("trainerfeedbackreport")]
        public HR_Training_Feedback_DTO trainerfeedbackreport([FromBody] HR_Training_Feedback_DTO dTO)
        {
            return _flr.trainerfeedbackreport(dTO);
        }

        [HttpPost]
        [Route("traineefeedbackreport")]
        public HR_Training_Feedback_DTO traineefeedbackreport([FromBody] HR_Training_Feedback_DTO dTO)
        {
            return _flr.traineefeedbackreport(dTO);
        }
        ///report////////////////////////////////////////////////
        [Route("reportloaddata")]
        public HR_Training_Feedback_DTO reportloaddata([FromBody] HR_Training_Feedback_DTO id)
        {
            return _flr.reportloaddata(id);
        }

        [HttpPost]
        [Route("reportpagetrainerfeedbackreport")]
        public HR_Training_Feedback_DTO reportpagetrainerfeedbackreport([FromBody] HR_Training_Feedback_DTO dTO)
        {
            return _flr.reportpagetrainerfeedbackreport(dTO);
        }

        [HttpPost]
        [Route("reportpagetraineefeedbackreport")]
        public HR_Training_Feedback_DTO reportpagetraineefeedbackreport([FromBody] HR_Training_Feedback_DTO dTO)
        {
            return _flr.reportpagetraineefeedbackreport(dTO);
        }
        [HttpPost]
        [Route("trainigheldreport")]
        public HR_Training_Feedback_DTO trainigheldreport([FromBody] HR_Training_Feedback_DTO dTO)
        {
            return _flr.trainigheldreport(dTO);
        }
    }
}