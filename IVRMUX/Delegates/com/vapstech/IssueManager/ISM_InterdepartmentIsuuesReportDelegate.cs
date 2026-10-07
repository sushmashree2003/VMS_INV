using CommonLibrary;
using PreadmissionDTOs.com.vaps.IssueManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.IssueManager
{
    public class ISM_InterdepartmentIsuuesReportDelegate 
    {
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<ISM_InterDepartmentReport, ISM_InterDepartmentReport> COMMM = new CommonDelegate<ISM_InterDepartmentReport, ISM_InterDepartmentReport>();

        public ISM_InterDepartmentReport getdetails(ISM_InterDepartmentReport data)
        {
            return COMMM.POSTIssueManager(data, "ISM_InterdepartmentIsuuesReportFacade/getdetails/");
        }
        public ISM_InterDepartmentReport getreport(ISM_InterDepartmentReport data)
        {
            return COMMM.POSTIssueManager(data, "ISM_InterdepartmentIsuuesReportFacade/getreport");
        }
        public ISM_InterDepartmentReport SendEmail(ISM_InterDepartmentReport data)
        {
            return COMMM.POSTIssueManager(data, "ISM_InterdepartmentIsuuesReportFacade/SendEmail");
        }
    }
}
