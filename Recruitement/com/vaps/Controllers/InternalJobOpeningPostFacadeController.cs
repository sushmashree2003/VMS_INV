using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.VMS.HRMS;
using Recruitment.com.vaps.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Recruitment.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class InternalJobOpeningPostFacadeController : Controller
    {
        public InternalJobOpeningPostInterface _abs;

        public InternalJobOpeningPostFacadeController(InternalJobOpeningPostInterface objf)
        {
            _abs = objf;
        }

        [Route("getalldetails")]
        public InternalJobOpeningPostDTO getalldetails([FromBody]InternalJobOpeningPostDTO data)
        {
            return _abs.getalldetails(data);
        }
    }
}
