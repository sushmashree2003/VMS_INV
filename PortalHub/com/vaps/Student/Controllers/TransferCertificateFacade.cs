using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PortalHub.com.vaps.Student.Interfaces;
using PreadmissionDTOs.com.vaps.Portals.Student;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PortalHub.com.vaps.Student.Controllers
{
    [Route("api/[controller]")]
    public class TransferCertificateFacade : Controller
    {
        // GET: api/<controller>
        public TransferCertificateInterface _ads;

        public TransferCertificateFacade(TransferCertificateInterface adstu)
        {
            _ads = adstu;
        }

        // POST api/values       
        [Route("getdetails")]
        public TransferCertificate_DTO getdetails([FromBody]TransferCertificate_DTO sddto)
        {
            return _ads.getdetails(sddto);
        }

        [Route("tcApply")]
        public Task<TransferCertificate_DTO> tcApply([FromBody]TransferCertificate_DTO sddto)
        {
            return _ads.tcApply(sddto);
        }

        [Route("deactiveY")]
        public TransferCertificate_DTO deactiveY([FromBody]TransferCertificate_DTO sddto)
        {
            return _ads.deactiveY(sddto);
        }

        [Route("certificateApproved")]
        public Task<TransferCertificate_DTO> certificateApproved([FromBody]TransferCertificate_DTO sddto)
        {
            return _ads.certificateApproved(sddto);
        }

        [Route("certificateRejected")]
        public Task<TransferCertificate_DTO> certificateRejected([FromBody]TransferCertificate_DTO sddto)
        {
            return _ads.certificateRejected(sddto);
        }

    }
}
