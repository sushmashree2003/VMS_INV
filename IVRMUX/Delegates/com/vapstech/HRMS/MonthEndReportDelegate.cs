using CommonLibrary;
using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace corewebapi18072016.Delegates.com.vapstech.HRMS
{
    public class MonthEndReportDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<MonthEndReportDTO, MonthEndReportDTO> COMMM = new CommonDelegate<MonthEndReportDTO, MonthEndReportDTO>();

        public MonthEndReportDTO onloadgetdetails(MonthEndReportDTO dto)
        {
            return COMMM.POSTDataHRMS(dto, "MonthEndReportFacade/onloadgetdetails");
        }

        public MonthEndReportDTO getEmployeedetailsBySelection(MonthEndReportDTO maspage)
        {
            return COMMM.POSTDataHRMS(maspage, "MonthEndReportFacade/getEmployeedetailsBySelection/");
        }

        public MonthEndReportDTO getalldetailsQForm(MonthEndReportDTO maspage)
        {
            return COMMM.POSTDataHRMS(maspage, "MonthEndReportFacade/getalldetailsQForm/");
        }

        public MonthEndReportDTO getEmployeedetailsQForm(MonthEndReportDTO maspage)
        {
            return COMMM.POSTDataHRMS(maspage, "MonthEndReportFacade/getEmployeedetailsQForm/");
        }

        public MonthEndReportDTO SearchUdetails(MonthEndReportDTO maspage)
        {
            return COMMM.POSTDataHRMS(maspage, "MonthEndReportFacade/SearchUdetails/");
        }

        public MonthEndReportDTO SearchFReport(MonthEndReportDTO maspage)
        {
            return COMMM.POSTDataHRMS(maspage, "MonthEndReportFacade/SearchFReport/");
        }
        
    }
}
