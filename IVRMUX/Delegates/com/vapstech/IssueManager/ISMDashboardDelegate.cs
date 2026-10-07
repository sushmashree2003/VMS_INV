using CommonLibrary;
using PreadmissionDTOs.com.vaps.IssueManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.IssueManager
{
    public class ISMDashboardDelegate
    {
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<ISMDashboardDTO, ISMDashboardDTO> COMMM = new CommonDelegate<ISMDashboardDTO, ISMDashboardDTO>();

        public ISMDashboardDTO getdetails(ISMDashboardDTO data)
        {
            return COMMM.POSTIssueManager(data, "ISMDashboardFacade/getdetails/");
        }
        public ISMDashboardDTO getlateearlyoutreport(ISMDashboardDTO data)
        {
            return COMMM.POSTIssueManager(data, "ISMDashboardFacade/getlateearlyoutreport/");
        }
        public ISMDashboardDTO getmodule(ISMDashboardDTO data)
        {
            return COMMM.POSTIssueManager(data, "ISMDashboardFacade/getmodule");
        }

        public ISMDashboardDTO loaddataMDDASHBOARD(ISMDashboardDTO data)
        {
            return COMMM.POSTIssueManager(data, "ISMDashboardFacade/loaddataMDDASHBOARD");
        }
        public ISMDashboardDTO pendingissues(ISMDashboardDTO data)
        {
            return COMMM.POSTIssueManager(data, "ISMDashboardFacade/pendingissues");
        }
        public ISMDashboardDTO ViewEmployeeIssueList(ISMDashboardDTO data)
        {
            return COMMM.POSTIssueManager(data, "ISMDashboardFacade/ViewEmployeeIssueList");
        }
        public ISMDashboardDTO OnClickViewHrPlannerDetails(ISMDashboardDTO data)
        {
            return COMMM.POSTIssueManager(data, "ISMDashboardFacade/OnClickViewHrPlannerDetails");
        }
        public ISMDashboardDTO QrCodeAPIUpdate(ISMDashboardDTO data)
        {
            return COMMM.POSTIssueManager(data, "ISMDashboardFacade/QrCodeAPIUpdate");
        }
        public ISMDashboardDTO updatevisitorscheckoutlist(ISMDashboardDTO data)
        {
            return COMMM.POSTIssueManager(data, "ISMDashboardFacade/updatevisitorscheckoutlist");
        }
        public ISMDashboardDTO savetrainingRemarklist(ISMDashboardDTO data)
        {
            return COMMM.POSTIssueManager(data, "ISMDashboardFacade/savetrainingRemarklist");
        }
        public ISMDashboardDTO getdatedetails(ISMDashboardDTO data)
        {
            return COMMM.POSTIssueManager(data, "ISMDashboardFacade/getdatedetails");
        }
        public ISMDashboardDTO ViewTenantDetails(ISMDashboardDTO data)        {            return COMMM.POSTIssueManager(data, "ISMDashboardFacade/ViewTenantDetails");        }
        //
        public ISMDashboardDTO onclickdata(ISMDashboardDTO data)
        {
            return COMMM.POSTIssueManager(data, "ISMDashboardFacade/onclickdata/");
        }

        //Praveen
        public ISMDashboardDTO getemployeedetail(ISMDashboardDTO data)
        {
            return COMMM.POSTIssueManager(data, "ISMDashboardFacade/getemployeedetail/");
        }


        public ISMDashboardDTO StatewiseleadDetails(ISMDashboardDTO data)
        {
            return COMMM.POSTIssueManager(data, "ISMDashboardFacade/StatewiseleadDetails/");
        }
        public ISMDashboardDTO employeetasklist(ISMDashboardDTO data)
        {
            return COMMM.POSTIssueManager(data, "ISMDashboardFacade/employeetasklist/");
        }

        //dinner
        public ISMDashboardDTO orderedinner(ISMDashboardDTO data)
        {
            return COMMM.POSTIssueManager(data, "ISMDashboardFacade/orderedinner/");
        }
        public ISMDashboardDTO dinnerorderlist(ISMDashboardDTO data)
        {
            return COMMM.POSTIssueManager(data, "ISMDashboardFacade/dinnerorderlist/");
        }


    }
}
