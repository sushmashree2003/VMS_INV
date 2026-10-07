using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.VMS.HRMS;
using Recruitment.com.vaps.Interfaces;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Recruitment.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class ISM_Sales_BOMFacadeController : Controller
    {

        public ISM_Sales_BOMInterface _ads;

        public ISM_Sales_BOMFacadeController(ISM_Sales_BOMInterface adstu)
        {
            _ads = adstu;
        }

        // GET: api/values
        //[HttpGet]
        [Route("getloaddata")]
        public ISM_Sales_BOMDTO getloaddata([FromBody] ISM_Sales_BOMDTO dto)
        {
            return _ads.getloaddata(dto);
        }

        [Route("savedetails")]
        public ISM_Sales_BOMDTO savedetails([FromBody] ISM_Sales_BOMDTO dto)
        {
            return _ads.savedetails(dto);
        }
        [Route("saveiteamsdetails")]
        public ISM_Sales_BOMDTO saveiteamsdetails([FromBody] ISM_Sales_BOMDTO dto)
        {
            return _ads.saveiteamsdetails(dto);
        }
        [Route("deactive")]
        public ISM_Sales_BOMDTO deactive([FromBody] ISM_Sales_BOMDTO dto)
        {
            return _ads.deactive(dto);
        }

        [Route("saveBOMdetails")]
        public ISM_Sales_BOMDTO saveBOMdetails([FromBody] ISM_Sales_BOMDTO dto)
        {
            return _ads.saveBOMdetails(dto);
        }

        [Route("viewservicedetails")]
        public ISM_Sales_BOMDTO viewservicedetails([FromBody] ISM_Sales_BOMDTO dto)
        {
            return _ads.viewservicedetails(dto);
        }
        [Route("Sales_BOM_edit")]
        public ISM_Sales_BOMDTO Sales_BOM_edit([FromBody] ISM_Sales_BOMDTO dto)
        {
            return _ads.Sales_BOM_edit(dto);
        }

        

    }
}
