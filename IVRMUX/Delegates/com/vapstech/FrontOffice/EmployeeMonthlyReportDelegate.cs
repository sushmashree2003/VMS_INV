using CommonLibrary;
using Newtonsoft.Json;
using PreadmissionDTOs.com.vaps.FrontOffice;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;


namespace corewebapi18072016.Delegates.com.vapstech.FrontOffice
{
    public class EmployeeMonthlyReportDelegate
    {
        CommonDelegate<EmployeeMonthlyReportDTO, EmployeeMonthlyReportDTO> COMFRNT = new CommonDelegate<EmployeeMonthlyReportDTO, EmployeeMonthlyReportDTO>();
        public EmployeeMonthlyReportDTO getdata(EmployeeMonthlyReportDTO data)
        {
            return COMFRNT.POSTDataHolidayReport(data, "EmployeeMonthlyReportFacade/getalldetails/");
        }
        public EmployeeMonthlyReportDTO get_departments(EmployeeMonthlyReportDTO student)
        {
            return COMFRNT.POSTDataHolidayReport(student, "EmployeeMonthlyReportFacade/get_departments/");
        }
        public EmployeeMonthlyReportDTO get_designation(EmployeeMonthlyReportDTO student)
        {
            return COMFRNT.POSTDataHolidayReport(student, "EmployeeYearlyReportFacade/get_designation/");
        }
        public EmployeeMonthlyReportDTO get_employee(EmployeeMonthlyReportDTO student)
        {
            return COMFRNT.POSTDataHolidayReport(student, "EmployeeMonthlyReportFacade/get_employee/");
        }

        public EmployeeMonthlyReportDTO get_employeeselected(EmployeeMonthlyReportDTO student)
        {
            return COMFRNT.POSTDataHolidayReport(student, "EmployeeMonthlyReportFacade/get_employeeselected/");
        }        

        public EmployeeMonthlyReportDTO getrpt(EmployeeMonthlyReportDTO student)
        {
            return COMFRNT.POSTDataHolidayReport(student, "EmployeeMonthlyReportFacade/getrpt/");
        }

        public EmployeeMonthlyReportDTO getalldetailsselected(EmployeeMonthlyReportDTO data)
        {
            return COMFRNT.POSTDataHolidayReport(data, "EmployeeMonthlyReportFacade/getalldetailsselected/");
        }
        public EmployeeMonthlyReportDTO get_departmentselected(EmployeeMonthlyReportDTO student)
        {
            return COMFRNT.POSTDataHolidayReport(student, "EmployeeMonthlyReportFacade/get_departmentselected/");
        }
        public EmployeeMonthlyReportDTO get_designationselected(EmployeeMonthlyReportDTO student)
        {
            return COMFRNT.POSTDataHolidayReport(student, "EmployeeMonthlyReportFacade/get_designationselected/");
        }
    }
}
