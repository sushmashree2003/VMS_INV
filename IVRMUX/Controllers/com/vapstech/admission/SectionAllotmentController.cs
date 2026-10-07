using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using corewebapi18072016.Delegates;
using PreadmissionDTOs;
using Microsoft.AspNetCore.Http;
using PreadmissionDTOs.com.vaps.admission;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers
{
    [ValidateAntiForgeryToken]
    // [EnableCors("AllowAll")]
    [Route("api/[controller]")]
    public class SectionAllotmentController : Controller
    {
        SectionAllotmentDelegate SectionAllotment = new SectionAllotmentDelegate();

        [HttpGet]
        [Route("getalldetails/{id:int}")]
        public SchoolYearWiseStudentDTO Get(int id)
        {
            SchoolYearWiseStudentDTO SchoolYearWiseStudentDTO = new SchoolYearWiseStudentDTO();
            SchoolYearWiseStudentDTO.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return SectionAllotment.getInstitutiondata(SchoolYearWiseStudentDTO);
        }
        //get student details by year

        [HttpGet]
        [Route("GetStudentListByYear/{id:int}")]
        public SchoolYearWiseStudentDTO GetStudentListByYear(long id)
        {
            SchoolYearWiseStudentDTO SchoolYearWiseStudentDTO = new SchoolYearWiseStudentDTO();
            SchoolYearWiseStudentDTO.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));

            return SectionAllotment.getStudentdataByYear(id);
        }

        // POST api/values
        [HttpPost]
        //[Route("SaveInstitution")]
        public SchoolYearWiseStudentDTO SaveSectionAllotment([FromBody] SchoolYearWiseStudentDTO Ins)
        {
            Ins.LoginId=Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            Ins.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return SectionAllotment.saveSectionAllotmentdetails(Ins);
        }

        [Route("GetStudentListByYearAndCLass")]
        public SchoolYearWiseStudentDTO GetStudentListByYearAndCLass_CS([FromBody] SchoolYearWiseStudentDTO yearclass)
        {
            yearclass.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return SectionAllotment.GetstudentdetailsbyYearandclass(yearclass);
        }

        [Route("GetStudentListByURN")]
        public SchoolYearWiseStudentDTO GetStudentListByURN([FromBody] SchoolYearWiseStudentDTO data)
        {
            data.MI_Id= Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return SectionAllotment.GetStudentListByURN(data);
        }
        [Route("GetStudentListByURNsave")]
        public Student_Update_RollNumber GetStudentListByURNsave([FromBody]Student_Update_RollNumber data)
        {
            data.MI_Id= Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return SectionAllotment.GetStudentListByURNsave(data);
        }
    }
}
