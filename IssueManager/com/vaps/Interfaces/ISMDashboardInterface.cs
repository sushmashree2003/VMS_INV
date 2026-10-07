using PreadmissionDTOs.com.vaps.IssueManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IssueManager.com.vaps.Interfaces
{
    public interface ISMDashboardInterface
    {
        Task<ISMDashboardDTO> getdetailsAsync(ISMDashboardDTO data);
        Task<ISMDashboardDTO> getlateearlyoutreport(ISMDashboardDTO data);
        ISMDashboardDTO getmodule(ISMDashboardDTO data);
        ISMDashboardDTO loaddataMDDASHBOARD(ISMDashboardDTO data);
        ISMDashboardDTO pendingissues(ISMDashboardDTO data);
        ISMDashboardDTO pendingissuesnotification(ISMDashboardDTO data);
        ISMDashboardDTO pendingissuesnotification_admin(ISMDashboardDTO data);
        ISMDashboardDTO ViewEmployeeIssueList(ISMDashboardDTO data);
        ISMDashboardDTO OnClickViewHrPlannerDetails(ISMDashboardDTO data);
        ISMDashboardDTO QrCodeAPIUpdate(ISMDashboardDTO data);
        ISMDashboardDTO QRcode_Emp_profile(ISMDashboardDTO data);
        ISMDashboardDTO updatevisitorscheckoutlist(ISMDashboardDTO data);
        ISMDashboardDTO savetrainingRemarklist(ISMDashboardDTO data);
        ISMDashboardDTO getdatedetails(ISMDashboardDTO data);
        ISMDashboardDTO absentPresent(ISMDashboardDTO data);
        ISMDashboardDTO ViewTenantDetails(ISMDashboardDTO data);
        ISMDashboardDTO onclickdata(ISMDashboardDTO data);
        //Praveen
        ISMDashboardDTO getemployeedetail(ISMDashboardDTO data);
        ISMDashboardDTO StatewiseleadDetails(ISMDashboardDTO data);
        
              ISMDashboardDTO employeetasklist(ISMDashboardDTO data);


        //dinner 

        ISMDashboardDTO orderedinner(ISMDashboardDTO data);
        ISMDashboardDTO dinnerorderlist(ISMDashboardDTO data);
    }
}
