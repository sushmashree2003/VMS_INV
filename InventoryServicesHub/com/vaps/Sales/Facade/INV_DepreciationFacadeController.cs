using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InventoryServicesHub.com.vaps.Sales.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Inventory;

namespace InventoryServicesHub.com.vaps.Sales.Facade
{
    [Produces("application/json")]
    [Route("api/INV_DepreciationFacade")]
    public class INV_DepreciationFacadeController : Controller
    {
        INV_DepreciationInterface _Inv;
        public INV_DepreciationFacadeController(INV_DepreciationInterface Inv)
        {
            _Inv = Inv;
        }
        [Route("getloaddata")]
        public INV_DepreciationDTO getloaddata([FromBody] INV_DepreciationDTO data)
        {
            return _Inv.getloaddata(data);
        }
        [HttpPost]  
        [Route("savedetails")]
        public INV_DepreciationDTO savedetails([FromBody] INV_DepreciationDTO data)
        {
            
            return _Inv.savedetails(data);
        }
        [Route("storechanges")]
        public INV_DepreciationDTO storechanges([FromBody] INV_DepreciationDTO data)
        {
            return _Inv.storechanges(data);
        }
        //Itemchanges
        [Route("Itemchanges")]
        public INV_DepreciationDTO Itemchanges([FromBody] INV_DepreciationDTO data)
        {
            return _Inv.Itemchanges(data);
        }
        [Route("deactive")]
        public INV_DepreciationDTO deactive([FromBody] INV_DepreciationDTO data)
        {
            return _Inv.deactive(data);
        }
        //deactive
    }
}