using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.VMS.HRMS;
using Recruitment.com.vaps.Interfaces;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Recruitment.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class GeneralSendEmailFacade : Controller
    {
        public GeneralSendEmailInterface _ads;
        public GeneralSendEmailFacade(GeneralSendEmailInterface adstu)
        {
            _ads = adstu;
        }

        [Route("sendsmsemail")]
        public SalesSMSEMAILDTO sendsmsemail([FromBody]SalesSMSEMAILDTO dto)
        {
            return _ads.sendsmsemail(dto);
        }
        //sendsmsemailsales
        [Route("onload")]
        public SalesSMSEMAILDTO onload([FromBody]SalesSMSEMAILDTO dto)
        {
            return _ads.onload(dto);
        }

        [HttpPost]
        [Route("sendsmsemailsales")]
        public async Task<SalesSMSEMAILDTO> sendsmsemailsales([FromBody]SalesSMSEMAILDTO data)
        {
            return await _ads.sendsmsemailsales(data);
        }
    }
}
