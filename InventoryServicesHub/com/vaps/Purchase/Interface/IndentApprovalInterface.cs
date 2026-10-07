using PreadmissionDTOs.com.vaps.Inventory;
using PreadmissionDTOs.com.vaps.Purchase.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryServicesHub.com.vaps.Purchase.Interface
{
    public interface IndentApprovalInterface
    {
        IndentApprovalDTO getloaddata(IndentApprovalDTO data);
        IndentApprovalDTO savedetails(IndentApprovalDTO data);
        Task<IndentApprovalDTO> getpidetails(IndentApprovalDTO data);
        IndentApprovalDTO getprDetail(IndentApprovalDTO data);
        IndentApprovalDTO deactive(IndentApprovalDTO data);
        IndentApprovalDTO get_details(IndentApprovalDTO data);
        IndentApprovalDTO edit(IndentApprovalDTO data);
        IndentApprovalDTO rejectallreq(IndentApprovalDTO data);
        IndentApprovalDTO deactiveM(IndentApprovalDTO data);
        IndentApprovalDTO OnClickIndividualViewComments(IndentApprovalDTO data);

    }
}