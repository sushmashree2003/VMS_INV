using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.VMS.HRMS;
using Recruitment.com.vaps.Interfaces;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Recruitment.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class OnlineTestCandidateFacadeController : Controller
    {

        public OnlineTestCandidateInterface _interface;

        public OnlineTestCandidateFacadeController(OnlineTestCandidateInterface adstu)
        {
            _interface = adstu;
        }

      

        #region EXAM
        [Route("loadExamdata")]
        public OnlineTestCandidateDTO loadExamdata([FromBody] OnlineTestCandidateDTO data)
        {
            return _interface.loadExamdata(data);
        }

        [Route("getQuestion")]
        public OnlineTestCandidateDTO getQuestion([FromBody] OnlineTestCandidateDTO data)
        {
            return _interface.getQuestion(data);
        }
        [Route("Saveanswer")]
        public OnlineTestCandidateDTO Saveanswer([FromBody] OnlineTestCandidateDTO data)
        {
            return _interface.Saveanswer(data);
        }
        [Route("submitexam")]
        public OnlineTestCandidateDTO submitexam([FromBody] OnlineTestCandidateDTO data)
        {
            return _interface.submitexam(data);
        }
        #endregion


        #region PAPER CORRECTION
        [Route("loadcorrection")]
        public OnlineTestCandidateDTO loadcorrection([FromBody] OnlineTestCandidateDTO data)
        {
            return _interface.loadcorrection(data);
        }
        [Route("GetSearchDetails")]
        public OnlineTestCandidateDTO GetSearchDetails([FromBody] OnlineTestCandidateDTO data)
        {
            return _interface.GetSearchDetails(data);
        }
        [Route("oncmpchange")]
        public OnlineTestCandidateDTO oncmpchange([FromBody] OnlineTestCandidateDTO data)
        {
            return _interface.oncmpchange(data);
        }
        [Route("ViewQuestionanswers")]
        public OnlineTestCandidateDTO ViewQuestionanswers([FromBody] OnlineTestCandidateDTO data)
        {
            return _interface.ViewQuestionanswers(data);
        }
        [Route("SaveSubjectiveMarks")]
        public OnlineTestCandidateDTO SaveSubjectiveMarks([FromBody] OnlineTestCandidateDTO data)
        {
            return _interface.SaveSubjectiveMarks(data);
        }

        #endregion
        
        #region REPORT
        [Route("loadDetailsRpt")]
        public OnlineTestCandidateDTO loadDetailsRpt([FromBody] OnlineTestCandidateDTO data)
        {
            return _interface.loadDetailsRpt(data);
        }
        [Route("GetReport")]
        public OnlineTestCandidateDTO GetReport([FromBody] OnlineTestCandidateDTO data)
        {
            return _interface.GetReport(data);
        }

        [Route("ViewResultRpt")]
        public OnlineTestCandidateDTO ViewResultRpt([FromBody] OnlineTestCandidateDTO data)
        {
            return _interface.ViewResultRpt(data);
        }
        #endregion

    }
}
