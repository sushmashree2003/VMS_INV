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
    public class SalesInvoiceReportFacadeController : Controller
    {
        // GET: api/values
        SalesInvoiceReportInterface _Inv;
        public SalesInvoiceReportFacadeController(SalesInvoiceReportInterface Inv)
        {
            _Inv = Inv;
        }
        [Route("getloaddata")]
        public Task<SalesInvoiceReportDTO> getloaddata([FromBody] SalesInvoiceReportDTO data)
        {
            return _Inv.getloaddata(data);
        }
        [Route("getStudentClsSec")]
        public Task<SalesInvoiceReportDTO> getStudentClsSec([FromBody] SalesInvoiceReportDTO data)
        {
            return _Inv.getStudentClsSec(data);
        }
        [Route("getsectionlist")]
        public SalesInvoiceReportDTO getsectionlist([FromBody] SalesInvoiceReportDTO data)
        {
            return _Inv.getsectionlist(data);
        }
        [Route("getStudentlist")]
        public Task<SalesInvoiceReportDTO> getStudentlist([FromBody] SalesInvoiceReportDTO data)
        {
            return _Inv.getStudentlist(data);
        }
        [Route("getitem")]
        public Task<SalesInvoiceReportDTO> getitem([FromBody] SalesInvoiceReportDTO data)
        {
            return _Inv.getitem(data);
        }
        [Route("getitemDetail")]
        public Task<SalesInvoiceReportDTO> getitemDetail([FromBody] SalesInvoiceReportDTO data)
        {
            return _Inv.getitemDetail(data);
        }
        [Route("savedetails")]
        public Task<SalesInvoiceReportDTO> savedetails([FromBody] SalesInvoiceReportDTO data)
        {
            return _Inv.savedetails(data);
        }
        [Route("getSaletypes")]
        public Task<SalesInvoiceReportDTO> getSaletypes([FromBody] SalesInvoiceReportDTO data)
        {
            return _Inv.getSaletypes(data);
        }
        [Route("getSaleItemDetails")]
        public Task<SalesInvoiceReportDTO> getSaleItemDetails([FromBody] SalesInvoiceReportDTO data)
        {
            return _Inv.getSaleItemDetails(data);
        }
        [Route("getSaleItemTax")]
        public SalesInvoiceReportDTO getSaleItemTax([FromBody] SalesInvoiceReportDTO data)
        {
            return _Inv.getSaleItemTax(data);
        }
        [Route("deactive")]
        public SalesInvoiceReportDTO deactive([FromBody] SalesInvoiceReportDTO data)
        {
            return _Inv.deactive(data);
        }
        [Route("deactiveS")]
        public SalesInvoiceReportDTO deactiveS([FromBody] SalesInvoiceReportDTO data)
        {
            return _Inv.deactiveS(data);
        }
        [Route("deactivetax")]
        public SalesInvoiceReportDTO deactivetax([FromBody] SalesInvoiceReportDTO data)
        {
            return _Inv.deactivetax(data);
        }


    }
}
