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
    public class ProformaInvoiceController : Controller
    {
        ProformaInvoiceDelegate _delegate = new ProformaInvoiceDelegate();

        [HttpGet]

        [Route("getloaddata/{id:int}")]
        public ProformaInvoiceDTO getloaddata(int id)
        {
            ProformaInvoiceDTO data = new ProformaInvoiceDTO();
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.getloaddata(data);
        }
        [Route("getStudentClsSec")]
        public ProformaInvoiceDTO getStudentClsSec([FromBody] ProformaInvoiceDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return _delegate.getStudentClsSec(data);
        }
        [Route("getsectionlist")]
        public ProformaInvoiceDTO getsectionlist([FromBody] ProformaInvoiceDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return _delegate.getsectionlist(data);
        }
        [Route("getStudentlist")]
        public ProformaInvoiceDTO getStudentlist([FromBody] ProformaInvoiceDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return _delegate.getStudentlist(data);
        }
        [Route("getitem")]
        public ProformaInvoiceDTO getitem([FromBody] ProformaInvoiceDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.getitem(data);
        }
        [Route("getitemDetail")]
        public ProformaInvoiceDTO getitemDetail([FromBody] ProformaInvoiceDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));            
            return _delegate.getitemDetail(data);
        }

        [Route("savedetails")]
        public ProformaInvoiceDTO savedetails([FromBody] ProformaInvoiceDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return _delegate.savedetails(data);
        }
        [Route("getSaletypes")]
        public ProformaInvoiceDTO getSaletypes([FromBody] ProformaInvoiceDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));       
            return _delegate.getSaletypes(data);
        }
        [Route("getSaleItemDetails")]
        public ProformaInvoiceDTO getSaleItemDetails([FromBody] ProformaInvoiceDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.getSaleItemDetails(data);
        }
        [Route("getSaleItemTax")]
        public ProformaInvoiceDTO getSaleItemTax([FromBody] ProformaInvoiceDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.getSaleItemTax(data);
        }
        [Route("deactive")]
        public ProformaInvoiceDTO deactive([FromBody] ProformaInvoiceDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.IMFY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("IMFY_Id"));
            return _delegate.deactive(data);
        }
        [Route("deactiveS")]
        public ProformaInvoiceDTO deactiveS([FromBody] ProformaInvoiceDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.deactiveS(data);
        }
        [Route("deactivetax")]
        public ProformaInvoiceDTO deactivetax([FromBody] ProformaInvoiceDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.deactivetax(data);
        }



    }
}
