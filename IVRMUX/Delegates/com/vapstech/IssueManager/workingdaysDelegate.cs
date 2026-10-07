using CommonLibrary;
using PreadmissionDTOs.com.vaps.IssueManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.IssueManager
{
    public class workingdaysDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<workingdays_DTO, workingdays_DTO> COMMM = new CommonDelegate<workingdays_DTO, workingdays_DTO>();

        public workingdays_DTO getdetails(workingdays_DTO dTO)
        {
            return COMMM.POSTIssueManager(dTO, "workingdaysFacade/getdetails/");
        }
        public workingdays_DTO saverecord(workingdays_DTO dTO)
        {
            return COMMM.POSTIssueManager(dTO, "workingdaysFacade/saverecord");
        }
        public workingdays_DTO deactiveY(workingdays_DTO dTO)
        {
            return COMMM.POSTIssueManager(dTO, "workingdaysFacade/deactiveY");
        }
        public workingdays_DTO editrecord(workingdays_DTO dTO)
        {
            return COMMM.POSTIssueManager(dTO, "workingdaysFacade/editrecord");
        }
        
    }
}
