using CommonLibrary;
using IVRMUX.Controllers.com.vapstech.IssueManager;
using IVRMUX.Delegates.com.vapstech.IssueManager;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.IssueManager;

namespace IVRMUX.Delegates.com.vapstech.IssueManager
{
    public class VMS_ChequePrinting_SettingDelegate
    {
      CommonDelegate<VMS_ChequePrinting_SettingDTO, VMS_ChequePrinting_SettingDTO> comm = new CommonDelegate<VMS_ChequePrinting_SettingDTO, VMS_ChequePrinting_SettingDTO>();

        public VMS_ChequePrinting_SettingDTO getloaddata(VMS_ChequePrinting_SettingDTO data)
        {
            return comm.POSTIssueManager(data, "VMS_ChequePrinting_SettingFacade/getloaddata");
        }
        public VMS_ChequePrinting_SettingDTO savedetails(VMS_ChequePrinting_SettingDTO data)
        {
            return comm.POSTIssueManager(data, "VMS_ChequePrinting_SettingFacade/savedetails");
        }
        public VMS_ChequePrinting_SettingDTO deactive(VMS_ChequePrinting_SettingDTO data)
        {
            return comm.POSTIssueManager(data, "VMS_ChequePrinting_SettingFacade/deactive");
        }
        public VMS_ChequePrinting_SettingDTO companychange(VMS_ChequePrinting_SettingDTO data)
        {
            return comm.POSTIssueManager(data, "VMS_ChequePrinting_SettingFacade/companychange");
        }
    }
}
