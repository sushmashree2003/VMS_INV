using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Exam;
using PreadmissionDTOs.com.vaps.VMS.Sales;
using Recruitment.com.vaps.Interfaces;

namespace Recruitment.com.vaps.Controllers
{
    [Produces("application/json")]
    [Route("api/ISM_Client_ProjectFacade")]
    public class ISM_Client_ProjectFacadeController : Controller
    {
        public ISM_Client_ProjectInterface _CPI;

        public ISM_Client_ProjectFacadeController(ISM_Client_ProjectInterface cpi)
        {
            _CPI = cpi;
        }
        [Route("getdate_cmc")]
        public ISM_Client_Project_DTO getdate_cmc([FromBody] ISM_Client_Project_DTO dto)
        {
            return _CPI.getdate_cmc(dto);
        }
        [Route("details_cmc")]
        public ISM_Client_Project_DTO details_cmc([FromBody] ISM_Client_Project_DTO dto)
        {
            return _CPI.details_cmc(dto);
        }
        [Route("SaveEdit_cmc")]
        public ISM_Client_Project_DTO SaveEdit_cmc([FromBody] ISM_Client_Project_DTO dto)
        {
            return _CPI.SaveEdit_cmc(dto);
        }
        [Route("deactivate_cmc")]
        public ISM_Client_Project_DTO deactivate_cmc([FromBody] ISM_Client_Project_DTO dto)
        {
            return _CPI.deactivate_cmc(dto);
        }

        //======================== BOM
        [Route("getdata_BOM")]
        public ISM_Client_Project_DTO getdata_BOM([FromBody] ISM_Client_Project_DTO dto)
        {
            return _CPI.getdata_BOM(dto);
        }

        [Route("details_BOM")]
        public ISM_Client_Project_DTO details_BOM([FromBody] ISM_Client_Project_DTO dto)
        {
            return _CPI.details_BOM(dto);
        }
        [Route("SaveEdit_BOM")]
        public ISM_Client_Project_DTO SaveEdit_BOM([FromBody] ISM_Client_Project_DTO dto)
        {
            return _CPI.SaveEdit_BOM(dto);
        }
        [Route("deactivate_BOM")]
        public ISM_Client_Project_DTO deactivate_BOM([FromBody] ISM_Client_Project_DTO dto)
        {
            return _CPI.deactivate_BOM(dto);
        }

        
        //======================== Man power
        [Route("getdata_MP")]
        public ISM_Client_Project_DTO getdata_MP([FromBody] ISM_Client_Project_DTO dto)
        {
            return _CPI.getdata_MP(dto);
        }

        [Route("details_MP")]
        public ISM_Client_Project_DTO details_MP([FromBody] ISM_Client_Project_DTO dto)
        {
            return _CPI.details_MP(dto);
        }
        [Route("SaveEdit_MP")]
        public ISM_Client_Project_DTO SaveEdit_MP([FromBody] ISM_Client_Project_DTO dto)
        {
            return _CPI.SaveEdit_MP(dto);
        }
        [Route("deactivate_MP")]
        public ISM_Client_Project_DTO deactivate_MP([FromBody] ISM_Client_Project_DTO dto)
        {
            return _CPI.deactivate_MP(dto);
        }
        //======================== DOC
        [Route("getdata_DOC")]
        public ISM_Client_Project_DTO getdata_DOC([FromBody] ISM_Client_Project_DTO dto)
        {
            return _CPI.getdata_DOC(dto);
        }

        [Route("details_DOC")]
        public ISM_Client_Project_DTO details_DOC([FromBody] ISM_Client_Project_DTO dto)
        {
            return _CPI.details_DOC(dto);
        }
        [Route("SaveEdit_DOC")]
        public ISM_Client_Project_DTO SaveEdit_DOC([FromBody] ISM_Client_Project_DTO dto)
        {
            return _CPI.SaveEdit_DOC(dto);
        }
        [Route("deactivate_DOC")]
        public ISM_Client_Project_DTO deactivate_DOC([FromBody] ISM_Client_Project_DTO dto)
        {
            return _CPI.deactivate_DOC(dto);
        }
         //========================MASTER DOC
        [Route("getdata_MDOC")]
        public ISM_Client_Project_DTO getdata_MDOC([FromBody] ISM_Client_Project_DTO dto)
        {
            return _CPI.getdata_MDOC(dto);
        }

        [Route("details_MDOC")]
        public ISM_Client_Project_DTO details_MDOC([FromBody] ISM_Client_Project_DTO dto)
        {
            return _CPI.details_MDOC(dto);
        }
        [Route("SaveEdit_MDOC")]
        public ISM_Client_Project_DTO SaveEdit_MDOC([FromBody] ISM_Client_Project_DTO dto)
        {
            return _CPI.SaveEdit_MDOC(dto);
        }
        [Route("deactivate_MDOC")]
        public ISM_Client_Project_DTO deactivate_MDOC([FromBody] ISM_Client_Project_DTO dto)
        {
            return _CPI.deactivate_MDOC(dto);
        }
        //=============Lead Comments
        [Route("getdata_CMT")]
        public ISM_Client_Project_DTO getdata_CMT([FromBody] ISM_Client_Project_DTO dto)
        {
            return _CPI.getdata_CMT(dto);
        }

        [Route("details_CMT")]
        public ISM_Client_Project_DTO details_CMT([FromBody] ISM_Client_Project_DTO dto)
        {
            return _CPI.details_CMT(dto);
        }
        [Route("SaveEdit_CMT")]
        public ISM_Client_Project_DTO SaveEdit_CMT([FromBody] ISM_Client_Project_DTO dto)
        {
            return _CPI.SaveEdit_CMT(dto);
        }
        [Route("deactivate_CMT")]
        public ISM_Client_Project_DTO deactivate_CMT([FromBody] ISM_Client_Project_DTO dto)
        {
            return _CPI.deactivate_CMT(dto);
        }
        [Route("getcmtrpt_data")]
        public ISM_Client_Project_DTO getcmtrpt_data([FromBody] ISM_Client_Project_DTO dto)
        {
            return _CPI.getcmtrpt_data(dto);
        }
        [Route("report_cmtrpt")]
        public async Task<ISM_Client_Project_DTO> report_cmtrpt([FromBody] ISM_Client_Project_DTO dto)
        {
            return await _CPI.report_cmtrpt(dto);
        }

        [Route("Sales_Lead_Remark")]
        public ISM_Client_Project_DTO Sales_Lead_Remark([FromBody] ISM_Client_Project_DTO dto)
        {
            return _CPI.Sales_Lead_Remark(dto);
        }


        [Route("report_cmtrptDetails")]
        public ISM_Client_Project_DTO report_cmtrptDetails([FromBody] ISM_Client_Project_DTO dto)
        {
            return _CPI.report_cmtrptDetails(dto);
        }
        [Route("Total_commentreport")]
        public ISM_Client_Project_DTO Total_commentreport([FromBody] ISM_Client_Project_DTO dto)
        {
            return _CPI.Total_commentreport(dto);
        }
        [Route("report_cmtrptDetailsverify")]
        public ISM_Client_Project_DTO report_cmtrptDetailsverify([FromBody] ISM_Client_Project_DTO dto)
        {
            return _CPI.report_cmtrptDetailsverify(dto);
        }
        [Route("report_cmtrptDetailsverifyDetails")]
        public ISM_Client_Project_DTO report_cmtrptDetailsverifyDetails([FromBody] ISM_Client_Project_DTO dto)
        {
            return _CPI.report_cmtrptDetailsverifyDetails(dto);
        }
        

        [Route("searchleadvalue")]
        public ISM_Client_Project_DTO searchleadvalue([FromBody] ISM_Client_Project_DTO dto)
        {
            return _CPI.searchleadvalue(dto);
        }
        [Route("get_desig")]
        public ISM_Client_Project_DTO get_desig([FromBody] ISM_Client_Project_DTO dto)
        {
            return _CPI.get_desig(dto);
        }
        [Route("get_depart")]
        public ISM_Client_Project_DTO get_depart([FromBody] ISM_Client_Project_DTO dto)
        {
            return _CPI.get_depart(dto);
        }

        [Route("get_employee")]
        public ISM_Client_Project_DTO get_employee([FromBody] ISM_Client_Project_DTO dto)
        {
            return _CPI.get_employee(dto);
        }


        [Route("getleads")]
        public ISM_Client_Project_DTO getleads([FromBody] ISM_Client_Project_DTO dto)
        {
            return _CPI.getleads(dto);
        }

        [Route("Transferload")]
        public ISM_Client_Project_DTO Transferload([FromBody] ISM_Client_Project_DTO dto)
        {
            return _CPI.Transferload(dto);
        }


        [Route("gettransferLeads")]
        public ISM_Client_Project_DTO gettransferLeads([FromBody] ISM_Client_Project_DTO dto)
        {
            return _CPI.gettransferLeads(dto);
        }
        [Route("getTransferDetails")]
        public ISM_Client_Project_DTO getTransferDetails([FromBody] ISM_Client_Project_DTO dto)
        {
            return _CPI.getTransferDetails(dto);
        }
        [Route("LeadTransferSave")]
        public ISM_Client_Project_DTO LeadTransferSave([FromBody] ISM_Client_Project_DTO dto)
        {
            return _CPI.LeadTransferSave(dto);
        }


        [Route("report_database")]
        public ISM_Client_Project_DTO report_database([FromBody] ISM_Client_Project_DTO dto)
        {
            return _CPI.report_database(dto);
        }
        [Route("report_Quotationrequest")]
        public ISM_Client_Project_DTO report_Quotationrequest([FromBody] ISM_Client_Project_DTO dto)
        {
            return _CPI.report_Quotationrequest(dto);
        }



        [Route("report_database_details")]
        public ISM_Client_Project_DTO report_database_details([FromBody] ISM_Client_Project_DTO dto)
        {
            return _CPI.report_database_details(dto);
        }
        [Route("report_Quotationrequest_details")]
        public ISM_Client_Project_DTO report_Quotationrequest_details([FromBody] ISM_Client_Project_DTO dto)
        {
            return _CPI.report_Quotationrequest_details(dto);
        }

        [Route("StatusUpdateOnLoad")]
        public ISM_Client_Project_DTO StatusUpdateOnLoad([FromBody] ISM_Client_Project_DTO dto)
        {
            return _CPI.StatusUpdateOnLoad(dto);
        }
       

        [Route("StatusDropDownData")]
        public ISM_Client_Project_DTO StatusDropDownData([FromBody] ISM_Client_Project_DTO dto)
        {
            return _CPI.StatusDropDownData(dto);
        }

        [Route("StatusUpdate")]
        public ISM_Client_Project_DTO StatusUpdate([FromBody] ISM_Client_Project_DTO dto)
        {
            return _CPI.StatusUpdate(dto);
        }

        [Route("getClientData")]
        public ISM_Client_Project_DTO getClientData([FromBody] ISM_Client_Project_DTO dto)
        {
            return _CPI.getClientData(dto);
        }

        [Route("getClientReport")]
        public ISM_Client_Project_DTO getClientReport([FromBody] ISM_Client_Project_DTO dto)
        {
            return _CPI.getClientReport(dto);
        }

        [Route("SalesPortfolioReport")]
        public ISM_Client_Project_DTO SalesPortfolioReport([FromBody] ISM_Client_Project_DTO dto)
        {
            return _CPI.SalesPortfolioReport(dto);
        }

        [Route("SalesRefferalLeadOnload")]
        public ISM_Client_Project_DTO SalesRefferalLeadOnload([FromBody] ISM_Client_Project_DTO dto)
        {
            return _CPI.SalesRefferalLeadOnload(dto);
        }

        [Route("SaveSalesRefferalLead")]
        public ISM_Client_Project_DTO SaveSalesRefferalLead([FromBody] ISM_Client_Project_DTO dto)
        {
            return _CPI.SaveSalesRefferalLead(dto);
        }

        [Route("SalesReferralLeadReport")]
        public ISM_Client_Project_DTO SalesReferralLeadReport([FromBody] ISM_Client_Project_DTO dto)
        {
            return _CPI.SalesReferralLeadReport(dto);
        }


        [Route("Ism_sales_ChannelParterReport/")]
        public ISM_Client_Project_DTO Ism_sales_ChannelParterReport([FromBody] ISM_Client_Project_DTO dto)
        {
            return _CPI.Ism_sales_ChannelParterReport(dto);
        }
        [Route("Ism_Sales_ChannelParter_lead_Report")]
        public ISM_Client_Project_DTO Ism_Sales_ChannelParter_lead_Report([FromBody] ISM_Client_Project_DTO dto)
        {
            return _CPI.Ism_Sales_ChannelParter_lead_Report(dto);
        }


        [Route("Save_HealthCheckupAssign")]

        public ISM_Client_Project_DTO Save_HealthCheckupAssign([FromBody] ISM_Client_Project_DTO dto)

        {

            return _CPI.Save_HealthCheckupAssign(dto);

        }

        [Route("Save_Elearning_Enhancement")]

        public ISM_Client_Project_DTO Save_Elearning_Enhancement([FromBody] ISM_Client_Project_DTO dto)

        {

            return _CPI.Save_Elearning_Enhancement(dto);

        }

        [Route("Impending_module")]

        public ISM_Client_Project_DTO Impending_module([FromBody] ISM_Client_Project_DTO dto)

        {

            return _CPI.Impending_module(dto);

        }



        [Route("editImplementationDetails")]

        public ISM_Client_Project_DTO editImplementationDetails([FromBody] ISM_Client_Project_DTO dto)

        {

            return _CPI.editImplementationDetails(dto);

        }

        [Route("deactivate_Healthcheckup")]

        public ISM_Client_Project_DTO deactivate_Healthcheckup([FromBody] ISM_Client_Project_DTO dto)

        {

            return _CPI.deactivate_Healthcheckup(dto);

        }


        [Route("Getmodules")]

        public ISM_Client_Project_DTO Getmodules([FromBody] ISM_Client_Project_DTO dto)

        {

            return _CPI.Getmodules(dto);

        }



        [Route("Save_accounts")]

        public ISM_Client_Project_DTO Save_accounts([FromBody] ISM_Client_Project_DTO dto)

        {

            return _CPI.Save_accounts(dto);

        }



        [Route("savedetail")]

        public async Task<General_SendSMSDTO> savedetail([FromBody] General_SendSMSDTO message)

        {

            return await _CPI.savedetail(message);

        }


    }
}