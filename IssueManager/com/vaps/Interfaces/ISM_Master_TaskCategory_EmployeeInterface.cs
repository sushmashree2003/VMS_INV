using PreadmissionDTOs.com.vaps.IssueManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IssueManager.com.vaps.Interfaces
{
   public interface ISM_Master_TaskCategory_EmployeeInterface
    {
        ISM_Master_TaskCategory_Employee_DTO loaddata(ISM_Master_TaskCategory_Employee_DTO data);
        ISM_Master_TaskCategory_Employee_DTO save(ISM_Master_TaskCategory_Employee_DTO data);
        ISM_Master_TaskCategory_Employee_DTO deactive(ISM_Master_TaskCategory_Employee_DTO data);
        ISM_Master_TaskCategory_Employee_DTO EditData(ISM_Master_TaskCategory_Employee_DTO data);
        ISM_Master_TaskCategory_Employee_DTO getcategory(ISM_Master_TaskCategory_Employee_DTO data);
    }
}
