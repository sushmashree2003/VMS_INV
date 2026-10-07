using CommonLibrary;
using PreadmissionDTOs.com.vaps.Exam;
using PreadmissionDTOs.com.vaps.VMS.Sales;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.VMS
{
    public class ISM_Client_ProjectDelegate
    {
        CommonVMSDelegate<ISM_Client_Project_DTO, ISM_Client_Project_DTO> CMMICP = new CommonVMSDelegate<ISM_Client_Project_DTO, ISM_Client_Project_DTO>();
        CommonVMSDelegate<General_SendSMSDTO, General_SendSMSDTO> CMMICP1 = new CommonVMSDelegate<General_SendSMSDTO, General_SendSMSDTO>();
        public ISM_Client_Project_DTO getdate_cmc(ISM_Client_Project_DTO dto)
        {
            return CMMICP.POSTData(dto, "ISM_Client_ProjectFacade/getdate_cmc/");
        }
        public ISM_Client_Project_DTO details_cmc(ISM_Client_Project_DTO dto)
        {
            return CMMICP.POSTData(dto, "ISM_Client_ProjectFacade/details_cmc/");
        }
         public ISM_Client_Project_DTO SaveEdit_cmc(ISM_Client_Project_DTO dto)
        {
            return CMMICP.POSTData(dto, "ISM_Client_ProjectFacade/SaveEdit_cmc/");
        }
         public ISM_Client_Project_DTO deactivate_cmc(ISM_Client_Project_DTO dto)
        {
            return CMMICP.POSTData(dto, "ISM_Client_ProjectFacade/deactivate_cmc/");
        }

        //==================BOM

        public ISM_Client_Project_DTO getdata_BOM(ISM_Client_Project_DTO dto)
        {
            return CMMICP.POSTData(dto, "ISM_Client_ProjectFacade/getdata_BOM/");
        }

        public ISM_Client_Project_DTO details_BOM(ISM_Client_Project_DTO dto)
        {
            return CMMICP.POSTData(dto, "ISM_Client_ProjectFacade/details_BOM/");
        }
        public ISM_Client_Project_DTO SaveEdit_BOM(ISM_Client_Project_DTO dto)
        {
            return CMMICP.POSTData(dto, "ISM_Client_ProjectFacade/SaveEdit_BOM/");
        }
        public ISM_Client_Project_DTO deactivate_BOM(ISM_Client_Project_DTO dto)
        {
            return CMMICP.POSTData(dto, "ISM_Client_ProjectFacade/deactivate_BOM/");
        }
        
        //==================Man power

        public ISM_Client_Project_DTO getdata_MP(ISM_Client_Project_DTO dto)
        {
            return CMMICP.POSTData(dto, "ISM_Client_ProjectFacade/getdata_MP/");
        }

        public ISM_Client_Project_DTO details_MP(ISM_Client_Project_DTO dto)
        {
            return CMMICP.POSTData(dto, "ISM_Client_ProjectFacade/details_MP/");
        }
        public ISM_Client_Project_DTO SaveEdit_MP(ISM_Client_Project_DTO dto)
        {
            return CMMICP.POSTData(dto, "ISM_Client_ProjectFacade/SaveEdit_MP/");
        }
        public ISM_Client_Project_DTO deactivate_MP(ISM_Client_Project_DTO dto)
        {
            return CMMICP.POSTData(dto, "ISM_Client_ProjectFacade/deactivate_MP/");
        }
        
        //==================DOC

        public ISM_Client_Project_DTO getdata_DOC(ISM_Client_Project_DTO dto)
        {
            return CMMICP.POSTData(dto, "ISM_Client_ProjectFacade/getdata_DOC/");
        }

        public ISM_Client_Project_DTO details_DOC(ISM_Client_Project_DTO dto)
        {
            return CMMICP.POSTData(dto, "ISM_Client_ProjectFacade/details_DOC/");
        }
        public ISM_Client_Project_DTO SaveEdit_DOC(ISM_Client_Project_DTO dto)
        {
            return CMMICP.POSTData(dto, "ISM_Client_ProjectFacade/SaveEdit_DOC/");
        }
        public ISM_Client_Project_DTO deactivate_DOC(ISM_Client_Project_DTO dto)
        {
            return CMMICP.POSTData(dto, "ISM_Client_ProjectFacade/deactivate_DOC/");
        }

        //================== MASTER DOC

        public ISM_Client_Project_DTO getdata_MDOC(ISM_Client_Project_DTO dto)
        {
            return CMMICP.POSTData(dto, "ISM_Client_ProjectFacade/getdata_MDOC/");
        }

        public ISM_Client_Project_DTO details_MDOC(ISM_Client_Project_DTO dto)
        {
            return CMMICP.POSTData(dto, "ISM_Client_ProjectFacade/details_MDOC/");
        }
        public ISM_Client_Project_DTO SaveEdit_MDOC(ISM_Client_Project_DTO dto)
        {
            return CMMICP.POSTData(dto, "ISM_Client_ProjectFacade/SaveEdit_MDOC/");
        }
        public ISM_Client_Project_DTO deactivate_MDOC(ISM_Client_Project_DTO dto)
        {
            return CMMICP.POSTData(dto, "ISM_Client_ProjectFacade/deactivate_MDOC/");
        }
        //=============Lead Comments
        public ISM_Client_Project_DTO getdata_CMT(ISM_Client_Project_DTO dto)
        {
            return CMMICP.POSTData(dto, "ISM_Client_ProjectFacade/getdata_CMT/");
        }

        public ISM_Client_Project_DTO details_CMT(ISM_Client_Project_DTO dto)
        {
            return CMMICP.POSTData(dto, "ISM_Client_ProjectFacade/details_CMT/");
        }
        public ISM_Client_Project_DTO SaveEdit_CMT(ISM_Client_Project_DTO dto)
        {
            return CMMICP.POSTData(dto, "ISM_Client_ProjectFacade/SaveEdit_CMT/");
        }
        public ISM_Client_Project_DTO deactivate_CMT(ISM_Client_Project_DTO dto)
        {
            return CMMICP.POSTData(dto, "ISM_Client_ProjectFacade/deactivate_CMT/");
        }
         public ISM_Client_Project_DTO getcmtrpt_data(ISM_Client_Project_DTO dto)
        {
            return CMMICP.POSTData(dto, "ISM_Client_ProjectFacade/getcmtrpt_data/");
        }
         public ISM_Client_Project_DTO report_cmtrpt(ISM_Client_Project_DTO dto)
        {
            return CMMICP.POSTData(dto, "ISM_Client_ProjectFacade/report_cmtrpt/");
        }
        public ISM_Client_Project_DTO Sales_Lead_Remark(ISM_Client_Project_DTO dto)
        {
            return CMMICP.POSTData(dto, "ISM_Client_ProjectFacade/Sales_Lead_Remark/");
        }

        
        public ISM_Client_Project_DTO report_cmtrptDetails(ISM_Client_Project_DTO dto)
        {
            return CMMICP.POSTData(dto, "ISM_Client_ProjectFacade/report_cmtrptDetails/");
        }
        public ISM_Client_Project_DTO Total_commentreport(ISM_Client_Project_DTO dto)
        {
            return CMMICP.POSTData(dto, "ISM_Client_ProjectFacade/Total_commentreport/");
        }

        public ISM_Client_Project_DTO report_cmtrptDetailsverify(ISM_Client_Project_DTO dto)
        {
            return CMMICP.POSTData(dto, "ISM_Client_ProjectFacade/report_cmtrptDetailsverify/");
        }
        public ISM_Client_Project_DTO report_cmtrptDetailsverifyDetails(ISM_Client_Project_DTO dto)
        {
            return CMMICP.POSTData(dto, "ISM_Client_ProjectFacade/report_cmtrptDetailsverifyDetails/");
        }
        


        public ISM_Client_Project_DTO searchleadvalue(ISM_Client_Project_DTO dto)
        {
            return CMMICP.POSTData(dto, "ISM_Client_ProjectFacade/searchleadvalue/");
        }
        public ISM_Client_Project_DTO get_desig(ISM_Client_Project_DTO dto)
        {
            return CMMICP.POSTData(dto, "ISM_Client_ProjectFacade/get_desig/");
        }

        public ISM_Client_Project_DTO get_depart(ISM_Client_Project_DTO dto)
        {
            return CMMICP.POSTData(dto, "ISM_Client_ProjectFacade/get_depart/");
        }

        
        public ISM_Client_Project_DTO get_employee(ISM_Client_Project_DTO dto)
        {
            return CMMICP.POSTData(dto, "ISM_Client_ProjectFacade/get_employee/");
        }


        public ISM_Client_Project_DTO getleads(ISM_Client_Project_DTO dto)
        {
            return CMMICP.POSTData(dto, "ISM_Client_ProjectFacade/getleads/");
        }
        public ISM_Client_Project_DTO Transferload(ISM_Client_Project_DTO dto)
        {
            return CMMICP.POSTData(dto, "ISM_Client_ProjectFacade/Transferload/");
        }
        public ISM_Client_Project_DTO gettransferLeads(ISM_Client_Project_DTO dto)
        {
            return CMMICP.POSTData(dto, "ISM_Client_ProjectFacade/gettransferLeads/");
        }
        public ISM_Client_Project_DTO getTransferDetails(ISM_Client_Project_DTO dto)
        {
            return CMMICP.POSTData(dto, "ISM_Client_ProjectFacade/getTransferDetails/");
        }
        public ISM_Client_Project_DTO LeadTransferSave(ISM_Client_Project_DTO dto)
        {
            return CMMICP.POSTData(dto, "ISM_Client_ProjectFacade/LeadTransferSave/");
        }


        public ISM_Client_Project_DTO report_database(ISM_Client_Project_DTO dto)
        {
            return CMMICP.POSTData(dto, "ISM_Client_ProjectFacade/report_database/");
        }
        public ISM_Client_Project_DTO report_Quotationrequest(ISM_Client_Project_DTO dto)
        {
            return CMMICP.POSTData(dto, "ISM_Client_ProjectFacade/report_Quotationrequest/");
        }



        public ISM_Client_Project_DTO report_database_details(ISM_Client_Project_DTO dto)
        {
            return CMMICP.POSTData(dto, "ISM_Client_ProjectFacade/report_database_details/");
        }
        public ISM_Client_Project_DTO report_Quotationrequest_details(ISM_Client_Project_DTO dto)
        {
            return CMMICP.POSTData(dto, "ISM_Client_ProjectFacade/report_Quotationrequest_details/");
        }


        public ISM_Client_Project_DTO StatusUpdateOnLoad(ISM_Client_Project_DTO dto)
        {
            return CMMICP.POSTData(dto, "ISM_Client_ProjectFacade/StatusUpdateOnLoad/");
        }
        //
        public ISM_Client_Project_DTO StatusDropDownData(ISM_Client_Project_DTO dto)
        {
            return CMMICP.POSTData(dto, "ISM_Client_ProjectFacade/StatusDropDownData/");
        }
        public ISM_Client_Project_DTO StatusUpdate(ISM_Client_Project_DTO dto)
        {
            return CMMICP.POSTData(dto, "ISM_Client_ProjectFacade/StatusUpdate/");
        }
        public ISM_Client_Project_DTO getClientData(ISM_Client_Project_DTO dto)
        {
            return CMMICP.POSTData(dto, "ISM_Client_ProjectFacade/getClientData/");
        }
        public ISM_Client_Project_DTO getClientReport(ISM_Client_Project_DTO dto)
        {
            return CMMICP.POSTData(dto, "ISM_Client_ProjectFacade/getClientReport/");
        }

        public ISM_Client_Project_DTO SalesPortfolioReport(ISM_Client_Project_DTO dto)
        {
            return CMMICP.POSTData(dto, "ISM_Client_ProjectFacade/SalesPortfolioReport/");
        }
        public ISM_Client_Project_DTO SalesRefferalLeadOnload(ISM_Client_Project_DTO dto)
        {
            return CMMICP.POSTData(dto, "ISM_Client_ProjectFacade/SalesRefferalLeadOnload/");
        }
        public ISM_Client_Project_DTO SaveSalesRefferalLead(ISM_Client_Project_DTO dto)
        {
            return CMMICP.POSTData(dto, "ISM_Client_ProjectFacade/SaveSalesRefferalLead/");
        }

        public ISM_Client_Project_DTO SalesReferralLeadReport(ISM_Client_Project_DTO dto)
        {
            return CMMICP.POSTData(dto, "ISM_Client_ProjectFacade/SalesReferralLeadReport/");
        }

        public ISM_Client_Project_DTO Ism_sales_ChannelParterReport(ISM_Client_Project_DTO dto)
        {
            return CMMICP.POSTData(dto, "ISM_Client_ProjectFacade/Ism_sales_ChannelParterReport/");
        }

        public ISM_Client_Project_DTO Ism_Sales_ChannelParter_lead_Report(ISM_Client_Project_DTO dto)
        {
            return CMMICP.POSTData(dto, "ISM_Client_ProjectFacade/Ism_Sales_ChannelParter_lead_Report/");
        }

        public ISM_Client_Project_DTO editImplementationDetails(ISM_Client_Project_DTO dto)

        {

            return CMMICP.POSTData(dto, "ISM_Client_ProjectFacade/editImplementationDetails/");

        }


        //added by praveenkumar health checkup assign 

        public ISM_Client_Project_DTO Save_HealthCheckupAssign(ISM_Client_Project_DTO dto)

        {

            return CMMICP.POSTData(dto, "ISM_Client_ProjectFacade/Save_HealthCheckupAssign/");

        }

        public ISM_Client_Project_DTO Save_Elearning_Enhancement(ISM_Client_Project_DTO dto)

        {

            return CMMICP.POSTData(dto, "ISM_Client_ProjectFacade/Save_Elearning_Enhancement/");

        }

        public ISM_Client_Project_DTO Impending_module(ISM_Client_Project_DTO dto)

        {

            return CMMICP.POSTData(dto, "ISM_Client_ProjectFacade/Impending_module/");

        }


        public ISM_Client_Project_DTO deactivate_Healthcheckup(ISM_Client_Project_DTO dto)

        {

            return CMMICP.POSTData(dto, "ISM_Client_ProjectFacade/deactivate_Healthcheckup/");

        }

        public ISM_Client_Project_DTO Getmodules(ISM_Client_Project_DTO dto)
        {

            return CMMICP.POSTData(dto, "ISM_Client_ProjectFacade/Getmodules/");

        }

    public ISM_Client_Project_DTO Save_accounts(ISM_Client_Project_DTO dto)

    {

        return CMMICP.POSTData(dto, "ISM_Client_ProjectFacade/Save_accounts/");

    }



      public General_SendSMSDTO savedetail(General_SendSMSDTO dto)

      {

        return CMMICP1.POSTData(dto, "ISM_Client_ProjectFacade/savedetail/");

      }


}

}
