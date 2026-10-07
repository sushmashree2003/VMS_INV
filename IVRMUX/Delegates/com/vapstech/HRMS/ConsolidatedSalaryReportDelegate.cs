using CommonLibrary;
using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.HRMS
{
    public class ConsolidatedSalaryReportDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<ConsolidatedSalaryReportDTO, ConsolidatedSalaryReportDTO> COMMM = new CommonDelegate<ConsolidatedSalaryReportDTO, ConsolidatedSalaryReportDTO>();

        public ConsolidatedSalaryReportDTO onloadgetdetails(ConsolidatedSalaryReportDTO dto)
        {
            return COMMM.POSTDataHRMS(dto, "ConsolidatedSalaryReportFacade/onloadgetdetails");
        }

        //FilterEmployeeData
        public ConsolidatedSalaryReportDTO salaryreport(ConsolidatedSalaryReportDTO maspage)
        {
            return COMMM.POSTDataHRMS(maspage, "ConsolidatedSalaryReportFacade/salaryreport/");
        }

    }
}
