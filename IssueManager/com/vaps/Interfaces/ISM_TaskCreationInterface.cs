using PreadmissionDTOs.com.vaps.IssueManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IssueManager.com.vaps.Interfaces
{
    public interface ISM_TaskCreationInterface
    {
        Task<ISM_TaskCreationDTO> getdetails(ISM_TaskCreationDTO data);
        ISM_TaskCreationDTO getmodule(ISM_TaskCreationDTO data);
        Task<ISM_TaskCreationDTO> getIEuser(ISM_TaskCreationDTO data);
        ISM_TaskCreationDTO savedata(ISM_TaskCreationDTO data);
        ISM_TaskCreationDTO deactive(ISM_TaskCreationDTO data);
        ISM_TaskCreationDTO gettaskAttachment(ISM_TaskCreationDTO data);
        ISM_TaskCreationDTO get_category(ISM_TaskCreationDTO data);

        Task<ISM_TaskCreationDTO> getdetailsClient(ISM_TaskCreationDTO data);
        ISM_TaskCreationDTO savedataClient(ISM_TaskCreationDTO data);
        ISM_TaskCreationDTO SaveTaskDeptWise(ISM_TaskCreationDTO data);

    }
}
