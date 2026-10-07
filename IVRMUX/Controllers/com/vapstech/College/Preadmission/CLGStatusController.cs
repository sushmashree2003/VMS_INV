using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.College.Admission;
using corewebapi18072016.Delegates.com.vapstech.College.Admission;
using Microsoft.AspNetCore.Http;
using IVRMUX.Delegates.com.vapstech.College.Preadmission;
using PreadmissionDTOs.com.vaps.College.Preadmission;
using PreadmissionDTOs;
using corewebapi18072016.Delegates;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.College.Preadmission
{
    [Route("api/[controller]")]
    public class CLGStatusController : Controller
    {
        CLGStatusDelegate delegates = new CLGStatusDelegate();
        StudentApplicationDelegate sad = new StudentApplicationDelegate();
        [Route("Getdetails")]
        public CollegePreadmissionstudnetDto Getdetails()
        {
            CollegePreadmissionstudnetDto dto = new CollegePreadmissionstudnetDto();
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.ASMAY_Id= Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            dto.ID = UserId;

            dto.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return delegates.Getdetails(dto);
        }
        [Route("getCourse/{YearId:int}")]
        public CollegePreadmissionstudnetDto getCourse(int YearId)
        {
            CollegePreadmissionstudnetDto obj = new CollegePreadmissionstudnetDto();
            obj.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id_Preadmission"));
            obj.ASMAY_Id = YearId;
            return delegates.getCourse(obj);
        }
        [Route("getBranch")]
        public CollegePreadmissionstudnetDto getBranch([FromBody]CollegePreadmissionstudnetDto dt)
        {
            dt.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id_Preadmission"));
            return delegates.getBranch(dt);
        }
        
        [Route("SearchData")]
        public CollegePreadmissionstudnetDto getStudentOnSearchFilter([FromBody] CollegePreadmissionstudnetDto cdto)
        {
            cdto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return delegates.SearchData(cdto);
        }

    }
}
