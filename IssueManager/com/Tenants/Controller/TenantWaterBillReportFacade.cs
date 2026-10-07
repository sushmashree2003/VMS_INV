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
    public class TenantWaterBillReportFacade : ControllerBase
    {
        public TenantWaterBillReportInterface _objinter;
        
        public TenantWaterBillReportFacade(TenantWaterBillReportInterface parameter)
        {
            _objinter = parameter;
        }

        [Route("getdetails")]
        public Task<TenantWaterBillReportDTO> getdetails([FromBody] TenantWaterBillReportDTO data)
        {
            return _objinter.getdetails(data);
        }

        [Route("get_report")]
        public Task<TenantWaterBillReportDTO> get_report([FromBody]TenantWaterBillReportDTO data)
        {
            return _objinter.get_report(data);
        }

        [Route("buildingchange")]
        public TenantWaterBillReportDTO buildingchange([FromBody] TenantWaterBillReportDTO data)
        {
            return _objinter.buildingchange(data);
        }

        [Route("Floorchange")]
        public TenantWaterBillReportDTO Floorchange([FromBody] TenantWaterBillReportDTO data)
        {
            return _objinter.Floorchange(data);
        }
        [Route("sendmail")]
        public TenantWaterBillReportDTO sendmail([FromBody] TenantWaterBillReportDTO data)
        {
            return _objinter.sendmail(data);
        }

    }
}
