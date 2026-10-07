using PreadmissionDTOs.com.vaps.IssueManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace IssueManager.com.vaps.Interfaces
{
    public interface DriverIndentInterface
    {
        ISM_Driver_IndentDTO getdetails(ISM_Driver_IndentDTO data);
        ISM_Driver_IndentDTO loaddata(ISM_Driver_IndentDTO data);
        ISM_Driver_IndentDTO save(ISM_Driver_IndentDTO data);
        ISM_Driver_IndentDTO edittab1(ISM_Driver_IndentDTO data);
        Task<ISM_Driver_IndentDTO> getApprovalStatusAsync(ISM_Driver_IndentDTO data);
        ISM_Driver_IndentDTO get_status(ISM_Driver_IndentDTO data);
        ISM_Driver_IndentDTO reject_status(ISM_Driver_IndentDTO data);
        ISM_Driver_IndentDTO getrpt(ISM_Driver_IndentDTO data);
        ISM_Driver_IndentDTO checkedVehicleno(ISM_Driver_IndentDTO data);
    }
}
