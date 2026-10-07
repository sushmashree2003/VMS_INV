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
    public class VmsAuditReportsFacade : Controller
    {
        public VmsAuditReportsInterface _objinter;

        public VmsAuditReportsFacade(VmsAuditReportsInterface parameter)
        {
            _objinter = parameter;
        }
        [Route("getalldata")]
        public VmsAuditReportsDTO getalldata([FromBody]VmsAuditReportsDTO data)
        {
            return _objinter.getalldata(data);
        }
        [Route("TadaAllwence")]
        public VmsAuditReportsDTO TadaAllwence([FromBody]VmsAuditReportsDTO data)
        {
            return _objinter.TadaAllwence(data);
        }
        [Route("savedta")]
        public VmsAuditReportsDTO savedta([FromBody]VmsAuditReportsDTO data)
        {
            return _objinter.savedta(data);
        }
        //edit
        [Route("edit")]
        public VmsAuditReportsDTO edit([FromBody]VmsAuditReportsDTO data)
        {
            return _objinter.edit(data);
        }
        //deactive
        [Route("deactive")]
        public VmsAuditReportsDTO deactive([FromBody]VmsAuditReportsDTO data)
        {
            return _objinter.deactive(data);
        }

        //Getdata
        [Route("Getdata")]
        public VmsAuditReportsDTO Getdata([FromBody]VmsAuditReportsDTO data)
        {
            return _objinter.Getdata(data);
        }
    }
}
