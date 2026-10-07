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
    public class ProformaInvoiceReportFacadeController : Controller
    {
        // GET: api/values
        ProformaInvoiceReportInterface _Inv;
        public ProformaInvoiceReportFacadeController(ProformaInvoiceReportInterface Inv)
        {
            _Inv = Inv;
        }
        [Route("getloaddata")]
        public Task<ProformaInvoiceReportDTO> getloaddata([FromBody] ProformaInvoiceReportDTO data)
        {
            return _Inv.getloaddata(data);
        }
        [Route("getStudentClsSec")]
        public Task<ProformaInvoiceReportDTO> getStudentClsSec([FromBody] ProformaInvoiceReportDTO data)
        {
            return _Inv.getStudentClsSec(data);
        }
        [Route("getsectionlist")]
        public ProformaInvoiceReportDTO getsectionlist([FromBody] ProformaInvoiceReportDTO data)
        {
            return _Inv.getsectionlist(data);
        }
        [Route("getStudentlist")]
        public Task<ProformaInvoiceReportDTO> getStudentlist([FromBody] ProformaInvoiceReportDTO data)
        {
            return _Inv.getStudentlist(data);
        }
        [Route("getitem")]
        public Task<ProformaInvoiceReportDTO> getitem([FromBody] ProformaInvoiceReportDTO data)
        {
            return _Inv.getitem(data);
        }
        [Route("getitemDetail")]
        public Task<ProformaInvoiceReportDTO> getitemDetail([FromBody] ProformaInvoiceReportDTO data)
        {
            return _Inv.getitemDetail(data);
        }
        [Route("savedetails")]
        public Task<ProformaInvoiceReportDTO> savedetails([FromBody] ProformaInvoiceReportDTO data)
        {
            return _Inv.savedetails(data);
        }
        [Route("getSaletypes")]
        public Task<ProformaInvoiceReportDTO> getSaletypes([FromBody] ProformaInvoiceReportDTO data)
        {
            return _Inv.getSaletypes(data);
        }
        [Route("getSaleItemDetails")]
        public Task<ProformaInvoiceReportDTO> getSaleItemDetails([FromBody] ProformaInvoiceReportDTO data)
        {
            return _Inv.getSaleItemDetails(data);
        }
        [Route("getSaleItemTax")]
        public ProformaInvoiceReportDTO getSaleItemTax([FromBody] ProformaInvoiceReportDTO data)
        {
            return _Inv.getSaleItemTax(data);
        }
        [Route("deactive")]
        public ProformaInvoiceReportDTO deactive([FromBody] ProformaInvoiceReportDTO data)
        {
            return _Inv.deactive(data);
        }
        [Route("deactiveS")]
        public ProformaInvoiceReportDTO deactiveS([FromBody] ProformaInvoiceReportDTO data)
        {
            return _Inv.deactiveS(data);
        }
        [Route("deactivetax")]
        public ProformaInvoiceReportDTO deactivetax([FromBody] ProformaInvoiceReportDTO data)
        {
            return _Inv.deactivetax(data);
        }
        [Route("Editdata")]
        public ProformaInvoiceReportDTO Editdata([FromBody] ProformaInvoiceReportDTO data)
        {
            return _Inv.Editdata(data);
        }


    }
}
