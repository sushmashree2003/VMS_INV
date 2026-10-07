using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.VMS.Sales;
using Recruitment.com.vaps.Interfaces;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Recruitment.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class SalesClientInformationFacadeController : Controller
    {
        public SalesClientInformationInterface _intface;
        public SalesClientInformationFacadeController(SalesClientInformationInterface intface)
        {
            _intface = intface;
        }

        [Route("OnLoadData/")]
        public SalesClientInformationDto OnLoadData([FromBody] SalesClientInformationDto dto)
        {
            return _intface.OnLoadData(dto);
        }

    }
}
