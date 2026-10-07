using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FMS.Com.Interface;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.FMS;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FMS.Com.Facade
{
    [Route("api/[controller]")]
    public class FMS_Correspondance_ReportFacadeController : Controller
    {
        public FMS_Correspondance_ReportInterface _interface;

        public FMS_Correspondance_ReportFacadeController(FMS_Correspondance_ReportInterface _inter)
        {
            _interface = _inter;

        }

        // GET: api/<controller>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [Route("onload")]
        public FMS_Correspondance_ReportDTO onload([FromBody] FMS_Correspondance_ReportDTO data)
        {
            return _interface.onload(data);
        }

        [Route("onchnageradiobtnsfile")]
        public FMS_Correspondance_ReportDTO onchnageradiobtnsfile([FromBody] FMS_Correspondance_ReportDTO data)
        {
            return _interface.onchnageradiobtnsfile(data);
        }

        [Route("getprojectist")]
        public FMS_Correspondance_ReportDTO getprojectist([FromBody] FMS_Correspondance_ReportDTO data)
        {
            return _interface.getprojectist(data);
        }

        [Route("getreport")]
        public FMS_Correspondance_ReportDTO getreport([FromBody] FMS_Correspondance_ReportDTO data)
        {
            return _interface.getreport(data);
        }

        [Route("SearchReport")]
        public FMS_Correspondance_ReportDTO SearchReport([FromBody] FMS_Correspondance_ReportDTO data)
        {
            return _interface.SearchReport(data);
        }

        [Route("viewreferencedetais")]
        public FMS_Correspondance_ReportDTO viewreferencedetais([FromBody] FMS_Correspondance_ReportDTO data)
        {
            return _interface.viewreferencedetais(data);
        }

        [Route("CountReport")]
        public FMS_Correspondance_ReportDTO CountReport([FromBody] FMS_Correspondance_ReportDTO data)
        {
            return _interface.CountReport(data);
        }

        [Route("ViewRefernceNoDetails")]
        public FMS_Correspondance_ReportDTO ViewRefernceNoDetails([FromBody] FMS_Correspondance_ReportDTO data)
        {
            return _interface.ViewRefernceNoDetails(data);
        }

        // File Cabinet Report
        [Route("OnloadFileCabinetReport")]
        public FMS_Correspondance_ReportDTO OnloadFileCabinetReport([FromBody] FMS_Correspondance_ReportDTO data)
        {
            return _interface.OnloadFileCabinetReport(data);
        }

        [Route("FileCabinetReport")]
        public FMS_Correspondance_ReportDTO FileCabinetReport([FromBody] FMS_Correspondance_ReportDTO data)
        {
            return _interface.FileCabinetReport(data);
        }
    }
}