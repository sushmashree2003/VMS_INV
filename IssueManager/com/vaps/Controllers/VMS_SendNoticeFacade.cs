using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IssueManager.com.vaps.Interface;

using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.IssueManager;
using PreadmissionDTOs.com.vaps.IssueManager.Tenants;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IssueManager.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class VMS_SendNoticeFacade : ControllerBase
    {
        public VMS_SendNoticeInterface _work;
        public VMS_SendNoticeFacade(VMS_SendNoticeInterface work)
        {
            _work = work;
        }
        // GET: api/values
        [Route("savedetail")]
        public VMS_SendNoticeDTO savedetail([FromBody]VMS_SendNoticeDTO data)
        {
            return _work.savedetail(data);
        }
        [Route("Getdetails")]
        public VMS_SendNoticeDTO Getdetails([FromBody]VMS_SendNoticeDTO data)
        {
            return _work.Getdetails(data);
        }
        [Route("deactivate")]
        public VMS_SendNoticeDTO deactivate([FromBody]VMS_SendNoticeDTO data)
        {
            return _work.deactivate(data);
        }
        [Route("editdetails")]
        public VMS_SendNoticeDTO editdetails([FromBody]VMS_SendNoticeDTO data)
        {
            return _work.editdetails(data);
        }


        [Route("viewData")]
        public VMS_SendNoticeDTO viewData([FromBody]VMS_SendNoticeDTO data)
        {
            return _work.viewData(data);
        }

        //[Route("sendmail")]
        //public VMS_SendNoticeDTO sendmail([FromBody]VMS_SendNoticeDTO data)
        //{
        //    return _work.sendmail(data);
        //}
        [Route("gettenant")]
        public VMS_SendNoticeDTO gettenant([FromBody]VMS_SendNoticeDTO data)
        {
            return _work.gettenant(data);
        }
    }
}
