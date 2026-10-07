using System;
using IVRMUX.Delegates.com.vapstech.IssueManager;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.IssueManager;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860
namespace IVRMUX.Controllers.com.vapstech.IssueManager
{
    [Route("api/[controller]")]
    public class ISM_ClientProject_MappingController : Controller
    {
        ISM_ClientProject_MappingDelegate del = new ISM_ClientProject_MappingDelegate();
        [Route("loaddata/{id:int}")]
        public ISM_ClientProject_MappingDTO loaddata(int id)
        {
          ISM_ClientProject_MappingDTO data = new ISM_ClientProject_MappingDTO();
          data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
          data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
          return del.loaddata(data);
        }
        [Route("savedata")]
        public ISM_ClientProject_MappingDTO savedata([FromBody]ISM_ClientProject_MappingDTO data)
        {           
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.savedata(data);
        }

        [Route("getproject")]
        public ISM_ClientProject_MappingDTO getproject([FromBody] ISM_ClientProject_MappingDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.getproject(data);
        }
        [Route("getmodule")]
        public ISM_ClientProject_MappingDTO getmodule([FromBody] ISM_ClientProject_MappingDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.getmodule(data);
        }
        [Route("EditData")]
        public ISM_ClientProject_MappingDTO Editdata([FromBody] ISM_ClientProject_MappingDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.Editdata(data);
        }
        [Route("clientDecative")]
        public ISM_ClientProject_MappingDTO clientDecative([FromBody]ISM_ClientProject_MappingDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.clientDecative(data);
        }
        //savedataLead
        [Route("savedataLead")]
        public ISM_ClientProject_MappingDTO savedataLead([FromBody]ISM_ClientProject_MappingDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.savedataLead(data);
        }

        //added by praveenkumar 
        [Route("SaveApprovalVerify")]
        public ISM_ClientProject_MappingDTO SaveApprovalVerify([FromBody]ISM_ClientProject_MappingDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.SaveApprovalVerify(data);
        }

        //added by praveenkumar 
        [Route("SaveOrderAcceptanceApproval")]
        public ISM_ClientProject_MappingDTO SaveOrderAcceptanceApproval([FromBody]ISM_ClientProject_MappingDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.SaveOrderAcceptanceApproval(data);
        }

        //added by praveenkumarSigned mou view details
        [Route("ViewMOUCheckUpFiles")]
        public ISM_ClientProject_MappingDTO ViewMOUCheckUpFiles([FromBody]ISM_ClientProject_MappingDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.ViewMOUCheckUpFiles(data);
        }

        [Route("viewtemplatedetails")]
        public ISM_ClientProject_MappingDTO viewtemplatedetails([FromBody] ISM_ClientProject_MappingDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            //data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.viewtemplatedetails(data);
        }
        //OnChangeOnChangeBOMList added by praveenkumar 
        [Route("OnChangeOnChangeBOMList")]
        public ISM_ClientProject_MappingDTO OnChangeOnChangeBOMList([FromBody] ISM_ClientProject_MappingDTO dto)
        {
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            //data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            dto.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.OnChangeOnChangeBOMList(dto);
        }
        //
        [Route("sendMail")]
        public ISM_ClientProject_MappingDTO sendMail([FromBody]ISM_ClientProject_MappingDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.sendMail(data);
        }
        [Route("releaseversion")]
        public ISM_ClientProject_MappingDTO releaseversion([FromBody] ISM_ClientProject_MappingDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            //data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.releaseversion(data);
        }
    }
}
