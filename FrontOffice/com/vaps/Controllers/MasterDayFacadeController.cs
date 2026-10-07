using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FrontOffice.com.vaps.Interfaces;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.FrontOffice;

namespace FrontOffice.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class MasterDayFacadeController : Controller
    {
        public MasterDayInterface _org;
        public MasterDayFacadeController(MasterDayInterface maspag)
        {
            _org = maspag;
        }

        [Route("getalldetails")]
        public MasterDayDTO getalldetails([FromBody] MasterDayDTO data)
        {
            return _org.getalldetails(data);
        }

       
        [Route("savedetails")]
        public MasterDayDTO savedetails([FromBody] MasterDayDTO data)
        {
            return _org.savedetails(data);
        } 

        [HttpPost]
        [Route("deactive")]

        public MasterDayDTO deactive([FromBody] MasterDayDTO data)
        {
           return _org.deactive(data);
        }



    }
}