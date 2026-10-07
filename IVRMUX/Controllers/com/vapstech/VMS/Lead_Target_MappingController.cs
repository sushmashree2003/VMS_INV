using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.VMS;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.VMS.HRMS;
using PreadmissionDTOs.com.vaps.VMS.Sales;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.VMS
{
    [Route("api/[controller]")]
    public class Lead_Target_MappingController : Controller
    {
        Lead_Target_MappingDelegate LTM = new Lead_Target_MappingDelegate();
        //GET: api/<controller>



        [Route("OnloadGetDetails/{id:int}")]
        public Lead_Target_MappingDTO OnloadGetDetails(int id)
        {

            Lead_Target_MappingDTO dto = new Lead_Target_MappingDTO();
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return LTM.onloadgetdetails(dto);

        
        }
        [Route("OnloadGetDetailsnego/{id:int}")]
        public Lead_Target_MappingDTO OnloadGetDetailsnego(int id)
        {

            Lead_Target_MappingDTO dto = new Lead_Target_MappingDTO();
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return LTM.OnloadGetDetailsnego(dto);


        }



        //[HttpPost]
        [Route("OnChangeOnChangeleadlist")]
        public Lead_Target_MappingDTO OnChangeOnChangeleadlist([FromBody]Lead_Target_MappingDTO dto)
        {
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return LTM.OnChangeOnChangeleadlist(dto);
        }

        [Route("OnChangeOnChangeproductlist")]
        public Lead_Target_MappingDTO OnChangeOnChangeproductlist([FromBody]Lead_Target_MappingDTO dto)
        {
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return LTM.OnChangeOnChangeproductlist(dto);
        }


        [Route("OnChangeOnChangeBOMList")]
        public Lead_Target_MappingDTO OnChangeOnChangeBOMList([FromBody]Lead_Target_MappingDTO dto)
        {
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return LTM.OnChangeOnChangeBOMList(dto);
        }

        [Route("Saveboomlistpackage")]
        public Lead_Target_MappingDTO Saveboomlistpackage([FromBody]Lead_Target_MappingDTO dto)
        {
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return LTM.Saveboomlistpackage(dto);
        }
        [Route("sendmailDetails")]
        public Lead_Target_MappingDTO sendmailDetails([FromBody]Lead_Target_MappingDTO dto)
        {
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return LTM.sendmailDetails(dto);
        }

        [Route("savedetails")]
        public Lead_Target_MappingDTO savedetails([FromBody]Lead_Target_MappingDTO dto)
        {
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return LTM.savedetails(dto);
        }

        [Route("get_quotation")]
        public Lead_Target_MappingDTO get_quotation([FromBody]Lead_Target_MappingDTO dto)
        {
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return LTM.get_quotation(dto);
        }

        [Route("saveNegotiationdetails")]
        public Lead_Target_MappingDTO saveNegotiationdetails([FromBody]Lead_Target_MappingDTO dto)
        {
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return LTM.saveNegotiationdetails(dto);
        }


        [Route("LeadTargetMapping_Edit")]
        public Lead_Target_MappingDTO LeadTargetMapping_Edit([FromBody]Lead_Target_MappingDTO dto)
        {
            //dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return LTM.LeadTargetMapping_Edit(dto);
        }

        [Route("OnChangeLTM_Institution")]
        public Lead_Target_MappingDTO OnChangeLTM_Institution([FromBody]Lead_Target_MappingDTO dto)
        {
            //dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return LTM.OnChangeLTM_Institution(dto);
        }

        [Route("sendProDetails")]
        public Lead_Target_MappingDTO sendProDetails([FromBody]Lead_Target_MappingDTO dto)
        {
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return LTM.sendProDetails(dto);
        }

        [Route("deactivate")]
        public Lead_Target_MappingDTO deactivate([FromBody]Lead_Target_MappingDTO data)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;
            return LTM.deactivate(data);
        }
        //SendMdApproval
        [Route("SendMdApproval")]
        public Lead_Target_MappingDTO SendMdApproval([FromBody]Lead_Target_MappingDTO data)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;
            data.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return LTM.SendMdApproval(data);
        }
        [Route("MdApproval")]
        public Lead_Target_MappingDTO MdApproval([FromBody]Lead_Target_MappingDTO data)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;
            data.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return LTM.MdApproval(data);
        }
        [Route("getdetails/{id:int}")]
        public Lead_Target_MappingDTO getdetail(int id)
        {
            return LTM.categoryDet(id);
        }

        [Route("SaveScienceParkTransaction")]
        public Lead_Target_MappingDTO SaveScienceParkTransaction([FromBody]Lead_Target_MappingDTO data)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;
            data.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return LTM.SaveScienceParkTransaction(data);
        }
        
        [Route("SaveISMSalesMultipleTransaction")]
        public Lead_Target_MappingDTO SaveISMSalesMultipleTransaction([FromBody]Lead_Target_MappingDTO data)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;
            data.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return LTM.SaveISMSalesMultipleTransaction(data);
        }
    }
}
