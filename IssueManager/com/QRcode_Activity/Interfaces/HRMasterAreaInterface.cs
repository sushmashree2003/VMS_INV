using PreadmissionDTOs.com.vaps.IssueManager.QRcode_Activity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IssueManager.com.QRcode_Activity.Interfaces
{
  public  interface HRMasterAreaInterface
    {
        HRMasterAreaDTO getloaddata(HRMasterAreaDTO data);

        HRMasterAreaDTO getfloors(HRMasterAreaDTO data);

        HRMasterAreaDTO getAreaName(HRMasterAreaDTO data);

        HRMasterAreaDTO getActivityName(HRMasterAreaDTO data);

        HRMasterAreaDTO savedetailsAREA(HRMasterAreaDTO data);

        HRMasterAreaDTO savedetailsAREAActivity(HRMasterAreaDTO data);

        HRMasterAreaDTO savedetailsAREAActivityTime(HRMasterAreaDTO data);

        HRMasterAreaDTO deactiveArea(HRMasterAreaDTO data);

        HRMasterAreaDTO deactiveAreaActivity(HRMasterAreaDTO data);

        HRMasterAreaDTO deactiveAreaActivityTime(HRMasterAreaDTO data);
        HRMasterAreaDTO FloorQRCODE(HRMasterAreaDTO dto);
        HRMasterAreaDTO edit(HRMasterAreaDTO dto);
    }
}
