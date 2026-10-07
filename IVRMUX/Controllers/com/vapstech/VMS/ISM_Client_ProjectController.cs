using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.VMS;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Exam;
using PreadmissionDTOs.com.vaps.VMS.Sales;

namespace IVRMUX.Controllers.com.vapstech.VMS
{

    [Produces("application/json")]
    [Route("api/ISM_Client_Project")]
    public class ISM_Client_ProjectController : Controller
    {
        ISM_Client_ProjectDelegate ICPD = new ISM_Client_ProjectDelegate();

        [HttpGet]
        [Route("getdate_cmc/{id:int}")]
        public ISM_Client_Project_DTO getdate_cmc(int id)
        {
            ISM_Client_Project_DTO dto = new ISM_Client_Project_DTO();
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return ICPD.getdate_cmc(dto);
        }
        [Route("details_cmc/{id:int}")]
        public ISM_Client_Project_DTO details_cmc(int id)
        {
            ISM_Client_Project_DTO dto = new ISM_Client_Project_DTO();
            dto.ISMCLTC_Id = id;
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return ICPD.details_cmc(dto);
        }
        [Route("SaveEdit_cmc")]
        public ISM_Client_Project_DTO SaveEdit_cmc([FromBody] ISM_Client_Project_DTO dto)
        {

            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return ICPD.SaveEdit_cmc(dto);
        }
        [Route("deactivate_cmc")]
        public ISM_Client_Project_DTO deactivate_cmc([FromBody] ISM_Client_Project_DTO dto)
        {

            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return ICPD.deactivate_cmc(dto);
        }

        //=============BOM

        [Route("getdata_BOM/{id:int}")]
        public ISM_Client_Project_DTO getdata_BOM(int id)
        {
            ISM_Client_Project_DTO dto = new ISM_Client_Project_DTO();
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return ICPD.getdata_BOM(dto);
        }
        [Route("details_BOM/{id:int}")]
        public ISM_Client_Project_DTO details_BOM(int id)
        {
            ISM_Client_Project_DTO dto = new ISM_Client_Project_DTO();
            dto.ISMCLTPRBOM_Id = id;
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return ICPD.details_BOM(dto);
        }
        [Route("SaveEdit_BOM")]
        public ISM_Client_Project_DTO SaveEdit_BOM([FromBody] ISM_Client_Project_DTO dto)
        {

            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return ICPD.SaveEdit_BOM(dto);
        }
        [Route("deactivate_BOM")]
        public ISM_Client_Project_DTO deactivate_BOM([FromBody] ISM_Client_Project_DTO dto)
        {

            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return ICPD.deactivate_BOM(dto);
        }

        //=============man power

        [Route("getdata_MP/{id:int}")]
        public ISM_Client_Project_DTO getdata_MP(int id)
        {
            ISM_Client_Project_DTO dto = new ISM_Client_Project_DTO();
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return ICPD.getdata_MP(dto);
        }
        [Route("details_MP/{id:int}")]
        public ISM_Client_Project_DTO details_MP(int id)
        {
            ISM_Client_Project_DTO dto = new ISM_Client_Project_DTO();
            dto.ISMCLTPRMP_Id = id;
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return ICPD.details_MP(dto);
        }
        [Route("SaveEdit_MP")]
        public ISM_Client_Project_DTO SaveEdit_MP([FromBody] ISM_Client_Project_DTO dto)
        {

            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return ICPD.SaveEdit_MP(dto);
        }
        [Route("deactivate_MP")]
        public ISM_Client_Project_DTO deactivate_MP([FromBody] ISM_Client_Project_DTO dto)
        {

            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return ICPD.deactivate_MP(dto);
        }

        //=============DOC

        [Route("getdata_DOC/{id:int}")]
        public ISM_Client_Project_DTO getdata_DOC(int id)
        {
            ISM_Client_Project_DTO dto = new ISM_Client_Project_DTO();
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return ICPD.getdata_DOC(dto);
        }
        [Route("details_DOC/{id:int}")]
        public ISM_Client_Project_DTO details_DOC(int id)
        {
            ISM_Client_Project_DTO dto = new ISM_Client_Project_DTO();
            dto.ISMCLTPRDOC_Id = id;
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return ICPD.details_DOC(dto);
        }
        [Route("SaveEdit_DOC")]
        public ISM_Client_Project_DTO SaveEdit_DOC([FromBody] ISM_Client_Project_DTO dto)
        {

            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return ICPD.SaveEdit_DOC(dto);
        }
        [Route("deactivate_DOC")]
        public ISM_Client_Project_DTO deactivate_DOC([FromBody] ISM_Client_Project_DTO dto)
        {

            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return ICPD.deactivate_DOC(dto);
        }

        //=============master DOC

        [Route("getdata_MDOC/{id:int}")]
        public ISM_Client_Project_DTO getdata_MDOC(int id)
        {
            ISM_Client_Project_DTO dto = new ISM_Client_Project_DTO();
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return ICPD.getdata_MDOC(dto);
        }
        [Route("details_MDOC/{id:int}")]
        public ISM_Client_Project_DTO details_MDOC(int id)
        {
            ISM_Client_Project_DTO dto = new ISM_Client_Project_DTO();
            dto.ISMCLTPRMDOC_Id = id;
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return ICPD.details_MDOC(dto);
        }
        [Route("SaveEdit_MDOC")]
        public ISM_Client_Project_DTO SaveEdit_MDOC([FromBody] ISM_Client_Project_DTO dto)
        {

            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return ICPD.SaveEdit_MDOC(dto);
        }
        [Route("deactivate_MDOC")]
        public ISM_Client_Project_DTO deactivate_MDOC([FromBody] ISM_Client_Project_DTO dto)
        {

            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return ICPD.deactivate_MDOC(dto);
        }

        //=============Lead Comments

        //[Route("getdata_CMT/{id:int}")]
        //public ISM_Client_Project_DTO getdata_CMT(int id)
        //{
        //    ISM_Client_Project_DTO dto = new ISM_Client_Project_DTO();
        //    dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
        //    dto.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
        //    return ICPD.getdata_CMT(dto);
        //}


        [Route("getdata_CMT")]
        public ISM_Client_Project_DTO getdata_CMT([FromBody] ISM_Client_Project_DTO dto)
        {

            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return ICPD.getdata_CMT(dto);
        }

        [Route("details_CMT/{id:int}")]
        public ISM_Client_Project_DTO details_CMT(int id)
        {
            ISM_Client_Project_DTO dto = new ISM_Client_Project_DTO();
            dto.ISMSLECOM_Id = id;
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return ICPD.details_CMT(dto);
        }
        [Route("SaveEdit_CMT")]
        public ISM_Client_Project_DTO SaveEdit_CMT([FromBody] ISM_Client_Project_DTO dto)
        {

            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return ICPD.SaveEdit_CMT(dto);
        }
        [Route("deactivate_CMT")]
        public ISM_Client_Project_DTO deactivate_CMT([FromBody] ISM_Client_Project_DTO dto)
        {

            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return ICPD.deactivate_CMT(dto);
        }
        [Route("getcmtrpt_data/{id:int}")]
        public ISM_Client_Project_DTO getcmtrpt_data(int id)
        {
            ISM_Client_Project_DTO dto = new ISM_Client_Project_DTO();
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return ICPD.getcmtrpt_data(dto);
        }

        [Route("report_cmtrpt")]
        public ISM_Client_Project_DTO report_cmtrpt([FromBody] ISM_Client_Project_DTO dto)
        {

            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return ICPD.report_cmtrpt(dto);
        }


        [Route("Sales_Lead_Remark")]
        public ISM_Client_Project_DTO Sales_Lead_Remark([FromBody] ISM_Client_Project_DTO dto)
        {

            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return ICPD.Sales_Lead_Remark(dto);
        }




        [Route("report_cmtrptDetails")]
        public ISM_Client_Project_DTO report_cmtrptDetails([FromBody] ISM_Client_Project_DTO dto)
        {

            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return ICPD.report_cmtrptDetails(dto);
        }
        [Route("Total_commentreport")]
        public ISM_Client_Project_DTO Total_commentreport([FromBody] ISM_Client_Project_DTO dto)
        {

            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return ICPD.Total_commentreport(dto);
        }
        [Route("report_cmtrptDetailsverify")]
        public ISM_Client_Project_DTO report_cmtrptDetailsverify([FromBody] ISM_Client_Project_DTO dto)
        {

            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return ICPD.report_cmtrptDetailsverify(dto);
        }

        [Route("report_cmtrptDetailsverifyDetails")]
        public ISM_Client_Project_DTO report_cmtrptDetailsverifyDetails([FromBody] ISM_Client_Project_DTO dto)
        {

            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return ICPD.report_cmtrptDetailsverifyDetails(dto);
        }


        [Route("searchleadvalue")]
        public ISM_Client_Project_DTO searchleadvalue([FromBody] ISM_Client_Project_DTO dto)
        {

            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return ICPD.searchleadvalue(dto);
        }
        [Route("get_desig")]
        public ISM_Client_Project_DTO get_desig([FromBody] ISM_Client_Project_DTO dto)
        {

            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return ICPD.get_desig(dto);
        }
        [Route("get_depart")]
        public ISM_Client_Project_DTO get_depart([FromBody] ISM_Client_Project_DTO dto)
        {

            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return ICPD.get_depart(dto);
        }

        [Route("get_employee")]
        public ISM_Client_Project_DTO get_employee([FromBody] ISM_Client_Project_DTO dto)
        {

            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return ICPD.get_employee(dto);
        }

        [Route("getleads")]
        public ISM_Client_Project_DTO getleads([FromBody] ISM_Client_Project_DTO dto)
        {

            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return ICPD.getleads(dto);
        }
        [Route("Transferload/{id:int}")]
        public ISM_Client_Project_DTO Transferload(int id)
        {

            ISM_Client_Project_DTO dto = new ISM_Client_Project_DTO();
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return ICPD.Transferload(dto);
        }
        [Route("gettransferLeads")]
        public ISM_Client_Project_DTO gettransferLeads([FromBody] ISM_Client_Project_DTO dto)
        {


            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return ICPD.gettransferLeads(dto);
        }
        [Route("getTransferDetails")]
        public ISM_Client_Project_DTO getTransferDetails([FromBody] ISM_Client_Project_DTO dto)
        {

            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return ICPD.getTransferDetails(dto);
        }
        [Route("LeadTransferSave")]
        public ISM_Client_Project_DTO LeadTransferSave([FromBody] ISM_Client_Project_DTO dto)
        {

            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return ICPD.LeadTransferSave(dto);
        }


        [Route("report_database")]
        public ISM_Client_Project_DTO report_database([FromBody] ISM_Client_Project_DTO dto)
        {

            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return ICPD.report_database(dto);
        }

        [Route("report_Quotationrequest")]
        public ISM_Client_Project_DTO report_Quotationrequest([FromBody] ISM_Client_Project_DTO dto)
        {

            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return ICPD.report_Quotationrequest(dto);
        }


        [Route("report_database_details")]
        public ISM_Client_Project_DTO report_database_details([FromBody] ISM_Client_Project_DTO dto)
        {

            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return ICPD.report_database_details(dto);
        }


        [Route("report_Quotationrequest_details")]
        public ISM_Client_Project_DTO report_Quotationrequest_details([FromBody] ISM_Client_Project_DTO dto)
        {

            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return ICPD.report_Quotationrequest_details(dto);
        }


        [Route("StatusUpdateOnLoad")]
        public ISM_Client_Project_DTO StatusUpdateOnLoad([FromBody] ISM_Client_Project_DTO dto)
        {

            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return ICPD.StatusUpdateOnLoad(dto);
        }
        [Route("StatusDropDownData")]
        public ISM_Client_Project_DTO StatusDropDownData([FromBody] ISM_Client_Project_DTO dto)
        {

            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return ICPD.StatusDropDownData(dto);
        }

        [Route("StatusUpdate")]
        public ISM_Client_Project_DTO StatusUpdate([FromBody] ISM_Client_Project_DTO dto)
        {

            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return ICPD.StatusUpdate(dto);
        }

        [Route("getClientData/{id:int}")]
        public ISM_Client_Project_DTO getClientData(int id)
        {
            ISM_Client_Project_DTO dto = new ISM_Client_Project_DTO();
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return ICPD.getClientData(dto);
        }
        [Route("getClientReport")]
        public ISM_Client_Project_DTO getClientReport([FromBody] ISM_Client_Project_DTO dto)
        {
            
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return ICPD.getClientReport(dto);
        }


        [Route("SalesPortfolioReport")]
        public ISM_Client_Project_DTO SalesPortfolioReport([FromBody] ISM_Client_Project_DTO dto)
        {

            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return ICPD.SalesPortfolioReport(dto);
        }

        [Route("SalesRefferalLeadOnload/{id:int}")]
        public ISM_Client_Project_DTO SalesRefferalLeadOnload(int id)
        {
            ISM_Client_Project_DTO dto = new ISM_Client_Project_DTO();
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return ICPD.SalesRefferalLeadOnload(dto);
        }

        [Route("SaveSalesRefferalLead")]
        public ISM_Client_Project_DTO SaveSalesRefferalLead([FromBody] ISM_Client_Project_DTO dto)
        {

            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return ICPD.SaveSalesRefferalLead(dto);
        }


        [Route("SalesReferralLeadReport")]
        public ISM_Client_Project_DTO SalesReferralLeadReport([FromBody] ISM_Client_Project_DTO dto)
        {

            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return ICPD.SalesReferralLeadReport(dto);
        }


        [Route("Ism_sales_ChannelParterReport")]
        public ISM_Client_Project_DTO Ism_sales_ChannelParterReport([FromBody] ISM_Client_Project_DTO dto)
        {

            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return ICPD.Ism_sales_ChannelParterReport(dto);
        }



        [Route("Ism_Sales_ChannelParter_lead_Report")]
        public ISM_Client_Project_DTO Ism_Sales_ChannelParter_lead_Report([FromBody] ISM_Client_Project_DTO dto)
        {

            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return ICPD.Ism_Sales_ChannelParter_lead_Report(dto);
        }

        [Route("editImplementationDetails/{id:int}")]

        public ISM_Client_Project_DTO editImplementationDetails(int id)

        {

            ISM_Client_Project_DTO dto = new ISM_Client_Project_DTO();

            dto.ISMMP_Id = id;

            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));

            return ICPD.editImplementationDetails(dto);

        }


        [Route("Save_HealthCheckupAssign")]

        public ISM_Client_Project_DTO Save_HealthCheckupAssign([FromBody] ISM_Client_Project_DTO dto)

        {



            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));

            dto.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));

            return ICPD.Save_HealthCheckupAssign(dto);

        }

        [Route("Save_Elearning_Enhancement")]

        public ISM_Client_Project_DTO Save_Elearning_Enhancement([FromBody] ISM_Client_Project_DTO dto)

        {



            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));

            dto.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));

            return ICPD.Save_Elearning_Enhancement(dto);

        }




        [Route("Impending_module")]

        public ISM_Client_Project_DTO Impending_module([FromBody] ISM_Client_Project_DTO dto)

        {



            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));

            dto.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));

            return ICPD.Impending_module(dto);

        }

        //added by praveenkumar

        [Route("deactivate_Healthcheckup")]

        public ISM_Client_Project_DTO deactivate_Healthcheckup([FromBody] ISM_Client_Project_DTO dto)

        {

            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));

            dto.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));

            return ICPD.deactivate_Healthcheckup(dto);

        }


        [Route("Getmodules")]

        public ISM_Client_Project_DTO Getmodules([FromBody] ISM_Client_Project_DTO dto)

        {



            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));

            dto.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));

            return ICPD.Getmodules(dto);

        }



    [Route("Save_accounts")]

    public ISM_Client_Project_DTO Save_accounts([FromBody] ISM_Client_Project_DTO dto)

    {

        dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));

        dto.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));

        return ICPD.Save_accounts(dto);

    }



    [Route("savedetail")]

    public General_SendSMSDTO savedetail([FromBody] General_SendSMSDTO message)

    {

        message.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));

        return ICPD.savedetail(message);

    }



}
}