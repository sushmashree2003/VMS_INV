using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.VMS;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.VMS;
using PreadmissionDTOs.com.vaps.VMS.Sales;

namespace IVRMUX.Controllers.com.vapstech.VMS
{
    [Produces("application/json")]
    [Route("api/Sales_Lead")]
    public class Sales_LeadController : Controller
    {
        Sales_Lead_Delegate LMD = new Sales_Lead_Delegate();

        //=====================Category===========
        [HttpGet]
        [Route("get_load_Cat/{id:int}")]
        public ISM_Sales_Master_Category_DTO get_load_Cat(int id)
        {            
            var mi = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            id = Convert.ToInt32(mi);
            return LMD.get_load_Cat(id);
        }

        [Route("SaveEdit_Cat")]
        public ISM_Sales_Master_Category_DTO SaveEdit_Cat([FromBody] ISM_Sales_Master_Category_DTO dto)
        {
            dto.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));            
            return LMD.SaveEdit_Cat(dto);
        }

        [Route("Edit_details_Cat")]
        public ISM_Sales_Master_Category_DTO Edit_details_Cat([FromBody] ISM_Sales_Master_Category_DTO dto)
        {
            return LMD.Edit_details_Cat(dto);
        }

        [Route("OnChangeCategoryInstitution")]
        public ISM_Sales_Master_Category_DTO OnChangeCategoryInstitution([FromBody] ISM_Sales_Master_Category_DTO dto)
        {           
            return LMD.OnChangeCategoryInstitution(dto);
        }

        [HttpPost]
        [Route("deactivate_Cat")]
        public ISM_Sales_Master_Category_DTO deactivate_Cat([FromBody] ISM_Sales_Master_Category_DTO dto)
        {
            dto.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
           
            return LMD.deactivate_Cat(dto);
        }

        //==============================Product============================

        [HttpGet]
        [Route("get_load_pro/{id:int}")]
        public ISM_Sales_Master_Product_DTO get_load_pro(int id)
        {
            var mi = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            id =Convert.ToInt32(mi);
            return LMD.get_load_pro(id);
        }

        [Route("SaveEdit_pro")]
        public ISM_Sales_Master_Product_DTO SaveEdit_pro([FromBody] ISM_Sales_Master_Product_DTO dto)
        {
            dto.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));             
            return LMD.SaveEdit_pro(dto);
        }

        [Route("Edit_details_pro")]
        public ISM_Sales_Master_Product_DTO Edit_details_pro([FromBody] ISM_Sales_Master_Product_DTO dto)
        { 
            return LMD.Edit_details_pro(dto);
        }

        [Route("OnChangeProductInstitution")]
        public ISM_Sales_Master_Product_DTO OnChangeProductInstitution([FromBody] ISM_Sales_Master_Product_DTO dto)
        { 
            return LMD.OnChangeProductInstitution(dto);
        }

        [HttpPost]
        [Route("deactivate_pro")]
        public ISM_Sales_Master_Product_DTO deactivate_pro([FromBody] ISM_Sales_Master_Product_DTO dto)
        {
            dto.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
                       return LMD.deactivate_pro(dto);
        }

        //=============================Source=========================       

        [HttpGet]
        [Route("get_load_src/{id:int}")]
        public ISM_Sales_Master_Source_DTO get_load_src(int id)
        {
            var mi = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            id = Convert.ToInt32(mi);
            return LMD.get_load_src(id);
        }
        [Route("SaveEdit_src")]
        public ISM_Sales_Master_Source_DTO SaveEdit_src([FromBody] ISM_Sales_Master_Source_DTO dto)
        {
            dto.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return LMD.SaveEdit_src(dto);
        }

        [Route("Edit_details_src")]
        public ISM_Sales_Master_Source_DTO Edit_details_src([FromBody] ISM_Sales_Master_Source_DTO dto)
        {
            return LMD.Edit_details_src(dto);
        }

        [Route("OnChangeSourceInstitution")]
        public ISM_Sales_Master_Source_DTO OnChangeSourceInstitution([FromBody] ISM_Sales_Master_Source_DTO dto)
        {
            return LMD.OnChangeSourceInstitution(dto);
        }

        [HttpPost]
        [Route("deactivate_src")]
        public ISM_Sales_Master_Source_DTO deactivate_src([FromBody] ISM_Sales_Master_Source_DTO dto)
        {
            dto.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return LMD.deactivate_src(dto);
        }

        //=============================Status=========================

        [HttpGet]
        [Route("get_load_sts/{id:int}")]
        public ISM_Sales_Master_Status_DTO get_load_sts(int id)
        {
            var mi = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            id = Convert.ToInt32(mi);
            return LMD.get_load_sts(id);
        }

        [Route("SaveEdit_sts")]
        public ISM_Sales_Master_Status_DTO SaveEdit_sts([FromBody] ISM_Sales_Master_Status_DTO dto)
        {
            dto.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));          
            return LMD.SaveEdit_sts(dto);
        }

        [Route("Edit_details_sts")]
        public ISM_Sales_Master_Status_DTO Edit_details_sts([FromBody] ISM_Sales_Master_Status_DTO dto)
        {           
            return LMD.Edit_details_sts(dto);
        }

        [Route("OnChangeStatusInstitution")]
        public ISM_Sales_Master_Status_DTO OnChangeStatusInstitution([FromBody] ISM_Sales_Master_Status_DTO dto)
        {           
            return LMD.OnChangeStatusInstitution(dto);
        }

        [HttpPost]
        [Route("deactivate_sts")]
        public ISM_Sales_Master_Status_DTO deactivate_sts([FromBody] ISM_Sales_Master_Status_DTO dto)
        {
            dto.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return LMD.deactivate_sts(dto);
        }

        //=========================lead ================================
        [Route("viewdetails")]
        public ISM_Sales_Lead_DTO viewdetails([FromBody]ISM_Sales_Lead_DTO data)
        {
            //data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return LMD.viewdetails(data);
        }
        [Route("load_all_ChannelPartner/{id:int}")]
        public ISM_Sales_Lead_DTO load_all_ChannelPartner(int id)
        {
            ISM_Sales_Lead_DTO dto = new ISM_Sales_Lead_DTO();
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.user_id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return LMD.load_all_ChannelPartner(dto);
        }
        [Route("getdetails")]
        public ISM_Sales_Lead_DTO getdetails([FromBody] ISM_Sales_Lead_DTO dto)
        {

            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return LMD.getdetails(dto);
        }
        [Route("get_lead")]
        public ISM_Sales_Lead_DTO get_lead([FromBody] ISM_Sales_Lead_DTO dto)
        {

            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return LMD.get_lead(dto);
        }
        [Route("SaveChannel")]
        public ISM_Sales_Lead_DTO SaveChannel([FromBody] ISM_Sales_Lead_DTO dto)
        {
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.user_id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return LMD.SaveChannel(dto);
        }

        [Route("SaveVerifyflag")]
        public ISM_Sales_Lead_DTO SaveVerifyflag([FromBody] ISM_Sales_Lead_DTO dto)
        {
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.user_id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return LMD.SaveVerifyflag(dto);
        }

        [Route("deactivate")]
        public ISM_Sales_Lead_DTO deactivate([FromBody] ISM_Sales_Lead_DTO d)
        {
            d.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return LMD.deactivate(d);
        }


        [Route("load_all_lead/{id:int}")]
        public ISM_Sales_Lead_DTO load_all_lead(int id)
        {
            ISM_Sales_Lead_DTO dto = new ISM_Sales_Lead_DTO();
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.user_id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return LMD.load_all_lead(dto);
        }

        [Route("Save_Edit_SaleLead")]
        public ISM_Sales_Lead_DTO Save_Edit_SaleLead([FromBody]ISM_Sales_Lead_DTO dto)
        {
            dto.user_id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return LMD.Save_Edit_SaleLead(dto);
        }

        [Route("Sales_lead_edit")]
        public ISM_Sales_Lead_DTO Sales_lead_edit([FromBody]ISM_Sales_Lead_DTO dto)
        {
            dto.user_id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return LMD.Sales_lead_edit(dto);
        }

        [Route("deactiv_prde")]
        public ISM_Sales_Lead_DTO deactiv_prde([FromBody] ISM_Sales_Lead_DTO dto)
        {
            dto.user_id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return LMD.deactiv_prde(dto);
        }

        [Route("Sales_lead_view/{id:int}")]
        public ISM_Sales_Lead_DTO Sales_lead_view(int id)
        {
            ISM_Sales_Lead_DTO dto = new ISM_Sales_Lead_DTO();
            dto.user_id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            dto.ISMSLE_Id = id;
            return LMD.Sales_lead_view(dto);
        }

        [Route("select_state/{id:int}")]
        public ISM_Sales_Lead_DTO select_state(int id)
        {
            ISM_Sales_Lead_DTO dto = new ISM_Sales_Lead_DTO();
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.user_id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            dto.IVRMMC_Id = id;
            return LMD.select_state(dto);
        }


        [Route("select_District/{id:int}")]
        public ISM_Sales_Lead_DTO select_District(int id)
        {
            ISM_Sales_Lead_DTO dto = new ISM_Sales_Lead_DTO();
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.user_id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            dto.IVRMMS_Id = id;
            return LMD.select_District(dto);
        }



        [Route("checkemailtemplet/{id:int}")]
        public ISM_Sales_Lead_DTO checkemailtemplet(int id)
        {
            ISM_Sales_Lead_DTO dto = new ISM_Sales_Lead_DTO();          
            dto.user_id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            dto.ISMSMSO_Id = id;
            return LMD.checkemailtemplet(dto);
        }

        [Route("getproduct")]
        public ISM_Sales_Lead_DTO getproduct([FromBody] ISM_Sales_Lead_DTO dto)
        {
            dto.user_id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return LMD.getproduct(dto);
        }

        [Route("getLead")]
        public ISM_Sales_Lead_DTO getLead([FromBody] ISM_Sales_Lead_DTO dto)
        {
            dto.user_id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return LMD.getLead(dto);
        }

        [Route("viewData")]
        public ISM_Sales_Lead_DTO viewData([FromBody]ISM_Sales_Lead_DTO dto)
        {
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return LMD.viewData(dto);
        }

        [Route("Sales_Lead_EmailMobNumSave")]
        public ISM_Sales_Lead_DTO Sales_Lead_EmailMobNumSave([FromBody]ISM_Sales_Lead_DTO dto)
        {
            dto.user_id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return LMD.Sales_Lead_EmailMobNumSave(dto);
        }

        [Route("LeadSearchByColumn")]
        public ISM_Sales_Lead_DTO LeadSearchByColumn([FromBody]ISM_Sales_Lead_DTO dto)
        {
            dto.user_id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return LMD.LeadSearchByColumn(dto);
        }

        //==========================Lead Demo=============================

        [Route("get_load_lead_demo/{id:int}")]
        public ISM_Sales_Lead_Demo_DTO get_load_lead_demo(int id)
        {
            ISM_Sales_Lead_Demo_DTO dto = new ISM_Sales_Lead_Demo_DTO();
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.user_id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return LMD.get_load_lead_demo(dto);
        }

        [Route("Edit_details_lead_demo")]
        public ISM_Sales_Lead_Demo_DTO Edit_details_lead_demo([FromBody]ISM_Sales_Lead_Demo_DTO dto)
        {
           
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.user_id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return LMD.Edit_details_lead_demo(dto);
        }

        [Route("Edit_response_lead_demo")]
        public ISM_Sales_Lead_Demo_DTO Edit_response_lead_demo([FromBody]ISM_Sales_Lead_Demo_DTO dto)
        {
           
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.user_id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return LMD.Edit_response_lead_demo(dto);
        }

        [Route("SaveEdit_lead_demo")]
        public ISM_Sales_Lead_Demo_DTO SaveEdit_lead_demo([FromBody] ISM_Sales_Lead_Demo_DTO dto)
        {
           
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.user_id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return LMD.SaveEdit_lead_demo(dto);
        }
        
        [Route("Save_response_lead_demo")]
        public ISM_Sales_Lead_Demo_DTO Save_response_lead_demo([FromBody]ISM_Sales_Lead_Demo_DTO dto)
        {
           
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.user_id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return LMD.Save_response_lead_demo(dto);
        }

        [Route("view_lead_demo")]
        public ISM_Sales_Lead_Demo_DTO view_lead_demo([FromBody]ISM_Sales_Lead_Demo_DTO dto)
        {
           
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.user_id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return LMD.view_lead_demo(dto);
        }

        [Route("getalllead/{id:int}")]
        public ISM_Sales_Lead_Demo_DTO getalllead(int id)
        {
            ISM_Sales_Lead_Demo_DTO dto = new ISM_Sales_Lead_Demo_DTO();
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.user_id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return LMD.getalllead(dto);
        }

        [Route("demo_report")]
        public ISM_Sales_Lead_Demo_DTO demo_report([FromBody]ISM_Sales_Lead_Demo_DTO dto)
        {
          
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.user_id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return LMD.demo_report(dto);
        }

        //demo status
        [Route("get_load_lead_demo_status/{id:int}")]
        public ISM_Sales_Lead_Demo_DTO get_load_lead_demo_status(int id)
        {
            ISM_Sales_Lead_Demo_DTO dto = new ISM_Sales_Lead_Demo_DTO();
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.user_id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return LMD.get_load_lead_demo_status(dto);
        }

        [Route("SaveEdit_lead_demo_response")]
        public ISM_Sales_Lead_Demo_DTO SaveEdit_lead_demo_response([FromBody] ISM_Sales_Lead_Demo_DTO dto)
        {

            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.user_id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return LMD.SaveEdit_lead_demo_response(dto);
        }
        [Route("getLeadResponseDetails/{id:int}")]
        public ISM_Sales_Lead_Demo_DTO getLeadResponseDetails(int id)
        {
            ISM_Sales_Lead_Demo_DTO dto = new ISM_Sales_Lead_Demo_DTO();
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.ISMSLE_Id = id;
            dto.user_id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return LMD.getLeadResponseDetails(dto);
          
        }
        //channel patner mapping
        [Route("load_all_lead_mapping/{id:int}")]
        public ISM_Sales_Lead_DTO load_all_lead_mapping(int id)
        {
            ISM_Sales_Lead_DTO dto = new ISM_Sales_Lead_DTO();
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.user_id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return LMD.load_all_lead_mapping(dto);
        }

        [Route("getChannelPatner")]
        public ISM_Sales_Lead_DTO getChannelPatner([FromBody]ISM_Sales_Lead_DTO dto)
        {

            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.user_id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return LMD.getChannelPatner(dto);
        }

        [Route("save_ChannelPatnerMapping")]
        public ISM_Sales_Lead_DTO save_ChannelPatnerMapping([FromBody] ISM_Sales_Lead_DTO dto)
        {
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return LMD.save_ChannelPatnerMapping(dto);
        }

        [Route("leadChannelmapping_edit")]
        public ISM_Sales_Lead_DTO leadChannelmapping_edit([FromBody] ISM_Sales_Lead_DTO dto)
        {

            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.user_id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return LMD.leadChannelmapping_edit(dto);
        }

        //=========================Sales Quotation ================================
        [Route("getverifyleaddetails/{id:int}")]
        public ISM_Sales_Lead_DTO getverifyleaddetails(int id)
        {

            ISM_Sales_Lead_DTO data = new ISM_Sales_Lead_DTO();
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.user_id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return LMD.getverifyleaddetails(data);
        }


        [Route("getleaddetails")]
        public ISM_Sales_Lead_DTO getleaddetails([FromBody] ISM_Sales_Lead_DTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.user_id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return LMD.getleaddetails(data);
        }


        [Route("getleaddetailsfinal")]
        public ISM_Sales_Lead_DTO getleaddetailsfinal([FromBody] ISM_Sales_Lead_DTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.user_id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return LMD.getleaddetailsfinal(data);
        }


        [Route("sendmailDetails")]
        public ISM_Sales_Lead_DTO sendmailDetails([FromBody] ISM_Sales_Lead_DTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.user_id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return LMD.sendmailDetails(data);
        }

        [Route("updatedetails")]
        public ISM_Sales_Lead_DTO updatedetails([FromBody]ISM_Sales_Lead_DTO dto)
        {

            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.user_id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return LMD.updatedetails(dto);
        }

        [Route("get_leads")]
        public ISM_Sales_Lead_DTO get_leads([FromBody]ISM_Sales_Lead_DTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            //data.IMFY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("IMFY_Id"));
            data.user_id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return LMD.get_leads(data);
        }

        [Route("Reschedule")]
        public ISM_Sales_Lead_DTO Reschedule([FromBody]ISM_Sales_Lead_DTO dto)
        {

            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.user_id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return LMD.Reschedule(dto);
        }

        [Route("onreport")]
        public ISM_Sales_Lead_Demo_DTO onreport([FromBody]ISM_Sales_Lead_Demo_DTO dto)
        {
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.user_id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return LMD.onreport(dto);
        }

        [Route("getleaddetail")]
        public ISM_Sales_Lead_Demo_DTO getleaddetail([FromBody]ISM_Sales_Lead_Demo_DTO dto)
        {
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.user_id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return LMD.getleaddetail(dto);
        }

        [Route("LeadUpdatemail")]
        public ISM_Sales_Lead_DTO LeadUpdatemail([FromBody]ISM_Sales_Lead_DTO dto)
        {

            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.user_id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return LMD.LeadUpdatemail(dto);

        }


        [Route("CancelNew")]
        public ISM_Sales_Lead_DTO CancelNew([FromBody]ISM_Sales_Lead_DTO dto)
        {

            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.user_id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return LMD.CancelNew(dto);
        }
       

        [Route("Sales_Raw_Report_OnclickGridData")]
        public ISM_Sales_Lead_DTO Sales_Raw_Report_OnclickGridData([FromBody]ISM_Sales_Lead_DTO dto)
        {

            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.user_id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return LMD.Sales_Raw_Report_OnclickGridData(dto);
        }

        [Route("Swap")]
        public ISM_Sales_Lead_DTO Swap([FromBody]ISM_Sales_Lead_DTO dto)
        {

            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.user_id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            // data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            dto.Role_flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            dto.IVRMRT_Id = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));

            return LMD.Swap(dto);
        }
        
        [Route("SwappingEmpList")]
        public ISM_Sales_Lead_DTO SwappingEmpList([FromBody]ISM_Sales_Lead_DTO dto)
        {

            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.user_id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return LMD.SwappingEmpList(dto);
        }


        [Route("DemoCancelReportNew")]
        public ISM_Sales_Lead_Demo_DTO DemoCancelReportNew([FromBody]ISM_Sales_Lead_Demo_DTO dto)
        {

            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.user_id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return LMD.DemoCancelReportNew(dto);
        }
    }
}