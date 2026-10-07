using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.com.vaps.IssueManager;
namespace IssueManager.com.vaps.Interfaces
{
    public interface EmailDepartmentMappingInterface 
    {
        EmailDepartmentMappingDTO getdetails(EmailDepartmentMappingDTO data);
        EmailDepartmentMappingDTO onchangedept(EmailDepartmentMappingDTO data);
        EmailDepartmentMappingDTO editdata(EmailDepartmentMappingDTO data);
        EmailDepartmentMappingDTO savedata(EmailDepartmentMappingDTO data);

        // Department Remarks Mapping 
        EmailDepartmentMappingDTO getloaddata(EmailDepartmentMappingDTO data);
        EmailDepartmentMappingDTO onchangeinstitution(EmailDepartmentMappingDTO data);
        EmailDepartmentMappingDTO onchangedepartment(EmailDepartmentMappingDTO data);
        EmailDepartmentMappingDTO saveremarksdata(EmailDepartmentMappingDTO data);
        EmailDepartmentMappingDTO Editremarksdata(EmailDepartmentMappingDTO data);
        EmailDepartmentMappingDTO deactivate(EmailDepartmentMappingDTO data);
    }
}
