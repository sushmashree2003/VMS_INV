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
    public class ISM_ExtensionFacade : Controller
    {

        public ISM_ExtensionInterface _objinter;

        public ISM_ExtensionFacade(ISM_ExtensionInterface parameter)
        {
            _objinter = parameter;
        }

        [Route("getdetails")]
        public Task<ISM_ExtensionDTO> getdetails([FromBody] ISM_ExtensionDTO data)
        {
            return _objinter.getdetails(data);
        }
        [Route("get_details")]
        public ISM_ExtensionDTO get_details([FromBody]ISM_ExtensionDTO data)
        {
            return _objinter.get_details(data);
        }       
        [Route("savedata")]
        public ISM_ExtensionDTO savedata([FromBody]ISM_ExtensionDTO data)
        {
            return _objinter.savedata(data);
        }


        [Route("getdata")]
        public Task<ISM_ExtensionDTO> getdata([FromBody]ISM_ExtensionDTO data)
        {
            return _objinter.getdata(data);
        }
        [Route("savedetails")]
        public ISM_ExtensionDTO savedetails([FromBody]ISM_ExtensionDTO data)
        {
            return _objinter.savedetails(data);
        }

    }
}
