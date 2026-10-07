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
    public class ISMDashboardController : Controller
    {

        public ISMDashboardDelegate _objdel = new ISMDashboardDelegate();

        [Route("getdetails/{id:int}")]
        public ISMDashboardDTO getdetails(int id)
        {
            ISMDashboardDTO data = new ISMDashboardDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.IVRMRT_Id = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            data.DR_NotApprovedTL = Convert.ToInt32(HttpContext.Session.GetInt32("DR_NotApprovedTL"));
            HttpContext.Session.SetInt32("DR_NotApprovedTL", 1);
            return _objdel.getdetails(data);
        }
        [Route("getlateearlyoutreport")]
        public ISMDashboardDTO getlateearlyoutreport([FromBody]ISMDashboardDTO data)
        {           
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            //data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.IVRMRT_Id = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return _objdel.getlateearlyoutreport(data);
        }

        [Route("getmodule")]
        public ISMDashboardDTO getmodule([FromBody]ISMDashboardDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return _objdel.getmodule(data);
        }

        [Route("loaddataMDDASHBOARD")]
        public ISMDashboardDTO loaddataMDDASHBOARD([FromBody]ISMDashboardDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return _objdel.loaddataMDDASHBOARD(data);
        }

        [Route("pendingissues")]
        public ISMDashboardDTO pendingissues([FromBody]ISMDashboardDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.IVRMRT_Id = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return _objdel.pendingissues(data);
        }

        [Route("ViewEmployeeIssueList")]
        public ISMDashboardDTO ViewEmployeeIssueList([FromBody]ISMDashboardDTO data)
        {            
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.IVRMRT_Id = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return _objdel.ViewEmployeeIssueList(data);
        }

        [Route("OnClickViewHrPlannerDetails")]
        public ISMDashboardDTO OnClickViewHrPlannerDetails([FromBody]ISMDashboardDTO data)
        {            
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.IVRMRT_Id = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return _objdel.OnClickViewHrPlannerDetails(data);
        }

        [Route("QrCodeAPIUpdate")]
        public ISMDashboardDTO QrCodeAPIUpdate([FromBody]ISMDashboardDTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.IVRMRT_Id = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return _objdel.QrCodeAPIUpdate(data);
        }

        [Route("updatevisitorscheckoutlist")]
        public ISMDashboardDTO updatevisitorscheckoutlist([FromBody]ISMDashboardDTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.IVRMRT_Id = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return _objdel.updatevisitorscheckoutlist(data);
        }
        [Route("savetrainingRemarklist")]
        public ISMDashboardDTO savetrainingRemarklist([FromBody]ISMDashboardDTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.IVRMRT_Id = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return _objdel.savetrainingRemarklist(data);
        }
        //DateOfPresent&AbsentEmployee
        [Route("getdatedetails")]
        public ISMDashboardDTO getdatedetails([FromBody]ISMDashboardDTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.IVRMRT_Id = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return _objdel.getdatedetails(data);
        }
        [Route("ViewTenantDetails")]        public ISMDashboardDTO ViewTenantDetails([FromBody]ISMDashboardDTO data)        {            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));            data.IVRMRT_Id = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));            return _objdel.ViewTenantDetails(data);
        }        //ada.ms
        [Route("onclickdata")]
        public ISMDashboardDTO onclickdata([FromBody] ISMDashboardDTO data)
        {
            // data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _objdel.onclickdata(data);
        }

        //Praveen
        [Route("getemployeedetail")]
        public ISMDashboardDTO getemployeedetail([FromBody]ISMDashboardDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _objdel.getemployeedetail(data);
        }
        [Route("StatewiseleadDetails")]
        public ISMDashboardDTO StatewiseleadDetails([FromBody]ISMDashboardDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _objdel.StatewiseleadDetails(data);
        }

        [Route("employeetasklist")]        public ISMDashboardDTO employeetasklist([FromBody]ISMDashboardDTO data)        {            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));            return _objdel.employeetasklist(data);        }


        //dinner 

        [Route("orderedinner")]
        public ISMDashboardDTO OrdereDinner([FromBody] ISMDashboardDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.IVRMRT_Id = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return _objdel.orderedinner(data);
        }
        [Route("dinnerorderlist")]
        public ISMDashboardDTO DinnerOrderList([FromBody] ISMDashboardDTO data)
        {

            return _objdel.dinnerorderlist(data);
        }

    }
}
