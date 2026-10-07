using CommonLibrary;
using PreadmissionDTOs.com.vaps.VMS.TADA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.VMS.TADA
{
    public class TADA_Advance_TourDelagate
    {
       
        CommonDelegate<TADA_Advance_TourDTO, TADA_Advance_TourDTO> COMMI = new CommonDelegate<TADA_Advance_TourDTO, TADA_Advance_TourDTO>();
        public TADA_Advance_TourDTO getalldata(TADA_Advance_TourDTO id)
        {
            return COMMI.POSTIssueManager(id, "TADA_Advance_TourFacade/getalldata/");
        }
    
        public TADA_Advance_TourDTO saveallowence(TADA_Advance_TourDTO dTO)
        {
            return COMMI.POSTIssueManager(dTO, "TADA_Advance_TourFacade/saveallowence");
        }
        //TadaAllwence
        public TADA_Advance_TourDTO TadaAllwence(TADA_Advance_TourDTO dTO)
        {
            return COMMI.POSTIssueManager(dTO, "TADA_Advance_TourFacade/TadaAllwence");
        }
        //deactive
        public TADA_Advance_TourDTO deactive(TADA_Advance_TourDTO dTO)
        {
            return COMMI.POSTIssueManager(dTO, "TADA_Advance_TourFacade/deactive");
        }
        //EditData
        public TADA_Advance_TourDTO EditData(TADA_Advance_TourDTO dTO)
        {
            return COMMI.POSTIssueManager(dTO, "TADA_Advance_TourFacade/EditData");
        }
        public TADA_Advance_TourDTO updateallowence(TADA_Advance_TourDTO dTO)
        {
            return COMMI.POSTIssueManager(dTO, "TADA_Advance_TourFacade/updateallowence");
        }
        //getView
        public TADA_Advance_TourDTO getView(TADA_Advance_TourDTO dTO)
        {
            return COMMI.POSTIssueManager(dTO, "TADA_Advance_TourFacade/getView");
        }
        //CalculateTada
        public TADA_Advance_TourDTO CalculateTada(TADA_Advance_TourDTO dTO)
        {
            return COMMI.POSTIssueManager(dTO, "TADA_Advance_TourFacade/CalculateTada");
        }
        //SaveTada
        public TADA_Advance_TourDTO SaveTada(TADA_Advance_TourDTO dTO)
        {
            return COMMI.POSTIssueManager(dTO, "TADA_Advance_TourFacade/SaveTada");
        }
        //Get_TourList
        public TADA_Advance_TourDTO Get_TourList(TADA_Advance_TourDTO dTO)
        {
            return COMMI.POSTIssueManager(dTO, "TADA_Advance_TourFacade/Get_TourList");
        }
        //TourPlanDates
        public TADA_Advance_TourDTO TourPlanDates(TADA_Advance_TourDTO dTO)
        {
            return COMMI.POSTIssueManager(dTO, "TADA_Advance_TourFacade/TourPlanDates");
        }
        //TourPlanDates
        public TADA_Advance_TourDTO savedata(TADA_Advance_TourDTO dTO)
        {
            return COMMI.POSTIssueManager(dTO, "TADA_Advance_TourFacade/savedata");
        }
        public TADA_Advance_TourDTO getload(TADA_Advance_TourDTO id)
        {
            return COMMI.POSTIssueManager(id, "TADA_Advance_TourFacade/getload/");
        }
        //savedta
        public TADA_Advance_TourDTO savedta(TADA_Advance_TourDTO data)
        {
            return COMMI.POSTIssueManager(data, "TADA_Advance_TourFacade/savedta");
        }
        //Tadadata
        public TADA_Advance_TourDTO Tadadata(TADA_Advance_TourDTO data)
        {
            return COMMI.POSTIssueManager(data, "TADA_Advance_TourFacade/Tadadata");
        }
        //FinalSubMission
        public TADA_Advance_TourDTO FinalSubMission(TADA_Advance_TourDTO data)
        {
            return COMMI.POSTIssueManager(data, "TADA_Advance_TourFacade/FinalSubMission");
        }
        //get_Wallet_load
        public TADA_Advance_TourDTO get_Wallet_load(TADA_Advance_TourDTO data)
        {
            return COMMI.POSTIssueManager(data, "TADA_Advance_TourFacade/get_Wallet_load");
        }
        //get_Wallet_Client
        public TADA_Advance_TourDTO get_Wallet_Client(TADA_Advance_TourDTO data)
        {
            return COMMI.POSTIssueManager(data, "TADA_Advance_TourFacade/get_Wallet_Client");
        }
        //Lead_Update
        public TADA_Advance_TourDTO Lead_Update(TADA_Advance_TourDTO data)
        {
            return COMMI.POSTIssueManager(data, "TADA_Advance_TourFacade/Lead_Update");
        }
        //SaveFinalSubMission
        public TADA_Advance_TourDTO SaveFinalSubMission(TADA_Advance_TourDTO data)
        {
            return COMMI.POSTIssueManager(data, "TADA_Advance_TourFacade/SaveFinalSubMission");
        }
        //Save_response_lead_demo
        public TADA_Advance_TourDTO Save_response_lead_demo(TADA_Advance_TourDTO data)
        {
            return COMMI.POSTIssueManager(data, "TADA_Advance_TourFacade/Save_response_lead_demo");
        }
        //view_lead_demo
        public TADA_Advance_TourDTO view_lead_demo(TADA_Advance_TourDTO data)
        {
            return COMMI.POSTIssueManager(data, "TADA_Advance_TourFacade/view_lead_demo");
        }
        //Edit_response_lead_demo
        public TADA_Advance_TourDTO Edit_response_lead_demo(TADA_Advance_TourDTO data)
        {
            return COMMI.POSTIssueManager(data, "TADA_Advance_TourFacade/Edit_response_lead_demo");
        }
        //LeadComments
        public TADA_Advance_TourDTO LeadComments(TADA_Advance_TourDTO data)
        {
            return COMMI.POSTIssueManager(data, "TADA_Advance_TourFacade/LeadComments");
        }
        public TADA_Advance_TourDTO TravelDetails(TADA_Advance_TourDTO data)
        {
            return COMMI.POSTIssueManager(data, "TADA_Advance_TourFacade/TravelDetails");
        }
        public TADA_Advance_TourDTO Getamount(TADA_Advance_TourDTO data)
        {
            return COMMI.POSTIssueManager(data, "TADA_Advance_TourFacade/Getamount");
        }
        //GetCity
        public TADA_Advance_TourDTO GetCity(TADA_Advance_TourDTO data)
        {
            return COMMI.POSTIssueManager(data, "TADA_Advance_TourFacade/GetCity");
        }
    }
}
