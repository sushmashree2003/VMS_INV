using PreadmissionDTOs.com.vaps.IssueManager.VersionControl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IssueManager.com.VersionControl.Interfaces
{
    public interface IVRM_VersionControlProcessInterface
    {
        IVRM_VersionControlProcessDTO getdetails(IVRM_VersionControlProcessDTO data);
        IVRM_VersionControlProcessDTO savedata(IVRM_VersionControlProcessDTO data);
        IVRM_VersionControlProcessDTO editdata(IVRM_VersionControlProcessDTO data);
        IVRM_VersionControlProcessDTO activedeactive(IVRM_VersionControlProcessDTO data);
        IVRM_VersionControlProcessDTO VTgetdetails(IVRM_VersionControlProcessDTO data);
        IVRM_VersionControlProcessDTO VTgettasklist(IVRM_VersionControlProcessDTO data);
        IVRM_VersionControlProcessDTO VTsave(IVRM_VersionControlProcessDTO data);
        IVRM_VersionControlProcessDTO VTeditdata(IVRM_VersionControlProcessDTO data);
        IVRM_VersionControlProcessDTO VTactivedeactive(IVRM_VersionControlProcessDTO data);
        IVRM_VersionControlProcessDTO ReportOnload(IVRM_VersionControlProcessDTO data);
        IVRM_VersionControlProcessDTO GetReport(IVRM_VersionControlProcessDTO data);
        IVRM_VersionControlProcessDTO Loadthetasklist(IVRM_VersionControlProcessDTO data);
        IVRM_VersionControlProcessDTO savetasklist(IVRM_VersionControlProcessDTO data);
        IVRM_VersionControlProcessDTO getversiondetails(IVRM_VersionControlProcessDTO data);
        IVRM_VersionControlProcessDTO savedeployment(IVRM_VersionControlProcessDTO data);
        IVRM_VersionControlProcessDTO IVRM_CLient_version(IVRM_VersionControlProcessDTO data);
    }
}
