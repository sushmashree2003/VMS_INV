using PreadmissionDTOs.com.vaps.Legal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IssueManager.com.vaps.Interfaces
{
    public interface ivrm_LegalProcessInterface
    {
        Legal_Notice_RequestDTO Onloaddata(Legal_Notice_RequestDTO dto);

        Legal_Notice_RequestDTO Onloadcourtdata(Legal_Notice_RequestDTO dto);
        Legal_Notice_RequestDTO Onloadcasetype(Legal_Notice_RequestDTO dto);
        Legal_Notice_RequestDTO Onloadcasetypemapping(Legal_Notice_RequestDTO dto);
        //Legal_Notice_RequestDTO onclickloaddata1(Legal_Notice_RequestDTO dto);
        Legal_Notice_RequestDTO savedetail(Legal_Notice_RequestDTO data);

        Legal_Notice_RequestDTO legalcourtsave(Legal_Notice_RequestDTO data);
        Legal_Notice_RequestDTO legalcasetypesave(Legal_Notice_RequestDTO data);
        Legal_Notice_RequestDTO Legalcasetypemappingsave(Legal_Notice_RequestDTO data);
       
    }

}
