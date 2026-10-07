using CommonLibrary;
using PreadmissionDTOs.com.vaps.VMS;
using PreadmissionDTOs.com.vaps.VMS.Sales;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.VMS
{
    public class Sales_Lead_Delegate
    {
        CommonVMSDelegate<ISM_Sales_Master_Category_DTO, ISM_Sales_Master_Category_DTO> COMMC = new CommonVMSDelegate<ISM_Sales_Master_Category_DTO, ISM_Sales_Master_Category_DTO>();
        CommonVMSDelegate<ISM_Sales_Master_Product_DTO, ISM_Sales_Master_Product_DTO> COMMP = new CommonVMSDelegate<ISM_Sales_Master_Product_DTO, ISM_Sales_Master_Product_DTO>();
        CommonVMSDelegate<ISM_Sales_Master_Source_DTO, ISM_Sales_Master_Source_DTO> COMMS = new CommonVMSDelegate<ISM_Sales_Master_Source_DTO, ISM_Sales_Master_Source_DTO>();
        CommonVMSDelegate<ISM_Sales_Lead_DTO, ISM_Sales_Lead_DTO> COMML = new CommonVMSDelegate<ISM_Sales_Lead_DTO, ISM_Sales_Lead_DTO>();
        CommonVMSDelegate<ISM_Sales_Master_Status_DTO, ISM_Sales_Master_Status_DTO> COMMSTS = new CommonVMSDelegate<ISM_Sales_Master_Status_DTO, ISM_Sales_Master_Status_DTO>();
        CommonVMSDelegate<ISM_Sales_Lead_Demo_DTO, ISM_Sales_Lead_Demo_DTO> COMMLD = new CommonVMSDelegate<ISM_Sales_Lead_Demo_DTO, ISM_Sales_Lead_Demo_DTO>();

        //----------------------------category------------------------------------
        public ISM_Sales_Master_Category_DTO get_load_Cat(int id)
        {
            return COMMC.GetDataById(id, "Sales_Lead_Facade/get_load_Cat/");
        }
        public ISM_Sales_Master_Category_DTO SaveEdit_Cat(ISM_Sales_Master_Category_DTO dto)
        {
            return COMMC.POSTData(dto, "Sales_Lead_Facade/SaveEdit_Cat/");
        }
        public ISM_Sales_Master_Category_DTO Edit_details_Cat(ISM_Sales_Master_Category_DTO dto)
        {
            return COMMC.POSTData(dto, "Sales_Lead_Facade/Edit_details_Cat/");
        }
        public ISM_Sales_Master_Category_DTO OnChangeCategoryInstitution(ISM_Sales_Master_Category_DTO dto)
        {
            return COMMC.POSTData(dto, "Sales_Lead_Facade/OnChangeCategoryInstitution/");
        }
        public ISM_Sales_Master_Category_DTO deactivate_Cat(ISM_Sales_Master_Category_DTO dto)
        {
            return COMMC.POSTData(dto, "Sales_Lead_Facade/deactivate_Cat/");
        }

        //=======================Product==============
        public ISM_Sales_Master_Product_DTO get_load_pro(int id)
        {
            return COMMP.GetDataById(id, "Sales_Lead_Facade/get_load_pro/");
        }
        public ISM_Sales_Master_Product_DTO SaveEdit_pro(ISM_Sales_Master_Product_DTO dto)
        {
            return COMMP.POSTData(dto, "Sales_Lead_Facade/SaveEdit_pro/");
        }
        public ISM_Sales_Master_Product_DTO Edit_details_pro(ISM_Sales_Master_Product_DTO dto)
        {
            return COMMP.POSTData(dto, "Sales_Lead_Facade/Edit_details_pro/");
        }
        public ISM_Sales_Master_Product_DTO OnChangeProductInstitution(ISM_Sales_Master_Product_DTO dto)
        {
            return COMMP.POSTData(dto, "Sales_Lead_Facade/OnChangeProductInstitution/");
        }
        public ISM_Sales_Master_Product_DTO deactivate_pro(ISM_Sales_Master_Product_DTO dto)
        {
            return COMMP.POSTData(dto, "Sales_Lead_Facade/deactivate_pro/");
        }

        //===================Source===================
        public ISM_Sales_Master_Source_DTO get_load_src(int id)
        {
            return COMMS.GetDataById(id, "Sales_Lead_Facade/get_load_src/");
        }
        public ISM_Sales_Master_Source_DTO SaveEdit_src(ISM_Sales_Master_Source_DTO dto)
        {
            return COMMS.POSTData(dto, "Sales_Lead_Facade/SaveEdit_src/");
        }
        public ISM_Sales_Master_Source_DTO Edit_details_src(ISM_Sales_Master_Source_DTO dto)
        {
            return COMMS.POSTData(dto, "Sales_Lead_Facade/Edit_details_src/");
        }
        public ISM_Sales_Master_Source_DTO deactivate_src(ISM_Sales_Master_Source_DTO dto)
        {
            return COMMS.POSTData(dto, "Sales_Lead_Facade/deactivate_src/");
        }
        public ISM_Sales_Master_Source_DTO OnChangeSourceInstitution(ISM_Sales_Master_Source_DTO dto)
        {
            return COMMS.POSTData(dto, "Sales_Lead_Facade/OnChangeSourceInstitution/");
        }

        //===================Status===================
        public ISM_Sales_Master_Status_DTO get_load_sts(int id)
        {
            return COMMSTS.GetDataById(id, "Sales_Lead_Facade/get_load_sts/");
        }
        public ISM_Sales_Master_Status_DTO SaveEdit_sts(ISM_Sales_Master_Status_DTO dto)
        {
            return COMMSTS.POSTData(dto, "Sales_Lead_Facade/SaveEdit_sts/");
        }
        public ISM_Sales_Master_Status_DTO Edit_details_sts(ISM_Sales_Master_Status_DTO dto)
        {
            return COMMSTS.POSTData(dto, "Sales_Lead_Facade/Edit_details_sts/");
        }
        public ISM_Sales_Master_Status_DTO deactivate_sts(ISM_Sales_Master_Status_DTO dto)
        {
            return COMMSTS.POSTData(dto, "Sales_Lead_Facade/deactivate_sts/");
        }
        public ISM_Sales_Master_Status_DTO OnChangeStatusInstitution(ISM_Sales_Master_Status_DTO dto)
        {
            return COMMSTS.POSTData(dto, "Sales_Lead_Facade/OnChangeStatusInstitution/");
        }

        //======================Lead====================
        public ISM_Sales_Lead_DTO viewdetails(ISM_Sales_Lead_DTO data)
        {
            return COMML.POSTData(data, "Sales_Lead_Facade/viewdetails");
        }
        public ISM_Sales_Lead_DTO load_all_ChannelPartner(ISM_Sales_Lead_DTO dto)
        {
            return COMML.POSTData(dto, "Sales_Lead_Facade/load_all_ChannelPartner/");
        }
        public ISM_Sales_Lead_DTO getdetails(ISM_Sales_Lead_DTO maspage)
        {
            return COMML.POSTData(maspage, "Sales_Lead_Facade/getdetails/");
        }
        public ISM_Sales_Lead_DTO get_lead(ISM_Sales_Lead_DTO maspage)
        {
            return COMML.POSTData(maspage, "Sales_Lead_Facade/get_lead/");
        }

        public ISM_Sales_Lead_DTO SaveChannel(ISM_Sales_Lead_DTO dto)
        {
            return COMML.POSTData(dto, "Sales_Lead_Facade/SaveChannel/");
        }

      
        
        public ISM_Sales_Lead_DTO SaveVerifyflag(ISM_Sales_Lead_DTO dto)
        {
            return COMML.POSTData(dto, "Sales_Lead_Facade/SaveVerifyflag/");
        }
        public ISM_Sales_Lead_DTO deactivate(ISM_Sales_Lead_DTO obj)
        {
            return COMML.POSTData(obj, "Sales_Lead_Facade/deactivate/");
        }
        public ISM_Sales_Lead_DTO load_all_lead(ISM_Sales_Lead_DTO dto)
        {
            return COMML.POSTData(dto, "Sales_Lead_Facade/load_all_lead/");
        }

  

        
        public ISM_Sales_Lead_DTO Save_Edit_SaleLead(ISM_Sales_Lead_DTO dto)
        {
            return COMML.POSTData(dto, "Sales_Lead_Facade/Save_Edit_SaleLead/");
        }
        public ISM_Sales_Lead_DTO Sales_lead_edit(ISM_Sales_Lead_DTO dto)
        {
            return COMML.POSTData(dto, "Sales_Lead_Facade/Sales_lead_edit/");
        }
        public ISM_Sales_Lead_DTO deactiv_prde(ISM_Sales_Lead_DTO dto)
        {
            return COMML.POSTData(dto, "Sales_Lead_Facade/deactiv_prde/");
        }
        public ISM_Sales_Lead_DTO Sales_lead_view(ISM_Sales_Lead_DTO dto)
        {
            return COMML.POSTData(dto, "Sales_Lead_Facade/Sales_lead_view/");
        }
        public ISM_Sales_Lead_DTO select_state(ISM_Sales_Lead_DTO dto)
        {
            return COMML.POSTData(dto, "Sales_Lead_Facade/select_state/");
        }
        public ISM_Sales_Lead_DTO select_District(ISM_Sales_Lead_DTO dto)
        {
            return COMML.POSTData(dto, "Sales_Lead_Facade/select_District/");
        }
        
        public ISM_Sales_Lead_DTO checkemailtemplet(ISM_Sales_Lead_DTO dto)
        {
            return COMML.POSTData(dto, "Sales_Lead_Facade/checkemailtemplet/");
        }
        public ISM_Sales_Lead_DTO getproduct(ISM_Sales_Lead_DTO dto)
        {
            return COMML.POSTData(dto, "Sales_Lead_Facade/getproduct/");
        }
        public ISM_Sales_Lead_DTO getLead(ISM_Sales_Lead_DTO dto)
        {
            return COMML.POSTData(dto, "Sales_Lead_Facade/getLead/");
        }
        
        public ISM_Sales_Lead_DTO viewData(ISM_Sales_Lead_DTO data)
        {
            return COMML.POSTData(data, "Sales_Lead_Facade/viewData/");
        }
        
         public ISM_Sales_Lead_DTO Sales_Lead_EmailMobNumSave(ISM_Sales_Lead_DTO data)
        {
            return COMML.POSTData(data, "Sales_Lead_Facade/Sales_Lead_EmailMobNumSave/");
        }

        public ISM_Sales_Lead_DTO LeadSearchByColumn(ISM_Sales_Lead_DTO data)
        {
            return COMML.POSTData(data, "Sales_Lead_Facade/LeadSearchByColumn/");
        }
        

        //===============Lead Demo===================
        public ISM_Sales_Lead_Demo_DTO get_load_lead_demo(ISM_Sales_Lead_Demo_DTO dto)
        {
            return COMMLD.POSTData(dto, "Sales_Lead_Facade/get_load_lead_demo/");
        }

      

        
        public ISM_Sales_Lead_Demo_DTO Edit_details_lead_demo(ISM_Sales_Lead_Demo_DTO dto)
        {
            return COMMLD.POSTData(dto, "Sales_Lead_Facade/Edit_details_lead_demo/");
        }
        public ISM_Sales_Lead_Demo_DTO Edit_response_lead_demo(ISM_Sales_Lead_Demo_DTO dto)
        {
            return COMMLD.POSTData(dto, "Sales_Lead_Facade/Edit_response_lead_demo/");
        }
        public ISM_Sales_Lead_Demo_DTO SaveEdit_lead_demo(ISM_Sales_Lead_Demo_DTO dto)
        {
            return COMMLD.POSTData(dto, "Sales_Lead_Facade/SaveEdit_lead_demo/");
        }

        

        public ISM_Sales_Lead_Demo_DTO Save_response_lead_demo(ISM_Sales_Lead_Demo_DTO dto)
        {
            return COMMLD.POSTData(dto, "Sales_Lead_Facade/Save_response_lead_demo/");
        }
        public ISM_Sales_Lead_Demo_DTO view_lead_demo(ISM_Sales_Lead_Demo_DTO dto)
        {
            return COMMLD.POSTData(dto, "Sales_Lead_Facade/view_lead_demo/");
        }
        public ISM_Sales_Lead_Demo_DTO getalllead(ISM_Sales_Lead_Demo_DTO dto)
        {
            return COMMLD.POSTData(dto, "Sales_Lead_Facade/getalllead/");
        }
        public ISM_Sales_Lead_Demo_DTO demo_report(ISM_Sales_Lead_Demo_DTO dto)
        {
            return COMMLD.POSTData(dto, "Sales_Lead_Facade/demo_report/");
        }

        //demo status
        public ISM_Sales_Lead_Demo_DTO get_load_lead_demo_status(ISM_Sales_Lead_Demo_DTO dto)
        {
            return COMMLD.POSTData(dto, "Sales_Lead_Facade/get_load_lead_demo_status/");
        }

        public ISM_Sales_Lead_Demo_DTO SaveEdit_lead_demo_response(ISM_Sales_Lead_Demo_DTO dto)
        {
            return COMMLD.POSTData(dto, "Sales_Lead_Facade/SaveEdit_lead_demo_response/");
        }

   
        
        public ISM_Sales_Lead_Demo_DTO getLeadResponseDetails(ISM_Sales_Lead_Demo_DTO dto)
        {
            return COMMLD.POSTData(dto, "Sales_Lead_Facade/getLeadResponseDetails/");
        }

        //channel patner mappingdemo_report
        public ISM_Sales_Lead_DTO load_all_lead_mapping(ISM_Sales_Lead_DTO dto)
        {
            return COMML.POSTData(dto, "Sales_Lead_Facade/load_all_lead_mapping/");
        }

        public ISM_Sales_Lead_DTO getChannelPatner(ISM_Sales_Lead_DTO dto)
        {
            return COMML.POSTData(dto, "Sales_Lead_Facade/getChannelPatner/");
        }

        public ISM_Sales_Lead_DTO save_ChannelPatnerMapping(ISM_Sales_Lead_DTO dto)
        {
            return COMML.POSTData(dto, "Sales_Lead_Facade/save_ChannelPatnerMapping/");
        }
        public ISM_Sales_Lead_DTO leadChannelmapping_edit(ISM_Sales_Lead_DTO dto)
        {
            return COMML.POSTData(dto, "Sales_Lead_Facade/leadChannelmapping_edit/");
        }
        //---------------Sales Quotation-----------------------
        public ISM_Sales_Lead_DTO getverifyleaddetails(ISM_Sales_Lead_DTO dto)
        {
            return COMML.POSTData(dto, "Sales_Lead_Facade/getverifyleaddetails/");
        }
        public ISM_Sales_Lead_DTO getleaddetails(ISM_Sales_Lead_DTO data)
        {
            return COMML.POSTData(data, "Sales_Lead_Facade/getleaddetails/");
        }
        public ISM_Sales_Lead_DTO getleaddetailsfinal(ISM_Sales_Lead_DTO data)
        {
            return COMML.POSTData(data, "Sales_Lead_Facade/getleaddetailsfinal/");
        }

        public ISM_Sales_Lead_DTO sendmailDetails(ISM_Sales_Lead_DTO data)
        {
            return COMML.POSTData(data, "Sales_Lead_Facade/sendmailDetails/");
        }

        public ISM_Sales_Lead_DTO updatedetails(ISM_Sales_Lead_DTO dto)
        {
            return COMML.POSTData(dto, "Sales_Lead_Facade/updatedetails/");
        }
        public ISM_Sales_Lead_DTO get_leads(ISM_Sales_Lead_DTO data)
        {
            return COMML.POSTData(data, "Sales_Lead_Facade/get_leads/");
        }
        public ISM_Sales_Lead_DTO Reschedule(ISM_Sales_Lead_DTO data)
        {
            return COMML.POSTData(data, "Sales_Lead_Facade/Reschedule/");
        }

        public ISM_Sales_Lead_Demo_DTO onreport(ISM_Sales_Lead_Demo_DTO dto)
        {
            return COMMLD.POSTData(dto, "Sales_Lead_Facade/onreport/");
        }
        public ISM_Sales_Lead_Demo_DTO getleaddetail(ISM_Sales_Lead_Demo_DTO dto)
        {
            return COMMLD.POSTData(dto, "Sales_Lead_Facade/getleaddetail/");
        }

        public ISM_Sales_Lead_DTO LeadUpdatemail(ISM_Sales_Lead_DTO data)
        {
            return COMML.POSTData(data, "Sales_Lead_Facade/LeadUpdatemail/");
        }
        public ISM_Sales_Lead_DTO CancelNew(ISM_Sales_Lead_DTO data)
        {
            return COMML.POSTData(data, "Sales_Lead_Facade/CancelNew/");
        }
        public ISM_Sales_Lead_DTO Sales_Raw_Report_OnclickGridData(ISM_Sales_Lead_DTO data)
        {
            return COMML.POSTData(data, "Sales_Lead_Facade/Sales_Raw_Report_OnclickGridData/");
        }
        public ISM_Sales_Lead_DTO Swap(ISM_Sales_Lead_DTO data)
        {
            return COMML.POSTData(data, "Sales_Lead_Facade/Swap/");
        }
        public ISM_Sales_Lead_DTO SwappingEmpList(ISM_Sales_Lead_DTO data)
        {
            return COMML.POSTData(data, "Sales_Lead_Facade/SwappingEmpList/");
        }
        

        public ISM_Sales_Lead_Demo_DTO DemoCancelReportNew(ISM_Sales_Lead_Demo_DTO dto)
        {
            return COMMLD.POSTData(dto, "Sales_Lead_Facade/DemoCancelReportNew/");
        }
    }
}