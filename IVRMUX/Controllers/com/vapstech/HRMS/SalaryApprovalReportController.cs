using IVRMUX.Delegates.com.vapstech.HRMS;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Controllers.com.vapstech.HRMS
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class SalaryApprovalReportController : Controller
    {
        SalaryApprovalReportDelegate del = new SalaryApprovalReportDelegate();

        [Route("getalldetails/{id:int}")]
        public HR_Employee_SalaryDTO getalldetails(int id)
        {
            HR_Employee_SalaryDTO data = new HR_Employee_SalaryDTO();

            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;
            return del.getalldetails(data);
        }


        [HttpPost]
        [Route("report")]
        public HR_Employee_SalaryDTO report([FromBody]HR_Employee_SalaryDTO data)
        {

            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));

            return del.report(data);
        }
    }

}

