using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IssueManager.com.Tenants.Interface;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.IssueManager.Tenants;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IssueManager.com.Tenants.Controller
{
    [Route("api/[controller]")]
    public class TenantUploadNoticeFacade :ControllerBase
    {
        public TenantUploadNoticeInterface _work;
        public TenantUploadNoticeFacade(TenantUploadNoticeInterface work)
        {
            _work = work;
        }
        // GET: api/values
        [Route("savedetail")]
        public TenantUploadNoticeDTO savedetail([FromBody]TenantUploadNoticeDTO data)
        {
            return _work.savedetail(data);
        }
        [Route("Getdetails")]
        public TenantUploadNoticeDTO Getdetails([FromBody]TenantUploadNoticeDTO data)
        {
            return _work.Getdetails(data);
        }
        [Route("deactivate")]
        public TenantUploadNoticeDTO deactivate([FromBody]TenantUploadNoticeDTO data)
        {
            return _work.deactivate(data);
        }
        [Route("editdetails")]
        public TenantUploadNoticeDTO editdetails([FromBody]TenantUploadNoticeDTO data)
        {
            return _work.editdetails(data);
        }

        [Route("getsection")]
        public TenantUploadNoticeDTO getsection([FromBody]TenantUploadNoticeDTO data)
        {
            return _work.getsection(data);
        }
        [Route("getstudent")]
        public TenantUploadNoticeDTO getstudent([FromBody]TenantUploadNoticeDTO data)
        {
            return _work.getstudent(data);
        }
        [Route("Deptselectiondetails")]
        public TenantUploadNoticeDTO Deptselectiondetails([FromBody]TenantUploadNoticeDTO data)
        {
            return _work.Deptselectiondetails(data);
        }
        [Route("Desgselectiondetails")]
        public TenantUploadNoticeDTO Desgselectiondetails([FromBody]TenantUploadNoticeDTO data)
        {
            return _work.Desgselectiondetails(data);
        }
        [Route("viewData")]
        public TenantUploadNoticeDTO viewData([FromBody]TenantUploadNoticeDTO data)
        {
            return _work.viewData(data);
        }
        [Route("viewrecords")]
        public TenantUploadNoticeDTO viewrecords([FromBody]TenantUploadNoticeDTO data)
        {
            return _work.viewrecords(data);
        }
        [Route("sendmail")]
        public TenantUploadNoticeDTO sendmail([FromBody]TenantUploadNoticeDTO data)
        {
            return _work.sendmail(data);
        }
        [Route("gettenant")]
        public TenantUploadNoticeDTO gettenant([FromBody]TenantUploadNoticeDTO data)
        {
            return _work.gettenant(data);
        }
    }
}
