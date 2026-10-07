using PreadmissionDTOs.com.vaps.VMS.TADA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IssueManager.com.TADA.Interface
{
   public interface TADA_Advance_TourInterface
    {
        TADA_Advance_TourDTO getalldata(TADA_Advance_TourDTO data);

        TADA_Advance_TourDTO saveallowence(TADA_Advance_TourDTO data);
        //TadaAllwence
        TADA_Advance_TourDTO TadaAllwence(TADA_Advance_TourDTO data);
        //deactive
        TADA_Advance_TourDTO deactive(TADA_Advance_TourDTO data);
        //EditData
        TADA_Advance_TourDTO EditData(TADA_Advance_TourDTO data);
        //updateallowence
        TADA_Advance_TourDTO updateallowence(TADA_Advance_TourDTO data);
        //getView
        TADA_Advance_TourDTO getView(TADA_Advance_TourDTO data);
        //CalculateTada
        TADA_Advance_TourDTO CalculateTada(TADA_Advance_TourDTO data);
        //SaveTada
        TADA_Advance_TourDTO SaveTada(TADA_Advance_TourDTO data);
        //Get_TourList
        TADA_Advance_TourDTO Get_TourList(TADA_Advance_TourDTO data);
        //TourPlanDates
        TADA_Advance_TourDTO TourPlanDates(TADA_Advance_TourDTO data);
        //getload
        TADA_Advance_TourDTO getload(TADA_Advance_TourDTO data);
        //savedata
        TADA_Advance_TourDTO savedata(TADA_Advance_TourDTO data);
        TADA_Advance_TourDTO savedta(TADA_Advance_TourDTO data);
        //Tadadata
        TADA_Advance_TourDTO Tadadata(TADA_Advance_TourDTO data);
        //FinalSubMission
        TADA_Advance_TourDTO FinalSubMission(TADA_Advance_TourDTO data);
        //get_Wallet_load
        TADA_Advance_TourDTO get_Wallet_load(TADA_Advance_TourDTO data);
        //get_Wallet_Client
        TADA_Advance_TourDTO get_Wallet_Client(TADA_Advance_TourDTO data);
        //Lead_Update
        TADA_Advance_TourDTO Lead_Update(TADA_Advance_TourDTO data);
        //SaveFinalSubMission
        TADA_Advance_TourDTO SaveFinalSubMission(TADA_Advance_TourDTO data);
        //Save_response_lead_demo
        TADA_Advance_TourDTO Save_response_lead_demo(TADA_Advance_TourDTO data);
        //view_lead_demo
        TADA_Advance_TourDTO view_lead_demo(TADA_Advance_TourDTO data);
        //Edit_response_lead_demo
        TADA_Advance_TourDTO Edit_response_lead_demo(TADA_Advance_TourDTO data);
        //LeadComments
        TADA_Advance_TourDTO LeadComments(TADA_Advance_TourDTO data);
        TADA_Advance_TourDTO TravelDetails(TADA_Advance_TourDTO data);
        TADA_Advance_TourDTO Getamount(TADA_Advance_TourDTO data);

        //GetCity
        TADA_Advance_TourDTO GetCity(TADA_Advance_TourDTO data);
    }
}
