using System;
using IVRMUX.Delegates.com.vapstech.IssueManager;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.IssueManager;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860
namespace IVRMUX.Controllers.com.vapstech.IssueManager
{
    [Route("api/[controller]")]
    public class Master_DepartmentHead_MappingController : Controller
    {
        Master_DepartmentHead_MappingDelegate del = new Master_DepartmentHead_MappingDelegate();
        [Route("loaddata/{id:int}")]
        public Master_DepartmentHead_MappingDTO loaddata(int id)
        {
          Master_DepartmentHead_MappingDTO data = new Master_DepartmentHead_MappingDTO();
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.loaddata(data);
        }
        [HttpPost]
        [Route("depchange")]
        public Master_DepartmentHead_MappingDTO depchange([FromBody] Master_DepartmentHead_MappingDTO data)
        {
            return del.depchange(data);
        }
        [Route("savedata")]
        public Master_DepartmentHead_MappingDTO savedata([FromBody]Master_DepartmentHead_MappingDTO data)
        {           
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.savedata(data);
        }
        [HttpPost]
        [Route("Decative")]
        public Master_DepartmentHead_MappingDTO Decative([FromBody] Master_DepartmentHead_MappingDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.Decative(data);
        }
        [Route("EditData")]
        public Master_DepartmentHead_MappingDTO Editdata([FromBody] Master_DepartmentHead_MappingDTO data)
        {

            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.Editdata(data);
        }

    }
}
