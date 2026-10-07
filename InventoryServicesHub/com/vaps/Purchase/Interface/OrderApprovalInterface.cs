using PreadmissionDTOs.com.vaps.Inventory;
using PreadmissionDTOs.com.vaps.Purchase.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryServicesHub.com.vaps.Purchase.Interface
{
    public interface OrderApprovalInterface
    {
        OrderApprovalDTO getloaddata(OrderApprovalDTO data);
        OrderApprovalDTO savedetails(OrderApprovalDTO data);
        Task<OrderApprovalDTO> getpidetails(OrderApprovalDTO data);
        OrderApprovalDTO getprDetail(OrderApprovalDTO data);
        OrderApprovalDTO deactive(OrderApprovalDTO data);
        OrderApprovalDTO get_details(OrderApprovalDTO data);
        OrderApprovalDTO edit(OrderApprovalDTO data);
        OrderApprovalDTO rejectallreq(OrderApprovalDTO data);
        OrderApprovalDTO deactiveM(OrderApprovalDTO data);
        OrderApprovalDTO viewsavedteple(OrderApprovalDTO data);
        OrderApprovalDTO viewtemplate(OrderApprovalDTO data);
        OrderApprovalDTO Rejectorder(OrderApprovalDTO data);
        OrderApprovalDTO OnClickIndividualViewComments(OrderApprovalDTO data);
    }
}