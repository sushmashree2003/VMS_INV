using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.Inventory.Sales;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Inventory;

namespace IVRMUX.Controllers.com.vapstech.Inventory.Sales
{
    [Produces("application/json")]
    [Route("api/INV_Depreciation")]
    public class INV_DepreciationController : Controller
    {
        INV_DepreciationDelegate _delegate = new INV_DepreciationDelegate();
        [HttpGet]

        [Route("getloaddata/{id:int}")]
        public INV_DepreciationDTO getloaddata(int id)
        {
            INV_DepreciationDTO data = new INV_DepreciationDTO();
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.getloaddata(data);
        }
        [HttpPost]
        [Route("savedetails")]
        public INV_DepreciationDTO savedetails([FromBody] INV_DepreciationDTO data)
        {
            data.User_Id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delegate.savedetails(data);
        }
        [Route("storechanges")]
        public INV_DepreciationDTO storechanges([FromBody] INV_DepreciationDTO data)
        {
            
            return _delegate.storechanges(data);
        }
        //Itemchanges
        [Route("Itemchanges")]
        public INV_DepreciationDTO Itemchanges([FromBody] INV_DepreciationDTO data)
        {

            return _delegate.Itemchanges(data);
        }
        // deactive
        [Route("deactive")]
        public INV_DepreciationDTO deactive([FromBody] INV_DepreciationDTO data)
        {

            return _delegate.deactive(data);
        }
        //deactive
    }
}