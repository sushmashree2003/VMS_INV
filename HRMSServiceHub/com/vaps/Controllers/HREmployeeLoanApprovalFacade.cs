using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HRMSServiceHub.com.vaps.Interfaces;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.HRMS;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HRMSServiceHub.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class HREmployeeLoanApprovalFacade : Controller
    {
        public HREmployeeLoanApprovalInterface _ads;

        public HREmployeeLoanApprovalFacade(HREmployeeLoanApprovalInterface adstu)
        {
            _ads = adstu;
        }

        // GET: api/values
        [Route("onloadgetdetails")]
        public HREmployeeLoanApprovalDTO getinitialdata([FromBody]HREmployeeLoanApprovalDTO dto)
        {
            return _ads.getBasicData(dto);
        }



        [Route("getapprvaldata")]
        public HREmployeeLoanApprovalDTO getapprvaldata([FromBody]HREmployeeLoanApprovalDTO dto)
        {
            return _ads.getapprvaldata(dto);
        }
        [Route("SalaryApproval")]
        public HREmployeeLoanApprovalDTO SalaryApproval([FromBody]HREmployeeLoanApprovalDTO dto)
        {
            return _ads.SalaryApproval(dto);
        }
        [Route("SalaryApprovalgriddata")]
        public HREmployeeLoanApprovalDTO SalaryApprovalgriddata([FromBody]HREmployeeLoanApprovalDTO dto)
        {
            return _ads.SalaryApprovalgriddata(dto);
        }
        [Route("report")]
        public HREmployeeLoanApprovalDTO report([FromBody]HREmployeeLoanApprovalDTO dto)
        {
            return _ads.report(dto);
        }

        [Route("ViewFiles")]
        public HREmployeeLoanApprovalDTO ViewFiles([FromBody] HREmployeeLoanApprovalDTO data)
        {
            return _ads.ViewFiles(data);
        }

        [Route("OBCB_Email")]
        public HREmployeeLoanApprovalDTO OBCB_Email([FromBody] HREmployeeLoanApprovalDTO data)
        {
            return _ads.OBCB_Email(data);
        }
        [Route("BondCompTaskCreate")]
        public HREmployeeLoanApprovalDTO BondCompTaskCreate([FromBody] HREmployeeLoanApprovalDTO data)
        {
            return _ads.BondCompTaskCreate(data);
        }
    }
}
