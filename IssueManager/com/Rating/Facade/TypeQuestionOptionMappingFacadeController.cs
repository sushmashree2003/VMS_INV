using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IssueManager.com.Rating.Interfacce;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.IssueManager.Rating;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IssueManager.com.Rating.Facade
{
    [Route("api/[controller]")]
    public class TypeQuestionOptionMappingFacadeController : Controller
    {
        public TypeQuestionOptionMappingInterface _interface;

        public TypeQuestionOptionMappingFacadeController(TypeQuestionOptionMappingInterface _interf)
        {
            _interface = _interf;
        }
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [Route("OnLoadRatingMasterType")]
        public Rating_ReportDTO OnLoadRatingMasterType([FromBody] Rating_ReportDTO data)
        {
            return _interface.OnLoadRatingMasterType(data);
        }

       
        [Route("getReport")]
        public Task<Rating_ReportDTO> getReport([FromBody] Rating_ReportDTO data)
        {
            return _interface.getReport(data);
        }       


        [Route("getDeptReport")]
        public Task<Rating_ReportDTO> getDeptReport([FromBody] Rating_ReportDTO data)
        {
            return _interface.getDeptReport(data);
        }
    }
}
