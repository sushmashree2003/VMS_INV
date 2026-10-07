using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FrontOffice.com.vaps.Interfaces;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.FrontOffice;

namespace FrontOffice.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class IEMonthEndReportApprovalFacadeController : Controller
    {
        public IEMonthEndReportApprovalInterface _interface;

        public IEMonthEndReportApprovalFacadeController(IEMonthEndReportApprovalInterface _inter)
        {
            _interface = _inter;
        }
        [Route("Onload")]
        public IEMontEndReportForReportSubmissionDTO Onload([FromBody]IEMontEndReportForReportSubmissionDTO data)
        {
            return _interface.Onload(data);
        }

        [Route("saverecord")]
        public IEMontEndReportForReportSubmissionDTO saverecord([FromBody]IEMontEndReportForReportSubmissionDTO data)
        {
            return _interface.saverecord(data);
        }
        [Route("DocView")]
        public IEMontEndReportForReportSubmissionDTO DocView([FromBody]IEMontEndReportForReportSubmissionDTO data)
        {
            return _interface.DocView(data);
        }
        [Route("Approve_status")]
        public IEMontEndReportForReportSubmissionDTO Approve_status([FromBody]IEMontEndReportForReportSubmissionDTO data)
        {
            return _interface.Approve_status(data);
        }



    }
}