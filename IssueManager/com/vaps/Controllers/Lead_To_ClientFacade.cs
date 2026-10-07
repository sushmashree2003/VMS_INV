using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IssueManager.com.vaps.Interfaces;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.IssueManager;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IssueManager.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class Lead_To_ClientFacade : Controller
    {

        public Lead_To_Clientinterface _objinter;

        public Lead_To_ClientFacade(Lead_To_Clientinterface parameter)
        {
            _objinter = parameter;
        }

        [Route("getdetails")]
        public Lead_To_ClientDTO getdetails([FromBody] Lead_To_ClientDTO data)
        {
            return _objinter.getdetails(data);
        }

        [Route("OnChangeTab1Inst")]
        public Lead_To_ClientDTO OnChangeTab1Inst([FromBody]Lead_To_ClientDTO data)
        {
            return _objinter.OnChangeTab1Inst(data);
        }

        [Route("saveClientdata")]
        public Lead_To_ClientDTO saveClientdata([FromBody]Lead_To_ClientDTO data)
        {
            return _objinter.saveClientdata(data);
        }

        [Route("clientDecative")]
        public Lead_To_ClientDTO clientDecative([FromBody]Lead_To_ClientDTO data)
        {
            return _objinter.clientDecative(data);
        }

        [Route("editClientdata")]
        public Lead_To_ClientDTO editClientdata([FromBody]Lead_To_ClientDTO data)
        {
            return _objinter.editClientdata(data);
        }

        [Route("OnChangeTab2Inst")]
        public Lead_To_ClientDTO OnChangeTab2Inst([FromBody]Lead_To_ClientDTO data)
        {
            return _objinter.OnChangeTab2Inst(data);
        }

        [Route("saveClientMappingdata")]
        public Lead_To_ClientDTO saveClientMappingdata([FromBody]Lead_To_ClientDTO data)
        {
            return _objinter.saveClientMappingdata(data);
        }

        [Route("editClientMappingdata")]
        public Lead_To_ClientDTO editClientMappingdata([FromBody]Lead_To_ClientDTO data)
        {
            return _objinter.editClientMappingdata(data);
        }

        [Route("deactiveClientMappingdata")]
        public Lead_To_ClientDTO deactiveClientMappingdata([FromBody]Lead_To_ClientDTO data)
        {
            return _objinter.deactiveClientMappingdata(data);
        }

        [Route("modalListdata")]
        public Lead_To_ClientDTO modalListdata([FromBody]Lead_To_ClientDTO data)
        {
            return _objinter.modalListdata(data);
        }

        //VMS Client And IVRM Client Mapping 
        [Route("OnChangeClientTab3")]
        public Lead_To_ClientDTO OnChangeClientTab3([FromBody]Lead_To_ClientDTO data)
        {
            return _objinter.OnChangeClientTab3(data);
        }

        [Route("SaveVMSIVRMMapping")]
        public Lead_To_ClientDTO SaveVMSIVRMMapping([FromBody]Lead_To_ClientDTO data)
        {
            return _objinter.SaveVMSIVRMMapping(data);
        }
    }
}
