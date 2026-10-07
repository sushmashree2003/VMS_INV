
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using corewebapi18072016.Delegates;
using DomainModel.Model.com.vaps.Exam;
using PreadmissionDTOs.com.vaps.Exam;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class exammasterController : Controller
    {

        exammasterDelegates exammasterdelStr = new exammasterDelegates();
      
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }
        [Route("Getdetails")]
        public exammasterDTO Getdetails(exammasterDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return exammasterdelStr.Getdetails(data);            
        }

        [Route("editdetails/{id:int}")]
        public exammasterDTO editdetails(int ID)
        {
            return exammasterdelStr.editdetails(ID);
        }
        
        [Route("validateordernumber")]
        public exammasterDTO validateordernumber([FromBody] exammasterDTO data)
        {
          //  data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return exammasterdelStr.validateordernumber(data);
        }

        [Route("savedetails")]
        public exammasterDTO savedetails([FromBody] exammasterDTO data)
        {     
            data.MI_Id= Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return exammasterdelStr.savedetails(data);
        }

        [Route("deactivate")]
        public exammasterDTO deactivate([FromBody] exammasterDTO data)
        {
            return exammasterdelStr.deactivate(data);         
        }

       
    }

}
