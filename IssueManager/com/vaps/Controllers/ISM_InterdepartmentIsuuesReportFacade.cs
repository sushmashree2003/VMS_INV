using System.Threading.Tasks;
using IssueManager.com.vaps.Interfaces;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.IssueManager;
namespace IssueManager.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class ISM_InterdepartmentIsuuesReportFacade : Controller
    {
        public ISM_InterdepartmentIsuuesReportInterface _objinter;
        public ISM_InterdepartmentIsuuesReportFacade(ISM_InterdepartmentIsuuesReportInterface parameter) 
        {
            _objinter = parameter;
        }
        [Route("getdetails")]
        public Task<ISM_InterDepartmentReport> getdetails([FromBody] ISM_InterDepartmentReport data)
        {
            return _objinter.getdetails(data);
        }
        //[Route("getreport")]
        //public ISM_InterDepartmentReport getreport([FromBody]ISM_InterDepartmentReport data)
        //{
        //    return _objinter.getreport(data);
        //}

        [Route("SendEmail")]
        public ISM_InterDepartmentReport SendEmail([FromBody]ISM_InterDepartmentReport data)
        {
            return _objinter.SendEmail(data);
        }
    }
}
