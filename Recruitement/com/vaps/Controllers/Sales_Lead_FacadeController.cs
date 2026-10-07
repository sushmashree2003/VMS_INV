using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.VMS.Sales;
using Recruitment.com.vaps.Interfaces;

namespace Recruitment.com.vaps.Controllers
{
    [Produces("application/json")]
    [Route("api/Sales_Lead_Facade")]
    public class Sales_Lead_FacadeController : Controller
    {
        public Sales_Lead_Master_Interface _sali;


        public Sales_Lead_FacadeController(Sales_Lead_Master_Interface lm)
        {
            _sali = lm;
        }
        //----------------------------category-----------------------------

        [Route("get_load_Cat/{id:int}")]
        public ISM_Sales_Master_Category_DTO get_load_Cat(int id)
        {
            return _sali.get_load_Cat(id);
        }

        [Route("SaveEdit_Cat")]
        public ISM_Sales_Master_Category_DTO SaveEdit_Cat([FromBody] ISM_Sales_Master_Category_DTO dto)
        {
            return _sali.SaveEdit_Cat(dto);
        }

        [Route("Edit_details_Cat")]
        public ISM_Sales_Master_Category_DTO Edit_details_Cat([FromBody] ISM_Sales_Master_Category_DTO dto)
        {
            return _sali.Edit_details_Cat(dto);
        }

        [Route("OnChangeCategoryInstitution")]
        public ISM_Sales_Master_Category_DTO OnChangeCategoryInstitution([FromBody] ISM_Sales_Master_Category_DTO dto)
        {
            return _sali.OnChangeCategoryInstitution(dto);
        }

        [HttpPost]
        [Route("deactivate_Cat")]
        public ISM_Sales_Master_Category_DTO deactivate_Cat([FromBody] ISM_Sales_Master_Category_DTO dTO)
        {
            return _sali.deactivate_Cat(dTO);
        }

        //----------------------------Product-----------------------------

        [Route("get_load_pro/{id:int}")]
        public ISM_Sales_Master_Product_DTO get_load_pro(int id)
        {
            return _sali.get_load_pro(id);
        }
        [Route("SaveEdit_pro")]
        public ISM_Sales_Master_Product_DTO SaveEdit_pro([FromBody] ISM_Sales_Master_Product_DTO dto)
        {
            return _sali.SaveEdit_pro(dto);
        }

        [Route("Edit_details_pro")]
        public ISM_Sales_Master_Product_DTO Edit_details_pro([FromBody] ISM_Sales_Master_Product_DTO dto)
        {
            return _sali.Edit_details_pro(dto);
        }

        [Route("OnChangeProductInstitution")]
        public ISM_Sales_Master_Product_DTO OnChangeProductInstitution([FromBody] ISM_Sales_Master_Product_DTO dto)
        {
            return _sali.OnChangeProductInstitution(dto);
        }

        [HttpPost]
        [Route("deactivate_pro")]
        public ISM_Sales_Master_Product_DTO deactivate_pro([FromBody] ISM_Sales_Master_Product_DTO dTO)
        {
            return _sali.deactivate_pro(dTO);
        }

        //----------------------------SOURCE-----------------------------
        [Route("get_load_src/{id:int}")]
        public ISM_Sales_Master_Source_DTO get_load_src(int id)
        {
            return _sali.get_load_src(id);
        }
        [Route("SaveEdit_src")]
        public ISM_Sales_Master_Source_DTO SaveEdit_src([FromBody] ISM_Sales_Master_Source_DTO dto)
        {
            return _sali.SaveEdit_src(dto);
        }

        [Route("Edit_details_src")]
        public ISM_Sales_Master_Source_DTO Edit_details_src([FromBody] ISM_Sales_Master_Source_DTO dto)
        {
            return _sali.Edit_details_src(dto);
        }

        [Route("OnChangeSourceInstitution")]
        public ISM_Sales_Master_Source_DTO OnChangeSourceInstitution([FromBody] ISM_Sales_Master_Source_DTO dto)
        {
            return _sali.OnChangeSourceInstitution(dto);
        }

        [HttpPost]
        [Route("deactivate_src")]
        public ISM_Sales_Master_Source_DTO deactivate_src([FromBody] ISM_Sales_Master_Source_DTO dTO)
        {
            return _sali.deactivate_src(dTO);
        }

        //=================Status============
        [Route("get_load_sts/{id:int}")]
        public ISM_Sales_Master_Status_DTO get_load_sts(int id)
        {
            return _sali.get_load_sts(id);
        }

        [Route("SaveEdit_sts")]
        public ISM_Sales_Master_Status_DTO SaveEdit_sts([FromBody] ISM_Sales_Master_Status_DTO dto)
        {
            return _sali.SaveEdit_sts(dto);
        }

        [Route("Edit_details_sts")]
        public ISM_Sales_Master_Status_DTO Edit_details_sts([FromBody] ISM_Sales_Master_Status_DTO dto)
        {
            return _sali.Edit_details_sts(dto);
        }

        [Route("OnChangeStatusInstitution")]
        public ISM_Sales_Master_Status_DTO OnChangeStatusInstitution([FromBody] ISM_Sales_Master_Status_DTO dto)
        {
            return _sali.OnChangeStatusInstitution(dto);
        }

        [HttpPost]
        [Route("deactivate_sts")]
        public ISM_Sales_Master_Status_DTO deactivate_sts([FromBody] ISM_Sales_Master_Status_DTO dTO)
        {
            return _sali.deactivate_sts(dTO);
        }

        //====================Lead=====================
        [Route("viewdetails")]
        public ISM_Sales_Lead_DTO viewdetails([FromBody] ISM_Sales_Lead_DTO data)
        {
            return _sali.viewdetails(data);
        }

        [Route("getdetails")]
        public ISM_Sales_Lead_DTO getdetails([FromBody]ISM_Sales_Lead_DTO dto)
        {
            return _sali.getdetails(dto);
        }
        [Route("get_lead")]
        public ISM_Sales_Lead_DTO get_lead([FromBody]ISM_Sales_Lead_DTO dto)
        {
            return _sali.get_lead(dto);
        }

        [Route("SaveChannel")]
        public ISM_Sales_Lead_DTO SaveChannel([FromBody] ISM_Sales_Lead_DTO dTO)
        {
            return _sali.SaveChannel(dTO);
        }
        [Route("SaveVerifyflag")]
        public ISM_Sales_Lead_DTO SaveVerifyflag([FromBody] ISM_Sales_Lead_DTO dTO)
        {
            return _sali.SaveVerifyflag(dTO);
        }
        [Route("deactivate")]
        public ISM_Sales_Lead_DTO deactivate([FromBody]ISM_Sales_Lead_DTO data)
        {
            return _sali.deactivate(data);
        }
        [Route("viewData")]
        public ISM_Sales_Lead_DTO viewData([FromBody]ISM_Sales_Lead_DTO data)
        {
            return _sali.viewData(data);
        }

        [Route("Sales_Lead_EmailMobNumSave")]
        public ISM_Sales_Lead_DTO Sales_Lead_EmailMobNumSave([FromBody]ISM_Sales_Lead_DTO data)
        {
            return _sali.Sales_Lead_EmailMobNumSave(data);
        }
        [Route("LeadSearchByColumn")]
        public ISM_Sales_Lead_DTO LeadSearchByColumn([FromBody]ISM_Sales_Lead_DTO data)
        {
            return _sali.LeadSearchByColumn(data);
        }


        [Route("load_all_ChannelPartner")]
        public ISM_Sales_Lead_DTO load_all_ChannelPartner([FromBody] ISM_Sales_Lead_DTO dTO)
        {
            return _sali.load_all_ChannelPartner(dTO);
        }
        [Route("load_all_lead")]
        public ISM_Sales_Lead_DTO load_all_lead([FromBody] ISM_Sales_Lead_DTO dTO)
        {
            return _sali.load_all_lead(dTO);
        }

        [Route("select_state")]
        public ISM_Sales_Lead_DTO select_state([FromBody] ISM_Sales_Lead_DTO dTO)
        {
            return _sali.select_state(dTO);
        }
        [Route("select_District")]
        public ISM_Sales_Lead_DTO select_District([FromBody] ISM_Sales_Lead_DTO dTO)
        {
            return _sali.select_District(dTO);
        }
        [Route("checkemailtemplet")]
        public ISM_Sales_Lead_DTO checkemailtemplet([FromBody] ISM_Sales_Lead_DTO dTO)
        {
            return _sali.checkemailtemplet(dTO);
        }

        [Route("getproduct")]
        public ISM_Sales_Lead_DTO getproduct([FromBody] ISM_Sales_Lead_DTO dTO)
        {
            return _sali.getproduct(dTO);
        }

        [Route("getLead")]
        public ISM_Sales_Lead_DTO getLead([FromBody] ISM_Sales_Lead_DTO dTO)
        {
            return _sali.getLead(dTO);
        }

        [Route("Save_Edit_SaleLead")]
        public ISM_Sales_Lead_DTO Save_Edit_SaleLead([FromBody] ISM_Sales_Lead_DTO dTO)
        {
            return _sali.Save_Edit_SaleLead(dTO);
        }

        [Route("Sales_lead_edit")]
        public ISM_Sales_Lead_DTO Sales_lead_edit([FromBody] ISM_Sales_Lead_DTO dTO)
        {
            return _sali.Sales_lead_edit(dTO);
        }



        [Route("deactiv_prde")]
        public ISM_Sales_Lead_DTO deactiv_prde([FromBody] ISM_Sales_Lead_DTO dTO)
        {
            return _sali.deactiv_prde(dTO);
        }

        [Route("get_load_lead_demo")]
        public ISM_Sales_Lead_Demo_DTO get_load_lead_demo([FromBody] ISM_Sales_Lead_Demo_DTO dTO)
        {
            return _sali.get_load_lead_demo(dTO);
        }

        

        [Route("Edit_details_lead_demo")]
        public ISM_Sales_Lead_Demo_DTO Edit_details_lead_demo([FromBody] ISM_Sales_Lead_Demo_DTO dTO)
        {
            return _sali.Edit_details_lead_demo(dTO);
        }

        [Route("Edit_response_lead_demo")]
        public ISM_Sales_Lead_Demo_DTO Edit_response_lead_demo([FromBody] ISM_Sales_Lead_Demo_DTO dTO)
        {
            return _sali.Edit_response_lead_demo(dTO);
        }

        [Route("SaveEdit_lead_demo")]
        public ISM_Sales_Lead_Demo_DTO SaveEdit_lead_demo([FromBody] ISM_Sales_Lead_Demo_DTO dTO)
        {
            return _sali.SaveEdit_lead_demo(dTO);
        }

        [Route("Save_response_lead_demo")]
        public ISM_Sales_Lead_Demo_DTO Save_response_lead_demo([FromBody] ISM_Sales_Lead_Demo_DTO dTO)
        {
            return _sali.Save_response_lead_demo(dTO);
        }

        [Route("view_lead_demo")]
        public ISM_Sales_Lead_Demo_DTO view_lead_demo([FromBody] ISM_Sales_Lead_Demo_DTO dTO)
        {
            return _sali.view_lead_demo(dTO);
        }

        [Route("getalllead")]
        public ISM_Sales_Lead_Demo_DTO getalllead([FromBody] ISM_Sales_Lead_Demo_DTO dTO)
        {
            return _sali.getalllead(dTO);
        }

        [Route("demo_report")]
        public ISM_Sales_Lead_Demo_DTO demo_report([FromBody] ISM_Sales_Lead_Demo_DTO dTO)
        {
            return _sali.demo_report(dTO);
        }
        //demo status

        [Route("get_load_lead_demo_status")]
        public ISM_Sales_Lead_Demo_DTO get_load_lead_demo_status([FromBody] ISM_Sales_Lead_Demo_DTO dTO)
        {
            return _sali.get_load_lead_demo_status(dTO);
        }
        [Route("SaveEdit_lead_demo_response")]
        public ISM_Sales_Lead_Demo_DTO SaveEdit_lead_demo_response([FromBody] ISM_Sales_Lead_Demo_DTO dTO)
        {
            return _sali.SaveEdit_lead_demo_response(dTO);
        }

        [Route("getLeadResponseDetails")]
        public ISM_Sales_Lead_Demo_DTO getLeadResponseDetails([FromBody] ISM_Sales_Lead_Demo_DTO dTO)
        {
            return _sali.getLeadResponseDetails(dTO);
        }

        [Route("load_all_lead_mapping")]
        public ISM_Sales_Lead_DTO load_all_lead_mapping([FromBody] ISM_Sales_Lead_DTO dTO)
        {
            return _sali.load_all_lead_mapping(dTO);
        }

        [Route("getChannelPatner")]
        public ISM_Sales_Lead_DTO getChannelPatner([FromBody] ISM_Sales_Lead_DTO dTO)
        {
            return _sali.getChannelPatner(dTO);
        }


        [Route("save_ChannelPatnerMapping")]
        public ISM_Sales_Lead_DTO save_ChannelPatnerMapping([FromBody] ISM_Sales_Lead_DTO dTO)
        {
            return _sali.save_ChannelPatnerMapping(dTO);
        }


        [Route("leadChannelmapping_edit")]
        public ISM_Sales_Lead_DTO leadChannelmapping_edit([FromBody] ISM_Sales_Lead_DTO dTO)
        {
            return _sali.leadChannelmapping_edit(dTO);
        }
        //-----------------Sales Quotation--------------------
        [Route("getverifyleaddetails")]
        public ISM_Sales_Lead_DTO getverifyleaddetails([FromBody] ISM_Sales_Lead_DTO data)
        {
            return _sali.getverifyleaddetails(data);
        }

        [Route("getleaddetails")]
        public ISM_Sales_Lead_DTO getleaddetails([FromBody] ISM_Sales_Lead_DTO data)
        {
            return _sali.getleaddetails(data);
        }
        [Route("getleaddetailsfinal")]
        public ISM_Sales_Lead_DTO getleaddetailsfinal([FromBody] ISM_Sales_Lead_DTO data)
        {
            return _sali.getleaddetailsfinal(data);
        }
        [Route("sendmailDetails")]
        public ISM_Sales_Lead_DTO sendmailDetails([FromBody] ISM_Sales_Lead_DTO data)
        {
            return _sali.sendmailDetails(data);
        }
        [Route("updatedetails")]
        public ISM_Sales_Lead_DTO updatedetails([FromBody] ISM_Sales_Lead_DTO dTO)
        {
            return _sali.updatedetails(dTO);
        }

        [Route("get_leads")]
        public ISM_Sales_Lead_DTO get_leads([FromBody] ISM_Sales_Lead_DTO data)
        {
            return _sali.get_leads(data);
        }

        [Route("Reschedule")]
        public ISM_Sales_Lead_DTO Reschedule([FromBody] ISM_Sales_Lead_DTO data)
        {
            return _sali.Reschedule(data);
        }

        [Route("onreport")]
        public ISM_Sales_Lead_Demo_DTO onreport([FromBody] ISM_Sales_Lead_Demo_DTO dTO)
        {
            return _sali.onreport(dTO);
        }

        [Route("getleaddetail")]
        public ISM_Sales_Lead_Demo_DTO getleaddetail([FromBody] ISM_Sales_Lead_Demo_DTO dTO)
        {
            return _sali.getleaddetail(dTO);
        }

        [Route("LeadUpdatemail")]
        public ISM_Sales_Lead_DTO LeadUpdatemail([FromBody] ISM_Sales_Lead_DTO data)
        {
            return _sali.LeadUpdatemail(data);

        }

        [Route("CancelNew")]
        public ISM_Sales_Lead_DTO CancelNew([FromBody] ISM_Sales_Lead_DTO data)
        {
            return _sali.CancelNew(data);
        }
        
        [Route("Sales_Raw_Report_OnclickGridData")]
        public ISM_Sales_Lead_DTO Sales_Raw_Report_OnclickGridData([FromBody] ISM_Sales_Lead_DTO data)
        {
            return _sali.Sales_Raw_Report_OnclickGridData(data);
        }

        [Route("Swap")]
        public ISM_Sales_Lead_DTO Swap([FromBody] ISM_Sales_Lead_DTO data)
        {
            return _sali.Swap(data);
        }

        [Route("SwappingEmpList")]
        public ISM_Sales_Lead_DTO SwappingEmpList([FromBody] ISM_Sales_Lead_DTO data)
        {
            return _sali.SwappingEmpList(data);
        }

        [Route("DemoCancelReportNew")]
        public ISM_Sales_Lead_Demo_DTO DemoCancelReportNew([FromBody] ISM_Sales_Lead_Demo_DTO dTO)
        {
            return _sali.DemoCancelReportNew(dTO);
        }
    }
}