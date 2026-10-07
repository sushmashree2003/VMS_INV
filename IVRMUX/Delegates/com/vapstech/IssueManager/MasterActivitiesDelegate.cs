using CommonLibrary;
using PreadmissionDTOs.com.vaps.IssueManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.IssueManager
{
    public class MasterActivitiesDelegate
    {
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<MasterActivities_DTO, MasterActivities_DTO> COMMM = new CommonDelegate<MasterActivities_DTO, MasterActivities_DTO>();

        public MasterActivities_DTO getdetails(MasterActivities_DTO dTO)
        {
            return COMMM.POSTIssueManager(dTO, "MasterActivitiesFacade/getdetails/");
        }
        public MasterActivities_DTO saverecord(MasterActivities_DTO dTO)
        {
            return COMMM.POSTIssueManager(dTO, "MasterActivitiesFacade/saverecord");
        }
        public MasterActivities_DTO deactiveY(MasterActivities_DTO dTO)
        {
            return COMMM.POSTIssueManager(dTO, "MasterActivitiesFacade/deactiveY");
        }

    }
}
