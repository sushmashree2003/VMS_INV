using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IssueManager.com.vaps.Interfaces;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Inventory;
using PreadmissionDTOs.com.vaps.IssueManager;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IssueManager.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class ISMDashboardFacade : Controller
    {

        public ISMDashboardInterface _objinter;

        public ISMDashboardFacade(ISMDashboardInterface parameter)
        {
            _objinter = parameter;
        }

        [Route("getdetails")]
        public Task<ISMDashboardDTO> getdetails([FromBody] ISMDashboardDTO data)
        {
            return _objinter.getdetailsAsync(data);
        }

        [Route("getlateearlyoutreport")]
        public Task<ISMDashboardDTO> getlateearlyoutreport([FromBody] ISMDashboardDTO data)
        {
            return _objinter.getlateearlyoutreport(data);
        }

        [Route("getmodule")]
        public ISMDashboardDTO getmodule([FromBody]ISMDashboardDTO data)
        {
            return _objinter.getmodule(data);
        }

        [Route("loaddataMDDASHBOARD")]
        public ISMDashboardDTO loaddataMDDASHBOARD([FromBody]ISMDashboardDTO data)
        {
            return _objinter.loaddataMDDASHBOARD(data);
        }
        [Route("pendingissues")]
        public ISMDashboardDTO pendingissues([FromBody]ISMDashboardDTO data)
        {
            return _objinter.pendingissues(data);
        }

        [Route("pendingissuesnotification")]
        public ISMDashboardDTO pendingissuesnotification([FromBody]ISMDashboardDTO dto)
        {
            return _objinter.pendingissuesnotification(dto);
        }

        [Route("pendingissuesnotification_admin")]
        public ISMDashboardDTO pendingissuesnotification_admin([FromBody]ISMDashboardDTO dto)
        {
            return _objinter.pendingissuesnotification_admin(dto);
        }

        [Route("ViewEmployeeIssueList")]
        public ISMDashboardDTO ViewEmployeeIssueList([FromBody]ISMDashboardDTO dto)
        {
            return _objinter.ViewEmployeeIssueList(dto);
        }

        [Route("OnClickViewHrPlannerDetails")]
        public ISMDashboardDTO OnClickViewHrPlannerDetails([FromBody]ISMDashboardDTO dto)
        {
            return _objinter.OnClickViewHrPlannerDetails(dto);
        }
        //QrCodeAPIUpdate
        [Route("QrCodeAPIUpdate")]
        public ISMDashboardDTO QrCodeAPIUpdate([FromBody]ISMDashboardDTO dto)
        {
            return _objinter.QrCodeAPIUpdate(dto);
        }
        
        [Route("QRcode_Emp_profile")]
        public ISMDashboardDTO QRcode_Emp_profile([FromBody]ISMDashboardDTO dto)
        {
            return _objinter.QRcode_Emp_profile(dto);
        }
        [Route("updatevisitorscheckoutlist")]
        public ISMDashboardDTO updatevisitorscheckoutlist([FromBody]ISMDashboardDTO dto)
        {
            return _objinter.updatevisitorscheckoutlist(dto);
        }
        [Route("savetrainingRemarklist")]
        public ISMDashboardDTO savetrainingRemarklist([FromBody]ISMDashboardDTO dto)
        {
            return _objinter.savetrainingRemarklist(dto);
        }
        [Route("getdatedetails")]
        public ISMDashboardDTO getdatedetails([FromBody]ISMDashboardDTO dto)
        {
            return _objinter.getdatedetails(dto);
        }

        [Route("absentPresent")]
        public ISMDashboardDTO absentPresent([FromBody]ISMDashboardDTO dto)
        {
            return _objinter.absentPresent(dto);
        }

        [Route("ViewTenantDetails")]        public ISMDashboardDTO ViewTenantDetails([FromBody]ISMDashboardDTO dto)        {            return _objinter.ViewTenantDetails(dto);        }
        //
        [Route("onclickdata")]
        public ISMDashboardDTO onclickdata([FromBody] ISMDashboardDTO dto)
        {
            return _objinter.onclickdata(dto);
        }

        //Praveen
        [Route("getemployeedetail")]
        public ISMDashboardDTO getemployeedetail([FromBody]ISMDashboardDTO data)
        {
            return _objinter.getemployeedetail(data);
        }
        [Route("StatewiseleadDetails")]
        public ISMDashboardDTO StatewiseleadDetails([FromBody]ISMDashboardDTO data)
        {
            return _objinter.StatewiseleadDetails(data);
        }
        
              [Route("employeetasklist")]
        public ISMDashboardDTO employeetasklist([FromBody]ISMDashboardDTO data)
        {
            return _objinter.employeetasklist(data);
        }


        //dinner
        [Route("orderedinner")]
        public ISMDashboardDTO orderedinner([FromBody] ISMDashboardDTO data)
        {
            return _objinter.orderedinner(data);
        }
        [Route("dinnerorderlist")]
        public ISMDashboardDTO dinnerorderlist([FromBody] ISMDashboardDTO data)
        {
            return _objinter.dinnerorderlist(data);
        }
    }
}
