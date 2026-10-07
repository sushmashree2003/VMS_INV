using CommonLibrary;
using PreadmissionDTOs.com.vaps.IssueManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.IssueManager
{
    public class ISM_Master_TaskCategory_EmployeeDelegate
    {
        CommonDelegate<ISM_Master_TaskCategory_Employee_DTO, ISM_Master_TaskCategory_Employee_DTO> comm = new CommonDelegate<ISM_Master_TaskCategory_Employee_DTO, ISM_Master_TaskCategory_Employee_DTO>();
        public ISM_Master_TaskCategory_Employee_DTO loaddata(ISM_Master_TaskCategory_Employee_DTO data)
        {
            return comm.POSTIssueManager(data, "ISM_Master_TaskCategory_EmployeeFacade/loaddata");

        }
        public ISM_Master_TaskCategory_Employee_DTO save(ISM_Master_TaskCategory_Employee_DTO data)
        {
            return comm.POSTIssueManager(data, "ISM_Master_TaskCategory_EmployeeFacade/save");
        }
        public ISM_Master_TaskCategory_Employee_DTO deactive(ISM_Master_TaskCategory_Employee_DTO data)
        {
            return comm.POSTIssueManager(data, "ISM_Master_TaskCategory_EmployeeFacade/deactive");
        }
        public ISM_Master_TaskCategory_Employee_DTO EditData(ISM_Master_TaskCategory_Employee_DTO data)
        {
            return comm.POSTIssueManager(data, "ISM_Master_TaskCategory_EmployeeFacade/EditData");
        }
        public ISM_Master_TaskCategory_Employee_DTO getcategory(ISM_Master_TaskCategory_Employee_DTO data)
        {
            return comm.POSTIssueManager(data, "ISM_Master_TaskCategory_EmployeeFacade/getcategory");
        }
    }
}
