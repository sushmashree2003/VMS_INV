using PreadmissionDTOs.com.vaps.VMS.Sales;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Recruitment.com.vaps.Interfaces
{
    public interface Sales_Lead_Master_Interface
    {
        //---------------------- Category-----------------------
        ISM_Sales_Master_Category_DTO get_load_Cat(int id);
        ISM_Sales_Master_Category_DTO deactivate_Cat(ISM_Sales_Master_Category_DTO id);
        ISM_Sales_Master_Category_DTO SaveEdit_Cat(ISM_Sales_Master_Category_DTO dto);
        ISM_Sales_Master_Category_DTO Edit_details_Cat(ISM_Sales_Master_Category_DTO dto);
        ISM_Sales_Master_Category_DTO OnChangeCategoryInstitution(ISM_Sales_Master_Category_DTO dto);

        //---------------------- Product-----------------------
        ISM_Sales_Master_Product_DTO get_load_pro(int id);
        ISM_Sales_Master_Product_DTO deactivate_pro(ISM_Sales_Master_Product_DTO id);
        ISM_Sales_Master_Product_DTO SaveEdit_pro(ISM_Sales_Master_Product_DTO dto);
        ISM_Sales_Master_Product_DTO Edit_details_pro(ISM_Sales_Master_Product_DTO dto);
        ISM_Sales_Master_Product_DTO OnChangeProductInstitution(ISM_Sales_Master_Product_DTO dto);

        //----------------------Source-----------------------
        ISM_Sales_Master_Source_DTO get_load_src(int id);
        ISM_Sales_Master_Source_DTO deactivate_src(ISM_Sales_Master_Source_DTO id);
        ISM_Sales_Master_Source_DTO SaveEdit_src(ISM_Sales_Master_Source_DTO dto);
        ISM_Sales_Master_Source_DTO OnChangeSourceInstitution(ISM_Sales_Master_Source_DTO dto);
        ISM_Sales_Master_Source_DTO Edit_details_src(ISM_Sales_Master_Source_DTO dto);

        //----------------------Status-----------------------
        ISM_Sales_Master_Status_DTO get_load_sts(int id);
        ISM_Sales_Master_Status_DTO deactivate_sts(ISM_Sales_Master_Status_DTO id);
        ISM_Sales_Master_Status_DTO SaveEdit_sts(ISM_Sales_Master_Status_DTO dto);
        ISM_Sales_Master_Status_DTO Edit_details_sts(ISM_Sales_Master_Status_DTO dto);
        ISM_Sales_Master_Status_DTO OnChangeStatusInstitution(ISM_Sales_Master_Status_DTO dto);

        //----------------------Lead-----------------
        ISM_Sales_Lead_DTO viewdetails(ISM_Sales_Lead_DTO data);
        ISM_Sales_Lead_DTO load_all_ChannelPartner(ISM_Sales_Lead_DTO dto);
        ISM_Sales_Lead_DTO getdetails(ISM_Sales_Lead_DTO dto);
        ISM_Sales_Lead_DTO get_lead(ISM_Sales_Lead_DTO dto);
        ISM_Sales_Lead_DTO SaveChannel(ISM_Sales_Lead_DTO dTO);
        ISM_Sales_Lead_DTO SaveVerifyflag(ISM_Sales_Lead_DTO dTO);
        ISM_Sales_Lead_DTO deactivate(ISM_Sales_Lead_DTO dto);
        ISM_Sales_Lead_DTO viewData(ISM_Sales_Lead_DTO data);
        ISM_Sales_Lead_DTO Sales_Lead_EmailMobNumSave(ISM_Sales_Lead_DTO data);
        ISM_Sales_Lead_DTO LeadSearchByColumn(ISM_Sales_Lead_DTO data);
        ISM_Sales_Lead_DTO load_all_lead(ISM_Sales_Lead_DTO dTO);
        ISM_Sales_Lead_DTO Save_Edit_SaleLead(ISM_Sales_Lead_DTO dTO);
        ISM_Sales_Lead_DTO Sales_lead_edit(ISM_Sales_Lead_DTO dTO);
        ISM_Sales_Lead_DTO deactiv_prde(ISM_Sales_Lead_DTO dTO);
        ISM_Sales_Lead_DTO select_state(ISM_Sales_Lead_DTO dTO);
        ISM_Sales_Lead_DTO select_District(ISM_Sales_Lead_DTO dTO);
        ISM_Sales_Lead_DTO checkemailtemplet(ISM_Sales_Lead_DTO dTO);
        ISM_Sales_Lead_DTO getproduct(ISM_Sales_Lead_DTO dTO);
        ISM_Sales_Lead_DTO getLead(ISM_Sales_Lead_DTO dTO);
        

        //----------------------Lead Demo-----------------
        ISM_Sales_Lead_Demo_DTO get_load_lead_demo(ISM_Sales_Lead_Demo_DTO dto);
        ISM_Sales_Lead_Demo_DTO Edit_details_lead_demo(ISM_Sales_Lead_Demo_DTO dto);
        ISM_Sales_Lead_Demo_DTO Edit_response_lead_demo(ISM_Sales_Lead_Demo_DTO dto);
        ISM_Sales_Lead_Demo_DTO SaveEdit_lead_demo(ISM_Sales_Lead_Demo_DTO dto);
        ISM_Sales_Lead_Demo_DTO Save_response_lead_demo(ISM_Sales_Lead_Demo_DTO dto);
        ISM_Sales_Lead_Demo_DTO view_lead_demo(ISM_Sales_Lead_Demo_DTO dto);
        ISM_Sales_Lead_Demo_DTO getalllead(ISM_Sales_Lead_Demo_DTO dto);
        ISM_Sales_Lead_Demo_DTO demo_report(ISM_Sales_Lead_Demo_DTO dto);
        ISM_Sales_Lead_Demo_DTO DemoCancelReportNew(ISM_Sales_Lead_Demo_DTO dto); 

         //demo status
         ISM_Sales_Lead_Demo_DTO get_load_lead_demo_status(ISM_Sales_Lead_Demo_DTO dto);
        ISM_Sales_Lead_Demo_DTO SaveEdit_lead_demo_response(ISM_Sales_Lead_Demo_DTO dTO);
        ISM_Sales_Lead_Demo_DTO getLeadResponseDetails(ISM_Sales_Lead_Demo_DTO dTO);

        ISM_Sales_Lead_DTO load_all_lead_mapping(ISM_Sales_Lead_DTO dTO);
        ISM_Sales_Lead_DTO getChannelPatner(ISM_Sales_Lead_DTO dTO);
        ISM_Sales_Lead_DTO save_ChannelPatnerMapping(ISM_Sales_Lead_DTO dTO);
        ISM_Sales_Lead_DTO leadChannelmapping_edit(ISM_Sales_Lead_DTO dTO);

        //-----------------Sales Quotation--------------------
        ISM_Sales_Lead_DTO getverifyleaddetails(ISM_Sales_Lead_DTO data);
        ISM_Sales_Lead_DTO getleaddetails(ISM_Sales_Lead_DTO data);
        ISM_Sales_Lead_DTO getleaddetailsfinal(ISM_Sales_Lead_DTO data);
        ISM_Sales_Lead_DTO sendmailDetails(ISM_Sales_Lead_DTO data);
        ISM_Sales_Lead_DTO updatedetails(ISM_Sales_Lead_DTO dto);
        ISM_Sales_Lead_DTO get_leads(ISM_Sales_Lead_DTO data);
        ISM_Sales_Lead_DTO Reschedule(ISM_Sales_Lead_DTO data);

        ISM_Sales_Lead_Demo_DTO onreport(ISM_Sales_Lead_Demo_DTO dto);

        ISM_Sales_Lead_Demo_DTO getleaddetail(ISM_Sales_Lead_Demo_DTO dto);

        ISM_Sales_Lead_DTO LeadUpdatemail(ISM_Sales_Lead_DTO data);

        ISM_Sales_Lead_DTO CancelNew(ISM_Sales_Lead_DTO data);
        ISM_Sales_Lead_DTO Sales_Raw_Report_OnclickGridData(ISM_Sales_Lead_DTO data);
        ISM_Sales_Lead_DTO Swap(ISM_Sales_Lead_DTO data);
        ISM_Sales_Lead_DTO  SwappingEmpList(ISM_Sales_Lead_DTO data);
    }
}
