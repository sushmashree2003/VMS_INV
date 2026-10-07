using CommonLibrary;
using PreadmissionDTOs.com.vaps.IssueManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.IssueManager
{
    public class Employee_VMS_DeactivateDelegate
    {
        CommonDelegate<Employee_VMS_DeactivateDTO, Employee_VMS_DeactivateDTO> comm = new CommonDelegate<Employee_VMS_DeactivateDTO, Employee_VMS_DeactivateDTO>();

        public Employee_VMS_DeactivateDTO Onloaddata(Employee_VMS_DeactivateDTO data)
        {
            return comm.POSTIssueManager(data, "Employee_VMS_DeactivateFacade/Onloaddata");
        }
        public Employee_VMS_DeactivateDTO savedata(Employee_VMS_DeactivateDTO data)
        {
            return comm.POSTIssueManager(data, "Employee_VMS_DeactivateFacade/SaveData");
        }

    }
}
