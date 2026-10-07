using CommonLibrary;
using PreadmissionDTOs.com.vaps.IssueManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.IssueManager
{
    public class ISMImportDelegate
    {
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<ISMImportDTO, ISMImportDTO> COMMM = new CommonDelegate<ISMImportDTO, ISMImportDTO>();

       
        public ISMImportDTO Savedata(ISMImportDTO data)
        {
            return COMMM.POSTIssueManager(data, "ISMImportFacade/Savedata");
        }
        public ISMImportDTO saveIP(ISMImportDTO data)
        {
            return COMMM.POSTIssueManager(data, "ISMImportFacade/saveIP");
        }
        public ISMImportDTO saveadvance(ISMImportDTO data)
        {
            return COMMM.POSTIssueManager(data, "ISMImportFacade/saveadvance");
        }
        public ISMImportDTO save_adv_planner_task(ISMImportDTO data)
        {
            return COMMM.POSTIssueManager(data, "ISMImportFacade/save_adv_planner_task");
        }


        public ISMImportDTO OnloadApproval(ISMImportDTO data)
        {
            return COMMM.POSTIssueManager(data, "ISMImportFacade/OnloadApproval/");
        }
        public ISMImportDTO getyearplannerreport(ISMImportDTO data)
        {
            return COMMM.POSTIssueManager(data, "ISMImportFacade/getyearplannerreport");
        }
        public ISMImportDTO yearplannerapp(ISMImportDTO data)
        {
            return COMMM.POSTIssueManager(data, "ISMImportFacade/yearplannerapp");
        }
        public ISMImportDTO department(ISMImportDTO data)
        {
            return COMMM.POSTIssueManager(data, "ISMImportFacade/department");
        }
        public ISMImportDTO Depemplist(ISMImportDTO data)
        {
            return COMMM.POSTIssueManager(data, "ISMImportFacade/Depemplist");
        }

        public ISMImportDTO Depemptasklist(ISMImportDTO data)
        {
            return COMMM.POSTIssueManager(data, "ISMImportFacade/Depemptasklist");
        }


        public ISMImportDTO swaptask_list(ISMImportDTO data)
        {
            return COMMM.POSTIssueManager(data, "ISMImportFacade/swaptask_list");
        }
        public ISMImportDTO swaptask_list_save(ISMImportDTO data)
        {
            return COMMM.POSTIssueManager(data, "ISMImportFacade/swaptask_list_save");
        }

    }
}
