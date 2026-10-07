using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IssueManager.com.TADA.Interface;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.VMS.TADA;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IssueManager.com.TADA.Controllers
{
    [Route("api/[controller]")]
    public class TADA_Advance_TourFacade : Controller
    {

        public TADA_Advance_TourInterface _objinter;

        public TADA_Advance_TourFacade(TADA_Advance_TourInterface parameter)
        {
            _objinter = parameter;
        }
        [Route("getalldata")]
        public TADA_Advance_TourDTO getalldata([FromBody]TADA_Advance_TourDTO data)
        {
            return _objinter.getalldata(data);
        }
        [Route("saveallowence")]
        public TADA_Advance_TourDTO saveallowence([FromBody]TADA_Advance_TourDTO data)
        {
            return _objinter.saveallowence(data);
        }
        //TadaAllwence
        [Route("TadaAllwence")]
        public TADA_Advance_TourDTO TadaAllwence([FromBody]TADA_Advance_TourDTO data)
        {
            return _objinter.TadaAllwence(data);
        }
        [Route("deactive")]
        public TADA_Advance_TourDTO deactive([FromBody]TADA_Advance_TourDTO data)
        {
            return _objinter.deactive(data);
        }
        //EditData
        [Route("EditData")]
        public TADA_Advance_TourDTO EditData([FromBody]TADA_Advance_TourDTO data)
        {
            return _objinter.EditData(data);
        }
        //updateallowence
        [Route("updateallowence")]
        public TADA_Advance_TourDTO updateallowence([FromBody]TADA_Advance_TourDTO data)
        {
            return _objinter.updateallowence(data);
        }
        //getView
        [Route("getView")]
        public TADA_Advance_TourDTO getView([FromBody]TADA_Advance_TourDTO data)
        {
            return _objinter.getView(data);
        }
        //CalculateTada
        [Route("CalculateTada")]
        public TADA_Advance_TourDTO CalculateTada([FromBody]TADA_Advance_TourDTO data)
        {
            return _objinter.CalculateTada(data);
        }
        //SaveTada
        [Route("SaveTada")]
        public TADA_Advance_TourDTO SaveTada([FromBody]TADA_Advance_TourDTO data)
        {
            return _objinter.SaveTada(data);
        }
        //Get_TourList
        [Route("Get_TourList")]
        public TADA_Advance_TourDTO Get_TourList([FromBody]TADA_Advance_TourDTO data)
        {
            return _objinter.Get_TourList(data);
        }
        //TourPlanDates
        [Route("TourPlanDates")]
        public TADA_Advance_TourDTO TourPlanDates([FromBody]TADA_Advance_TourDTO data)
        {
            return _objinter.TourPlanDates(data);
        }
        [Route("getload")]
        public TADA_Advance_TourDTO getload([FromBody]TADA_Advance_TourDTO data)
        {
            return _objinter.getload(data);
        }
        [Route("savedata")]
        public TADA_Advance_TourDTO savedata([FromBody]TADA_Advance_TourDTO data)
        {
            return _objinter.savedata(data);
        }
        //Getdata
        [Route("savedta")]
        public TADA_Advance_TourDTO savedta([FromBody]TADA_Advance_TourDTO data)
        {
            return _objinter.savedta(data);
        }
        //Tadadata
        [Route("Tadadata")]
        public TADA_Advance_TourDTO Tadadata([FromBody]TADA_Advance_TourDTO data)
        {
            return _objinter.Tadadata(data);
        }
        //FinalSubMission
        [Route("FinalSubMission")]
        public TADA_Advance_TourDTO FinalSubMission([FromBody]TADA_Advance_TourDTO data)
        {
            return _objinter.FinalSubMission(data);
        }
        //get_Wallet_load
        [Route("get_Wallet_load")]
        public TADA_Advance_TourDTO get_Wallet_load([FromBody]TADA_Advance_TourDTO data)
        {
            return _objinter.get_Wallet_load(data);
        }
        //get_Wallet_Client
        [Route("get_Wallet_Client")]
        public TADA_Advance_TourDTO get_Wallet_Client([FromBody]TADA_Advance_TourDTO data)
        {
            return _objinter.get_Wallet_Client(data);
        }
        //Lead_Update
        [Route("Lead_Update")]
        public TADA_Advance_TourDTO Lead_Update([FromBody]TADA_Advance_TourDTO data)
        {
            return _objinter.Lead_Update(data);
        }
        //SaveFinalSubMission
        [Route("SaveFinalSubMission")]
        public TADA_Advance_TourDTO SaveFinalSubMission([FromBody]TADA_Advance_TourDTO data)
        {
            return _objinter.SaveFinalSubMission(data);
        }
        //Save_response_lead_demo
        [Route("Save_response_lead_demo")]
        public TADA_Advance_TourDTO Save_response_lead_demo([FromBody]TADA_Advance_TourDTO data)
        {
            return _objinter.Save_response_lead_demo(data);
        }
        //view_lead_demo
        [Route("view_lead_demo")]
        public TADA_Advance_TourDTO view_lead_demo([FromBody]TADA_Advance_TourDTO data)
        {
            return _objinter.view_lead_demo(data);
        }
        //Edit_response_lead_demo
        [Route("Edit_response_lead_demo")]
        public TADA_Advance_TourDTO Edit_response_lead_demo([FromBody]TADA_Advance_TourDTO data)
        {
            return _objinter.Edit_response_lead_demo(data);
        }
        //LeadComments
        [Route("LeadComments")]
        public TADA_Advance_TourDTO LeadComments([FromBody]TADA_Advance_TourDTO data)
        {
            return _objinter.LeadComments(data);
        }
        [Route("TravelDetails")]
        public TADA_Advance_TourDTO TravelDetails([FromBody]TADA_Advance_TourDTO data)
        {
            return _objinter.TravelDetails(data);
        }
        [Route("Getamount")]
        public TADA_Advance_TourDTO Getamount([FromBody]TADA_Advance_TourDTO data)
        {
            return _objinter.Getamount(data);
        }
        //GetCity
        [Route("GetCity")]
        public TADA_Advance_TourDTO GetCity([FromBody]TADA_Advance_TourDTO data)
        {
            return _objinter.GetCity(data);
        }
    }
}
