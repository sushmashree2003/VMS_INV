using System.Threading.Tasks;
using HRMSServiceHub.com.vaps.Interfaces;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.HRMS;

namespace HRMSServiceHub.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class FF_SettlementAdminReportFacadeController : Controller
    {
        public FF_SettlementAdminReportInterface _ads;

            public FF_SettlementAdminReportFacadeController(FF_SettlementAdminReportInterface adstu)
            {
                _ads = adstu;
            }


            [Route("onloadgetdetails")]
            public AdvanceReportDTO getinitialdata([FromBody]AdvanceReportDTO data)
            {
                return _ads.getBasicData(data);
            }

            [Route("getEmployeedetailsBySelection")]
            public Task<AdvanceReportDTO> getEmployeedetailsBySelection([FromBody]AdvanceReportDTO dto)
            {
                return _ads.getEmployeedetailsBySelection(dto);
            }
            [Route("get_depts")]
            public AdvanceReportDTO get_depts([FromBody]AdvanceReportDTO dto)
            {
                return _ads.get_depts(dto);
            }
            [Route("get_desig")]
            public AdvanceReportDTO get_desig([FromBody]AdvanceReportDTO dto)
            {
                return _ads.get_desig(dto);
            }
    }
}
