using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IssueManager.com.QRcode_Activity.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.IssueManager.QRcode_Activity;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IssueManager.com.QRcode_Activity.Facade
{
    [Route("api/[controller]")]
    public class StaffAreaActivityRecordFacade : Controller
    {
        // GET: api/values
        StaffAreaActivityRecordInterface _Inv;

        public StaffAreaActivityRecordFacade(StaffAreaActivityRecordInterface Inv)
        {
            _Inv = Inv;
        }
        [Route("getloaddata")]
        public StaffAreaActivityRecordDTO getloaddata([FromBody] StaffAreaActivityRecordDTO data)
        {
            return _Inv.getloaddata(data);
        }
        
        [Route("savedetails")]
        public StaffAreaActivityRecordDTO savedetails([FromBody] StaffAreaActivityRecordDTO data)
        {
          
            return _Inv.savedetails(data);
        }
        [Route("Activitysavedetails")]
        public StaffAreaActivityRecordDTO Activitysavedetails([FromBody] StaffAreaActivityRecordDTO data)
        {

            return _Inv.Activitysavedetails(data);
        }


    }
}
