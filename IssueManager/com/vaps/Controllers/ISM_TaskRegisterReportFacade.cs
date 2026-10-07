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
    public class ISM_TaskRegisterReportFacade : Controller
    {

        public ISM_TaskRegisterReportInterface _objinter;

        public ISM_TaskRegisterReportFacade(ISM_TaskRegisterReportInterface parameter)
        {
            _objinter = parameter;
        }
        [Route("getdetails")]
        public Task<ISM_TaskCreationDTO> getdetails([FromBody] ISM_TaskCreationDTO data)
        {
            return _objinter.getdetails(data);
        }
          [Route("getemployeelist")]
        public Task<ISM_TaskCreationDTO> getemployeelist([FromBody] ISM_TaskCreationDTO data)
        {
            return _objinter.getemployeelist(data);
        }

        [Route("gettaskregisterReport")]
        public Task<ISM_TaskCreationDTO> gettaskregisterReport([FromBody]ISM_TaskCreationDTO data)
        {
            return _objinter.gettaskregisterReport(data);
        }
        [Route("Deptselectiondetails")]
        public Task<ISM_TaskCreationDTO> Deptselectiondetails([FromBody]ISM_TaskCreationDTO data)
        {
            return _objinter.Deptselectiondetails(data);
        }
        [Route("Desgselectiondetails")]
        public Task<ISM_TaskCreationDTO> Desgselectiondetails([FromBody]ISM_TaskCreationDTO data)
        {
            return _objinter.Desgselectiondetails(data);
        }
        //=========== makerchecker consolidate report
        [Route("getconsuldatedetails")]
        public Task<ISM_TaskCreationDTO> getconsuldatedetails([FromBody]ISM_TaskCreationDTO data)
        {
            return _objinter.getconsuldatedetails(data);
        }

        [Route("getmakercheckerReport")]
        public Task<ISM_TaskCreationDTO> getmakercheckerReport([FromBody]ISM_TaskCreationDTO data)
        {
            return _objinter.getmakercheckerReport(data);
        }
        [Route("getmakercheckerview")]
        public Task<ISM_TaskCreationDTO> getmakercheckerview([FromBody]ISM_TaskCreationDTO data)
        {
            return _objinter.getmakercheckerview(data);
        }
        //ISM_TaskRegister
        [Route("ISM_TaskRegister")]
        public Task<ISM_TaskCreationDTO> ISM_TaskRegister([FromBody]ISM_TaskCreationDTO data)
        {
            return _objinter.ISM_TaskRegister(data);
        }
        //ISM_TaskReport
        [Route("getmakerRejectedReport")]
        public Task<ISM_TaskCreationDTO> getmakerRejectedReport([FromBody]ISM_TaskCreationDTO data)
        {
            return _objinter.getmakerRejectedReport(data);
        }
    }
}
