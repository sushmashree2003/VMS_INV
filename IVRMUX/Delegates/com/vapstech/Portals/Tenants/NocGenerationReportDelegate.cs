using CommonLibrary;
using PreadmissionDTOs.com.vaps.IssueManager.Tenants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.IssueManager.Tenants
{
    public class NocGenerationReportDelegate
    {

        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<NocGenerationReportDTO, NocGenerationReportDTO> COMMM = new CommonDelegate<NocGenerationReportDTO, NocGenerationReportDTO>();

        public NocGenerationReportDTO getdetails(NocGenerationReportDTO data)
        {
            return COMMM.POSTIssueManager(data, "NocGenerationReportFacade/getdetails/");
        }
        public NocGenerationReportDTO get_report(NocGenerationReportDTO data)
        {
            return COMMM.POSTIssueManager(data, "NocGenerationReportFacade/get_report");
        }


        public NocGenerationReportDTO buildingchange(NocGenerationReportDTO data)
        {
            return COMMM.POSTIssueManager(data, "NocGenerationReportFacade/buildingchange");
        }
        public NocGenerationReportDTO Floorchange(NocGenerationReportDTO data)
        {
            return COMMM.POSTIssueManager(data, "NocGenerationReportFacade/Floorchange");
        }

        //ELECTRICAL
        public NocGenerationReportDTO getallofflinedetails(NocGenerationReportDTO data)
        {
            return COMMM.POSTIssueManager(data, "NocGenerationReportFacade/getallofflinedetails/");
        }

        public NocGenerationReportDTO SaveElectricalbill(NocGenerationReportDTO data)
        {
            return COMMM.POSTIssueManager(data, "NocGenerationReportFacade/SaveElectricalbill/");
        }


        public NocGenerationReportDTO editdetails(NocGenerationReportDTO data)
        {
            return COMMM.POSTIssueManager(data, "NocGenerationReportFacade/editdetails/");
        }
        public NocGenerationReportDTO deactive(NocGenerationReportDTO data)
        {
            return COMMM.POSTIssueManager(data, "NocGenerationReportFacade/deactive/");
        }
        public NocGenerationReportDTO sendmail(NocGenerationReportDTO data)
        {
            return COMMM.POSTIssueManager(data, "NocGenerationReportFacade/sendmail/");
        }
    }
}
