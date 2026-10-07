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
    public class OrderApprovalFacade : Controller
    {
        // GET: api/values
        OrderApprovalInterface _Inv;
        public OrderApprovalFacade(OrderApprovalInterface Inv)
        {
            _Inv = Inv;
        }

        [Route("getloaddata")]
        public OrderApprovalDTO getloaddata([FromBody] OrderApprovalDTO data)
        {
            return _Inv.getloaddata(data);
        }

        [Route("savedetails")]
        public OrderApprovalDTO savedetails([FromBody] OrderApprovalDTO data)
        {
            return _Inv.savedetails(data);
        }

        [Route("getpidetails")]
        public Task<OrderApprovalDTO> getpidetails([FromBody] OrderApprovalDTO data)
        {
            return _Inv.getpidetails(data);
        }

        [Route("getprDetail")]
        public OrderApprovalDTO getprDetail([FromBody] OrderApprovalDTO data)
        {
            return _Inv.getprDetail(data);
        }

        [Route("deactiveM")]
        public OrderApprovalDTO deactiveM([FromBody] OrderApprovalDTO data)
        {
            return _Inv.deactiveM(data);
        }

        [Route("deactive")]
        public OrderApprovalDTO deactive([FromBody] OrderApprovalDTO data)
        {
            return _Inv.deactive(data);
        }

        [Route("get_details")]
        public OrderApprovalDTO get_details([FromBody] OrderApprovalDTO data)
        {
            return _Inv.get_details(data);
        }

        [Route("edit")]
        public OrderApprovalDTO edit([FromBody] OrderApprovalDTO data)
        {
            return _Inv.edit(data);
        }

        [Route("rejectallreq")]
        public OrderApprovalDTO rejectallreq([FromBody] OrderApprovalDTO data)
        {
            return _Inv.rejectallreq(data);
        }

        [Route("viewsavedteple")]
        public OrderApprovalDTO viewsavedteple([FromBody] OrderApprovalDTO data)
        {
            return _Inv.viewsavedteple(data);
        }

        [Route("viewtemplate")]
        public OrderApprovalDTO viewtemplate([FromBody] OrderApprovalDTO data)
        {
            return _Inv.viewtemplate(data);
        }

        [Route("Rejectorder")]
        public OrderApprovalDTO Rejectorder([FromBody] OrderApprovalDTO data)
        {
            return _Inv.Rejectorder(data);
        }

        [Route("OnClickIndividualViewComments")]
        public OrderApprovalDTO OnClickIndividualViewComments([FromBody] OrderApprovalDTO data)
        {
            return _Inv.OnClickIndividualViewComments(data);
        }

    }
}