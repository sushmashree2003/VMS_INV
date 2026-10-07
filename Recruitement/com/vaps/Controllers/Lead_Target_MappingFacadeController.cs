using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.VMS.HRMS;
using Recruitment.com.vaps.Interfaces;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Recruitment.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class Lead_Target_MappingFacadeController : Controller
    {

        public Lead_Target_MappingInterface _ads;

        public Lead_Target_MappingFacadeController(Lead_Target_MappingInterface adstu)
        {
            _ads = adstu;
        }


        [Route("OnloadGetDetails")]
        public Lead_Target_MappingDTO OnloadGetDetails([FromBody] Lead_Target_MappingDTO dto)
        {
            return _ads.getBasicData(dto);
        }

        [Route("OnloadGetDetailsnego")]
        public Lead_Target_MappingDTO OnloadGetDetailsnego([FromBody] Lead_Target_MappingDTO dto)
        {
            return _ads.OnloadGetDetailsnego(dto);
        }

        [HttpPost]
        [Route("OnChangeOnChangeleadlist")]
        public Lead_Target_MappingDTO OnChangeOnChangeleadlist([FromBody]Lead_Target_MappingDTO dto)
        {
            // dto.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _ads.OnChangeOnChangeleadlist(dto);
        }

        [HttpPost]
        [Route("OnChangeOnChangeproductlist")]
        public Lead_Target_MappingDTO OnChangeOnChangeproductlist([FromBody]Lead_Target_MappingDTO dto)
        {
            // dto.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _ads.OnChangeOnChangeproductlist(dto);
        }

        [HttpPost]
        [Route("OnChangeOnChangeBOMList")]
        public Lead_Target_MappingDTO OnChangeOnChangeBOMList([FromBody]Lead_Target_MappingDTO dto)
        {
            // dto.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _ads.OnChangeOnChangeBOMList(dto);
        }
        [HttpPost]
        [Route("Saveboomlistpackage")]
        public Lead_Target_MappingDTO Saveboomlistpackage([FromBody]Lead_Target_MappingDTO dto)
        {
            // dto.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _ads.Saveboomlistpackage(dto);
        }
        [HttpPost]
        [Route("sendmailDetails")]
        public async Task<Lead_Target_MappingDTO> sendmailDetails([FromBody]Lead_Target_MappingDTO data)
        {
            return await _ads.sendmailDetails(data);
        }


        [Route("leadTargetmapping_Edit")]
        public Lead_Target_MappingDTO leadTargetmapping_Edit([FromBody]Lead_Target_MappingDTO dto)
        {
            return _ads.Edit_LeadTargetMapping(dto);
        }


        [Route("savedetails")]
        public Lead_Target_MappingDTO savedetails([FromBody]Lead_Target_MappingDTO dto)
        {
            return _ads.savedetails(dto);
        }

        [Route("get_quotation")]
        public Lead_Target_MappingDTO get_quotation([FromBody]Lead_Target_MappingDTO dto)
        {
            return _ads.get_quotation(dto);
        }

        [Route("saveNegotiationdetails")]
        public Lead_Target_MappingDTO saveNegotiationdetails([FromBody]Lead_Target_MappingDTO dto)
        {
            return _ads.saveNegotiationdetails(dto);
        }




        [Route("OnChangeLTM_Institution")]
        public Lead_Target_MappingDTO OnChangeLTM_Institution([FromBody]Lead_Target_MappingDTO dto)
        {
            return _ads.OnChangeLTM_Institution(dto);
        }

        [HttpPost]
        [Route("sendProDetails")]
        public async Task<Lead_Target_MappingDTO> sendProDetails([FromBody]Lead_Target_MappingDTO data)
        {
            return await _ads.sendProDetails(data);
        }

        [Route("deactivate")]
        public Lead_Target_MappingDTO deactivate([FromBody]Lead_Target_MappingDTO data)
        {
            return _ads.deactivate(data);
        }
        //SendMdApproval
        [Route("SendMdApproval")]
        public async Task<Lead_Target_MappingDTO> SendMdApproval([FromBody]Lead_Target_MappingDTO data)
        {
            return await _ads.SendMdApproval(data);
        }
        [Route("MdApproval")]
        public async Task<Lead_Target_MappingDTO> MdApproval([FromBody]Lead_Target_MappingDTO data)
        {
            return await _ads.MdApproval(data);
        }
        [Route("getdetails/{id:int}")]
        public Lead_Target_MappingDTO getcatgrydet(int id)
        {
            return _ads.getdetails(id);
        }
        [Route("SaveScienceParkTransaction")]
        public async Task<Lead_Target_MappingDTO> SaveScienceParkTransaction([FromBody]Lead_Target_MappingDTO data)
        {
            return await _ads.SaveScienceParkTransaction(data);
        }


        [Route("SaveISMSalesMultipleTransaction")]
        public async Task<Lead_Target_MappingDTO> SaveISMSalesMultipleTransaction([FromBody]Lead_Target_MappingDTO data)
        {
            return await _ads.SaveISMSalesMultipleTransaction(data);
        }
        
    }
}
