using PreadmissionDTOs.com.vaps.Inventory.Purchase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryServicesHub.com.vaps.Purchase.Interface
{
    public interface INV_RequisitionNotificationInterface
    {
        INV_RequisitionNotificationDTO getloaddata(INV_RequisitionNotificationDTO data);
        INV_RequisitionNotificationDTO get_prdetails(INV_RequisitionNotificationDTO data);

        INV_RequisitionNotificationDTO getitemDetail(INV_RequisitionNotificationDTO data);
        INV_RequisitionNotificationDTO savedetails(INV_RequisitionNotificationDTO data);
        INV_RequisitionNotificationDTO edit(INV_RequisitionNotificationDTO data);
        INV_RequisitionNotificationDTO deactiveM(INV_RequisitionNotificationDTO data);
        INV_RequisitionNotificationDTO deactive(INV_RequisitionNotificationDTO data);
        INV_RequisitionNotificationDTO GetOnchangeData(INV_RequisitionNotificationDTO data);
    }
}
