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
    public class IEMontEndReportForReportSubmissionFacadeController : Controller
    {
        public IEMontEndReportForReportSubmissionInterface _interface;

        public IEMontEndReportForReportSubmissionFacadeController(IEMontEndReportForReportSubmissionInterface _inter)
        {
            _interface = _inter;
        }
        [Route("Onload")]
        public IEMontEndReportForReportSubmissionDTO Onload ([FromBody]IEMontEndReportForReportSubmissionDTO data)
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
         [Route("deactiveY")]
        public IEMontEndReportForReportSubmissionDTO deactiveY([FromBody]IEMontEndReportForReportSubmissionDTO data)
        {
            return _interface.deactiveY(data);
        }
        [Route("EditData")]
        public IEMontEndReportForReportSubmissionDTO EditData([FromBody]IEMontEndReportForReportSubmissionDTO data)
        {
            return _interface.EditData(data);
        }



    }
}