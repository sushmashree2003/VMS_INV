using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InventoryServicesHub.com.vaps.Purchase.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Inventory;

namespace InventoryServicesHub.com.vaps.Purchase.Facade
{

    [Route("api/[controller]")]
    public class GRN_Return_ApprovalFacadeController : Controller
    {
         GRN_Return_Approval_Interface _inter;
        public GRN_Return_ApprovalFacadeController(GRN_Return_Approval_Interface inter)
        {
            _inter = inter;
        }
        [Route("getloaddata")]
        public GRN_Return_Apply_DTO getloaddata([FromBody] GRN_Return_Apply_DTO dto)
        {
            return _inter.getloaddata(dto);
        }
        [Route("getpidetails")]
        public Task<GRN_Return_Apply_DTO> getpidetails([FromBody] GRN_Return_Apply_DTO dto)
        {
            return _inter.getpidetails(dto);
        }
         [Route("savedetails")]
        public GRN_Return_Apply_DTO savedetails([FromBody] GRN_Return_Apply_DTO dto)
        {
            return _inter.savedetails(dto);
        }

    }
}