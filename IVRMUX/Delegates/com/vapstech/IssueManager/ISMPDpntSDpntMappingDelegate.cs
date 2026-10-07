using CommonLibrary;
using PreadmissionDTOs.com.vaps.IssueManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.IssueManager
{
    public class ISMPDpntSDpntMappingDelegate
    {
        CommonDelegate<ISM_PDpnt_SDpnt_MappingDTO, ISM_PDpnt_SDpnt_MappingDTO> COMMM = new CommonDelegate<ISM_PDpnt_SDpnt_MappingDTO, ISM_PDpnt_SDpnt_MappingDTO>();
        public ISM_PDpnt_SDpnt_MappingDTO loaddata(ISM_PDpnt_SDpnt_MappingDTO data)
        {
            return COMMM.POSTIssueManager(data, "ISM_PDpnt_SDpnt_Mapping/loaddata/");
        }
        public ISM_PDpnt_SDpnt_MappingDTO save(ISM_PDpnt_SDpnt_MappingDTO data)
        {
            return COMMM.POSTIssueManager(data, "ISM_PDpnt_SDpnt_Mapping/save/");
        }
        public ISM_PDpnt_SDpnt_MappingDTO editdata(ISM_PDpnt_SDpnt_MappingDTO data)
        {
            return COMMM.POSTIssueManager(data, "ISM_PDpnt_SDpnt_Mapping/editdata/");
        }
        public ISM_PDpnt_SDpnt_MappingDTO deletedata(ISM_PDpnt_SDpnt_MappingDTO data)
        {
            return COMMM.POSTIssueManager(data, "ISM_PDpnt_SDpnt_Mapping/deletedata/");
        }
        public ISM_PDpnt_SDpnt_MappingDTO loaddepartment(ISM_PDpnt_SDpnt_MappingDTO data)
        {
            return COMMM.POSTIssueManager(data, "ISM_PDpnt_SDpnt_Mapping/loaddepartment/");
        }
        
    }
}
