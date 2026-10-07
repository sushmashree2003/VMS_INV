using PreadmissionDTOs.com.vaps.VisitorsManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VisitorsManagementServiceHub.Interfaces
{
    public interface VM_WebJob_SchedulerInterface
    {
        VM_webJob_SchedulerDTO vmwebjobscheduler(VM_webJob_SchedulerDTO data);       
        VM_webJob_SchedulerDTO vmhourwebjobscheduler(VM_webJob_SchedulerDTO data);
        VM_webJob_SchedulerDTO vmminwebjobscheduler(VM_webJob_SchedulerDTO data);
    }
}
