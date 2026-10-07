using CommonLibrary;
using PreadmissionDTOs.com.vaps.IssueManager.VersionControl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.IssueManager
{
    public class IVRM_VersionControlProcessDelegate
    {


        CommonDelegate<IVRM_VersionControlProcessDTO, IVRM_VersionControlProcessDTO> COMMM = new CommonDelegate<IVRM_VersionControlProcessDTO, IVRM_VersionControlProcessDTO>();

        public IVRM_VersionControlProcessDTO getdetails(IVRM_VersionControlProcessDTO data)
        {
            return COMMM.POSTIssueManager(data, "IVRM_VersionControlProcessFacade/getdetails/");
        }
        public IVRM_VersionControlProcessDTO savedata(IVRM_VersionControlProcessDTO data)
        {
            return COMMM.POSTIssueManager(data, "IVRM_VersionControlProcessFacade/savedata/");
        }
        public IVRM_VersionControlProcessDTO editdata(IVRM_VersionControlProcessDTO data)
        {
            return COMMM.POSTIssueManager(data, "IVRM_VersionControlProcessFacade/editdata/");
        }
        public IVRM_VersionControlProcessDTO activedeactive(IVRM_VersionControlProcessDTO data)
        {
            return COMMM.POSTIssueManager(data, "IVRM_VersionControlProcessFacade/activedeactive/");
        }

        public IVRM_VersionControlProcessDTO VTgetdetails(IVRM_VersionControlProcessDTO data)
        {
            return COMMM.POSTIssueManager(data, "IVRM_VersionControlProcessFacade/VTgetdetails/");
        }
        public IVRM_VersionControlProcessDTO VTgettasklist(IVRM_VersionControlProcessDTO data)
        {
            return COMMM.POSTIssueManager(data, "IVRM_VersionControlProcessFacade/VTgettasklist/");
        }
        public IVRM_VersionControlProcessDTO VTsave(IVRM_VersionControlProcessDTO data)
        {
            return COMMM.POSTIssueManager(data, "IVRM_VersionControlProcessFacade/VTsave/");
        }
        public IVRM_VersionControlProcessDTO VTeditdata(IVRM_VersionControlProcessDTO data)
        {
            return COMMM.POSTIssueManager(data, "IVRM_VersionControlProcessFacade/VTeditdata/");
        }
        public IVRM_VersionControlProcessDTO VTactivedeactive(IVRM_VersionControlProcessDTO data)
        {
            return COMMM.POSTIssueManager(data, "IVRM_VersionControlProcessFacade/VTactivedeactive/");
        }
        public IVRM_VersionControlProcessDTO ReportOnload(IVRM_VersionControlProcessDTO data)
        {
            return COMMM.POSTIssueManager(data, "IVRM_VersionControlProcessFacade/ReportOnload/");
        }
        public IVRM_VersionControlProcessDTO GetReport(IVRM_VersionControlProcessDTO data)
        {
            return COMMM.POSTIssueManager(data, "IVRM_VersionControlProcessFacade/GetReport/");
        }
        public IVRM_VersionControlProcessDTO Loadthetasklist(IVRM_VersionControlProcessDTO data)
        {
            return COMMM.POSTIssueManager(data, "IVRM_VersionControlProcessFacade/Loadthetasklist/");
        }
        public IVRM_VersionControlProcessDTO savetasklist(IVRM_VersionControlProcessDTO data)
        {
            return COMMM.POSTIssueManager(data, "IVRM_VersionControlProcessFacade/savetasklist/");
        }
        public IVRM_VersionControlProcessDTO getversiondetails(IVRM_VersionControlProcessDTO data)
        {
            return COMMM.POSTIssueManager(data, "IVRM_VersionControlProcessFacade/getversiondetails/");
        }
        public IVRM_VersionControlProcessDTO savedeployment(IVRM_VersionControlProcessDTO data)
        {
            return COMMM.POSTIssueManager(data, "IVRM_VersionControlProcessFacade/savedeployment/");
        }
    }
}
