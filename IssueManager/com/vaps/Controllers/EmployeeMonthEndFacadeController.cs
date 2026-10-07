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
    public class EmployeeMonthEndFacadeController : Controller
    {
        public EmployeeMonthEndInterface _objinter;

        public EmployeeMonthEndFacadeController(EmployeeMonthEndInterface parameter)
        {
            _objinter = parameter;
        }

        [Route("getemplist")]
        public EmployeeMonthEndDTO getemplist([FromBody] EmployeeMonthEndDTO data)
        {
            return _objinter.getemplist(data);
        }

        [Route("getempdetails")]
        public Task<EmployeeMonthEndDTO> getempdetails([FromBody] EmployeeMonthEndDTO data)
        {
            return _objinter.getempdetailsAsync(data);
        }

        [Route("savereport")]
        public EmployeeMonthEndDTO savereport([FromBody] EmployeeMonthEndDTO data)
        {
            return _objinter.savereport(data);
        }
        //[Route("save")]
        //public EmployeeMonthEndDTO save([FromBody] EmployeeMonthEndDTO data)
        //{
        //    return _objinter.save(data);
        //}

        //[Route("edittab1")]
        //public EmployeeMonthEndDTO edittab1([FromBody] EmployeeMonthEndDTO data)
        //{
        //    return _objinter.edittab1(data);
        //}

        //[Route("getApprovalStatus")]
        //public Task<EmployeeMonthEndDTO> getApprovalStatus([FromBody] EmployeeMonthEndDTO data)
        //{
        //    return _objinter.getApprovalStatusAsync(data);
        //}

        //[Route("get_status")]
        //public EmployeeMonthEndDTO get_status([FromBody] EmployeeMonthEndDTO data)
        //{
        //    return _objinter.get_status(data);
        //}

        //[Route("reject_status")]
        //public EmployeeMonthEndDTO reject_status([FromBody] EmployeeMonthEndDTO data)
        //{
        //    return _objinter.reject_status(data);
        //}

        //[Route("getrpt")]
        //public EmployeeMonthEndDTO getrpt([FromBody] EmployeeMonthEndDTO data)
        //{
        //    return _objinter.getrpt(data);
        //}
    }
}
