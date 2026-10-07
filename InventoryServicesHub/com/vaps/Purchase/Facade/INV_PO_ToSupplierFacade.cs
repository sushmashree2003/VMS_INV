using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Inventory;
using InventoryServicesHub.com.vaps.Interface;
using InventoryServicesHub.com.vaps.Purchase.Interface;
using PreadmissionDTOs.com.vaps.Purchase.Inventory;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace InventoryServicesHub.com.vaps.Purchase.Controllers
{
    [Route("api/[controller]")]
    public class INV_PO_ToSupplierFacade : Controller
    {
        // GET: api/values
        INV_PO_ToSupplierInterface _Inv;
        public INV_PO_ToSupplierFacade(INV_PO_ToSupplierInterface Inv)
        {
            _Inv = Inv;
        }
        [Route("getloaddata")]
        public INV_PO_ToSupplierDTO getloaddata([FromBody] INV_PO_ToSupplierDTO data)
        {
            return _Inv.getloaddata(data);
        }

        [Route("getpiDetail")]
        public INV_PO_ToSupplierDTO getpiDetail([FromBody] INV_PO_ToSupplierDTO data)
        {
            return _Inv.getpiDetail(data);
        }
        [Route("savedetails")]
        public Task<INV_PO_ToSupplierDTO> savedetails([FromBody] INV_PO_ToSupplierDTO data)
        {
            return _Inv.savedetails(data);
        }

        [Route("deactive")]
        public INV_PO_ToSupplierDTO deactive([FromBody] INV_PO_ToSupplierDTO data)
        {
            return _Inv.deactive(data);
        }
        [Route("viewtemplatedetails")]
        public INV_PO_ToSupplierDTO viewtemplatedetails([FromBody] INV_PO_ToSupplierDTO data)
        {
            return _Inv.viewtemplatedetails(data);
        }
        [Route("sendmail")]
        public Task<INV_PO_ToSupplierDTO> sendmail([FromBody] INV_PO_ToSupplierDTO data)
        {
            return _Inv.sendmail(data);
        }









    }
}
