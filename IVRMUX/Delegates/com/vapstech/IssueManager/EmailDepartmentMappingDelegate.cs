using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommonLibrary;
using PreadmissionDTOs.com.vaps.IssueManager;

namespace IVRMUX.Delegates.com.vapstech.IssueManager
{
    public class EmailDepartmentMappingDelegate
    {
        CommonDelegate<EmailDepartmentMappingDTO, EmailDepartmentMappingDTO> _comm = new CommonDelegate<EmailDepartmentMappingDTO, EmailDepartmentMappingDTO>();

        public EmailDepartmentMappingDTO getdetails(EmailDepartmentMappingDTO data)
        {
            return _comm.POSTIssueManager(data, "EmailDepartmentMappingFacade/getdetails");
        }
        public EmailDepartmentMappingDTO onchangedept(EmailDepartmentMappingDTO data)
        {
            return _comm.POSTIssueManager(data, "EmailDepartmentMappingFacade/onchangedept");
        }
        public EmailDepartmentMappingDTO editdata(EmailDepartmentMappingDTO data)
        {
            return _comm.POSTIssueManager(data, "EmailDepartmentMappingFacade/editdata");
        }
        public EmailDepartmentMappingDTO savedata(EmailDepartmentMappingDTO data)
        {
            return _comm.POSTIssueManager(data, "EmailDepartmentMappingFacade/savedata");
        }

        // Department Remarks Mapping 
        public EmailDepartmentMappingDTO getloaddata(EmailDepartmentMappingDTO data)
        {
            return _comm.POSTIssueManager(data, "EmailDepartmentMappingFacade/getloaddata");
        }
        public EmailDepartmentMappingDTO onchangeinstitution(EmailDepartmentMappingDTO data)
        {
            return _comm.POSTIssueManager(data, "EmailDepartmentMappingFacade/onchangeinstitution");
        }
        public EmailDepartmentMappingDTO onchangedepartment(EmailDepartmentMappingDTO data)
        {
            return _comm.POSTIssueManager(data, "EmailDepartmentMappingFacade/onchangedepartment");
        }
        public EmailDepartmentMappingDTO saveremarksdata(EmailDepartmentMappingDTO data)
        {
            return _comm.POSTIssueManager(data, "EmailDepartmentMappingFacade/saveremarksdata");
        }
        public EmailDepartmentMappingDTO Editremarksdata(EmailDepartmentMappingDTO data)
        {
            return _comm.POSTIssueManager(data, "EmailDepartmentMappingFacade/Editremarksdata");
        }
        public EmailDepartmentMappingDTO deactivate(EmailDepartmentMappingDTO data)
        {
            return _comm.POSTIssueManager(data, "EmailDepartmentMappingFacade/deactivate");
        }
    }
}
