using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FrontOffice.com.vaps.Interfaces;
using FrontOfficeHub.com.vaps.Interfaces;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.FrontOffice;

namespace FrontOffice.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class IEMonthEndSubmissionAndApprovalReportFacadeController : Controller
    {
        public IEMonthEndSubmissionAndApprovalReportInterface _interface;

        public IEMonthEndSubmissionAndApprovalReportFacadeController(IEMonthEndSubmissionAndApprovalReportInterface _inter)
        {
            _interface = _inter;
        }
        [Route("Onload")]
        public IEMonthEndSubmissionAndApprovalReportDTO Onload([FromBody]IEMonthEndSubmissionAndApprovalReportDTO data)
        {
            return _interface.Onload(data);
        }

        [Route("getmonthreport")]
        public IEMonthEndSubmissionAndApprovalReportDTO getmonthreport([FromBody]IEMonthEndSubmissionAndApprovalReportDTO data)
        {
            return _interface.getmonthreport(data);
        }
        [Route("DocView")]
        public IEMonthEndSubmissionAndApprovalReportDTO DocView([FromBody]IEMonthEndSubmissionAndApprovalReportDTO data)
        {
            return _interface.DocView(data);
        }
        [Route("getIEDetails")]
        public IEMonthEndSubmissionAndApprovalReportDTO getIEDetails([FromBody]IEMonthEndSubmissionAndApprovalReportDTO data)
        {
            return _interface.getIEDetails(data);
        }

    }
}