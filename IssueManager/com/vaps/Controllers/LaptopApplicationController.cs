using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IssueManager.com.vaps.Interfaces;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.IssueManager;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IssueManager.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class LaptopApplicationController : Controller
    {
        public LaptopApplicationInterface _objinter;

        public LaptopApplicationController(LaptopApplicationInterface parameter)
        {
            _objinter = parameter;
        }

        [Route("getdetails")]
        public  VMS_Laptop_Application_DTO getdetails([FromBody] VMS_Laptop_Application_DTO data)
        {
            return _objinter.getdetails(data);
        }

        [Route("loaddata")]
        public VMS_Laptop_Application_DTO loaddata([FromBody] VMS_Laptop_Application_DTO data)
        {
            return _objinter.loaddata(data);
        }

        [Route("save")]
        public VMS_Laptop_Application_DTO save([FromBody] VMS_Laptop_Application_DTO data)
        {
            return _objinter.save(data);
        }

        [Route("edittab1")]
        public VMS_Laptop_Application_DTO edittab1([FromBody] VMS_Laptop_Application_DTO data)
        {
            return _objinter.edittab1(data);
        }

        [Route("getApprovalStatus")]
        public Task<VMS_Laptop_Application_DTO> getApprovalStatus([FromBody] VMS_Laptop_Application_DTO data)
        {
            return _objinter.getApprovalStatusAsync(data);
        }

        [Route("get_status")]
        public VMS_Laptop_Application_DTO get_status([FromBody] VMS_Laptop_Application_DTO data)
        {
            return _objinter.get_status(data);
        }

        [Route("reject_status")]
        public VMS_Laptop_Application_DTO reject_status([FromBody] VMS_Laptop_Application_DTO data)
        {
            return _objinter.reject_status(data);
        }

        [Route("getrpt")]
        public VMS_Laptop_Application_DTO getrpt([FromBody] VMS_Laptop_Application_DTO data)
        {
            return _objinter.getrpt(data);
        }
    }
}
