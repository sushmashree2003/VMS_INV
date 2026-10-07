using PreadmissionDTOs.com.vaps.IssueManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IssueManager.com.vaps.Interfaces
{
    public interface VMS_ChequePrinting_SettingInterface
    {
        VMS_ChequePrinting_SettingDTO getloaddata(VMS_ChequePrinting_SettingDTO data);
        VMS_ChequePrinting_SettingDTO savedetails(VMS_ChequePrinting_SettingDTO data);
        VMS_ChequePrinting_SettingDTO deactive(VMS_ChequePrinting_SettingDTO data);
        VMS_ChequePrinting_SettingDTO companychange(VMS_ChequePrinting_SettingDTO data);
    }
}
