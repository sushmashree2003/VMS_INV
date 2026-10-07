using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HRMSServiceHub.com.vaps.Interfaces;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.HRMS;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HRMSServiceHub.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class FF_Settlement_Process_FacadeController : Controller
    {
        // GET: /<controller>/

        public FF_Settlement_ProcessInterface _ads;

        public FF_Settlement_Process_FacadeController(FF_Settlement_ProcessInterface adstu)
        {
            _ads = adstu;
        }
        [Route("getalldetails")]
        public FF_Settlement_Process_DTO getalldetails([FromBody]FF_Settlement_Process_DTO dto)
        {
            return _ads.getalldetails(dto);
        }
        [Route("SaveUpdate")]
        public FF_Settlement_Process_DTO SaveUpdate([FromBody]FF_Settlement_Process_DTO dto)
        {
            return _ads.SaveUpdate(dto);
        }
        [Route("Edit")]
        public FF_Settlement_Process_DTO Edit([FromBody]FF_Settlement_Process_DTO dto)
        {
            return _ads.Edit(dto);
        }
        [Route("deactivate")]
        public FF_Settlement_Process_DTO deactivate([FromBody]FF_Settlement_Process_DTO dto)
        {
            return _ads.deactivate(dto);
        }
      

        /////////////////////////Approval/////////////////////////
        [Route("getData")]
        public FF_Settlement_Process_DTO getData([FromBody]FF_Settlement_Process_DTO dto)
        {
            return _ads.getData(dto);
        }
         
          [Route("empdetail")]
        public FF_Settlement_Process_DTO empdetail([FromBody]FF_Settlement_Process_DTO dto)
        {
            return _ads.empdetail(dto);
        }
          [Route("approvedrecord")]
        public FF_Settlement_Process_DTO approvedrecord([FromBody]FF_Settlement_Process_DTO dto)
        {
            return _ads.approvedrecord(dto);
        }
        /////////////////Report/////////////////////////
          [Route("Onload")]
        public FF_Settlement_Process_DTO Onload([FromBody]FF_Settlement_Process_DTO dto)
        {
            return _ads.Onload(dto);
        }
          [Route("getapprovalreport")]
        public FF_Settlement_Process_DTO getapprovalreport([FromBody]FF_Settlement_Process_DTO dto)
        {
            return _ads.getapprovalreport(dto);
        }
    }
}
