using CommonLibrary;
using PreadmissionDTOs.com.vaps.IssueManager.Tenants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.IssueManager.Tenants
{
    public class NocCreationDelegate
    {
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<NocCreationDTO, NocCreationDTO> COMINV = new CommonDelegate<NocCreationDTO, NocCreationDTO>();

        public NocCreationDTO getloaddata(NocCreationDTO data)
        {
            return COMINV.POSTIssueManager(data, "NocCreationFacade/getloaddata/");
        }

        public NocCreationDTO getfloors(NocCreationDTO data)
        {
            return COMINV.POSTIssueManager(data, "NocCreationFacade/getfloors/");
        }

        public NocCreationDTO getAreaName(NocCreationDTO data)
        {
            return COMINV.POSTIssueManager(data, "NocCreationFacade/getAreaName/");
        }

        public NocCreationDTO getActivityName(NocCreationDTO data)
        {
            return COMINV.POSTIssueManager(data, "NocCreationFacade/getActivityName/");
        }
        public NocCreationDTO gettenantname(NocCreationDTO data)
        {
            return COMINV.POSTIssueManager(data, "NocCreationFacade/gettenantname/");
        }


        public NocCreationDTO getEmployeeName(NocCreationDTO data)
        {
            return COMINV.POSTIssueManager(data, "NocCreationFacade/getEmployeeName/");
        }

        public NocCreationDTO getUsername(NocCreationDTO data)
        {
            return COMINV.POSTIssueManager(data, "NocCreationFacade/getUsername/");
        }

        public NocCreationDTO savedetailsStaffMapping(NocCreationDTO data)
        {
            return COMINV.POSTIssueManager(data, "NocCreationFacade/savedetailsStaffMapping/");
        }

        public NocCreationDTO deactive(NocCreationDTO data)
        {
            return COMINV.POSTIssueManager(data, "NocCreationFacade/deactive/");
        }
        //public NocCreationDTO edit(NocCreationDTO data)
        //{
        //    return COMINV.POSTIssueManager(data, "NocCreationFacade/edit/");
        //}


    }
}
