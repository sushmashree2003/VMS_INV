using FrontOffice.com.vaps.Interfaces;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.FrontOffice;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FrontOffice.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class MasterHolidayWorkingTypeFacadeController: Controller
    {
        public MasterHolidayWorkingTypeInterface _org;

        public MasterHolidayWorkingTypeFacadeController(MasterHolidayWorkingTypeInterface maspag)
        {
            _org = maspag;
        }

        [HttpPost]
        [Route("getalldetails")]

        public MasterHolidayWorkingTypeDTO Getdet([FromBody] MasterHolidayWorkingTypeDTO data)
        {
            return _org.getdata(data);
        }

        [HttpPost]
        [Route("savedetails")]
        public MasterHolidayWorkingTypeDTO savedetails([FromBody] MasterHolidayWorkingTypeDTO data)
        {
            return _org.savedetails(data);
        }

    }
}
