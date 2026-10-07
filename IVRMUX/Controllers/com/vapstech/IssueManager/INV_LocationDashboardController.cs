using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.IssueManager;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.IssueManager;

namespace IVRMUX.Controllers.com.vapstech.IssueManager
{
    [Route("api/[controller]")]
    public class INV_LocationDashboardController : Controller
    {

        public INV_LocationDashboardDelegate _objdel = new INV_LocationDashboardDelegate();

        [Route("getalldetails/{id:int}")]
        public INV_LocationDashboardDTO getalldetails(int id)
        {
            INV_LocationDashboardDTO data = new INV_LocationDashboardDTO();
            data.MI_ID = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _objdel.onfloordetails(data);

        }
        [Route("getemployeedetail")]
        public INV_LocationDashboardDTO getemployeedetail([FromBody]INV_LocationDashboardDTO data)
        {
            data.MI_ID = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _objdel.getemployeedetail(data);
        }

       
        [Route("SaveDetails")]
        public INV_LocationDashboardDTO SaveDetails([FromBody] INV_LocationDashboardDTO data)
        {
            data.MI_ID = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _objdel.SaveDetails(data);
        }
        //removelocationemployee
        [Route("removelocationemployee")]
        public INV_LocationDashboardDTO removelocationemployee([FromBody] INV_LocationDashboardDTO data)
        {
            return _objdel.removelocationemployee(data);
        }
        //OnChangeLocation
        [Route("OnChangeLocation")]
        public INV_LocationDashboardDTO OnChangeLocation([FromBody] INV_LocationDashboardDTO data)
        {
            data.MI_ID = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _objdel.OnChangeLocation(data);
        }
        //suplier
        [Route("OnChange_Suplier")]
        public INV_LocationDashboardDTO OnChange_Suplier([FromBody] INV_LocationDashboardDTO data)
        {
            data.MI_ID = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _objdel.OnChange_Suplier(data);
        }
        [Route("Suplier_Details")]
        public INV_LocationDashboardDTO Suplier_Details([FromBody] INV_LocationDashboardDTO data)
        {
            data.MI_ID = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _objdel.Suplier_Details(data);
        }
        [Route("Details_supplier")]
        public INV_LocationDashboardDTO Details_supplier([FromBody] INV_LocationDashboardDTO data)
        {
            data.MI_ID = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _objdel.Details_supplier(data);
        }

        //sushma common api
        [Route("getemployeedetailCommon")]
        public INV_LocationDashboardDTO getemployeedetailCommon([FromBody]INV_LocationDashboardDTO data)
        {
            data.MI_ID = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _objdel.getemployeedetailCommon(data);
        }

        [Route("SaveEmployeeAssets")]
        public INV_LocationDashboardDTO SaveEmployeeAssets([FromBody] INV_LocationDashboardDTO data)
        {
            if (data == null)
            {
                data = new INV_LocationDashboardDTO();
            }

            int? sessionMiId = HttpContext.Session.GetInt32("Session_MI_Id");
            if (sessionMiId != null && sessionMiId.HasValue && sessionMiId.Value > 0)
            {
                data.MI_ID = Convert.ToInt32(sessionMiId.Value);
            }

            int? sessionUserId = HttpContext.Session.GetInt32("UserId");
            if (sessionUserId != null && sessionUserId.HasValue)
            {
                data.User_Id = Convert.ToInt32(sessionUserId.Value);
            }

            return _objdel.SaveEmployeeAssets(data);
        }
        //Sanjai
        [Route("GetPendingAcknowledgementReport")]
        public INV_LocationDashboardDTO GetPendingAcknowledgementReport([FromBody] INV_LocationDashboardDTO data)
        {
            if (data == null)
            {
                data = new INV_LocationDashboardDTO();
            }

            int? sessionMiId = HttpContext.Session.GetInt32("Session_MI_Id");
            if (sessionMiId != null && sessionMiId.HasValue && sessionMiId.Value > 0)
            {
                data.MI_ID = Convert.ToInt32(sessionMiId.Value);
            }

            int? sessionUserId = HttpContext.Session.GetInt32("UserId");
            if (sessionUserId != null && sessionUserId.HasValue)
            {
                data.User_Id = Convert.ToInt32(sessionUserId.Value);
            }

            return _objdel.GetPendingAcknowledgementReport(data);
        }

        [Route("SavePendingAssetLocationTransfer")]
        public INV_LocationDashboardDTO SavePendingAssetLocationTransfer([FromBody] INV_LocationDashboardDTO data)
        {
            if (data == null)
            {
                data = new INV_LocationDashboardDTO();
            }

            int? sessionMiId = HttpContext.Session.GetInt32("Session_MI_Id");
            if (sessionMiId != null && sessionMiId.HasValue && sessionMiId.Value > 0)
            {
                data.MI_ID = Convert.ToInt32(sessionMiId.Value);
            }

            int? sessionUserId = HttpContext.Session.GetInt32("UserId");
            if (sessionUserId != null && sessionUserId.HasValue)
            {
                data.User_Id = Convert.ToInt32(sessionUserId.Value);
            }

            return _objdel.SavePendingAssetLocationTransfer(data);
        }
    }
}
