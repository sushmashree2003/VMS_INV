using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IssueManager.com.TADA.Interface;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.VMS.TADA;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IssueManager.com.TADA.Controllers
{
    [Route("api/[controller]")]
    public class TADA_GPsLOcataionFacade : Controller
    {
        public TADA_GPsLOcataionInterface _objinter;
        public TADA_GPsLOcataionFacade(TADA_GPsLOcataionInterface parameter)
        {
            _objinter = parameter;
        }

        [Route("saveLocation")]
        public TADA_GPsLOcataionDTO saveLocation([FromBody]TADA_GPsLOcataionDTO data)
        {
            return _objinter.saveLocation(data);
        }
        [Route("GetLocation")]
        public TADA_GPsLOcataionDTO GetLocation([FromBody]TADA_GPsLOcataionDTO data)
        {
            return _objinter.GetLocation(data);
        }
    }
}
