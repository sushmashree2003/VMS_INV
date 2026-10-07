using CommonLibrary;
using PreadmissionDTOs.com.vaps.IssueManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.IssueManager
{
    public class DriverIndentDelegate
    {
        CommonDelegate<ISM_Driver_IndentDTO, ISM_Driver_IndentDTO> COMMM = new CommonDelegate<ISM_Driver_IndentDTO, ISM_Driver_IndentDTO>();
        public ISM_Driver_IndentDTO loaddata(ISM_Driver_IndentDTO data)
        {
            return COMMM.POSTIssueManager(data, "DriverIndent/loaddata/");
        }
        public ISM_Driver_IndentDTO save(ISM_Driver_IndentDTO data)
        {
            return COMMM.POSTIssueManager(data, "DriverIndent/save/");
        }
        public ISM_Driver_IndentDTO edittab1(ISM_Driver_IndentDTO data)
        {
            return COMMM.POSTIssueManager(data, "DriverIndent/edittab1/");
        }
        public ISM_Driver_IndentDTO getApprovalStatus(ISM_Driver_IndentDTO data)
        {
            return COMMM.POSTIssueManager(data, "DriverIndent/getApprovalStatus/");
        }
        public ISM_Driver_IndentDTO get_status(ISM_Driver_IndentDTO data)
        {
            return COMMM.POSTIssueManager(data, "DriverIndent/get_status/");
        }
        public ISM_Driver_IndentDTO reject_status(ISM_Driver_IndentDTO data)
        {
            return COMMM.POSTIssueManager(data, "DriverIndent/reject_status/");
        }
        public ISM_Driver_IndentDTO getrpt(ISM_Driver_IndentDTO data)
        {
            return COMMM.POSTIssueManager(data, "DriverIndent/getrpt/");
        }
        public ISM_Driver_IndentDTO checkedVehicleno(ISM_Driver_IndentDTO data)
        {
            return COMMM.POSTIssueManager(data, "DriverIndent/checkedVehicleno/");
        }
    }
}
