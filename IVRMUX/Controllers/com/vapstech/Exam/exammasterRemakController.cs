
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
    [Route("api/[controller]")]
    public class exammasterRemakController : Controller
    {

        exammasterRemarkDelegates exammasterRemarkDelegatesStr = new exammasterRemarkDelegates();
      
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }
        [Route("Getdetails")]
        public exammasterRemarkDTO Getdetails(exammasterRemarkDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return exammasterRemarkDelegatesStr.Getdetails(data);            
        }

        [Route("editdetails/{id:int}")]
        public exammasterRemarkDTO editdetails(int ID)
        {
            return exammasterRemarkDelegatesStr.editdetails(ID);
        }
        
        [Route("validateordernumber")]
        public exammasterRemarkDTO validateordernumber([FromBody] exammasterRemarkDTO data)
        {
          //  data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return exammasterRemarkDelegatesStr.validateordernumber(data);
        }

        [Route("savedetails")]
        public exammasterRemarkDTO savedetails([FromBody] exammasterRemarkDTO data)
        {     
            data.MI_Id= Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return exammasterRemarkDelegatesStr.savedetails(data);
        }

        [Route("deactivate")]
        public exammasterRemarkDTO deactivate([FromBody] exammasterRemarkDTO data)
        {
            return exammasterRemarkDelegatesStr.deactivate(data);         
        }

        // Exam wise Student Remarks
        [Route("studentdataload")]
        public exammasterRemarkDTO studentdataload(exammasterRemarkDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserName = Convert.ToString(HttpContext.Session.GetString("UserName"));
            data.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            data.roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return exammasterRemarkDelegatesStr.studentdataload(data);
        }
        [Route("onchangeyear")]
        public exammasterRemarkDTO onchangeyear([FromBody]exammasterRemarkDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserName = Convert.ToString(HttpContext.Session.GetString("UserName"));
            data.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            data.roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return exammasterRemarkDelegatesStr.onchangeyear(data);
        }

        [Route("onchangeclass")]
        public exammasterRemarkDTO onchangeclass([FromBody]exammasterRemarkDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserName = Convert.ToString(HttpContext.Session.GetString("UserName"));
            data.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            data.roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return exammasterRemarkDelegatesStr.onchangeclass(data);
        }

        [Route("onchangesection")]
        public exammasterRemarkDTO onchangesection([FromBody]exammasterRemarkDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserName = Convert.ToString(HttpContext.Session.GetString("UserName"));
            data.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            data.roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return exammasterRemarkDelegatesStr.onchangesection(data);
        }
        [Route("searchdata")]
        public exammasterRemarkDTO searchdata([FromBody]exammasterRemarkDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserName = Convert.ToString(HttpContext.Session.GetString("UserName"));
            data.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            data.roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return exammasterRemarkDelegatesStr.searchdata(data);
        }
        [Route("savemapping")]
        public exammasterRemarkDTO savemapping([FromBody]exammasterRemarkDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserName = Convert.ToString(HttpContext.Session.GetString("UserName"));
            data.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            data.roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return exammasterRemarkDelegatesStr.savemapping(data);
        }
        [Route("editmappingdetails")]
        public exammasterRemarkDTO editmappingdetails([FromBody]exammasterRemarkDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserName = Convert.ToString(HttpContext.Session.GetString("UserName"));
            data.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            data.roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return exammasterRemarkDelegatesStr.editmappingdetails(data);
        }



    }

}
