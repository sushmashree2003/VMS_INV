using CommonLibrary;
using PreadmissionDTOs.com.vaps.IssueManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.IssueManager
{
    public class INV_LocationDashboardDelegate
    {
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<INV_LocationDashboardDTO, INV_LocationDashboardDTO> COMMM = new CommonDelegate<INV_LocationDashboardDTO, INV_LocationDashboardDTO>();

        public INV_LocationDashboardDTO onfloordetails(INV_LocationDashboardDTO data)
        {
            return COMMM.POSTIssueManager(data, "INV_LocationDashboardFacade/onfloordetails/");
        }
        public INV_LocationDashboardDTO getemployeedetail(INV_LocationDashboardDTO data)
        {
            return COMMM.POSTIssueManager(data, "INV_LocationDashboardFacade/getemployeedetail/");
        }
      
        public INV_LocationDashboardDTO SaveDetails(INV_LocationDashboardDTO data)
        {
            return COMMM.POSTIssueManager(data, "INV_LocationDashboardFacade/SaveDetails");
        }
        //removelocationemployee
        public INV_LocationDashboardDTO removelocationemployee(INV_LocationDashboardDTO data)
        {
            return COMMM.POSTIssueManager(data, "INV_LocationDashboardFacade/removelocationemployee");
        }
        //OnChangeLocation
        public INV_LocationDashboardDTO OnChangeLocation(INV_LocationDashboardDTO data)
        {
            return COMMM.POSTIssueManager(data, "INV_LocationDashboardFacade/OnChangeLocation");
        }
        //suplier
        public INV_LocationDashboardDTO OnChange_Suplier(INV_LocationDashboardDTO data)
        {
            return COMMM.POSTIssueManager(data, "INV_LocationDashboardFacade/OnChange_Suplier");
        }
        public INV_LocationDashboardDTO Suplier_Details(INV_LocationDashboardDTO data)
        {
            return COMMM.POSTIssueManager(data, "INV_LocationDashboardFacade/Suplier_Details");
        }
        public INV_LocationDashboardDTO Details_supplier(INV_LocationDashboardDTO data)
        {
            return COMMM.POSTIssueManager(data, "INV_LocationDashboardFacade/Details_supplier");
        }

        //sushma common api
        public INV_LocationDashboardDTO getemployeedetailCommon(INV_LocationDashboardDTO data)
        {
            return COMMM.POSTIssueManager(data, "INV_LocationDashboardFacade/getemployeedetailCommon/");
        }

        public INV_LocationDashboardDTO SaveEmployeeAssets(INV_LocationDashboardDTO data)
        {
            return COMMM.POSTIssueManager(data, "INV_LocationDashboardFacade/SaveEmployeeAssets");
        }

        //Sanjai
        public INV_LocationDashboardDTO GetPendingAcknowledgementReport(INV_LocationDashboardDTO data)
        {
            return COMMM.POSTIssueManager(data, "INV_LocationDashboardFacade/GetPendingAcknowledgementReport/");
        }

        public INV_LocationDashboardDTO SavePendingAssetLocationTransfer(INV_LocationDashboardDTO data)
        {
            return COMMM.POSTIssueManager(data, "INV_LocationDashboardFacade/SavePendingAssetLocationTransfer/");
        }

    }
}
