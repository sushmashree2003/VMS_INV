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
    public class ClientInvoiceReportFacadeController : Controller
    {
        // GET: api/values
        ClientInvoiceReportInterface _Inv;
        public ClientInvoiceReportFacadeController(ClientInvoiceReportInterface Inv)
        {
            _Inv = Inv;
        }
        [Route("getloaddata")]
        public Task<ClientInvoiceReportDTO> getloaddata([FromBody] ClientInvoiceReportDTO data)
        {
            return _Inv.getloaddata(data);
        }
        [Route("getStudentClsSec")]
        public Task<ClientInvoiceReportDTO> getStudentClsSec([FromBody] ClientInvoiceReportDTO data)
        {
            return _Inv.getStudentClsSec(data);
        }
        [Route("getsectionlist")]
        public ClientInvoiceReportDTO getsectionlist([FromBody] ClientInvoiceReportDTO data)
        {
            return _Inv.getsectionlist(data);
        }
        [Route("getStudentlist")]
        public Task<ClientInvoiceReportDTO> getStudentlist([FromBody] ClientInvoiceReportDTO data)
        {
            return _Inv.getStudentlist(data);
        }
        [Route("getitem")]
        public Task<ClientInvoiceReportDTO> getitem([FromBody] ClientInvoiceReportDTO data)
        {
            return _Inv.getitem(data);
        }
        [Route("getitemDetail")]
        public Task<ClientInvoiceReportDTO> getitemDetail([FromBody] ClientInvoiceReportDTO data)
        {
            return _Inv.getitemDetail(data);
        }
        [Route("savedetails")]
        public Task<ClientInvoiceReportDTO> savedetails([FromBody] ClientInvoiceReportDTO data)
        {
            return _Inv.savedetails(data);
        }
        [Route("getSaletypes")]
        public Task<ClientInvoiceReportDTO> getSaletypes([FromBody] ClientInvoiceReportDTO data)
        {
            return _Inv.getSaletypes(data);
        }
        [Route("getSaleItemDetails")]
        public Task<ClientInvoiceReportDTO> getSaleItemDetails([FromBody] ClientInvoiceReportDTO data)
        {
            return _Inv.getSaleItemDetails(data);
        }
        [Route("getSaleItemTax")]
        public ClientInvoiceReportDTO getSaleItemTax([FromBody] ClientInvoiceReportDTO data)
        {
            return _Inv.getSaleItemTax(data);
        }
        [Route("deactive")]
        public ClientInvoiceReportDTO deactive([FromBody] ClientInvoiceReportDTO data)
        {
            return _Inv.deactive(data);
        }
        [Route("deactiveS")]
        public ClientInvoiceReportDTO deactiveS([FromBody] ClientInvoiceReportDTO data)
        {
            return _Inv.deactiveS(data);
        }
        [Route("deactivetax")]
        public ClientInvoiceReportDTO deactivetax([FromBody] ClientInvoiceReportDTO data)
        {
            return _Inv.deactivetax(data);
        }
        [Route("Editdata")]
        public ClientInvoiceReportDTO Editdata([FromBody] ClientInvoiceReportDTO data)
        {
            return _Inv.Editdata(data);
        }


    }
}
