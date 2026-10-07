using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using HRMSServicesHub.com.vaps.Interfaces;
using PreadmissionDTOs.com.vaps.HRMS;
using HRMSServiceHub.com.vaps.Interfaces;
using PreadmissionDTOs.com.vaps.VMS.HRMS;
using PreadmissionDTOs.com.vaps.VMS.Exit;


// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace HRMSServiceHub.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class HRAluminiEmolpyeeBGVFacadeController : Controller
    {
        // GET: api/values

        public HRAluminiEmployeeBGVerificationInterface _ads;

        public HRAluminiEmolpyeeBGVFacadeController(HRAluminiEmployeeBGVerificationInterface adstu)
        {
            _ads = adstu;
        }

        [Route("sendResignationMail")]
        public HRAluminiEmployeeBGVerificationDTO sendResignationMail([FromBody] HRAluminiEmployeeBGVerificationDTO dto)
        {
            return _ads.sendResignationMail(dto);
        }

        // GET: api/values
        [Route("onloadgetdetails")]
        public HRAluminiEmployeeBGVerificationDTO onloadgetdetails([FromBody]HRAluminiEmployeeBGVerificationDTO data)
        {
            return _ads.getBasicData(data);
        }

        [HttpPost]      
        public HRAluminiEmployeeBGVerificationDTO Post([FromBody] HRAluminiEmployeeBGVerificationDTO data)
        {
            return _ads.savedata(data);
        }
        [Route("deactivate")]
        public HRAluminiEmployeeBGVerificationDTO deactivate([FromBody] HRAluminiEmployeeBGVerificationDTO data)
        {
            return _ads.deactivate(data);
        }

        [Route("Editdata")]
        public HRAluminiEmployeeBGVerificationDTO Editdata([FromBody] HRAluminiEmployeeBGVerificationDTO data)
        {
            return _ads.Editdata(data);
        }

        [Route("nocdetails")]
        public ISM_Certificate_RequestDTO nocdetails([FromBody]ISM_Certificate_RequestDTO data)
        {
            return _ads.nocdetails(data);
        }


        [Route("FANDFSettlement")]
        public ISM_Resignation_DTO FANDFSettlement([FromBody]ISM_Resignation_DTO data)
        {
            return _ads.FANDFSettlement(data);
        }

    }
}