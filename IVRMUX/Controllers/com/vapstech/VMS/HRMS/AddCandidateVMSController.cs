using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using PreadmissionDTOs.com.vaps.VMS.HRMS;
using corewebapi18072016.Delegates.com.vapstech.HRMS;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers.com.vapstech.VMS.HRMS
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class AddCandidateVMSController : Controller
    {
        AddCandidateVMSDelegate del = new AddCandidateVMSDelegate();
        // GET: api/values
        [HttpGet]
        [Route("getalldetails/{id:int}")]
        public HR_Candidate_DetailsDTO getalldetails(int id)
        {
            HR_Candidate_DetailsDTO dto = new HR_Candidate_DetailsDTO();
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            //dto.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return del.onloadgetdetails(dto);
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public HR_Candidate_DetailsDTO Post([FromBody]HR_Candidate_DetailsDTO dto)
        {
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.HRCD_UpdatedBy = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            //dto.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));

            return del.savedetails(dto);
        }

        [Route("editRecord/{id:int}")]
        public HR_Candidate_DetailsDTO editRecord(int id)
        {
          //  HR_Candidate_DetailsDTO dto = new HR_Candidate_DetailsDTO();
           // dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.getRecorddetailsById(id);
        }

        [Route("Get_Desgination")]
        public HR_Candidate_DetailsDTO Get_Desgination([FromBody] HR_Candidate_DetailsDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.Get_Desgination(data);
        }
        [Route("saveAppointmentdata")]
        public HR_Candidate_DetailsDTO saveAppointmentdata([FromBody] HR_Candidate_DetailsDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.saveAppointmentdata(data);
        }

        [Route("addQualification")]
        public HR_Candidate_DetailsDTO addQualification([FromBody] HR_Candidate_DetailsDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.addQualification(data);
        }

        [Route("addexperience")]
        public HR_Candidate_DetailsDTO addexperience([FromBody] HR_Candidate_DetailsDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.addexperience(data);
        }

        [Route("addfamily")]
        public HR_Candidate_DetailsDTO addfamily([FromBody] HR_Candidate_DetailsDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.addfamily(data);
        }

        [Route("addlanguage")]
        public HR_Candidate_DetailsDTO addlanguage([FromBody] HR_Candidate_DetailsDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.addlanguage(data);
        }
        //[Route("ActiveDeactiveRecord/{id:int}")]
        //public HR_Candidate_DetailsDTO ActiveDeactiveRecord(int id)
        //{
        //    HR_Candidate_DetailsDTO dto = new HR_Candidate_DetailsDTO();
        //    dto.HRMET_Id = id;
        //    dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
        //    dto.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));

        //    return del.deleterec(dto);
        //}

        
        [Route("sendCallLettermail")]
        public HR_Candidate_DetailsDTO sendCallLettermail([FromBody] HR_Candidate_DetailsDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.sendCallLettermail(data);
        }

        [Route("saveAppointmenttab")]
        public HR_Candidate_DetailsDTO saveAppointmenttab([FromBody]HR_Candidate_DetailsDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.saveAppointmenttab(data);
        }
        [Route("EditRecord")]
        public HR_Candidate_DetailsDTO EditRecord([FromBody]HR_Candidate_DetailsDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.EditRecord(data);
        }
        [Route("activedeactive")]
        public HR_Candidate_DetailsDTO activedeactive([FromBody]HR_Candidate_DetailsDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.activedeactive(data);
        }
        [Route("AddCandidateDocuments")]
        public HR_Candidate_DetailsDTO AddCandidateDocuments([FromBody] HR_Candidate_DetailsDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.AddCandidateDocuments(data);
        }
        [HttpGet]
        [Route("getalldetailsDocVerfy/{id:int}")]
        public HR_Candidate_DetailsDTO getalldetailsDocVerfy(int id)
        {
            HR_Candidate_DetailsDTO dto = new HR_Candidate_DetailsDTO();
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            //dto.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return del.getalldetailsDocVerfy(dto);
        }

        [Route("saveVerify")]
        public HR_Candidate_DetailsDTO saveVerify([FromBody] HR_Candidate_DetailsDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.saveVerify(data);
        }
        [Route("VerifyReports")]
        public HR_Candidate_DetailsDTO VerifyReports([FromBody] HR_Candidate_DetailsDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            //data.HRME_Id = Convert.ToInt32(HttpContext.Session.GetInt32("HRME_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.VerifyReports(data);
        }
    }
}
