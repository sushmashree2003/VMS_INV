using IVRMUX.Delegates.com.vapstech.Inventory;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Controllers.com.vapstech.Inventory
{
    [Route("api/[controller]")]
    public class ClientInvoiceReportController : Controller
    {
        ClientInvoiceReportDelegate _delegate = new ClientInvoiceReportDelegate();

        [HttpGet]

        [Route("getloaddata/{id:int}")]
        public ClientInvoiceReportDTO getloaddata(int id)
        {
            ClientInvoiceReportDTO data = new ClientInvoiceReportDTO();
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delegate.getloaddata(data);
        }
        [Route("getStudentClsSec")]
        public ClientInvoiceReportDTO getStudentClsSec([FromBody] ClientInvoiceReportDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return _delegate.getStudentClsSec(data);
        }
        [Route("getsectionlist")]
        public ClientInvoiceReportDTO getsectionlist([FromBody] ClientInvoiceReportDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return _delegate.getsectionlist(data);
        }
        [Route("getStudentlist")]
        public ClientInvoiceReportDTO getStudentlist([FromBody] ClientInvoiceReportDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return _delegate.getStudentlist(data);
        }
        [Route("getitem")]
        public ClientInvoiceReportDTO getitem([FromBody] ClientInvoiceReportDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.getitem(data);
        }
        [Route("getitemDetail")]
        public ClientInvoiceReportDTO getitemDetail([FromBody] ClientInvoiceReportDTO data)
        {
            //data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));            
            return _delegate.getitemDetail(data);
        }

        [Route("savedetails")]
        public ClientInvoiceReportDTO savedetails([FromBody] ClientInvoiceReportDTO data)
        {
            //data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            //data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return _delegate.savedetails(data);
        }
        [Route("getSaletypes")]
        public ClientInvoiceReportDTO getSaletypes([FromBody] ClientInvoiceReportDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));       
            return _delegate.getSaletypes(data);
        }
        [Route("getSaleItemDetails")]
        public ClientInvoiceReportDTO getSaleItemDetails([FromBody] ClientInvoiceReportDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.getSaleItemDetails(data);
        }
        [Route("getSaleItemTax")]
        public ClientInvoiceReportDTO getSaleItemTax([FromBody] ClientInvoiceReportDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.getSaleItemTax(data);
        }
        [Route("deactive")]
        public ClientInvoiceReportDTO deactive([FromBody] ClientInvoiceReportDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.IMFY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("IMFY_Id"));
            return _delegate.deactive(data);
        }
        [Route("deactiveS")]
        public ClientInvoiceReportDTO deactiveS([FromBody] ClientInvoiceReportDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.deactiveS(data);
        }
        [Route("deactivetax")]
        public ClientInvoiceReportDTO deactivetax([FromBody] ClientInvoiceReportDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.deactivetax(data);
        }
         [Route("Editdata")]
        public ClientInvoiceReportDTO Editdata([FromBody] ClientInvoiceReportDTO data)
        {
            //data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.Editdata(data);
        }



    }
}
