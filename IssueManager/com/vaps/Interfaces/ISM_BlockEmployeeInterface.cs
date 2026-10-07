using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.com.vaps.IssueManager;
namespace IssueManager.com.vaps.Interfaces
{
    public interface ISM_BlockEmployeeInterface
    {

        Task<ISM_BlockEmployeeDTO> getloaddata(ISM_BlockEmployeeDTO data);

        ISM_BlockEmployeeDTO savedata(ISM_BlockEmployeeDTO data);
        ISM_BlockEmployeeDTO getblockedEmpDeatils(ISM_BlockEmployeeDTO data);
        ISM_BlockEmployeeDTO deactivate(ISM_BlockEmployeeDTO data);
    }
}

