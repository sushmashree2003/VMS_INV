using PreadmissionDTOs.com.vaps.IssueManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IssueManager.com.vaps.Interfaces
{
    public interface Employee_VMS_DeactivateInterface
    {
        Employee_VMS_DeactivateDTO Onloaddata(Employee_VMS_DeactivateDTO data);
        Employee_VMS_DeactivateDTO SaveData(Employee_VMS_DeactivateDTO data);
        Employee_VMS_DeactivateDTO DeavtivEmployee(Employee_VMS_DeactivateDTO data);
    }
}
