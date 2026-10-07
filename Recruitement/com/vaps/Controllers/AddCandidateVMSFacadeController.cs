using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.VMS.HRMS;
using Recruitment.com.vaps.Interfaces;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Recruitment.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class AddCandidateVMSFacadeController : Controller
    {

        public AddCandidateVMSInterface _ads;

        public AddCandidateVMSFacadeController(AddCandidateVMSInterface adstu)
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

        [Route("addQualification")]
        public HR_Candidate_DetailsDTO addQualification([FromBody]HR_Candidate_DetailsDTO dto)
        {
            return _ads.addQualification(dto);
        }
        [Route("addexperience")]
        public HR_Candidate_DetailsDTO addexperience([FromBody]HR_Candidate_DetailsDTO dto)
        {
            return _ads.addexperience(dto);
        }
        [Route("addfamily")]
        public HR_Candidate_DetailsDTO addfamily([FromBody]HR_Candidate_DetailsDTO dto)
        {
            return _ads.addfamily(dto);
        }
        [Route("addlanguage")]
        public HR_Candidate_DetailsDTO addlanguage([FromBody]HR_Candidate_DetailsDTO dto)
        {
            return _ads.addlanguage(dto);
        }

        [Route("sendCallLettermail")]
        public HR_Candidate_DetailsDTO sendCallLettermail([FromBody]HR_Candidate_DetailsDTO dto)
        {
            return _ads.sendCallLettermail(dto);
        }

        [Route("saveAppointmenttab")]
        public HR_Candidate_DetailsDTO saveAppointmenttab([FromBody]HR_Candidate_DetailsDTO dto)
        {
            return _ads.saveAppointmenttab(dto);
        }
        [Route("EditRecord")]
        public HR_Candidate_DetailsDTO EditRecord([FromBody]HR_Candidate_DetailsDTO dto)
        {
            return _ads.EditRecord(dto);
        }
        [Route("activedeactive")]
        public HR_Candidate_DetailsDTO activedeactive([FromBody]HR_Candidate_DetailsDTO dto)
        {
            return _ads.activedeactive(dto);
        }
        [Route("AddCandidateDocuments")]
        public HR_Candidate_DetailsDTO AddCandidateDocuments([FromBody]HR_Candidate_DetailsDTO dto)
        {
            return _ads.AddCandidateDocuments(dto);
        }
        [Route("getalldetailsDocVerfy")]
        public HR_Candidate_DetailsDTO getalldetailsDocVerfy([FromBody]HR_Candidate_DetailsDTO dto)
        {
            return _ads.getalldetailsDocVerfy(dto);
        }
        [Route("saveVerify")]
        public HR_Candidate_DetailsDTO saveVerify([FromBody]HR_Candidate_DetailsDTO dto)
        {
            return _ads.saveVerify(dto);
        }
        [Route("VerifyReports")]
        public HR_Candidate_DetailsDTO VerifyReports([FromBody]HR_Candidate_DetailsDTO dto)
        {
            return _ads.VerifyReports(dto);
        }
    }
}
