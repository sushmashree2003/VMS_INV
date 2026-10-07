using PreadmissionDTOs.com.vaps.IssueManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace IssueManager.com.vaps.Interfaces
{
    public interface EmployeeMonthEndInterface
    {
        EmployeeMonthEndDTO getemplist(EmployeeMonthEndDTO data);
        Task<EmployeeMonthEndDTO> getempdetailsAsync(EmployeeMonthEndDTO data);
        EmployeeMonthEndDTO savereport(EmployeeMonthEndDTO data);
    }
}
