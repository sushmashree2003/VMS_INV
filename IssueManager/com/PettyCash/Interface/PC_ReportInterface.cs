using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.com.vaps.IssueManager.PettyCash;

namespace IssueManager.com.PettyCash.Interface
{
    public interface PC_ReportInterface
    {
        PC_ReportDTO onloaddata(PC_ReportDTO data);
        PC_ReportDTO getrequisitionreport(PC_ReportDTO data);
        PC_ReportDTO getindentreport(PC_ReportDTO data);
        PC_ReportDTO getindentapprovedreport(PC_ReportDTO data);
        PC_ReportDTO getexpenditurereport(PC_ReportDTO data);
        PC_ReportDTO getexpenditurereports(PC_ReportDTO data);
        PC_ReportDTO onloaddatabalancesheet(PC_ReportDTO data);
        PC_ReportDTO OnChangeBalanceSheetInstitution(PC_ReportDTO data);
        PC_ReportDTO getbalancesheetreports(PC_ReportDTO data);
        PC_ReportDTO getexpenditurebalancereports(PC_ReportDTO data);
    }
}
