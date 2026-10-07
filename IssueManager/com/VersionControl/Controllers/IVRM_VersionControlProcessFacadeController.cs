using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IssueManager.com.VersionControl.Interfaces;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.IssueManager.VersionControl;

namespace IssueManager.com.VersionControl.Controllers
{
    [Route("api/[controller]")]
    public class IVRM_VersionControlProcessFacadeController : Controller
    {

        public IVRM_VersionControlProcessInterface _objinter;

        public IVRM_VersionControlProcessFacadeController(IVRM_VersionControlProcessInterface parameter)
        {
            _objinter = parameter;
        }

        [Route("getdetails")]
        public IVRM_VersionControlProcessDTO getdetails([FromBody] IVRM_VersionControlProcessDTO data)
        {
            return _objinter.getdetails(data);
        }
        [Route("savedata")]
        public IVRM_VersionControlProcessDTO savedata([FromBody] IVRM_VersionControlProcessDTO data)
        {
            return _objinter.savedata(data);
        }
        [Route("editdata")]
        public IVRM_VersionControlProcessDTO editdata([FromBody] IVRM_VersionControlProcessDTO data)
        {
            return _objinter.editdata(data);
        }
        [Route("activedeactive")]
        public IVRM_VersionControlProcessDTO activedeactive([FromBody] IVRM_VersionControlProcessDTO data)
        {
            return _objinter.activedeactive(data);
        }

        [Route("VTgetdetails")]
        public IVRM_VersionControlProcessDTO VTgetdetails([FromBody] IVRM_VersionControlProcessDTO data)
        {
            return _objinter.VTgetdetails(data);
        }
        [Route("VTgettasklist")]
        public IVRM_VersionControlProcessDTO VTgettasklist([FromBody] IVRM_VersionControlProcessDTO data)
        {
            return _objinter.VTgettasklist(data);
        }
        [Route("VTsave")]
        public IVRM_VersionControlProcessDTO VTsave([FromBody] IVRM_VersionControlProcessDTO data)
        {
            return _objinter.VTsave(data);
        }
        [Route("VTeditdata")]
        public IVRM_VersionControlProcessDTO VTeditdata([FromBody] IVRM_VersionControlProcessDTO data)
        {
            return _objinter.VTeditdata(data);
        }
        [Route("VTactivedeactive")]
        public IVRM_VersionControlProcessDTO VTactivedeactive([FromBody] IVRM_VersionControlProcessDTO data)
        {
            return _objinter.VTactivedeactive(data);
        }
        [Route("ReportOnload")]
        public IVRM_VersionControlProcessDTO ReportOnload([FromBody] IVRM_VersionControlProcessDTO data)
        {
            return _objinter.ReportOnload(data);
        }
        [Route("GetReport")]
        public IVRM_VersionControlProcessDTO GetReport([FromBody] IVRM_VersionControlProcessDTO data)
        {
            return _objinter.GetReport(data);
        }
        //---------Get task list for approval of deployment
        [Route("Loadthetasklist")]
        public IVRM_VersionControlProcessDTO Loadthetasklist([FromBody] IVRM_VersionControlProcessDTO data)
        {
            return _objinter.Loadthetasklist(data);
        }
        //---------Save task list for approval of deployment
        [Route("savetasklist")]
        public IVRM_VersionControlProcessDTO Loadthetsavetasklistasklist([FromBody] IVRM_VersionControlProcessDTO data)
        {
            return _objinter.savetasklist(data);
        }
        //---------Save task list for approval of deployment
        [Route("getversiondetails")]
        public IVRM_VersionControlProcessDTO getversiondetails([FromBody] IVRM_VersionControlProcessDTO data)
        {
            return _objinter.getversiondetails(data);
        }
        //---------Save the deployment status
        [Route("savedeployment")]
        public IVRM_VersionControlProcessDTO savedeployment([FromBody] IVRM_VersionControlProcessDTO data)
        {
            return _objinter.savedeployment(data);
        }

        [Route("IVRM_CLient_version")]
        public IVRM_VersionControlProcessDTO IVRM_CLient_version([FromBody] IVRM_VersionControlProcessDTO data)
        {
            return _objinter.IVRM_CLient_version(data);
        }
    }
}