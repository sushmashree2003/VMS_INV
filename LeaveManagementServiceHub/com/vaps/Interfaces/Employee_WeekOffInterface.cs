using PreadmissionDTOs.com.vaps.LeaveManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LeaveManagementServiceHub.com.vaps.Interfaces
{
    public interface Employee_WeekOffInterface
    {
        Employee_DataDto OnLoadData(Employee_DataDto dto);
        Employee_DataDto Save(Employee_DataDto dto);
    }
}
