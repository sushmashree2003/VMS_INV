using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Inventory;
using InventoryServicesHub.com.vaps.Interface;
using InventoryServicesHub.com.vaps.Sales.Interface;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace InventoryServicesHub.com.vaps.Sales.Controllers
{
    [Route("api/[controller]")]
    public class SalesInvoiceFacadeController : Controller
    {
        // GET: api/values
        SalesInvoiceInterface _Inv;
        public SalesInvoiceFacadeController(SalesInvoiceInterface Inv)
        {
            _Inv = Inv;
        }
        [Route("getloaddata")]
        public Task<SalesInvoiceDTO> getloaddata([FromBody] SalesInvoiceDTO data)
        {
            return _Inv.getloaddata(data);
        }
        [Route("getStudentClsSec")]
        public Task<SalesInvoiceDTO> getStudentClsSec([FromBody] SalesInvoiceDTO data)
        {
            return _Inv.getStudentClsSec(data);
        }
        [Route("getsectionlist")]
        public SalesInvoiceDTO getsectionlist([FromBody] SalesInvoiceDTO data)
        {
            return _Inv.getsectionlist(data);
        }
        [Route("getStudentlist")]
        public Task<SalesInvoiceDTO> getStudentlist([FromBody] SalesInvoiceDTO data)
        {
            return _Inv.getStudentlist(data);
        }
        [Route("getitem")]
        public Task<SalesInvoiceDTO> getitem([FromBody] SalesInvoiceDTO data)
        {
            return _Inv.getitem(data);
        }
        [Route("getitemDetail")]
        public Task<SalesInvoiceDTO> getitemDetail([FromBody] SalesInvoiceDTO data)
        {
            return _Inv.getitemDetail(data);
        }
        [Route("savedetails")]
        public Task<SalesInvoiceDTO> savedetails([FromBody] SalesInvoiceDTO data)
        {
            return _Inv.savedetails(data);
        }
        [Route("getSaletypes")]
        public Task<SalesInvoiceDTO> getSaletypes([FromBody] SalesInvoiceDTO data)
        {
            return _Inv.getSaletypes(data);
        }
        [Route("getSaleItemDetails")]
        public Task<SalesInvoiceDTO> getSaleItemDetails([FromBody] SalesInvoiceDTO data)
        {
            return _Inv.getSaleItemDetails(data);
        }
        [Route("getSaleItemTax")]
        public SalesInvoiceDTO getSaleItemTax([FromBody] SalesInvoiceDTO data)
        {
            return _Inv.getSaleItemTax(data);
        }
        [Route("deactive")]
        public SalesInvoiceDTO deactive([FromBody] SalesInvoiceDTO data)
        {
            return _Inv.deactive(data);
        }
        [Route("deactiveS")]
        public SalesInvoiceDTO deactiveS([FromBody] SalesInvoiceDTO data)
        {
            return _Inv.deactiveS(data);
        }
        [Route("deactivetax")]
        public SalesInvoiceDTO deactivetax([FromBody] SalesInvoiceDTO data)
        {
            return _Inv.deactivetax(data);
        }


    }
}
