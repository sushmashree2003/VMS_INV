using CommonLibrary;
using PreadmissionDTOs.com.vaps.IssueManager;
using System;

namespace IVRMUX.Delegates.com.vapstech.IssueManager
{
    public class EmployeeMonthEndDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<EmployeeMonthEndDTO, EmployeeMonthEndDTO> COMMM = new CommonDelegate<EmployeeMonthEndDTO, EmployeeMonthEndDTO>();

        public EmployeeMonthEndDTO getemplist(EmployeeMonthEndDTO data)
        {
            return COMMM.POSTIssueManager(data, "EmployeeMonthEndFacade/getemplist/");
        }

        public EmployeeMonthEndDTO getempdetails(EmployeeMonthEndDTO dto)
        {
            return COMMM.POSTIssueManager(dto, "EmployeeMonthEndFacade/getempdetails");
        }

        public EmployeeMonthEndDTO savereport(EmployeeMonthEndDTO dto)
        {
            return COMMM.POSTIssueManager(dto, "EmployeeMonthEndFacade/savereport");
        }
        
    }
}
