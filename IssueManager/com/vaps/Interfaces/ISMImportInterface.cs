using PreadmissionDTOs.com.vaps.IssueManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IssueManager.com.vaps.Interfaces
{
    public interface ISMImportInterface
    {
      
        ISMImportDTO Savedata(ISMImportDTO data);     
        ISMImportDTO saveIP(ISMImportDTO data);     
        ISMImportDTO saveadvance(ISMImportDTO data);     
        ISMImportDTO save_adv_planner_task(ISMImportDTO data);


        ISMImportDTO OnloadApproval(ISMImportDTO data);
        ISMImportDTO getyearplannerreport(ISMImportDTO data);
        ISMImportDTO yearplannerapp(ISMImportDTO data);
        ISMImportDTO department(ISMImportDTO data);
        ISMImportDTO Depemplist(ISMImportDTO data);
        ISMImportDTO Depemptasklist(ISMImportDTO data);

        ISMImportDTO swaptask_list(ISMImportDTO data);
        ISMImportDTO swaptask_list_save(ISMImportDTO data);

    }
}
