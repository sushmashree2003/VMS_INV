using PreadmissionDTOs.com.vaps.VisitorsManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VisitorsManagementServiceHub.Interfaces
{
    public interface AppointmentStatusInterface
    {
        AppointmentStatusDTO getDetails(AppointmentStatusDTO data);
        AppointmentStatusDTO EditDetails(AppointmentStatusDTO id);
        AppointmentStatusDTO saveData(AppointmentStatusDTO data);
        AppointmentStatusDTO change_radiobutton(AppointmentStatusDTO data);
    }
}
