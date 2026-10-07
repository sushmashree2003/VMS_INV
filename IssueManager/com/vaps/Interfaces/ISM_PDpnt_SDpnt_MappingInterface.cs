using PreadmissionDTOs.com.vaps.IssueManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace IssueManager.com.vaps.Interfaces
{
    public interface ISM_PDpnt_SDpnt_MappingInterface
    {
        Task<ISM_PDpnt_SDpnt_MappingDTO> loaddataAsync(ISM_PDpnt_SDpnt_MappingDTO data);
        ISM_PDpnt_SDpnt_MappingDTO save(ISM_PDpnt_SDpnt_MappingDTO data);
        ISM_PDpnt_SDpnt_MappingDTO editdata(ISM_PDpnt_SDpnt_MappingDTO data);
        ISM_PDpnt_SDpnt_MappingDTO deletedata(ISM_PDpnt_SDpnt_MappingDTO data);
        ISM_PDpnt_SDpnt_MappingDTO loaddepartment(ISM_PDpnt_SDpnt_MappingDTO data);
    }
}
