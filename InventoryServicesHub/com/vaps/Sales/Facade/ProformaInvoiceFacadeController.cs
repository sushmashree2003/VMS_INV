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
    public class ProformaInvoiceFacadeController : Controller
    {
        // GET: api/values
        ProformaInvoiceInterface _Inv;
        public ProformaInvoiceFacadeController(ProformaInvoiceInterface Inv)
        {
            _Inv = Inv;
        }
        [Route("getloaddata")]
        public Task<ProformaInvoiceDTO> getloaddata([FromBody] ProformaInvoiceDTO data)
        {
            return _Inv.getloaddata(data);
        }
        [Route("getStudentClsSec")]
        public Task<ProformaInvoiceDTO> getStudentClsSec([FromBody] ProformaInvoiceDTO data)
        {
            return _Inv.getStudentClsSec(data);
        }
        [Route("getsectionlist")]
        public ProformaInvoiceDTO getsectionlist([FromBody] ProformaInvoiceDTO data)
        {
            return _Inv.getsectionlist(data);
        }
        [Route("getStudentlist")]
        public Task<ProformaInvoiceDTO> getStudentlist([FromBody] ProformaInvoiceDTO data)
        {
            return _Inv.getStudentlist(data);
        }
        [Route("getitem")]
        public Task<ProformaInvoiceDTO> getitem([FromBody] ProformaInvoiceDTO data)
        {
            return _Inv.getitem(data);
        }
        [Route("getitemDetail")]
        public Task<ProformaInvoiceDTO> getitemDetail([FromBody] ProformaInvoiceDTO data)
        {
            return _Inv.getitemDetail(data);
        }
        [Route("savedetails")]
        public Task<ProformaInvoiceDTO> savedetails([FromBody] ProformaInvoiceDTO data)
        {
            return _Inv.savedetails(data);
        }
        [Route("getSaletypes")]
        public Task<ProformaInvoiceDTO> getSaletypes([FromBody] ProformaInvoiceDTO data)
        {
            return _Inv.getSaletypes(data);
        }
        [Route("getSaleItemDetails")]
        public Task<ProformaInvoiceDTO> getSaleItemDetails([FromBody] ProformaInvoiceDTO data)
        {
            return _Inv.getSaleItemDetails(data);
        }
        [Route("getSaleItemTax")]
        public ProformaInvoiceDTO getSaleItemTax([FromBody] ProformaInvoiceDTO data)
        {
            return _Inv.getSaleItemTax(data);
        }
        [Route("deactive")]
        public ProformaInvoiceDTO deactive([FromBody] ProformaInvoiceDTO data)
        {
            return _Inv.deactive(data);
        }
        [Route("deactiveS")]
        public ProformaInvoiceDTO deactiveS([FromBody] ProformaInvoiceDTO data)
        {
            return _Inv.deactiveS(data);
        }
        [Route("deactivetax")]
        public ProformaInvoiceDTO deactivetax([FromBody] ProformaInvoiceDTO data)
        {
            return _Inv.deactivetax(data);
        }


    }
}
