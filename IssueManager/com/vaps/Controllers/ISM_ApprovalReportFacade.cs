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
    public class ISM_ApprovalReportFacade : Controller
    {
        public ISM_ApprovalReportInterface _objinter;

        public ISM_ApprovalReportFacade(ISM_ApprovalReportInterface _obji)
        {
            _objinter = _obji;
        }

        // GET: api/<controller>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<controller>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        [Route("getdetails")]
        public ISM_PlannerReportsDTO getdetails([FromBody] ISM_PlannerReportsDTO data)
        {
            return _objinter.getdetails(data);
        }
        [Route("getPlannerlist")]
        public ISM_PlannerReportsDTO getPlannerlist([FromBody] ISM_PlannerReportsDTO data)
        {
            return _objinter.getPlannerlist(data);
        }

        [Route("getApprovalReport")]
        public Task<ISM_PlannerReportsDTO> getApprovalReport([FromBody] ISM_PlannerReportsDTO data)
        {
            return _objinter.getApprovalReport(data);
        }
        [Route("getPlannerDReport")]
        public Task<ISM_PlannerReportsDTO> getPlannerDReport([FromBody] ISM_PlannerReportsDTO data)
        {
            return _objinter.getPlannerDReport(data);
        }
        [Route("Desgselectiondetails")]
        public Task<ISM_PlannerReportsDTO> Desgselectiondetails([FromBody] ISM_PlannerReportsDTO data)
        {
            return _objinter.Desgselectiondetails(data);
        }
        [Route("Deptselectiondetails")]
        public Task<ISM_PlannerReportsDTO> Deptselectiondetails([FromBody] ISM_PlannerReportsDTO data)
        {
            return _objinter.Deptselectiondetails(data);
        }
        [Route("getempdetailsReport")]
        public Task<ISM_PlannerReportsDTO> getempdetailsReport([FromBody] ISM_PlannerReportsDTO data)
        {
            return _objinter.getempdetailsReport(data);
        }

        [Route("send_feedback")]
        public ISM_PlannerReportsDTO send_feedback([FromBody] ISM_PlannerReportsDTO data)
        {
            return _objinter.send_feedback(data);
        }

        [Route("viewresponse")]
        public ISM_PlannerReportsDTO viewresponse([FromBody] ISM_PlannerReportsDTO data)
        {
            return _objinter.viewresponse(data);
        }

        [Route("SummaryReport")]
        public ISM_PlannerReportsDTO SummaryReport([FromBody] ISM_PlannerReportsDTO data)
        {
            return _objinter.SummaryReport(data);
        }

        [Route("getcatempdetails")]
        public ISM_PlannerReportsDTO getcatempdetails([FromBody] ISM_PlannerReportsDTO data)
        {
            return _objinter.getcatempdetails(data);
        }

        [Route("getempcatedetails")]
        public Task<ISM_PlannerReportsDTO> getempcatedetails([FromBody] ISM_PlannerReportsDTO data)
        {
            return _objinter.getempcatedetails(data);
        }

        [Route("saveAddLop")]
        public ISM_PlannerReportsDTO saveAddLop([FromBody] ISM_PlannerReportsDTO data)
        {
            return _objinter.saveAddLop(data);
        }
        [Route("deactivate")]
        public ISM_PlannerReportsDTO deactivate([FromBody] ISM_PlannerReportsDTO data)
        {
            return _objinter.deactivate(data);
        }
        [Route("getonlodatdata")]
        public ISM_PlannerReportsDTO getonlodatdata([FromBody] ISM_PlannerReportsDTO data)
        {
            return _objinter.getonlodatdata(data);
        }
        [Route("getconcalllopdetails")]
        public ISM_PlannerReportsDTO getconcalllopdetails([FromBody] ISM_PlannerReportsDTO data)
        {
            return _objinter.getconcalllopdetails(data);
        }
    }
}