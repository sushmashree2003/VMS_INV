using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using PreadmissionDTOs.com.vaps.VMS.HRMS;
using corewebapi18072016.Delegates.com.vapstech.VMS.OT;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers.com.vapstech.VMS.OT
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class OnlineTestCandidateController : Controller
    {
        OnlineTestCandidateDelegate _delg = new OnlineTestCandidateDelegate();
        // GET: api/values
        #region EXAM 
        [Route("loadExamdata/{id:int}")]
        public OnlineTestCandidateDTO loadExamdata(int id)
        {
            OnlineTestCandidateDTO data = new OnlineTestCandidateDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.Userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            data.User_Name = Convert.ToString(HttpContext.Session.GetString("UserName"));
            data.rolename = Convert.ToString(HttpContext.Session.GetString("RoleNme"));
            return _delg.loadExamdata(data);
        }

        [Route("getQuestion")]
        public OnlineTestCandidateDTO getQuestion([FromBody]  OnlineTestCandidateDTO data)
        {
           
          //  data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.Userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return _delg.getQuestion(data);
        }
        [Route("Saveanswer")]
        public OnlineTestCandidateDTO Saveanswer([FromBody]  OnlineTestCandidateDTO data)
        {
           
           // data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.Userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return _delg.Saveanswer(data);
        }
        [Route("submitexam")]
        public OnlineTestCandidateDTO submitexam([FromBody]  OnlineTestCandidateDTO data)
        {
           
          //  data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.Userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return _delg.submitexam(data);
        }

        #endregion


        #region PAPER CORRECTION  
        [Route("loadcorrection/{id:int}")]
        public OnlineTestCandidateDTO loadcorrection(int id)
        {
            OnlineTestCandidateDTO data = new OnlineTestCandidateDTO();
          //  data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.Userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            data.User_Name = Convert.ToString(HttpContext.Session.GetString("UserName"));
            return _delg.loadcorrection(data);
        }



        [Route("GetSearchDetails")]
        public OnlineTestCandidateDTO GetSearchDetails([FromBody] OnlineTestCandidateDTO data)
        {
            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.Userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            data.User_Name = Convert.ToString(HttpContext.Session.GetString("UserName"));
            return _delg.GetSearchDetails(data);
        }

        [Route("oncmpchange")]
        public OnlineTestCandidateDTO oncmpchange([FromBody] OnlineTestCandidateDTO data)
        {
            data.Userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            data.User_Name = Convert.ToString(HttpContext.Session.GetString("UserName"));
            return _delg.oncmpchange(data);
        }
        [Route("ViewQuestionanswers")]
        public OnlineTestCandidateDTO ViewQuestionanswers([FromBody] OnlineTestCandidateDTO data)
        {
           // data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.Userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            data.User_Name = Convert.ToString(HttpContext.Session.GetString("UserName"));
            return _delg.ViewQuestionanswers(data);
        }
          [Route("SaveSubjectiveMarks")]
        public OnlineTestCandidateDTO SaveSubjectiveMarks([FromBody] OnlineTestCandidateDTO data)
        {
          //  data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.Userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            data.User_Name = Convert.ToString(HttpContext.Session.GetString("UserName"));
            return _delg.SaveSubjectiveMarks(data);
        }


        #endregion


        #region REPORT
        [Route("loadDetailsRpt/{id:int}")]
        public OnlineTestCandidateDTO loadDetailsRpt(int id)
        {
            OnlineTestCandidateDTO data = new OnlineTestCandidateDTO();
           // data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.Userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            data.User_Name = Convert.ToString(HttpContext.Session.GetString("UserName"));
            return _delg.loadDetailsRpt(data);
        }


        [Route("GetReport")]
        public OnlineTestCandidateDTO GetReport([FromBody] OnlineTestCandidateDTO data)
        {
          //  data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.Userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            data.User_Name = Convert.ToString(HttpContext.Session.GetString("UserName"));
            return _delg.GetReport(data);
        }
        [Route("ViewResultRpt")]
        public OnlineTestCandidateDTO ViewResultRpt([FromBody] OnlineTestCandidateDTO data)
        {
           // data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.Userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            data.User_Name = Convert.ToString(HttpContext.Session.GetString("UserName"));
            return _delg.ViewResultRpt(data);
        }

        [Route("ViewResultIndRpt")]
        public OnlineTestCandidateDTO ViewResultIndRpt([FromBody] OnlineTestCandidateDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.Userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            data.User_Name = Convert.ToString(HttpContext.Session.GetString("UserName"));
            return _delg.ViewResultRpt(data);
        }
        #endregion
    }
}
