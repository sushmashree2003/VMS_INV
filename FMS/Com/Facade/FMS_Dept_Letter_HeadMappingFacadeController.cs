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
    public class FMS_Dept_Letter_HeadMappingFacadeController : Controller
    {
        public FMS_Dept_Letter_HeadMappingInterface _interface; 

        public FMS_Dept_Letter_HeadMappingFacadeController(FMS_Dept_Letter_HeadMappingInterface _inter)
        {
            _interface = _inter;
        }

        // GET: api/<controller>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [Route("LoadData")]
        public FMS_Dept_Letter_HeadMappingDTO LoadData([FromBody] FMS_Dept_Letter_HeadMappingDTO data)
        {
            return _interface.LoadData(data);
        }

        [Route("OnChangeInstiution")]
        public FMS_Dept_Letter_HeadMappingDTO OnChangeInstiution([FromBody] FMS_Dept_Letter_HeadMappingDTO data)
        {
            return _interface.OnChangeInstiution(data);
        }

        [Route("OnSaveData")]
        public FMS_Dept_Letter_HeadMappingDTO OnSaveData([FromBody] FMS_Dept_Letter_HeadMappingDTO data)
        {
            return _interface.OnSaveData(data);
        }

        [Route("EditLetterHead")]
        public FMS_Dept_Letter_HeadMappingDTO EditLetterHead([FromBody] FMS_Dept_Letter_HeadMappingDTO data)
        {
            return _interface.EditLetterHead(data);
        }

        [Route("OnDelete")]
        public FMS_Dept_Letter_HeadMappingDTO OnDelete([FromBody] FMS_Dept_Letter_HeadMappingDTO data)
        {
            return _interface.OnDelete(data);
        }

        // Letter Head Tracking
        [Route("TrackingLoadData")]
        public FMS_Dept_Letter_HeadMappingDTO TrackingLoadData([FromBody] FMS_Dept_Letter_HeadMappingDTO data)
        {
            return _interface.TrackingLoadData(data);
        }

        [Route("TrackingOnChangeInstiution")]
        public FMS_Dept_Letter_HeadMappingDTO TrackingOnChangeInstiution([FromBody] FMS_Dept_Letter_HeadMappingDTO data)
        {
            return _interface.TrackingOnChangeInstiution(data);
        }

        [Route("TrackingOnChangeDept")]
        public FMS_Dept_Letter_HeadMappingDTO TrackingOnChangeDept([FromBody] FMS_Dept_Letter_HeadMappingDTO data)
        {
            return _interface.TrackingOnChangeDept(data);
        }

        [Route("TrackingOnSaveData")]
        public FMS_Dept_Letter_HeadMappingDTO TrackingOnSaveData([FromBody] FMS_Dept_Letter_HeadMappingDTO data)
        {
            return _interface.TrackingOnSaveData(data);
        }

        [Route("TrackingEditLetterHead")]
        public FMS_Dept_Letter_HeadMappingDTO TrackingEditLetterHead([FromBody] FMS_Dept_Letter_HeadMappingDTO data)
        {
            return _interface.TrackingEditLetterHead(data);
        }

        [Route("TrackingOnDelete")]
        public FMS_Dept_Letter_HeadMappingDTO TrackingOnDelete([FromBody] FMS_Dept_Letter_HeadMappingDTO data)
        {
            return _interface.TrackingOnDelete(data);
        }

        // Report 
        [Route("ReportLoadData")]
        public FMS_Dept_Letter_HeadMappingDTO ReportLoadData([FromBody] FMS_Dept_Letter_HeadMappingDTO data)
        {
            return _interface.ReportLoadData(data);
        }

        [Route("ReportOnChangeInstiution")]
        public FMS_Dept_Letter_HeadMappingDTO ReportOnChangeInstiution([FromBody] FMS_Dept_Letter_HeadMappingDTO data)
        {             
            return _interface.ReportOnChangeInstiution(data);
        }

        [Route("ReportOnChangeDept")]
        public FMS_Dept_Letter_HeadMappingDTO ReportOnChangeDept([FromBody] FMS_Dept_Letter_HeadMappingDTO data)
        {             
            return _interface.ReportOnChangeDept(data);
        }

        [Route("GetLetterHeadReport")]
        public FMS_Dept_Letter_HeadMappingDTO GetLetterHeadReport([FromBody] FMS_Dept_Letter_HeadMappingDTO data)
        { 
            return _interface.GetLetterHeadReport(data);
        }
    }
}
