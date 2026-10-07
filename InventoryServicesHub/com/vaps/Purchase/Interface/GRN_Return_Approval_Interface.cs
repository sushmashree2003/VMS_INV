using PreadmissionDTOs.com.vaps.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryServicesHub.com.vaps.Purchase.Interface
{
    public interface GRN_Return_Approval_Interface
    {
        GRN_Return_Apply_DTO getloaddata(GRN_Return_Apply_DTO dto);
         Task<GRN_Return_Apply_DTO> getpidetails(GRN_Return_Apply_DTO dto);
         GRN_Return_Apply_DTO savedetails(GRN_Return_Apply_DTO dto);
    }
}
