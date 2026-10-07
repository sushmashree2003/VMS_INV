using CommonLibrary;
using PreadmissionDTOs.com.vaps.IssueManager.QRcode_Activity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.IssueManager.QRcode_Activity
{
    public class HRMasterAreaStaffMappingDelegate
    {
        CommonDelegate<HRMasterAreaStaffMappingDTO, HRMasterAreaStaffMappingDTO> COMINV = new CommonDelegate<HRMasterAreaStaffMappingDTO, HRMasterAreaStaffMappingDTO>();

        public HRMasterAreaStaffMappingDTO getloaddata(HRMasterAreaStaffMappingDTO data)
        {
            return COMINV.POSTIssueManager(data, "HRMasterAreaStaffMappingFacade/getloaddata/");
        }

        public HRMasterAreaStaffMappingDTO getfloors(HRMasterAreaStaffMappingDTO data)
        {
            return COMINV.POSTIssueManager(data, "HRMasterAreaStaffMappingFacade/getfloors/");
        }

        public HRMasterAreaStaffMappingDTO getAreaName(HRMasterAreaStaffMappingDTO data)
        {
            return COMINV.POSTIssueManager(data, "HRMasterAreaStaffMappingFacade/getAreaName/");
        }

        public HRMasterAreaStaffMappingDTO getActivityName(HRMasterAreaStaffMappingDTO data)
        {
            return COMINV.POSTIssueManager(data, "HRMasterAreaStaffMappingFacade/getActivityName/");
        }

        public HRMasterAreaStaffMappingDTO getEmployeeName(HRMasterAreaStaffMappingDTO data)
        {
            return COMINV.POSTIssueManager(data, "HRMasterAreaStaffMappingFacade/getEmployeeName/");
        }

        public HRMasterAreaStaffMappingDTO getUsername(HRMasterAreaStaffMappingDTO data)
        {
            return COMINV.POSTIssueManager(data, "HRMasterAreaStaffMappingFacade/getUsername/");
        }

        public HRMasterAreaStaffMappingDTO savedetailsStaffMapping(HRMasterAreaStaffMappingDTO data)
        {
            return COMINV.POSTIssueManager(data, "HRMasterAreaStaffMappingFacade/savedetailsStaffMapping/");
        }

        public HRMasterAreaStaffMappingDTO deactive(HRMasterAreaStaffMappingDTO data)
        {
            return COMINV.POSTIssueManager(data, "HRMasterAreaStaffMappingFacade/deactive/");
        }
        public HRMasterAreaStaffMappingDTO edit(HRMasterAreaStaffMappingDTO data)
        {
            return COMINV.POSTIssueManager(data, "HRMasterAreaStaffMappingFacade/edit/");
        }


    }
}
