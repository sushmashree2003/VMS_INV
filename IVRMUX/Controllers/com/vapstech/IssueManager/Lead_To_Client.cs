using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.IssueManager;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.IssueManager;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.IssueManager
{
    [Route("api/[controller]")]
    public class Lead_To_Client : Controller
    {
        public Lead_To_ClientDelegate _objdel = new Lead_To_ClientDelegate();

        [Route("getdetails/{id:int}")]
        public Lead_To_ClientDTO getdetails(int id)
        {
            Lead_To_ClientDTO data = new Lead_To_ClientDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _objdel.getdetails(data);
        }

        [Route("OnChangeTab1Inst")]
        public Lead_To_ClientDTO OnChangeTab1Inst([FromBody]Lead_To_ClientDTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _objdel.OnChangeTab1Inst(data);
        }

        [Route("saveClientdata")]
        public Lead_To_ClientDTO saveClientdata([FromBody]Lead_To_ClientDTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _objdel.saveClientdata(data);
        }

        [Route("clientDecative")]
        public Lead_To_ClientDTO clientDecative([FromBody]Lead_To_ClientDTO data)
        {
            return _objdel.clientDecative(data);
        }

        [Route("editClientdata")]
        public Lead_To_ClientDTO editClientdata([FromBody]Lead_To_ClientDTO data)
        {
            return _objdel.editClientdata(data);
        }

        [Route("OnChangeTab2Inst")]
        public Lead_To_ClientDTO OnChangeTab2Inst([FromBody]Lead_To_ClientDTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _objdel.OnChangeTab2Inst(data);
        }

        [Route("saveClientMappingdata")]
        public Lead_To_ClientDTO saveClientMappingdata([FromBody]Lead_To_ClientDTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _objdel.saveClientMappingdata(data);
        }

        [Route("editClientMappingdata")]
        public Lead_To_ClientDTO editClientMappingdata([FromBody]Lead_To_ClientDTO data)
        {
            return _objdel.editClientMappingdata(data);
        }

        [Route("deactiveClientMappingdata")]
        public Lead_To_ClientDTO deactiveClientMappingdata([FromBody]Lead_To_ClientDTO data)
        {
            return _objdel.deactiveClientMappingdata(data);
        }

        [Route("modalListdata")]
        public Lead_To_ClientDTO modalListdata([FromBody]Lead_To_ClientDTO data)
        {
            return _objdel.modalListdata(data);
        }

        //VMS Client And IVRM Client Mapping 
        [Route("OnChangeClientTab3")]
        public Lead_To_ClientDTO OnChangeClientTab3([FromBody]Lead_To_ClientDTO data)
        {
            return _objdel.OnChangeClientTab3(data);
        }

        [Route("SaveVMSIVRMMapping")]
        public Lead_To_ClientDTO SaveVMSIVRMMapping([FromBody]Lead_To_ClientDTO data)
        {
            return _objdel.SaveVMSIVRMMapping(data);
        }
    }
}
