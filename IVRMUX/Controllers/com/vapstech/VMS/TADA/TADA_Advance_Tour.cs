using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.VMS.TADA;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.VMS.TADA;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.VMS.TADA
{
    [Route("api/[controller]")]
    public class TADA_Advance_Tour : Controller
    {
        TADA_Advance_TourDelagate _delegate = new TADA_Advance_TourDelagate();
        [HttpGet]
        [Route("getalldata/{id:int}")]
        public TADA_Advance_TourDTO getalldata(int id)
        {
            TADA_Advance_TourDTO dto = new TADA_Advance_TourDTO();
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delegate.getalldata(dto);


        }

        [HttpPost]
        [Route("saveallowence")]
        public TADA_Advance_TourDTO saveallowence([FromBody]TADA_Advance_TourDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delegate.saveallowence(data);
        }
        //TadaAllwence
        [Route("TadaAllwence")]
        public TADA_Advance_TourDTO TadaAllwence([FromBody]TADA_Advance_TourDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delegate.TadaAllwence(data);
        }
        //deactive
        [Route("deactive")]
        public TADA_Advance_TourDTO deactive([FromBody]TADA_Advance_TourDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delegate.deactive(data);
        }
        //EditData
        [Route("EditData")]
        public TADA_Advance_TourDTO EditData([FromBody]TADA_Advance_TourDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delegate.EditData(data);
        }
        //updateallowence
        [Route("updateallowence")]
        public TADA_Advance_TourDTO updateallowence([FromBody]TADA_Advance_TourDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delegate.updateallowence(data);
        }
        //getView
        [Route("getView")]
        public TADA_Advance_TourDTO getView([FromBody]TADA_Advance_TourDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delegate.getView(data);
        }
        //CalculateTada
        [Route("CalculateTada")]
        public TADA_Advance_TourDTO CalculateTada([FromBody]TADA_Advance_TourDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delegate.CalculateTada(data);
        }
        //SaveTada
        [Route("SaveTada")]
        public TADA_Advance_TourDTO SaveTada([FromBody]TADA_Advance_TourDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delegate.SaveTada(data);
        }
        //Get_TourList
        [Route("Get_TourList")]
        public TADA_Advance_TourDTO Get_TourList([FromBody]TADA_Advance_TourDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delegate.Get_TourList(data);
        }
        //TourPlanDates
        [Route("TourPlanDates")]
        public TADA_Advance_TourDTO TourPlanDates([FromBody]TADA_Advance_TourDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delegate.TourPlanDates(data);
        }

        //SaveTada
        [Route("TADA_Advance_Tour_Lead_save")]
        public TADA_Advance_TourDTO savedata([FromBody]TADA_Advance_TourDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delegate.savedata(data);
        }
        [Route("get_ier_load/{id:int}")]
        public TADA_Advance_TourDTO getload(int id)
        {
            TADA_Advance_TourDTO dto = new TADA_Advance_TourDTO();
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delegate.getload(dto);

        }
        //get_ier_load
        [Route("get_Wallet_load/{id:int}")]
        public TADA_Advance_TourDTO get_Wallet_load(int id)
        {
            TADA_Advance_TourDTO dto = new TADA_Advance_TourDTO();
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delegate.get_Wallet_load(dto);

        }
        //savedta
        [HttpPost]
        [Route("savedta")]
        public TADA_Advance_TourDTO savedta([FromBody]TADA_Advance_TourDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delegate.savedta(data);
        }
        //Tadadata
        [Route("Tadadata")]
        public TADA_Advance_TourDTO Tadadata([FromBody]TADA_Advance_TourDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delegate.Tadadata(data);
        }
        //FinalSubMission
        [Route("FinalSubMission")]
        public TADA_Advance_TourDTO FinalSubMission([FromBody]TADA_Advance_TourDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delegate.FinalSubMission(data);
        }
        //SaveFinalSubMission
        [Route("SaveFinalSubMission")]
        public TADA_Advance_TourDTO SaveFinalSubMission([FromBody]TADA_Advance_TourDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delegate.SaveFinalSubMission(data);
        }
        //get_Wallet_Client
        [Route("get_Wallet_Client")]
        public TADA_Advance_TourDTO get_Wallet_Client([FromBody]TADA_Advance_TourDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delegate.get_Wallet_Client(data);
        }
        //Lead_Update
        [Route("Lead_Update")]
        public TADA_Advance_TourDTO Lead_Update([FromBody]TADA_Advance_TourDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delegate.Lead_Update(data);
        }
        //Save_response_lead_demo
        [Route("Save_response_lead_demo")]
        public TADA_Advance_TourDTO Save_response_lead_demo([FromBody]TADA_Advance_TourDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delegate.Save_response_lead_demo(data);
        }
        //view_lead_demo
        [Route("view_lead_demo")]
        public TADA_Advance_TourDTO view_lead_demo([FromBody]TADA_Advance_TourDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delegate.view_lead_demo(data);
        }
        //Edit_response_lead_demo
        [Route("Edit_response_lead_demo")]
        public TADA_Advance_TourDTO Edit_response_lead_demo([FromBody]TADA_Advance_TourDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delegate.Edit_response_lead_demo(data);
        }
        //LeadComments
        [Route("LeadComments")]
        public TADA_Advance_TourDTO LeadComments([FromBody]TADA_Advance_TourDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delegate.LeadComments(data);
        }
        [Route("TravelDetails")]
        public TADA_Advance_TourDTO TravelDetails([FromBody]TADA_Advance_TourDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delegate.TravelDetails(data);
        }
        [Route("Getamount")]
        public TADA_Advance_TourDTO Getamount([FromBody]TADA_Advance_TourDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delegate.Getamount(data);
        }
        //GetCity
        [Route("GetCity")]
        public TADA_Advance_TourDTO GetCity([FromBody]TADA_Advance_TourDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delegate.GetCity(data);
        }
    }
}
