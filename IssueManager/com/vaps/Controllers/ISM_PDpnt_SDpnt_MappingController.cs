using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IssueManager.com.vaps.Interfaces;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.IssueManager;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IssueManager.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class ISM_PDpnt_SDpnt_MappingController : Controller
    {
        public ISM_PDpnt_SDpnt_MappingInterface _objinter;

        public ISM_PDpnt_SDpnt_MappingController(ISM_PDpnt_SDpnt_MappingInterface parameter)
        {
            _objinter = parameter;
        }

        [Route("loaddata")]
        public Task<ISM_PDpnt_SDpnt_MappingDTO> loaddata([FromBody] ISM_PDpnt_SDpnt_MappingDTO data)
        {
            return _objinter.loaddataAsync(data);
        }

        [Route("save")]
        public ISM_PDpnt_SDpnt_MappingDTO save([FromBody] ISM_PDpnt_SDpnt_MappingDTO data)
        {
            return _objinter.save(data);
        }

        [Route("editdata")]
        public ISM_PDpnt_SDpnt_MappingDTO editdata([FromBody] ISM_PDpnt_SDpnt_MappingDTO data)
        {
            return _objinter.editdata(data);
        }

        [Route("deletedata")]
        public ISM_PDpnt_SDpnt_MappingDTO deletedata([FromBody] ISM_PDpnt_SDpnt_MappingDTO data)
        {
            return _objinter.deletedata(data);
        }

        [Route("loaddepartment")]
        public ISM_PDpnt_SDpnt_MappingDTO loaddepartment([FromBody] ISM_PDpnt_SDpnt_MappingDTO data)
        {
            return _objinter.loaddepartment(data);
        }
        
    }
}
