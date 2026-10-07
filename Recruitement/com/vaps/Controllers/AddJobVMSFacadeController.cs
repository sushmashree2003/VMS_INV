using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.VMS.HRMS;
using Recruitment.com.vaps.Interfaces;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Recruitment.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class AddJobVMSFacadeController : Controller
    {

        public AddJobVMSInterface _ads;

        public AddJobVMSFacadeController(AddJobVMSInterface adstu)
        {
            _ads = adstu;
        }

        // GET: api/values
        [Route("onloadgetdetails")]
        public HR_MRFRequisitionDTO getinitialdata([FromBody]HR_MRFRequisitionDTO dto)
        {
            return _ads.getBasicData(dto);
        }

        // POST api/values
        [HttpPost]
        public HR_MRFRequisitionDTO Post([FromBody]HR_MRFRequisitionDTO dto)
        {
            return _ads.SaveUpdate(dto);
        }

        [Route("staterecord")]
        public HR_MRFRequisitionDTO staterecord([FromBody]HR_MRFRequisitionDTO dto)
        {
            return _ads.staterecord(dto);
        }
        [Route("deleterecord")]
        public HR_MRFRequisitionDTO deleterecord([FromBody]HR_MRFRequisitionDTO dto)
        {
            return _ads.deleterecord(dto);
        }
        [Route("getRecordById/{id:int}")]

        public HR_MRFRequisitionDTO getcatgrydet(int id)
        {
            // id = 12;
            return _ads.editData(id);
        }
        [Route("deactivateRecordById")]
        public HR_MRFRequisitionDTO deactivateRecordById([FromBody]HR_MRFRequisitionDTO dto)
        {
            return _ads.deactivate(dto);
        }

        [Route("JobDisplay")]
        public HR_MRFRequisitionDTO JobDisplay([FromBody]HR_MRFRequisitionDTO dto)
        {
            return _ads.JobDisplay(dto);
        }
        [Route("saverecord")]
        public HR_MRFRequisitionDTO saverecord([FromBody]HR_MRFRequisitionDTO dto)
        {
            return _ads.saverecord(dto);
        }

        [Route("activedeactive")]
        public HR_MRFRequisitionDTO activedeactive([FromBody]HR_MRFRequisitionDTO dto)
        {
            return _ads.activedeactive(dto);
        }
    }
}
