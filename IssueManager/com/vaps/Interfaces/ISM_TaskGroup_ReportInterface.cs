using PreadmissionDTOs.com.vaps.IssueManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IssueManager.com.vaps.Interfaces
{
    public interface ISM_TaskGroup_ReportInterface
    {
        Task<ISM_TaskGroup_ReportDTO> getdetails(ISM_TaskGroup_ReportDTO data);
        //Task<ISM_TaskGroup_ReportDTO> getdepartments(ISM_TaskGroup_ReportDTO data);
        Task<ISM_TaskGroup_ReportDTO> getplannerReport(ISM_TaskGroup_ReportDTO data);
        Task<ISM_TaskGroup_ReportDTO> getReport(ISM_TaskGroup_ReportDTO data);

        /*CATEGORY REPORT*/
        ISM_TaskGroup_ReportDTO getcategorydetails(ISM_TaskGroup_ReportDTO data);
        ISM_TaskGroup_ReportDTO onchangedept(ISM_TaskGroup_ReportDTO data);
        ISM_TaskGroup_ReportDTO categorygroupReport(ISM_TaskGroup_ReportDTO data);
        //
        ISM_TaskGroup_ReportDTO SaveFeedBack(ISM_TaskGroup_ReportDTO data);
    }
}
