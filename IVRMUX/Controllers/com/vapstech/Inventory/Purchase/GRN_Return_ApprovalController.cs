using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.Inventory.Purchase;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Inventory;

namespace IVRMUX.Controllers.com.vapstech.Inventory.Purchase
{
   
    [Route("api/[controller]")]
    public class GRN_Return_ApprovalController : Controller
    {
        public GRN_Return_Approval_Deligate del = new GRN_Return_Approval_Deligate();

        [Route("getloaddata/{id:int}")]
        public GRN_Return_Apply_DTO getloaddata (int id)
        {
            GRN_Return_Apply_DTO data = new GRN_Return_Apply_DTO();
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.getloaddata(data);
        }
        [Route("getpidetails")]
        public GRN_Return_Apply_DTO getpidetails([FromBody] GRN_Return_Apply_DTO dto)
        {
          
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            dto.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.getpidetails(dto);
        }
        [Route("savedetails")]
        public GRN_Return_Apply_DTO savedetails([FromBody] GRN_Return_Apply_DTO dto)
        {
          
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            dto.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.savedetails(dto);
        }

    }
}