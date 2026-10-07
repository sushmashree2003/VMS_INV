using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InventoryServicesHub.com.vaps.Purchase.Interface;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Inventory.Purchase;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace InventoryServicesHub.com.vaps.Purchase.Facade
{
    [Route("api/[controller]")]
    public class INV_RequisitionNotificationFacade : Controller
    {
        INV_RequisitionNotificationInterface _Inv;
        public INV_RequisitionNotificationFacade(INV_RequisitionNotificationInterface Inv)
        {
            _Inv = Inv;
        }
        [Route("getloaddata")]
        public INV_RequisitionNotificationDTO getloaddata([FromBody] INV_RequisitionNotificationDTO data)
        {
            return _Inv.getloaddata(data);
        }

        [Route("get_prdetails")]
        public INV_RequisitionNotificationDTO get_prdetails([FromBody] INV_RequisitionNotificationDTO data)
        {
            return _Inv.get_prdetails(data);
        }
        [Route("getitemDetail")]
        public INV_RequisitionNotificationDTO getitemDetail([FromBody] INV_RequisitionNotificationDTO data)
        {
            return _Inv.getitemDetail(data);
        }
        [Route("savedetails")]
        public INV_RequisitionNotificationDTO savedetails([FromBody] INV_RequisitionNotificationDTO data)
        {
            return _Inv.savedetails(data);
        }
        [Route("edit")]
        public INV_RequisitionNotificationDTO edit([FromBody] INV_RequisitionNotificationDTO data)
        {
            return _Inv.edit(data);
        }
        [Route("deactive")]
        public INV_RequisitionNotificationDTO deactive([FromBody] INV_RequisitionNotificationDTO data)
        {
            return _Inv.deactive(data);
        }
        [Route("deactiveM")]
        public INV_RequisitionNotificationDTO deactiveM([FromBody] INV_RequisitionNotificationDTO data)
        {
            return _Inv.deactiveM(data);
        }
        [Route("GetOnchangeData")]
        public INV_RequisitionNotificationDTO GetOnchangeData([FromBody] INV_RequisitionNotificationDTO data)
        {
            return _Inv.GetOnchangeData(data);
        }
    }
}
