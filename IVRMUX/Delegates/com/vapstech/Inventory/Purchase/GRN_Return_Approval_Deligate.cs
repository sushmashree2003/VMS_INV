using CommonLibrary;
using PreadmissionDTOs.com.vaps.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.Inventory.Purchase
{
    public class GRN_Return_Approval_Deligate
    {
        CommonDelegate<GRN_Return_Apply_DTO, GRN_Return_Apply_DTO> COMM = new CommonDelegate<GRN_Return_Apply_DTO, GRN_Return_Apply_DTO>();
        public GRN_Return_Apply_DTO getloaddata (GRN_Return_Apply_DTO dto)
        {
            return COMM.POSTDataInventory(dto, "GRN_Return_ApprovalFacade/getloaddata/");
        }
        public GRN_Return_Apply_DTO getpidetails(GRN_Return_Apply_DTO dto)
        {
            return COMM.POSTDataInventory(dto, "GRN_Return_ApprovalFacade/getpidetails/");
        }
        public GRN_Return_Apply_DTO savedetails(GRN_Return_Apply_DTO dto)
        {
            return COMM.POSTDataInventory(dto, "GRN_Return_ApprovalFacade/savedetails/");
        }

    }
}
