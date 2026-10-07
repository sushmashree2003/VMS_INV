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
    public class SalesInvoiceReportController : Controller
    {
        SalesInvoiceReportDelegate _delegate = new SalesInvoiceReportDelegate();

        [HttpGet]

        [Route("getloaddata/{id:int}")]
        public SalesInvoiceReportDTO getloaddata(int id)
        {
            SalesInvoiceReportDTO data = new SalesInvoiceReportDTO();
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delegate.getloaddata(data);
        }
        [Route("getStudentClsSec")]
        public SalesInvoiceReportDTO getStudentClsSec([FromBody] SalesInvoiceReportDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return _delegate.getStudentClsSec(data);
        }
        [Route("getsectionlist")]
        public SalesInvoiceReportDTO getsectionlist([FromBody] SalesInvoiceReportDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return _delegate.getsectionlist(data);
        }
        [Route("getStudentlist")]
        public SalesInvoiceReportDTO getStudentlist([FromBody] SalesInvoiceReportDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return _delegate.getStudentlist(data);
        }
        [Route("getitem")]
        public SalesInvoiceReportDTO getitem([FromBody] SalesInvoiceReportDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.getitem(data);
        }
        [Route("getitemDetail")]
        public SalesInvoiceReportDTO getitemDetail([FromBody] SalesInvoiceReportDTO data)
        {
            //data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));            
            return _delegate.getitemDetail(data);
        }

        [Route("savedetails")]
        public SalesInvoiceReportDTO savedetails([FromBody] SalesInvoiceReportDTO data)
        {
            //data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            //data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return _delegate.savedetails(data);
        }
        [Route("getSaletypes")]
        public SalesInvoiceReportDTO getSaletypes([FromBody] SalesInvoiceReportDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));       
            return _delegate.getSaletypes(data);
        }
        [Route("getSaleItemDetails")]
        public SalesInvoiceReportDTO getSaleItemDetails([FromBody] SalesInvoiceReportDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.getSaleItemDetails(data);
        }
        [Route("getSaleItemTax")]
        public SalesInvoiceReportDTO getSaleItemTax([FromBody] SalesInvoiceReportDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.getSaleItemTax(data);
        }
        [Route("deactive")]
        public SalesInvoiceReportDTO deactive([FromBody] SalesInvoiceReportDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.IMFY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("IMFY_Id"));
            return _delegate.deactive(data);
        }
        [Route("deactiveS")]
        public SalesInvoiceReportDTO deactiveS([FromBody] SalesInvoiceReportDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.deactiveS(data);
        }
        [Route("deactivetax")]
        public SalesInvoiceReportDTO deactivetax([FromBody] SalesInvoiceReportDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.deactivetax(data);
        }



    }
}
