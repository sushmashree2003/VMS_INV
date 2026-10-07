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
    public class ISM_PlannerDeptDeviationReportFacade : Controller
    {

        public ISM_PlannerDeptDeviationReportInterface _objinter;

        public ISM_PlannerDeptDeviationReportFacade(ISM_PlannerDeptDeviationReportInterface parameter)
        {
            _objinter = parameter;
        }

        [Route("getdetails")]
        public Task<ISM_PlannerReportsDTO> getdetails([FromBody] ISM_PlannerReportsDTO data)
        {
            return _objinter.getdetails(data);
        }

        [Route("getdeviationReport")]
        public Task<ISM_PlannerReportsDTO> getdeviationReport([FromBody]ISM_PlannerReportsDTO data)
        {
            return _objinter.getdeviationReport(data);
        }

        [Route("Deptselectiondetails")]
        public Task<ISM_PlannerReportsDTO> Deptselectiondetails([FromBody]ISM_PlannerReportsDTO data)
        {
            return _objinter.Deptselectiondetails(data);
        }
        [Route("DeptselectionHeaddetails")]
        public Task<ISM_PlannerReportsDTO> DeptselectionHeaddetails([FromBody]ISM_PlannerReportsDTO data)
        {
            return _objinter.DeptselectionHeaddetails(data);
        }
        [Route("gettaskdetails")]
        public Task<ISM_PlannerReportsDTO> gettaskdetails([FromBody]ISM_PlannerReportsDTO data)
        {
            return _objinter.gettaskdetails(data);
        }
        [Route("gettaskdetailsreport")]
        public Task<ISM_PlannerReportsDTO> gettaskdetailsreport([FromBody]ISM_PlannerReportsDTO data)
        {
            return _objinter.gettaskdetailsreport(data);
        }
        [Route("Desgselectiondetails")]
        public Task<ISM_PlannerReportsDTO> Desgselectiondetails([FromBody]ISM_PlannerReportsDTO data)
        {
            return _objinter.Desgselectiondetails(data);
        }
        [Route("empcommunication")]
        public Task<ISM_PlannerReportsDTO> empcommunication([FromBody]ISM_PlannerReportsDTO data)
        {
            return _objinter.empcommunication(data);
        }
        [Route("apiempcommunication")]
        public Task<ISM_PlannerReportsDTO> apiempcommunication([FromBody]ISM_PlannerReportsDTO data)
        {
            return _objinter.apiempcommunication(data);
        }

        //============================= weekly deviation
        [Route("getdetails_weekly")]
        public Task<ISM_PlannerReportsDTO> getdetails_weekly([FromBody] ISM_PlannerReportsDTO data)
        {
            return _objinter.getdetails_weekly(data);
        }

        [Route("getdeviationReport_weekly")]
        public Task<ISM_PlannerReportsDTO> getdeviationReport_weekly([FromBody]ISM_PlannerReportsDTO data)
        {
            return _objinter.getdeviationReport_weekly(data);
        }

        //====================dailydeviation
        [Route("getdaily_deviation")]
        public ISM_PlannerReportsDTO getdaily_deviation([FromBody]ISM_PlannerReportsDTO data)
        {
            return _objinter.getdaily_deviation(data);
        }

        //=====================rating report==========
        [Route("getEmp_ratingReport")]
        public ISM_PlannerReportsDTO getEmp_ratingReport([FromBody]ISM_PlannerReportsDTO data)
        {
            return _objinter.getEmp_ratingReport(data);
        }
        [Route("taskRejectReport")]
        public ISM_PlannerReportsDTO taskRejectReport([FromBody]ISM_PlannerReportsDTO data)
        {
            return _objinter.taskRejectReport(data);
        }
        
    }
}
