using CommonLibrary;
using PreadmissionDTOs.com.vaps.LeaveManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.LeaveManagement
{
    public class Employee_WeekOffDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<Employee_DataDto, Employee_DataDto> COMMM = new CommonDelegate<Employee_DataDto, Employee_DataDto>();


        public Employee_DataDto getdatedetails(Employee_DataDto data)
        {
            return COMMM.POSTDataOnlineLeave(data, "Employee_WeekOffFacade/OnLoadData/");
        }

        public Employee_DataDto Save(Employee_DataDto dto)
        {
            return COMMM.POSTDataOnlineLeave(dto, "Employee_WeekOffFacade/Save/");
        }
    }
}
