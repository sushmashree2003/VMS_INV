using CommonLibrary;
using Newtonsoft.Json;
using PreadmissionDTOs.com.vaps.HRMS;
using PreadmissionDTOs.com.vaps.Portals.Employee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace corewebapi18072016.Delegates.com.vapstech.Portals.Employee
{
    public class DepartmentWiseSalaryReportDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<DepartmentWiseSalaryReportDTO, DepartmentWiseSalaryReportDTO> COMMM = new CommonDelegate<DepartmentWiseSalaryReportDTO, DepartmentWiseSalaryReportDTO>();

        public DepartmentWiseSalaryReportDTO getalldetails(DepartmentWiseSalaryReportDTO data)
        {
            return COMMM.POSTDataHRMS(data, "DepartmentWiseSalaryReportFacade/getdata/");
        }
        public DepartmentWiseSalaryReportDTO getdaily_data(DepartmentWiseSalaryReportDTO data)
        {
            return COMMM.POSTDataHRMS(data, "DepartmentWiseSalaryReportFacade/getdaily_data/");
        }

        public DepartmentWiseSalaryReportDTO getsalaryalldetails(DepartmentWiseSalaryReportDTO data)
        {
            return COMMM.POSTDataHRMS(data, "DepartmentWiseSalaryReportFacade/getsalaryalldetails/");
        }

        public DepartmentWiseSalaryReportDTO get_depts(DepartmentWiseSalaryReportDTO maspage)
        {
            return COMMM.POSTDataHRMS(maspage, "DepartmentWiseSalaryReportFacade/get_depts/");
        }
        public DepartmentWiseSalaryReportDTO Getreport(DepartmentWiseSalaryReportDTO maspage)
        {
            return COMMM.POSTDataHRMS(maspage, "DepartmentWiseSalaryReportFacade/Getreport/");
        }


    }
}
