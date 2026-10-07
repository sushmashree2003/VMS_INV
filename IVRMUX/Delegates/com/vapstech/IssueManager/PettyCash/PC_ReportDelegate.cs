using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommonLibrary;
using PreadmissionDTOs.com.vaps.IssueManager.PettyCash;
namespace IVRMUX.Delegates.com.vapstech.IssueManager.PettyCash
{
    public class PC_ReportDelegate
    {
        CommonDelegate<PC_ReportDTO, PC_ReportDTO> _comm = new CommonDelegate<PC_ReportDTO, PC_ReportDTO>();
        public PC_ReportDTO onloaddata(PC_ReportDTO data)
        {
            return _comm.POSTIssueManager(data, "PC_ReportFacade/onloaddata");
        }
        public PC_ReportDTO getrequisitionreport(PC_ReportDTO data)
        {
            return _comm.POSTIssueManager(data, "PC_ReportFacade/getrequisitionreport");
        }       
        public PC_ReportDTO getindentreport(PC_ReportDTO data)
        {
            return _comm.POSTIssueManager(data, "PC_ReportFacade/getindentreport");
        }
        public PC_ReportDTO getindentapprovedreport(PC_ReportDTO data)
        {
            return _comm.POSTIssueManager(data, "PC_ReportFacade/getindentapprovedreport");
        }
        public PC_ReportDTO getexpenditurereport(PC_ReportDTO data)
        {
            return _comm.POSTIssueManager(data, "PC_ReportFacade/getexpenditurereport");
        }
        public PC_ReportDTO getexpenditurereports(PC_ReportDTO data)
        {
            return _comm.POSTIssueManager(data, "PC_ReportFacade/getexpenditurereports");
        }
        public PC_ReportDTO onloaddatabalancesheet(PC_ReportDTO data)
        {
            return _comm.POSTIssueManager(data, "PC_ReportFacade/onloaddatabalancesheet");
        }
        public PC_ReportDTO OnChangeBalanceSheetInstitution(PC_ReportDTO data)
        {
            return _comm.POSTIssueManager(data, "PC_ReportFacade/OnChangeBalanceSheetInstitution");
        }
        public PC_ReportDTO getbalancesheetreports(PC_ReportDTO data)
        {
            return _comm.POSTIssueManager(data, "PC_ReportFacade/getbalancesheetreports");
        } public PC_ReportDTO getexpenditurebalancereports(PC_ReportDTO data)
        {
            return _comm.POSTIssueManager(data, "PC_ReportFacade/getexpenditurebalancereports");
        }
    }
}
