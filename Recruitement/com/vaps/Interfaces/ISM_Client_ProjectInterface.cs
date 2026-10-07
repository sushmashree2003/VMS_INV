using PreadmissionDTOs.com.vaps.Exam;
using PreadmissionDTOs.com.vaps.VMS.Sales;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Recruitment.com.vaps.Interfaces
{
    public interface ISM_Client_ProjectInterface
    {
        //client Components module
        ISM_Client_Project_DTO getdate_cmc(ISM_Client_Project_DTO dto);
        ISM_Client_Project_DTO details_cmc(ISM_Client_Project_DTO dto);
        ISM_Client_Project_DTO SaveEdit_cmc(ISM_Client_Project_DTO dto);
        ISM_Client_Project_DTO deactivate_cmc(ISM_Client_Project_DTO dto);

        //========================BOM
        ISM_Client_Project_DTO getdata_BOM(ISM_Client_Project_DTO dto);
        ISM_Client_Project_DTO SaveEdit_BOM(ISM_Client_Project_DTO dto);
        ISM_Client_Project_DTO details_BOM(ISM_Client_Project_DTO dto);
        ISM_Client_Project_DTO deactivate_BOM(ISM_Client_Project_DTO dto); 
        
        //========================Man power
        ISM_Client_Project_DTO getdata_MP(ISM_Client_Project_DTO dto);
        ISM_Client_Project_DTO SaveEdit_MP(ISM_Client_Project_DTO dto);
        ISM_Client_Project_DTO details_MP(ISM_Client_Project_DTO dto);
        ISM_Client_Project_DTO deactivate_MP(ISM_Client_Project_DTO dto);

         //======================== DOC
        ISM_Client_Project_DTO getdata_DOC(ISM_Client_Project_DTO dto);
        ISM_Client_Project_DTO SaveEdit_DOC(ISM_Client_Project_DTO dto);
        ISM_Client_Project_DTO details_DOC(ISM_Client_Project_DTO dto);
        ISM_Client_Project_DTO deactivate_DOC(ISM_Client_Project_DTO dto);
         //======================== MASTER DOC
        ISM_Client_Project_DTO getdata_MDOC(ISM_Client_Project_DTO dto);
        ISM_Client_Project_DTO SaveEdit_MDOC(ISM_Client_Project_DTO dto);
        ISM_Client_Project_DTO details_MDOC(ISM_Client_Project_DTO dto);
        ISM_Client_Project_DTO deactivate_MDOC(ISM_Client_Project_DTO dto);
        //=============Lead Comments
        ISM_Client_Project_DTO getdata_CMT(ISM_Client_Project_DTO dto);
        ISM_Client_Project_DTO SaveEdit_CMT(ISM_Client_Project_DTO dto);
        ISM_Client_Project_DTO details_CMT(ISM_Client_Project_DTO dto);
        ISM_Client_Project_DTO deactivate_CMT(ISM_Client_Project_DTO dto);
        ISM_Client_Project_DTO getcmtrpt_data(ISM_Client_Project_DTO dto);
        Task<ISM_Client_Project_DTO> report_cmtrpt(ISM_Client_Project_DTO dto);
        ISM_Client_Project_DTO Sales_Lead_Remark(ISM_Client_Project_DTO dto);
        ISM_Client_Project_DTO report_cmtrptDetails(ISM_Client_Project_DTO dto);
        ISM_Client_Project_DTO Total_commentreport(ISM_Client_Project_DTO dto);
        ISM_Client_Project_DTO report_cmtrptDetailsverify(ISM_Client_Project_DTO dto);
        ISM_Client_Project_DTO report_cmtrptDetailsverifyDetails(ISM_Client_Project_DTO dto);



        ISM_Client_Project_DTO searchleadvalue(ISM_Client_Project_DTO dto);
        ISM_Client_Project_DTO get_employee(ISM_Client_Project_DTO dto);
        
        ISM_Client_Project_DTO Transferload(ISM_Client_Project_DTO dto);
        ISM_Client_Project_DTO get_desig(ISM_Client_Project_DTO dto);
        ISM_Client_Project_DTO get_depart(ISM_Client_Project_DTO dto);
        ISM_Client_Project_DTO getleads(ISM_Client_Project_DTO dto);
        ISM_Client_Project_DTO gettransferLeads(ISM_Client_Project_DTO dto);
        ISM_Client_Project_DTO getTransferDetails(ISM_Client_Project_DTO dto);
        ISM_Client_Project_DTO LeadTransferSave(ISM_Client_Project_DTO dto);


        ISM_Client_Project_DTO report_database(ISM_Client_Project_DTO dto);
        ISM_Client_Project_DTO report_Quotationrequest(ISM_Client_Project_DTO dto);

        ISM_Client_Project_DTO report_database_details(ISM_Client_Project_DTO dto);
        ISM_Client_Project_DTO report_Quotationrequest_details(ISM_Client_Project_DTO dto);
        ISM_Client_Project_DTO StatusUpdateOnLoad (ISM_Client_Project_DTO dto);
        ISM_Client_Project_DTO StatusDropDownData(ISM_Client_Project_DTO dto);
        ISM_Client_Project_DTO StatusUpdate(ISM_Client_Project_DTO dto);
        ISM_Client_Project_DTO getClientData(ISM_Client_Project_DTO dto);
        ISM_Client_Project_DTO getClientReport(ISM_Client_Project_DTO dto);

        ISM_Client_Project_DTO SalesPortfolioReport(ISM_Client_Project_DTO dto);

        ISM_Client_Project_DTO SalesRefferalLeadOnload(ISM_Client_Project_DTO dto);

        ISM_Client_Project_DTO SaveSalesRefferalLead(ISM_Client_Project_DTO dto);

        ISM_Client_Project_DTO SalesReferralLeadReport(ISM_Client_Project_DTO dto);

        ISM_Client_Project_DTO Ism_sales_ChannelParterReport(ISM_Client_Project_DTO dto);
        ISM_Client_Project_DTO Ism_Sales_ChannelParter_lead_Report(ISM_Client_Project_DTO dto);


        ISM_Client_Project_DTO Save_HealthCheckupAssign(ISM_Client_Project_DTO dto);

        ISM_Client_Project_DTO Save_Elearning_Enhancement(ISM_Client_Project_DTO dto);

        ISM_Client_Project_DTO Impending_module(ISM_Client_Project_DTO dto);

        ISM_Client_Project_DTO editImplementationDetails(ISM_Client_Project_DTO dto);
        ISM_Client_Project_DTO deactivate_Healthcheckup(ISM_Client_Project_DTO dto);
        ISM_Client_Project_DTO Getmodules(ISM_Client_Project_DTO dto);
        ISM_Client_Project_DTO Save_accounts(ISM_Client_Project_DTO dto);
        Task<General_SendSMSDTO> savedetail(General_SendSMSDTO data);


    }
}
