
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommonLibrary;
using PreadmissionDTOs.com.vaps.IssueManager;

namespace IVRMUX.Delegates.com.vapstech.IssueManager
{
    public class ISM_BlockEmployeeDelegate
    {
        CommonDelegate<ISM_BlockEmployeeDTO, ISM_BlockEmployeeDTO> _comm = new CommonDelegate<ISM_BlockEmployeeDTO, ISM_BlockEmployeeDTO>();

        public ISM_BlockEmployeeDTO getloaddata(ISM_BlockEmployeeDTO data)
        {
            return _comm.POSTIssueManager(data, "ISM_BlockEmployeeFacade/getloaddata");
        }

        public ISM_BlockEmployeeDTO savedata(ISM_BlockEmployeeDTO data)
        {
            return _comm.POSTIssueManager(data, "ISM_BlockEmployeeFacade/savedata");
        }
        public ISM_BlockEmployeeDTO getblockedEmpDeatils(ISM_BlockEmployeeDTO data)
        {
            return _comm.POSTIssueManager(data, "ISM_BlockEmployeeFacade/getblockedEmpDeatils");
        }

        public ISM_BlockEmployeeDTO deactivate(ISM_BlockEmployeeDTO data)
        {
            return _comm.POSTIssueManager(data, "ISM_BlockEmployeeFacade/deactivate");
        }
    }
}
