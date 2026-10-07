using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.VMS.HRMS;
using Recruitment.com.vaps.Interfaces;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Recruitment.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class AppointmentFacadeController : Controller
    {

        public AppointmentInterface _ads;

        public AppointmentFacadeController(AppointmentInterface adstu)
        {
            _ads = adstu;
        }

        // GET: api/values
        [Route("onloadgetdetails")]
        public HR_Candidate_DetailsDTO getinitialdata([FromBody]HR_Candidate_DetailsDTO dto)
        {
            return _ads.getBasicData(dto);
        }

        // POST api/values
        [HttpPost]
        public HR_Candidate_DetailsDTO Post([FromBody]HR_Candidate_DetailsDTO dto)
        {
            return _ads.SaveUpdate(dto);
        }

        [Route("getRecordById/{id:int}")]

        public HR_Candidate_DetailsDTO getcatgrydet(int id)
        {
            // id = 12;
            return _ads.editData(id);
        }
        [Route("deactivateRecordById")]
        public HR_Candidate_DetailsDTO deactivateRecordById([FromBody]HR_Candidate_DetailsDTO dto)
        {
            return _ads.deactivate(dto);
        }
        [Route("Get_Desgination")]
        public HR_Candidate_DetailsDTO Get_Desgination([FromBody]HR_Candidate_DetailsDTO dto)
        {
            return _ads.Get_Desgination(dto);
        }
        [Route("saveAppointmentdata")]
        public HR_Candidate_DetailsDTO saveAppointmentdata([FromBody]HR_Candidate_DetailsDTO dto)
        {
            return _ads.saveAppointmentdata(dto);
        }
        [Route("getEmployeeSalaryDetailsByHead")]
        public HR_Candidate_DetailsDTO getEmployeeSalaryDetailsByHead([FromBody]HR_Candidate_DetailsDTO dto)
        {
            return _ads.getEmployeeSalaryDetailsByHead(dto);
        }
        [Route("getcandidate")]
        public HR_Candidate_DetailsDTO getcandidate([FromBody]HR_Candidate_DetailsDTO dto)
        {
            return _ads.getcandidate(dto);
        }  
        [Route("getCandidateList")]
        public HR_Candidate_DetailsDTO getCandidateList([FromBody]HR_Candidate_DetailsDTO dto)
        {
            return _ads.getCandidateList(dto);
        }
        [Route("getcandidatename")]
        public HR_Candidate_DetailsDTO getcandidatename([FromBody]HR_Candidate_DetailsDTO dto)
        {
            return _ads.getcandidatename(dto);
        }
        [Route("savesalarydata")]
        public HR_Candidate_DetailsDTO savesalarydata([FromBody]HR_Candidate_DetailsDTO dto)
        {
            return _ads.savesalarydata(dto);
        }
        [Route("getcompanydetails")]
        public HR_Candidate_DetailsDTO getcompanydetails([FromBody]HR_Candidate_DetailsDTO dto)
        {
            return _ads.getcompanydetails(dto);
        }
        [Route("savescopenature")]
        public HR_Candidate_DetailsDTO savescopenature([FromBody]HR_Candidate_DetailsDTO dto)
        {
            return _ads.savescopenature(dto);
        }
        [Route("deactivate1")]
        public HR_Candidate_DetailsDTO deactivate1([FromBody]HR_Candidate_DetailsDTO data)
        {
            return _ads.deactivate1(data);
        }
        [Route("SaveUpdateApproval")]
        public HR_Candidate_DetailsDTO SaveUpdateApproval([FromBody]HR_Candidate_DetailsDTO dto)
        {
            return _ads.SaveUpdateApproval(dto);
        }
        [Route("On_Desgination")]
        public HR_Candidate_DetailsDTO On_Desgination([FromBody]HR_Candidate_DetailsDTO dto)
        {
            return _ads.On_Desgination(dto);
        }


        [Route("onfloordetails")]
        public HR_Candidate_DetailsDTO getfloordetail([FromBody] HR_Candidate_DetailsDTO data)
        {
            return _ads.getfloordetail(data);
        }
        [Route("SaveMapDetails")]
        public HR_Candidate_DetailsDTO SaveMapDetails([FromBody] HR_Candidate_DetailsDTO data)
        {
            return _ads.SaveMapDetails(data);
        }

        [Route("SaveFODetails")]
        public HR_Candidate_DetailsDTO SaveFODetails([FromBody] HR_Candidate_DetailsDTO data)
        {
            return _ads.SaveFODetails(data);
        }
        [Route("SaveHRDetails")]
        public HR_Candidate_DetailsDTO SaveHRDetails([FromBody] HR_Candidate_DetailsDTO data)
        {
            return _ads.SaveHRDetails(data);
        }
        [Route("getReport")]
        public HR_Candidate_DetailsDTO getReport([FromBody] HR_Candidate_DetailsDTO data)
        {
            return _ads.getReport(data);
        }
        [Route("SaveAppraisal")]
        public HR_Candidate_DetailsDTO SaveAppraisal([FromBody] HR_Candidate_DetailsDTO data)
        {
            return _ads.SaveAppraisal(data);
        }
        [Route("saveVerifyApproval")]
        public HR_Candidate_DetailsDTO saveVerifyApproval([FromBody] HR_Candidate_DetailsDTO data)
        {
            return _ads.saveVerifyApproval(data);
        }
        //[Route("SaVeAppointmentsHead")]
        //public HR_Candidate_DetailsDTO SaVeAppointmentsHead([FromBody] HR_Candidate_DetailsDTO data)
        //{
        //    return _ads.SaVeAppointmentsHead(data);

        //}
    }
}
