using CommonLibrary;
using PreadmissionDTOs.com.vaps.Inventory;
using PreadmissionDTOs.com.vaps.Purchase.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.Inventory
{
    public class OrderApprovalDelegate
    {
        CommonDelegate<OrderApprovalDTO, OrderApprovalDTO> COMINV = new CommonDelegate<OrderApprovalDTO, OrderApprovalDTO>();
        public OrderApprovalDTO getloaddata(OrderApprovalDTO data)
        {
            return COMINV.POSTDataInventory(data, "OrderApprovalFacade/getloaddata/");
        }

        
        public OrderApprovalDTO getpidetails(OrderApprovalDTO data)
        {
            return COMINV.POSTDataInventory(data, "OrderApprovalFacade/getpidetails/");
        }
        public OrderApprovalDTO savedetails(OrderApprovalDTO data)
        {
            return COMINV.POSTDataInventory(data, "OrderApprovalFacade/savedetails/");
        }
        public OrderApprovalDTO Rejectorder(OrderApprovalDTO data)
        {
            return COMINV.POSTDataInventory(data, "OrderApprovalFacade/Rejectorder/");
        }
        public OrderApprovalDTO getprDetail(OrderApprovalDTO data)
        {
            return COMINV.POSTDataInventory(data, "OrderApprovalFacade/getprDetail/");
        }
        public OrderApprovalDTO deactiveM(OrderApprovalDTO data)
        {
            return COMINV.POSTDataInventory(data, "OrderApprovalFacade/deactiveM/");
        }
        public OrderApprovalDTO deactive(OrderApprovalDTO data)
        {
            return COMINV.POSTDataInventory(data, "OrderApprovalFacade/deactive/");
        }
        public OrderApprovalDTO get_details(OrderApprovalDTO data)
        {
            return COMINV.POSTDataInventory(data, "OrderApprovalFacade/get_details/");
        }
        public OrderApprovalDTO edit(OrderApprovalDTO data)
        {
            return COMINV.POSTDataInventory(data, "OrderApprovalFacade/edit/");
        }
        public OrderApprovalDTO rejectallreq(OrderApprovalDTO data)
        {
            return COMINV.POSTDataInventory(data, "OrderApprovalFacade/rejectallreq/");
        }
        public OrderApprovalDTO viewsavedteple(OrderApprovalDTO data)
        {
            return COMINV.POSTDataInventory(data, "OrderApprovalFacade/viewsavedteple/");
        }
        public OrderApprovalDTO viewtemplate(OrderApprovalDTO data)
        {
            return COMINV.POSTDataInventory(data, "OrderApprovalFacade/viewtemplate/");
        }
        public OrderApprovalDTO OnClickIndividualViewComments(OrderApprovalDTO data)
        {
            return COMINV.POSTDataInventory(data, "OrderApprovalFacade/OnClickIndividualViewComments/");
        }
    }
}