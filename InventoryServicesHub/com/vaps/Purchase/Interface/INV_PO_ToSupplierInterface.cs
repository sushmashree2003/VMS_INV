using PreadmissionDTOs.com.vaps.Inventory;
using PreadmissionDTOs.com.vaps.Purchase.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryServicesHub.com.vaps.Purchase.Interface
{
    public interface INV_PO_ToSupplierInterface
    {
        INV_PO_ToSupplierDTO getloaddata(INV_PO_ToSupplierDTO data);
        INV_PO_ToSupplierDTO getpiDetail(INV_PO_ToSupplierDTO data);
        Task<INV_PO_ToSupplierDTO> savedetails(INV_PO_ToSupplierDTO data);

        INV_PO_ToSupplierDTO deactive(INV_PO_ToSupplierDTO data);
        INV_PO_ToSupplierDTO viewtemplatedetails(INV_PO_ToSupplierDTO data);
         Task<INV_PO_ToSupplierDTO> sendmail(INV_PO_ToSupplierDTO data);

    }


}
