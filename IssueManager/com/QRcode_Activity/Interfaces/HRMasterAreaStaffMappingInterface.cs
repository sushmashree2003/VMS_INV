using PreadmissionDTOs.com.vaps.IssueManager.QRcode_Activity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IssueManager.com.QRcode_Activity.Interfaces
{
  public  interface HRMasterAreaStaffMappingInterface
    {
        HRMasterAreaStaffMappingDTO getloaddata(HRMasterAreaStaffMappingDTO data);

        HRMasterAreaStaffMappingDTO getfloors(HRMasterAreaStaffMappingDTO data);

        HRMasterAreaStaffMappingDTO getAreaName(HRMasterAreaStaffMappingDTO data);

        HRMasterAreaStaffMappingDTO getActivityName(HRMasterAreaStaffMappingDTO data);

        HRMasterAreaStaffMappingDTO getEmployeeName(HRMasterAreaStaffMappingDTO data);

        HRMasterAreaStaffMappingDTO getUsername(HRMasterAreaStaffMappingDTO data);

        HRMasterAreaStaffMappingDTO AutoStaffMapping(HRMasterAreaStaffMappingDTO data);

        HRMasterAreaStaffMappingDTO savedetailsStaffMapping(HRMasterAreaStaffMappingDTO data);
  

        HRMasterAreaStaffMappingDTO deactive(HRMasterAreaStaffMappingDTO data);

        HRMasterAreaStaffMappingDTO edit(HRMasterAreaStaffMappingDTO data);

    }
}
