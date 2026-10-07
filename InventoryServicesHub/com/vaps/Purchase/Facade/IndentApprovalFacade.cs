using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Inventory;
using InventoryServicesHub.com.vaps.Interface;
using InventoryServicesHub.com.vaps.Purchase.Interface;
using PreadmissionDTOs.com.vaps.Purchase.Inventory;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace InventoryServicesHub.com.vaps.Purchase.Controllers
{
    [Route("api/[controller]")]
    public class IndentApprovalFacade : Controller
    {
        // GET: api/values
        IndentApprovalInterface _Inv;
        public IndentApprovalFacade(IndentApprovalInterface Inv)
        {
            _Inv = Inv;
        }

        [Route("getloaddata")]
        public IndentApprovalDTO getloaddata([FromBody] IndentApprovalDTO data)
        {
            return _Inv.getloaddata(data);
        }

        [Route("savedetails")]
        public IndentApprovalDTO savedetails([FromBody] IndentApprovalDTO data)
        {
            return _Inv.savedetails(data);
        }

        [Route("getpidetails")]
        public Task<IndentApprovalDTO> getpidetails([FromBody] IndentApprovalDTO data)
        {
            return _Inv.getpidetails(data);
        }

        [Route("getprDetail")]
        public IndentApprovalDTO getprDetail([FromBody] IndentApprovalDTO data)
        {
            return _Inv.getprDetail(data);
        }

        [Route("deactiveM")]
        public IndentApprovalDTO deactiveM([FromBody] IndentApprovalDTO data)
        {
            return _Inv.deactiveM(data);
        }

        [Route("deactive")]
        public IndentApprovalDTO deactive([FromBody] IndentApprovalDTO data)
        {
            return _Inv.deactive(data);
        }

        [Route("get_details")]
        public IndentApprovalDTO get_details([FromBody] IndentApprovalDTO data)
        {
            return _Inv.get_details(data);
        }

        [Route("edit")]
        public IndentApprovalDTO edit([FromBody] IndentApprovalDTO data)
        {
            return _Inv.edit(data);
        }

        [Route("rejectallreq")]
        public IndentApprovalDTO rejectallreq([FromBody] IndentApprovalDTO data)
        {
            return _Inv.rejectallreq(data);
        }

        [Route("OnClickIndividualViewComments")]
        public IndentApprovalDTO OnClickIndividualViewComments([FromBody] IndentApprovalDTO data)
        {
            return _Inv.OnClickIndividualViewComments(data);
        }
    }
}