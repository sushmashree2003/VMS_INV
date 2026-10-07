using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InventoryServicesHub.com.vaps.Purchase.Implementation;
using InventoryServicesHub.com.vaps.Purchase.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Inventory;

namespace InventoryServicesHub.com.vaps.Purchase.Facade
{

    [Route("api/[controller]")]
    public class INV_T_GRN_ReturnFacadeController : Controller
    {
        INV_T_GRN_ReturnInterface _Inv;
        public INV_T_GRN_ReturnFacadeController(INV_T_GRN_ReturnInterface Inv)
        {
            _Inv = Inv;
        }
        [Route("getloaddata")]
        public GRN_ReturnDTO getloaddata([FromBody] GRN_ReturnDTO data)
        {
            return _Inv.getloaddata(data);
        }

        [Route("getitem")]
        public GRN_ReturnDTO getitem([FromBody] GRN_ReturnDTO data)
        {
            return _Inv.getitem(data);
        }
        [Route("getitemDetail")]
        public GRN_ReturnDTO getitemDetail([FromBody] GRN_ReturnDTO data)
        {
            return _Inv.getitemDetail(data);
        }
        [Route("savedetails")]
        public GRN_ReturnDTO savedetails([FromBody] GRN_ReturnDTO data)
        {
            return _Inv.savedetails(data);
        }
        [Route("get_GRNitemDetails")]
        public GRN_ReturnDTO get_GRNitemDetails([FromBody] GRN_ReturnDTO data)
        {
            return _Inv.get_GRNitemDetails(data);
        }
        [Route("deactive")]
        public GRN_ReturnDTO deactive([FromBody] GRN_ReturnDTO data)
        {
            return _Inv.deactive(data);
        }



    }
}