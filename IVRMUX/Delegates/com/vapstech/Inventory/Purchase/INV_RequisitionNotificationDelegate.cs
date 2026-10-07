using CommonLibrary;
using PreadmissionDTOs.com.vaps.Inventory.Purchase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.Inventory.Purchase
{
    public class INV_RequisitionNotificationDelegate
    {
        CommonDelegate<INV_RequisitionNotificationDTO, INV_RequisitionNotificationDTO> COMINV = new CommonDelegate<INV_RequisitionNotificationDTO, INV_RequisitionNotificationDTO>();
        public INV_RequisitionNotificationDTO getloaddata(INV_RequisitionNotificationDTO data)
        {
            return COMINV.POSTDataInventory(data, "INV_RequisitionNotificationFacade/getloaddata/");
        }

        public INV_RequisitionNotificationDTO get_prdetails(INV_RequisitionNotificationDTO data)
        {
            return COMINV.POSTDataInventory(data, "INV_RequisitionNotificationFacade/get_prdetails/");
        }
        public INV_RequisitionNotificationDTO getitemDetail(INV_RequisitionNotificationDTO data)
        {
            return COMINV.POSTDataInventory(data, "INV_RequisitionNotificationFacade/getitemDetail/");
        }
        public INV_RequisitionNotificationDTO savedetails(INV_RequisitionNotificationDTO data)
        {
            return COMINV.POSTDataInventory(data, "INV_RequisitionNotificationFacade/savedetails/");
        }
        public INV_RequisitionNotificationDTO edit(INV_RequisitionNotificationDTO data)
        {
            return COMINV.POSTDataInventory(data, "INV_RequisitionNotificationFacade/edit/");
        }
        public INV_RequisitionNotificationDTO deactive(INV_RequisitionNotificationDTO data)
        {
            return COMINV.POSTDataInventory(data, "INV_RequisitionNotificationFacade/deactive/");
        }
        public INV_RequisitionNotificationDTO deactiveM(INV_RequisitionNotificationDTO data)
        {
            return COMINV.POSTDataInventory(data, "INV_PurchaseRequisitionFacade/deactiveM/");
        }
        public INV_RequisitionNotificationDTO GetOnchangeData(INV_RequisitionNotificationDTO data)
        {
            return COMINV.POSTDataInventory(data, "INV_PurchaseRequisitionFacade/GetOnchangeData/");
        }



    }
}
