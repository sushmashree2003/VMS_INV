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
    public class EmpLoanPayoutFacade : Controller
    {
        public EmpLoanPayoutInterface _ads;

        public EmpLoanPayoutFacade(EmpLoanPayoutInterface adstu)
        {
            _ads = adstu;
        }

        // GET: api/values
        [Route("onloadgetdetails")]
        public EmpLoanPayoutDTO getinitialdata([FromBody]EmpLoanPayoutDTO dto)
        {
            return _ads.getBasicData(dto);
        }

        // POST api/values
        [HttpPost]
        public EmpLoanPayoutDTO Post([FromBody]EmpLoanPayoutDTO dto)
        {
            return _ads.SaveUpdate(dto);
        }

        [Route("getRecordById/{id:int}")]

        public EmpLoanPayoutDTO getcatgrydet(int id)
        {
           
            return _ads.editData(id);
        }
        [Route("employeechange/{id:int}")]

        public EmpLoanPayoutDTO employeechange(int id)
        {
           
            return _ads.employeechange(id);
        }
        [Route("deactivateRecordById")]
        public EmpLoanPayoutDTO deactivateRecordById([FromBody]EmpLoanPayoutDTO dto)
        {
            return _ads.deactivate(dto);
        }
    }
}
