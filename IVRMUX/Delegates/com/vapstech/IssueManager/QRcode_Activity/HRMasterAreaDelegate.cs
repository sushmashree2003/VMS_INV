using CommonLibrary;
using PreadmissionDTOs.com.vaps.IssueManager.QRcode_Activity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.IssueManager.QRcode_Activity
{
    public class HRMasterAreaDelegate
    {
        CommonDelegate<HRMasterAreaDTO, HRMasterAreaDTO> COMINV = new CommonDelegate<HRMasterAreaDTO, HRMasterAreaDTO>();

        public HRMasterAreaDTO getloaddata(HRMasterAreaDTO data)
        {
            return COMINV.POSTIssueManager(data, "HRMasterAreaFacade/getloaddata/");
        }


        public HRMasterAreaDTO getfloors(HRMasterAreaDTO data)
        {
            return COMINV.POSTIssueManager(data, "HRMasterAreaFacade/getfloors/");
        }

        public HRMasterAreaDTO getAreaName(HRMasterAreaDTO data)
        {
            return COMINV.POSTIssueManager(data, "HRMasterAreaFacade/getAreaName/");
        }

        public HRMasterAreaDTO getActivityName(HRMasterAreaDTO data)
        {
            return COMINV.POSTIssueManager(data, "HRMasterAreaFacade/getActivityName/");
        }



        public HRMasterAreaDTO savedetailsAREA(HRMasterAreaDTO data)
        {
            return COMINV.POSTIssueManager(data, "HRMasterAreaFacade/savedetailsAREA/");
        }

        public HRMasterAreaDTO savedetailsAREAActivity(HRMasterAreaDTO data)
        {
            return COMINV.POSTIssueManager(data, "HRMasterAreaFacade/savedetailsAREAActivity/");
        }

        public HRMasterAreaDTO savedetailsAREAActivityTime(HRMasterAreaDTO data)
        {
            return COMINV.POSTIssueManager(data, "HRMasterAreaFacade/savedetailsAREAActivityTime/");
        }

        public HRMasterAreaDTO deactiveArea(HRMasterAreaDTO data)
        {
            return COMINV.POSTIssueManager(data, "HRMasterAreaFacade/deactiveArea/");
        }

        public HRMasterAreaDTO deactiveAreaActivity(HRMasterAreaDTO data)
        {
            return COMINV.POSTIssueManager(data, "HRMasterAreaFacade/deactiveAreaActivity/");
        }

        public HRMasterAreaDTO deactiveAreaActivityTime(HRMasterAreaDTO data)
        {
            return COMINV.POSTIssueManager(data, "HRMasterAreaFacade/deactiveAreaActivityTime/");
        }
        public HRMasterAreaDTO FloorQRCODE(HRMasterAreaDTO data)
        {
            return COMINV.POSTIssueManager(data, "HRMasterAreaFacade/FloorQRCODE/");
        }
        public HRMasterAreaDTO edit(HRMasterAreaDTO data)
        {
            return COMINV.POSTIssueManager(data, "HRMasterAreaFacade/edit/");
        }

    }
}
