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
    public class OnlineTestController : Controller
    {
        OnlineTestDelegate _delg = new OnlineTestDelegate();
        // GET: api/values
        #region  MASTER QUESTION 
        //**********  MASTER QUESTION  *****************//
        [Route("getmasterquestionloaddata/{id:int}")]
        public OnlineTestDTO getmasterquestionloaddata(int id)
        {
            OnlineTestDTO data = new OnlineTestDTO();
            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.Userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return _delg.getmasterquestionloaddata(data);
        }
        [Route("getmastercompliexities/{id:int}")]
        public OnlineTestDTO getmastercompliexities(int id)
        {
            OnlineTestDTO data = new OnlineTestDTO();
            data.Userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return _delg.getmastercompliexities(data);
        }

        [Route("getqnspapertype")]
        public OnlineTestDTO getqnspapertype([FromBody] OnlineTestDTO data)
        {
           // data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.Userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return _delg.getqnspapertype(data);
        }
       
        [Route("SaveMasterQuestionDetails")]
        public OnlineTestDTO SaveMasterQuestionDetails([FromBody] OnlineTestDTO data)
        {
          //  data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.Userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return _delg.SaveMasterQuestionDetails(data);
        }

        [Route("EditMasterQuestion")]
        public OnlineTestDTO EditMasterQuestion([FromBody] OnlineTestDTO data)
        {
           // data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.Userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return _delg.EditMasterQuestion(data);
        }

        [Route("ViewMasterQuesDoc")]
        public OnlineTestDTO ViewMasterQuesDoc([FromBody] OnlineTestDTO data)
        {
           // data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.Userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return _delg.ViewMasterQuesDoc(data);
        }

        [Route("DeactivateActivateDocument")]
        public OnlineTestDTO DeactivateActivateDocument([FromBody] OnlineTestDTO data)
        {
         //   data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.Userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return _delg.DeactivateActivateDocument(data);
        }

        [Route("ViewMasterQuesOptions")]
        public OnlineTestDTO ViewMasterQuesOptions([FromBody] OnlineTestDTO data)
        {
          //  data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.Userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return _delg.ViewMasterQuesOptions(data);
        }

        [Route("DeactivateActivateQuesOption")]
        public OnlineTestDTO DeactivateActivateQuesOption([FromBody] OnlineTestDTO data)
        {
           // data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.Userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return _delg.DeactivateActivateQuesOption(data);
        }
        [Route("AddMoreOptions")]
        public OnlineTestDTO AddMoreOptions([FromBody] OnlineTestDTO data)
        {
           // data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.Userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return _delg.AddMoreOptions(data);
        }

        [Route("loadcdata")]
        public OnlineTestDTO loadcdata([FromBody] OnlineTestDTO data)
        {
           // data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.Userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return _delg.loadcdata(data);
        }

        [Route("DeactivateActivateQues")]
        public OnlineTestDTO DeactivateActivateQues([FromBody] OnlineTestDTO data)
        {
           // data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.Userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return _delg.DeactivateActivateQues(data);
        }
        [Route("ViewUploadOptionFiles")]
        public OnlineTestDTO ViewUploadOptionFiles([FromBody] OnlineTestDTO data)
        {
           // data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.Userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return _delg.ViewUploadOptionFiles(data);
        }
        [Route("DeactivateActivateOptionsDocument")]
        public OnlineTestDTO DeactivateActivateOptionsDocument([FromBody] OnlineTestDTO data)
        {
           // data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.Userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return _delg.DeactivateActivateOptionsDocument(data);
        }
        [Route("DeactivateActivateComplexities")]
        public OnlineTestDTO DeactivateActivateComplexities([FromBody] OnlineTestDTO data)
        {
           // data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.Userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return _delg.DeactivateActivateComplexities(data);
        }
        [Route("SaveMasterComplexity")]
        public OnlineTestDTO SaveMasterComplexity([FromBody] OnlineTestDTO data)
        {
           // data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.Userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return _delg.SaveMasterComplexity(data);
        }

        #endregion

      

    }
}
