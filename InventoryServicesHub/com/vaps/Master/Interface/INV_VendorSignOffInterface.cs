using PreadmissionDTOs.com.vaps.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryServicesHub.com.vaps.Master.Interface
{
    public interface INV_VendorSignOffInterface
    {
        Task<INV_VendorSignOffDTO> getloaddata(INV_VendorSignOffDTO dto);
        Task<INV_VendorSignOffDTO> savedetails(INV_VendorSignOffDTO dto);
        Task<INV_VendorSignOffDTO> deactive(INV_VendorSignOffDTO dto);
    }
}
