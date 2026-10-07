using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FrontOfficeHub.com.vaps.Interfaces;
using PreadmissionDTOs.com.vaps.FrontOffice;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace FrontOfficeHub.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class GPSLocationFacade : Controller
    {
        public GPSLocationInterface _org;
        public GPSLocationFacade(GPSLocationInterface maspag)
        {
            _org = maspag;
        }

        [HttpPost]
        [Route("getalldetails")]
        public VMS_Attendance_GPSLocationDTO Getdet([FromBody] VMS_Attendance_GPSLocationDTO data)
        {
            return _org.getdata(data);
        }

        [HttpPost]
        [Route("savedata")]
        public VMS_Attendance_GPSLocationDTO savedata([FromBody]VMS_Attendance_GPSLocationDTO data)
        {
            return _org.savedata(data);
        }

    }
}
