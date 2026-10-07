using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Exam;
using corewebapi18072016.Delegates.com.vapstech.Exam;
using Microsoft.AspNetCore.Http;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers.com.vapstech.Exam
{
    [Route("api/[controller]")]
    public class StudentTransactionController : Controller
    {
        StudentTransactionDelegate delegates = new StudentTransactionDelegate();
        [Route("Onload/{id:int}")]
        public StudentTransactionDTO Onload(int id)
        {
            StudentTransactionDTO dto = new StudentTransactionDTO();
            dto.MI_Id= Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return delegates.getDetails(dto);
        }
        [Route("getStudentList")]
        public StudentTransactionDTO getStudentList([FromBody] StudentTransactionDTO dto)
        {
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return delegates.getStudentList(dto);
        }
        [Route("saveRecord")]
        public StudentTransactionDTO saveRecord([FromBody] StudentTransactionDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return delegates.save(data);
        }


    }
}
