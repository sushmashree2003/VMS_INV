using CommonLibrary;
using PreadmissionDTOs.com.vaps.Legal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.Legal
{
    public class ivrm_LegalProcessDelegate
    {
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<Legal_Notice_RequestDTO, Legal_Notice_RequestDTO> COMMM = new CommonDelegate<Legal_Notice_RequestDTO, Legal_Notice_RequestDTO>();

        public Legal_Notice_RequestDTO Onloaddata(Legal_Notice_RequestDTO dto)
        {
            return COMMM.POSTIssueManager(dto, "ivrm_LegalProcessFacade/Onloaddata");
        }

        public Legal_Notice_RequestDTO Onloadcourtdata(Legal_Notice_RequestDTO dto)
        {
            return COMMM.POSTIssueManager(dto, "ivrm_LegalProcessFacade/Onloadcourtdata");
        }

        public Legal_Notice_RequestDTO Onloadcasetype(Legal_Notice_RequestDTO dto)
        {
            return COMMM.POSTIssueManager(dto, "ivrm_LegalProcessFacade/Onloadcasetype");
        }

        public Legal_Notice_RequestDTO Onloadcasetypemapping(Legal_Notice_RequestDTO dto)
        {
            return COMMM.POSTIssueManager(dto, "ivrm_LegalProcessFacade/Onloadcasetypemapping");
        }

        public Legal_Notice_RequestDTO onclickloaddata1(Legal_Notice_RequestDTO dto)
        {
            return COMMM.POSTIssueManager(dto, "ivrm_LegalProcessFacade/onclickloaddata1");
        }
        public Legal_Notice_RequestDTO savedetail(Legal_Notice_RequestDTO dto)
        {
            return COMMM.POSTIssueManager(dto, "ivrm_LegalProcessFacade/savedetail/");
        }

        public Legal_Notice_RequestDTO legalcourtsave(Legal_Notice_RequestDTO dto)
        {
            return COMMM.POSTIssueManager(dto, "ivrm_LegalProcessFacade/legalcourtsave/");
        }

        public Legal_Notice_RequestDTO legalcasetypesave(Legal_Notice_RequestDTO dto)
        {
            return COMMM.POSTIssueManager(dto, "ivrm_LegalProcessFacade/legalcasetypesave/");
        }

        public Legal_Notice_RequestDTO Legalcasetypemappingsave(Legal_Notice_RequestDTO dto)
        {
            return COMMM.POSTIssueManager(dto, "ivrm_LegalProcessFacade/Legalcasetypemappingsave/");
        }

       

    }
}
