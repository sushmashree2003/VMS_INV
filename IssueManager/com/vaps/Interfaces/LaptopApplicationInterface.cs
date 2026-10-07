using PreadmissionDTOs.com.vaps.IssueManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace IssueManager.com.vaps.Interfaces
{
    public interface LaptopApplicationInterface
    {
        VMS_Laptop_Application_DTO getdetails(VMS_Laptop_Application_DTO data);
        VMS_Laptop_Application_DTO loaddata(VMS_Laptop_Application_DTO data);
        VMS_Laptop_Application_DTO save(VMS_Laptop_Application_DTO data);
        VMS_Laptop_Application_DTO edittab1(VMS_Laptop_Application_DTO data);
        Task<VMS_Laptop_Application_DTO> getApprovalStatusAsync(VMS_Laptop_Application_DTO data);
        VMS_Laptop_Application_DTO get_status(VMS_Laptop_Application_DTO data);
        VMS_Laptop_Application_DTO reject_status(VMS_Laptop_Application_DTO data);
        VMS_Laptop_Application_DTO getrpt(VMS_Laptop_Application_DTO data);
    }
}
