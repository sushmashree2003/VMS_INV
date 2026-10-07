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
    public class ProformaInvoiceReportController : Controller
    {
        ProformaInvoiceReportDelegate _delegate = new ProformaInvoiceReportDelegate();

        [HttpGet]

        [Route("getloaddata/{id:int}")]
        public ProformaInvoiceReportDTO getloaddata(int id)
        {
            ProformaInvoiceReportDTO data = new ProformaInvoiceReportDTO();
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delegate.getloaddata(data);
        }
        [Route("getStudentClsSec")]
        public ProformaInvoiceReportDTO getStudentClsSec([FromBody] ProformaInvoiceReportDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return _delegate.getStudentClsSec(data);
        }
        [Route("getsectionlist")]
        public ProformaInvoiceReportDTO getsectionlist([FromBody] ProformaInvoiceReportDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return _delegate.getsectionlist(data);
        }
        [Route("getStudentlist")]
        public ProformaInvoiceReportDTO getStudentlist([FromBody] ProformaInvoiceReportDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return _delegate.getStudentlist(data);
        }
        [Route("getitem")]
        public ProformaInvoiceReportDTO getitem([FromBody] ProformaInvoiceReportDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.getitem(data);
        }
        [Route("getitemDetail")]
        public ProformaInvoiceReportDTO getitemDetail([FromBody] ProformaInvoiceReportDTO data)
        {
            //data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));            
            return _delegate.getitemDetail(data);
        }

        [Route("savedetails")]
        public ProformaInvoiceReportDTO savedetails([FromBody] ProformaInvoiceReportDTO data)
        {
            //data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            //data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return _delegate.savedetails(data);
        }
        [Route("getSaletypes")]
        public ProformaInvoiceReportDTO getSaletypes([FromBody] ProformaInvoiceReportDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));       
            return _delegate.getSaletypes(data);
        }
        [Route("getSaleItemDetails")]
        public ProformaInvoiceReportDTO getSaleItemDetails([FromBody] ProformaInvoiceReportDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.getSaleItemDetails(data);
        }
        [Route("getSaleItemTax")]
        public ProformaInvoiceReportDTO getSaleItemTax([FromBody] ProformaInvoiceReportDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.getSaleItemTax(data);
        }
        [Route("deactive")]
        public ProformaInvoiceReportDTO deactive([FromBody] ProformaInvoiceReportDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.IMFY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("IMFY_Id"));
            return _delegate.deactive(data);
        }
        [Route("deactiveS")]
        public ProformaInvoiceReportDTO deactiveS([FromBody] ProformaInvoiceReportDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.deactiveS(data);
        }
        [Route("deactivetax")]
        public ProformaInvoiceReportDTO deactivetax([FromBody] ProformaInvoiceReportDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.deactivetax(data);
        }
         [Route("Editdata")]
        public ProformaInvoiceReportDTO Editdata([FromBody] ProformaInvoiceReportDTO data)
        {
            //data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.Editdata(data);
        }



    }
}
