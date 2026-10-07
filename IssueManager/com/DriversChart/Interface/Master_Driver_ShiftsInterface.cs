using PreadmissionDTOs.com.vaps.IssueManager.DriversChart;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IssueManager.com.DriversChart.Interface
{
    public interface Master_Driver_ShiftsInterface
    {
        DriverShiftsDTO getdetails(DriverShiftsDTO data);
        DriverShiftsDTO savedata(DriverShiftsDTO data);
        DriverShiftsDTO Editdata(DriverShiftsDTO data);
    }
}
