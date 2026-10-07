using PreadmissionDTOs.com.vaps.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryServicesHub.com.vaps.Purchase.Interface
{
    public interface INV_T_GRN_ReturnInterface
    {
        GRN_ReturnDTO getloaddata(GRN_ReturnDTO data);
        GRN_ReturnDTO getitem(GRN_ReturnDTO data);
        GRN_ReturnDTO getitemDetail(GRN_ReturnDTO data);
        GRN_ReturnDTO savedetails(GRN_ReturnDTO data);
        GRN_ReturnDTO get_GRNitemDetails(GRN_ReturnDTO data);
        GRN_ReturnDTO deactive(GRN_ReturnDTO data);
        
    }
}
