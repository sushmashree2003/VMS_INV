using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FrontOfficeHub.com.vaps.Interfaces;
using PreadmissionDTOs.com.vaps.FrontOffice;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace FrontOfficeHub.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class DriverOTFacade : Controller
    {
        public DriverOTInterface _org;
        public DriverOTFacade(DriverOTInterface maspag)
        {
            _org = maspag;
        }

        [HttpPost]
        [Route("getalldetails")]
        public DriverOTDTO Getdet([FromBody] DriverOTDTO data)
        {
            return _org.getdata(data);
        }
        [Route("employeereport")]
        public DriverOTDTO employeereport([FromBody] DriverOTDTO data)
        {
            return _org.employeereport(data);
        }
        [Route("savedata")]
        public DriverOTDTO savedata([FromBody] DriverOTDTO data)
        {
            return _org.savedata(data);
        }

        [Route("getdetails/{id:int}")]
        public DriverOTDTO getdetails(int id)
        {
            return _org.getdetails(id);
        }

        [Route("deletedata/{id:int}")]
        public DriverOTDTO deletedata(int id)
        {
            return _org.deletedata(id);
        }

        [Route("driverInTimeEntry")]
        public DriverOTDTO driverInTimeEntry([FromBody] DriverOTDTO data)
        {
            return _org.driverInTimeEntry(data);
        }

        [Route("driverOutTimeEntry")]
        public DriverOTDTO driverOutTimeEntry([FromBody] DriverOTDTO data)
        {
            return _org.driverOutTimeEntry(data);
        }

        [HttpPost]
        [Route("getApprovalStatus")]
        public Task<DriverOTDTO> getApprovalStatus([FromBody] DriverOTDTO data)
        {
            return _org.getApprovalStatusAsync(data);
        }

        [HttpPost]
        [Route("get_status")]
        public DriverOTDTO get_status([FromBody] DriverOTDTO data)
        {
            return _org.get_status(data);
        }

        [HttpPost]
        [Route("reject_status")]
        public DriverOTDTO reject_status([FromBody] DriverOTDTO data)
        {
            return _org.reject_status(data);
        }

        [HttpPost]
        [Route("OTFinalReportAsync")]
        public Task<DriverOTDTO> OTFinalReportAsync([FromBody] DriverOTDTO data)
        {
            return _org.OTFinalReportAsync(data);
        }
    }
}
