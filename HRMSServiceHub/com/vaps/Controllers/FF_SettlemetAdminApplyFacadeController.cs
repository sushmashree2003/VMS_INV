using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HRMSServiceHub.com.vaps.Interfaces;
using HRMSServicesHub.com.vaps.Interfaces;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.HRMS;

namespace HRMSServiceHub.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class FF_SettlemetAdminApplyFacadeController : Controller
    {
        // GET: api/values

        public FF_SettlemetAdminApplyInterface _ads;

        public FF_SettlemetAdminApplyFacadeController(FF_SettlemetAdminApplyInterface adstu)
        {
            _ads = adstu;
        }

        // GET: api/values
        [Route("onloadgetdetails")]
        public FF_Settlement_Process_DTO getinitialdata([FromBody]FF_Settlement_Process_DTO dto)
        {
            return _ads.getBasicData(dto);
        }

        // POST api/values
        [HttpPost]
        public FF_Settlement_Process_DTO Post([FromBody]FF_Settlement_Process_DTO dto)
        {
            return _ads.SaveUpdate(dto);

        }
        [HttpPost]
        [Route("empdetails")]
        public FF_Settlement_Process_DTO empdetails([FromBody]FF_Settlement_Process_DTO dto)
        {
            return _ads.empdetails(dto);
        }

        [Route("getRecordById/{id:int}")]

        public FF_Settlement_Process_DTO getcatgrydet(int id)
        {
            // id = 12;
            return _ads.editData(id);
        }
        [Route("deactivateRecordById")]
        public FF_Settlement_Process_DTO deactivateRecordById([FromBody]FF_Settlement_Process_DTO dto)
        {
            return _ads.deactivate(dto);
        }
        [Route("getDetailsByEmployee")]
        public FF_Settlement_Process_DTO getDetailsByEmployee([FromBody]FF_Settlement_Process_DTO dto)
        {
            return _ads.getDetailsByEmployee(dto);
        }


        [Route("searchfilter")]
        public FF_Settlement_Process_DTO searchfilter([FromBody]FF_Settlement_Process_DTO dto)
        {
            return _ads.searchfilter(dto);
        }


    }
}
