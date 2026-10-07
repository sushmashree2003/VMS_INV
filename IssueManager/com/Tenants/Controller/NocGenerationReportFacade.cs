using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IssueManager.com.Tenants.Interface;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.IssueManager.Tenants;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IssueManager.com.Tenants.Controller
{
    [Route("api/[controller]")]
    public class NocGenerationReportFacade : ControllerBase
    {
            public NocGenerationReportInterface _objinter;

            public NocGenerationReportFacade(NocGenerationReportInterface parameter)
            {
                _objinter = parameter;
            }

            [Route("getdetails")]
            public Task<NocGenerationReportDTO> getdetails([FromBody] NocGenerationReportDTO data)
            {
                return _objinter.getdetails(data);
            }

            [Route("get_report")]
            public Task<NocGenerationReportDTO> get_report([FromBody]NocGenerationReportDTO data)
            {
                return _objinter.get_report(data);
            }

            [Route("buildingchange")]
            public NocGenerationReportDTO buildingchange([FromBody] NocGenerationReportDTO data)
            {
                return _objinter.buildingchange(data);
            }

            [Route("Floorchange")]
            public NocGenerationReportDTO Floorchange([FromBody] NocGenerationReportDTO data)
            {
                return _objinter.Floorchange(data);
            }
            [Route("getallofflinedetails")]
            public NocGenerationReportDTO getallofflinedetails([FromBody] NocGenerationReportDTO data)
            {
                return _objinter.getallofflinedetails(data);
            }

            [Route("SaveElectricalbill")]
            public NocGenerationReportDTO SaveElectricalbill([FromBody] NocGenerationReportDTO data)
            {
                return _objinter.SaveElectricalbill(data);
            }

            [Route("editdetails")]
            public NocGenerationReportDTO editdetails([FromBody] NocGenerationReportDTO data)
            {
                return _objinter.editdetails(data);
            }

            [Route("deactive")]
            public NocGenerationReportDTO deactive([FromBody] NocGenerationReportDTO data)
            {
                return _objinter.deactive(data);
            }

            [Route("sendmail")]
            public NocGenerationReportDTO sendmail([FromBody] NocGenerationReportDTO data)
            {
                return _objinter.sendmail(data);
            }
        }
}
