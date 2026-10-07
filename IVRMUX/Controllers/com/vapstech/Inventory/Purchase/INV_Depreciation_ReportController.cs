using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.Inventory.Purchase;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Inventory.Purchase;

namespace IVRMUX.Controllers.com.vapstech.Inventory.Purchase
{
    [Produces("application/json")]
    [Route("api/INV_Depreciation_Report")]
    public class INV_Depreciation_ReportController : Controller
    {
        INV_Depreciation_ReportDelegate _delegate = new INV_Depreciation_ReportDelegate();

        [HttpGet]
        [Route("getloaddata/{id:int}")]
        public INV_Depreciation_ReportDTO getloaddata(int id)
        {
            INV_Depreciation_ReportDTO data = new INV_Depreciation_ReportDTO();
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.getloaddata(data);
        }
        [HttpPost]
        [Route("onreport")]
        public INV_Depreciation_ReportDTO onreport([FromBody] INV_Depreciation_ReportDTO data)
        {
           // data.User_Id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delegate.onreport(data);
        }
        //DepreciationCalculation
        [Route("DepreciationCalculation")]
        public INV_Depreciation_ReportDTO DepreciationCalculation([FromBody] INV_Depreciation_ReportDTO data)
        {
            // data.User_Id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delegate.DepreciationCalculation(data);
        }
    }
}