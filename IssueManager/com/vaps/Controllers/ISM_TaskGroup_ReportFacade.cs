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
    public class ISM_TaskGroup_ReportFacade : Controller
    {
        public ISM_TaskGroup_ReportInterface inter;
        public ISM_TaskGroup_ReportFacade(ISM_TaskGroup_ReportInterface parameter)
        {
            inter = parameter;
        }
        [Route("getdetails")]
        public Task<ISM_TaskGroup_ReportDTO> getdetails([FromBody] ISM_TaskGroup_ReportDTO data)
        {
            return inter.getdetails(data);
        }
        //[Route("getdepartments")]
        //public Task<ISM_TaskGroup_ReportDTO> getdepartments([FromBody]ISM_TaskGroup_ReportDTO data)
        //{
        //    return inter.getdepartments(data);
        //}
        [Route("getplannerReport")]
        public Task<ISM_TaskGroup_ReportDTO> getplannerReport([FromBody]ISM_TaskGroup_ReportDTO data)
        {
            return inter.getplannerReport(data);
        }
        [Route("getReport")]
        public Task<ISM_TaskGroup_ReportDTO> getReport([FromBody]ISM_TaskGroup_ReportDTO data)
        {
            return inter.getReport(data);
        } 
        /* CATEGORY REPORT */
        [Route("getcategorydetails")]
        public ISM_TaskGroup_ReportDTO getcategorydetails([FromBody]ISM_TaskGroup_ReportDTO data)
        {
            return inter.getcategorydetails(data);
        }
        [Route("onchangedept")]
        public ISM_TaskGroup_ReportDTO onchangedept([FromBody]ISM_TaskGroup_ReportDTO data)
        {
            return inter.onchangedept(data);
        }
        [Route("categorygroupReport")]
        public ISM_TaskGroup_ReportDTO categorygroupReport([FromBody]ISM_TaskGroup_ReportDTO data)
        {
            return inter.categorygroupReport(data);
        }
        //SaveFeedBack
        [Route("SaveFeedBack")]
        public ISM_TaskGroup_ReportDTO SaveFeedBack([FromBody]ISM_TaskGroup_ReportDTO data)
        {
            return inter.SaveFeedBack(data);
        }
    }
}
