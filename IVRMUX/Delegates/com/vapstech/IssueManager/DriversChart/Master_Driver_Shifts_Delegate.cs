using CommonLibrary;
using PreadmissionDTOs.com.vaps.IssueManager.DriversChart;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.IssueManager.DriversChart
{
    public class Master_Driver_Shifts_Delegate
    {
        CommonDelegate<DriverShiftsDTO, DriverShiftsDTO> comm = new CommonDelegate<DriverShiftsDTO, DriverShiftsDTO>();
        public DriverShiftsDTO getdetails(DriverShiftsDTO data)
        {
            return comm.POSTIssueManager(data, "Master_Driver_ShiftsFacade/getdetails");
        }
        public DriverShiftsDTO savedata(DriverShiftsDTO data)
        {
            return comm.POSTIssueManager(data, "Master_Driver_ShiftsFacade/savedata");
        }
        public DriverShiftsDTO Editdata(DriverShiftsDTO data)
        {
            return comm.POSTIssueManager(data, "Master_Driver_ShiftsFacade/Editdata");
        }
    }
}
