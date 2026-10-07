using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.FrontOffice;
using corewebapi18072016.Delegates.com.vapstech.FrontOffice;
using Microsoft.AspNetCore.Http;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers.com.vapstech.FrontOffice
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class ChecklistDetailsMappingController : Controller
    {
        ChecklistDetailsMappingDelegate dele = new ChecklistDetailsMappingDelegate();
        // GET: api/values
        [HttpGet("getalldetails/{id:int}")]
        public ISM_Checklist_DetailsDTO getalldetails(int id)
        {
            ISM_Checklist_DetailsDTO obj = new ISM_Checklist_DetailsDTO();
            obj.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));

            return dele.getalldetails(obj);
        }
        [HttpPost("editRecord")]     
        public ISM_Checklist_DetailsDTO editRecord([FromBody]ISM_Checklist_DetailsDTO id)
        {
            id.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return dele.editRecord(id);
        }

        [HttpPost]
        [Route("savedata")]
        public ISM_Checklist_DetailsDTO savedata([FromBody]ISM_Checklist_DetailsDTO obj)
        {
            obj.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return dele.savedata(obj);
        }

        [HttpGet("approvalloaddata/{id:int}")]
        public ISM_Checklist_DetailsDTO approvalloaddata(int id)
        {
            ISM_Checklist_DetailsDTO obj = new ISM_Checklist_DetailsDTO();
            obj.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return dele.approvalloaddata(obj);
        }

        [Route("findchecklist")]
        public ISM_Checklist_DetailsDTO findchecklist([FromBody]ISM_Checklist_DetailsDTO obj)
        {
            obj.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return dele.findchecklist(obj);
        }

        [Route("saveapproval")]
        public ISM_Checklist_DetailsDTO saveapproval([FromBody]ISM_Checklist_DetailsDTO obj)
        {
            obj.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return dele.saveapproval(obj);
        }

        [Route("checklistreport")]
        public ISM_Checklist_DetailsDTO checklistreport([FromBody]ISM_Checklist_DetailsDTO obj)
        {
            obj.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return dele.checklistreport(obj);
        }
        //[Route("Change")]
        //public ISM_Checklist_DetailsDTO Change([FromBody]ISM_Checklist_DetailsDTO obj)
        //{
        //    obj.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
        //    return dele.Change(obj);
        //}

        // POST api/values

        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }
        [HttpPost("DeleteYss")]
        public ISM_Checklist_DetailsDTO DeleteYss([FromBody]ISM_Checklist_DetailsDTO id)
        {
            id.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return dele.DeleteYss(id);
        }
        

        // DELETE api/values/5
        //  [Route("delete/{id:int}")]
        //  public ISM_Checklist_DetailsDTO delete_data(int id)
        //    {
        //    return dele.delete_data(id);
        //}
    }
}
