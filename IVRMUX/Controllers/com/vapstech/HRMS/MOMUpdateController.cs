using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.HRMS;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.HRMS;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.HRMS
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class MOMUpdateController : Controller
    {
        MOMUpdateDelegate delobj = new MOMUpdateDelegate();


        /////======================================================////////////////////transaction/////////////////////====================//////////////////////////////

        [Route("oldmeetingdetail")]
        public MOMUpdateDTO oldmeetingdetail([FromBody]MOMUpdateDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            data.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return delobj.oldmeetingdetail(data);
        }

        [Route("getonloaddata/{id:int}")]
        public MOMUpdateDTO getonloaddata(int id)
        {
            MOMUpdateDTO dto = new MOMUpdateDTO();
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return delobj.getonloaddata(dto);
        }
        [Route("savetransaction")]
        public MOMUpdateDTO savetransaction([FromBody]MOMUpdateDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return delobj.savetransaction(data);
        }
        [Route("viewflies")]
        public MOMUpdateDTO viewflies([FromBody]MOMUpdateDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return delobj.viewflies(data);
        }

        [HttpPost]
        [Route("deactivate")]
        public MOMUpdateDTO deactivate([FromBody] MOMUpdateDTO data)
        {

            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return delobj.deactivate(data);
        }

        [HttpPost]
        [Route("Edittrans")]
        public MOMUpdateDTO Edittrans([FromBody] MOMUpdateDTO data)
        {

            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return delobj.Edittrans(data);
        }


        //=====================update==============================/////////////
        [Route("getDetails/{id:int}")]
        public MOMUpdateDTO getDetails(int id)
        {
            MOMUpdateDTO dto = new MOMUpdateDTO();
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return delobj.getDetails(dto);
        }
        [Route("UpdateDetails")]
        public MOMUpdateDTO UpdateDetails([FromBody] MOMUpdateDTO dto)
        {
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return delobj.UpdateDetails(dto);
        }
        [Route("Edit")]
        public MOMUpdateDTO Edit([FromBody] MOMUpdateDTO dto)
        {
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return delobj.Edit(dto);
        }
        [Route("viewuploadflies")]
        public MOMUpdateDTO viewuploadflies([FromBody]MOMUpdateDTO data)
        {
            data.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return delobj.viewuploadflies(data);
        }
        [Route("saveData")]
        public MOMUpdateDTO saveData([FromBody]MOMUpdateDTO data)
        {
           // data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return delobj.saveData(data);
        }
        [Route("checkvalidation")]
        public MOMUpdateDTO checkvalidation([FromBody] MOMUpdateDTO data)
        {            
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return delobj.checkvalidation(data);
        }

        //[Route("save_excel_data")]
        //public MOMUpdateDTO save_excel_data([FromBody] MOMUpdateDTO data)
        //{
        //    data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
        //    return delobj.save_excel_data(data);
        //}
        //=======================================Approval///////////////////////////////
        [Route("getdata/{id:int}")]
        public MOMUpdateDTO getdata(int id)
        {
            MOMUpdateDTO dto = new MOMUpdateDTO();
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return delobj.getdata(dto);
        }

        [Route("approvaldata")]
        public MOMUpdateDTO approvaldata([FromBody]MOMUpdateDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            data.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return delobj.approvaldata(data);
        }
        [Route("getstatus")]
        public MOMUpdateDTO getstatus([FromBody]MOMUpdateDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            data.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return delobj.getstatus(data);
        }
        [Route("oldmeeting")]
        public MOMUpdateDTO oldmeeting([FromBody]MOMUpdateDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            data.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return delobj.oldmeeting(data);
        }


        [Route("sendapprovalMail")]
        public MOMUpdateDTO sendapprovalMail([FromBody] MOMUpdateDTO dto)
        {
            dto.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return delobj.sendapprovalMail(dto);
        }
        //==========================================report===================================////////////////////////////////

        [Route("getreport/{id:int}")]
        public MOMUpdateDTO getreport(int id)
        {
            MOMUpdateDTO dto = new MOMUpdateDTO();
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return delobj.getreport(dto);
        }
        [HttpPost]
        [Route("showreport")]
        public MOMUpdateDTO showreport([FromBody]MOMUpdateDTO data)
        {

            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            data.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return delobj.showreport(data);
        }
        [Route("viewdetails")]
        public MOMUpdateDTO viewdetails([FromBody]MOMUpdateDTO data)
        {

            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            data.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return delobj.viewdetails(data);
        }

        [Route("onloadVendors/{id:int}")]
        public MOMUpdateDTO onloadVendors(int id)
        {
            MOMUpdateDTO dto = new MOMUpdateDTO();
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return delobj.onloadVendors(dto);
        }


        [Route("SaveVendors")]
        public MOMUpdateDTO SaveVendors([FromBody]MOMUpdateDTO data)
        {

            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            data.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return delobj.SaveVendors(data);
        }

    }
}
