using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.VMS;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.VMS;
using PreadmissionDTOs.com.vaps.VMS.Exit;
using PreadmissionDTOs.com.vaps.VMS.HRMS;

namespace IVRMUX.Controllers.com.vapstech.VMS
{
    [Produces("application/json")]
    [Route("api/ISM_Certificate_Process")]
    public class ISM_Certificate_ProcessController : Controller
    {
        ISM_Certificate_Process_Delegate EED = new ISM_Certificate_Process_Delegate();

        //-------------ISM_Certificate_Request----------------


        [Route("getloaddata/{id:int}")]
        public ISM_Certificate_RequestDTO getloaddata(int id)
        {
            ISM_Certificate_RequestDTO data = new ISM_Certificate_RequestDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return EED.getloaddata(data);
        }

        [Route("savedetails")]
        public ISM_Certificate_RequestDTO savedetails([FromBody]ISM_Certificate_RequestDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return EED.savedetails(data);
        }

        [Route("deactive")]
        public ISM_Certificate_RequestDTO deactive([FromBody] ISM_Certificate_RequestDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return EED.deactive(data);
        }

        [Route("Edit")]
        public ISM_Certificate_RequestDTO Edit([FromBody] ISM_Certificate_RequestDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return EED.Edit(data);
        }
        [Route("Viewdata")]
        public ISM_Certificate_RequestDTO Viewdata([FromBody] ISM_Certificate_RequestDTO data)
        {

            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return EED.Viewdata(data);
        }

        //--------------------- ISM ADMIN Certificate Apply-------------------

        [Route("getadminloaddata/{id:int}")]
        public ISM_Certificate_RequestDTO getadminloaddata(int id)
        {
            ISM_Certificate_RequestDTO data = new ISM_Certificate_RequestDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return EED.getadminloaddata(data);
        }

        [Route("getcertificate")]
        public ISM_Certificate_RequestDTO getcertificate([FromBody] ISM_Certificate_RequestDTO data)
        {

            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return EED.getcertificate(data);
        }

        [Route("saveadmindetails")]
        public ISM_Certificate_RequestDTO saveadmindetails([FromBody]ISM_Certificate_RequestDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return EED.saveadmindetails(data);
        }



        //-----------------------certificate approval--------------
        [Route("loaddata/{id:int}")]
        public ISM_Certificate_RequestDTO loaddata(int id)
        {
            ISM_Certificate_RequestDTO data = new ISM_Certificate_RequestDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return EED.loaddata(data);
        }



        [Route("viewuploadflies")]
        public ISM_Certificate_RequestDTO viewuploadflies([FromBody] ISM_Certificate_RequestDTO d)
        {
            d.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            d.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return EED.viewuploadflies(d);
        }
        [Route("approvedrecord")]
        public ISM_Certificate_RequestDTO approvedrecord([FromBody] ISM_Certificate_RequestDTO data)
        {
            data.roleid = Convert.ToInt64(HttpContext.Session.GetInt32("RoleId"));
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            //data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            // data.Role_flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            return EED.approvedrecord(data);
        }

        //------------------ ISM Certificate Approval Report------------------------------
        [Route("onloaddata/{id:int}")]
        public ISM_Certificate_RequestDTO Onload(int n)
        {
            ISM_Certificate_RequestDTO data = new ISM_Certificate_RequestDTO();
            data.roleid = Convert.ToInt64(HttpContext.Session.GetInt32("RoleId"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return EED.Onload(data);
        }

        [HttpPost]
        [Route("getapprovalreport")]
        public ISM_Certificate_RequestDTO getapprovalreport([FromBody]ISM_Certificate_RequestDTO data)
        {
            data.roleid = Convert.ToInt64(HttpContext.Session.GetInt32("RoleId"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return EED.getapprovalreport(data);
        }


    }
}