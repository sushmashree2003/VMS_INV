using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using corewebapi18072016.Delegates;
using DomainModel.Model;
using PreadmissionDTOs;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Cors;
using PreadmissionDTOs.com.vaps.admission;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class AdmissionStandardController : Controller
    {
        AdmissionStandardDelegate adsd = new AdmissionStandardDelegate();
       

        // POST api/values
        [HttpPost]
        [Route("savedata")]
        public AdmissionStandardDTO getclassstudentlist([FromBody]AdmissionStandardDTO student)
        {
            student.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return adsd.getlisttwo(student);
        }

        [HttpGet]
        [Route("loaddata/{id:int}")]
       // [Route("loaddata")]
        public AdmissionStandardDTO getdata(int id)
        {
            id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return adsd.getlistdata(id);
        }
      
    }
}
