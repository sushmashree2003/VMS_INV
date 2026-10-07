using CommonLibrary;
using PreadmissionDTOs.com.vaps.IssueManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.IssueManager
{
    public class LaptopApplicationDelegate
    {
        CommonDelegate<VMS_Laptop_Application_DTO, VMS_Laptop_Application_DTO> COMMM = new CommonDelegate<VMS_Laptop_Application_DTO, VMS_Laptop_Application_DTO>();
        public VMS_Laptop_Application_DTO loaddata(VMS_Laptop_Application_DTO data)
        {
            return COMMM.POSTIssueManager(data, "LaptopApplication/loaddata/");
        }
        public VMS_Laptop_Application_DTO save(VMS_Laptop_Application_DTO data)
        {
            return COMMM.POSTIssueManager(data, "LaptopApplication/save/");
        }
        public VMS_Laptop_Application_DTO edittab1(VMS_Laptop_Application_DTO data)
        {
            return COMMM.POSTIssueManager(data, "LaptopApplication/edittab1/");
        }
        public VMS_Laptop_Application_DTO getApprovalStatus(VMS_Laptop_Application_DTO data)
        {
            return COMMM.POSTIssueManager(data, "LaptopApplication/getApprovalStatus/");
        }
        public VMS_Laptop_Application_DTO get_status(VMS_Laptop_Application_DTO data)
        {
            return COMMM.POSTIssueManager(data, "LaptopApplication/get_status/");
        }
        public VMS_Laptop_Application_DTO reject_status(VMS_Laptop_Application_DTO data)
        {
            return COMMM.POSTIssueManager(data, "LaptopApplication/reject_status/");
        }
        public VMS_Laptop_Application_DTO getrpt(VMS_Laptop_Application_DTO data)
        {
            return COMMM.POSTIssueManager(data, "LaptopApplication/getrpt/");
        }
    }
}
