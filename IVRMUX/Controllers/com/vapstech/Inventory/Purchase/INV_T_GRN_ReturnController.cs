using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.Inventory.Purchase;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Inventory;

namespace IVRMUX.Controllers.com.vapstech.Inventory.Purchase
{

    [Route("api/[controller]")]
    public class INV_T_GRN_ReturnController : Controller
    {
        INV_T_GRN_Returndelegate _delegate = new INV_T_GRN_Returndelegate();


        [Route("getloaddata/{id:int}")]
        public GRN_ReturnDTO getloaddata(int id)
        {
            GRN_ReturnDTO data = new GRN_ReturnDTO();
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.getloaddata(data);
        }
        [Route("getitem")]
        public GRN_ReturnDTO getitem([FromBody] GRN_ReturnDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.getitem(data);
        }
        [Route("getitemDetail")]
        public GRN_ReturnDTO getitemDetail([FromBody] GRN_ReturnDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.getitemDetail(data);
        }
        [Route("savedata")]
        public GRN_ReturnDTO savedata([FromBody] GRN_ReturnDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delegate.savedetails(data);
        }
        [Route("get_GRNitemDetails")]
        public GRN_ReturnDTO get_GRNitemDetails([FromBody] GRN_ReturnDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.get_GRNitemDetails(data);
        }
        [Route("deactive")]
        public GRN_ReturnDTO deactive([FromBody] GRN_ReturnDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.deactive(data);
        }


       
    }
}