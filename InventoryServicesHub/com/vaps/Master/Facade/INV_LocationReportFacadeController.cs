using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Inventory;
using InventoryServicesHub.com.vaps.Interface;
using InventoryServicesHub.com.vaps.Purchase.Interface;
using InventoryServicesHub.com.vaps.Master.Interface;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace InventoryServicesHub.com.vaps.Master.Controllers
{
    [Route("api/[controller]")]
    public class INV_LocationReportFacadeController : Controller
    {
        INV_LocationReportInterface _Inv;
        public INV_LocationReportFacadeController(INV_LocationReportInterface Inv)
        {
            _Inv = Inv;
        }
        //[Route("getloaddata")]
        //public INV_LocationReportDTO getloaddata([FromBody] INV_LocationReportDTO data)
        //{
        //    return _Inv.getloaddata(data);
        //}
        //[Route("onreport")]
        //public INV_LocationReportDTO onreport([FromBody] INV_LocationReportDTO data)
        //{
        //    return _Inv.onreport(data);
        //}

        [Route("getloaddata")]
        public Task<INV_LocationReportDTO> getloaddata([FromBody] INV_LocationReportDTO data)
        {
            return _Inv.getloaddata(data);
        }
        [Route("onreport")]
        public Task<INV_LocationReportDTO> onreport([FromBody] INV_LocationReportDTO data)
        {
            return _Inv.onreport(data);
        }
        //onchangeloaddata
        [Route("onchangeloaddata")]
        public Task<INV_LocationReportDTO> onchangeloaddata([FromBody] INV_LocationReportDTO data)
        {
            return _Inv.onchangeloaddata(data);
        }

    }
}
