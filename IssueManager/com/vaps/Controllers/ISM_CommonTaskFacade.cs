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
    public class ISM_CommonTaskFacade : Controller
    {
        public ISM_CommonTaskInterface _Interface;
        public ISM_CommonTaskFacade(ISM_CommonTaskInterface parameter)
        {
            _Interface = parameter;
        }

        [Route("Loaddata")]
        public ISM_CommonTask_DTO Loaddata([FromBody]ISM_CommonTask_DTO data)
        {
            return _Interface.Loaddata(data);
        }

        [Route("get_Designation")]
        public ISM_CommonTask_DTO get_Designation([FromBody]ISM_CommonTask_DTO data)
        {
            return _Interface.get_Designation(data);
        }
        [Route("get_Employee")]
        public ISM_CommonTask_DTO get_Employee([FromBody]ISM_CommonTask_DTO data)
        {
            return _Interface.get_Employee(data);
        }
    }
}
