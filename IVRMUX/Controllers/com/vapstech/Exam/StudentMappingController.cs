
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
    public class StudentMappingController : Controller
    {

        StudentMappingDelegates mStudentMappingdelStr = new StudentMappingDelegates();
      
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }
        [Route("Getdetails")]
        public StudentMappingDTO Getdetails(StudentMappingDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            StudentMappingDTO tdata= mStudentMappingdelStr.Getdetails(data);
            tdata.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));

            return tdata;
        }

        [Route("Studentdetails")]
        public StudentMappingDTO Studentdetails([FromBody]StudentMappingDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return mStudentMappingdelStr.Studentdetails(data);
        }

        [Route("getcategory/{id:int}")]
        public StudentMappingDTO getcategory(int id)
        {
            StudentMappingDTO data = new StudentMappingDTO();
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = id; 
            return mStudentMappingdelStr.getcategory(data);

        }

        //[Route("getclassid/{id:int}")]
        //public StudentMappingDTO getclassid(int id)
        //{
        //    StudentMappingDTO data = new StudentMappingDTO();
        //    data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
        //    data.EMCA_Id = id;
        //    return mStudentMappingdelStr.getclassid(data);

        //}
        [Route("getclassid")]
        public StudentMappingDTO getclassid([FromBody] StudentMappingDTO data)
        {
           // StudentMappingDTO data = new StudentMappingDTO();
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            //data.EMCA_Id = id;
            return mStudentMappingdelStr.getclassid(data);

        }

        [Route("getsubject/{id:int}")]
        public StudentMappingDTO getsubject(int id)
        {
            StudentMappingDTO data = new StudentMappingDTO();
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.EMG_Id = id;
            return mStudentMappingdelStr.getsubject(data);

        }

        [Route("savedetails")]
        public StudentMappingDTO savedetails([FromBody] StudentMappingDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return mStudentMappingdelStr.savedetails(data);
        }


        [Route("deactivate")]
        public StudentMappingDTO deactivate([FromBody] StudentMappingDTO data)
        {
            
            return mStudentMappingdelStr.deactivate(data);
        }

        
        [Route("editdetails/{id:int}")]
        public StudentMappingDTO editdetails(int ID)
        {
            return mStudentMappingdelStr.editdetails(ID);         
        }

        [Route("getalldetailsviewrecords/{id:int}")]
        public StudentMappingDTO getalldetailsviewrecords(int ID)
        {
            return mStudentMappingdelStr.getalldetailsviewrecords(ID);
        }

        [HttpPost]
        [Route("get_cls_sections")]
        public StudentMappingDTO get_cls_sections([FromBody] StudentMappingDTO categorypage)
        {
            categorypage.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return mStudentMappingdelStr.get_cls_sections(categorypage);

        }
    }

}
