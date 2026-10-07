using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommonLibrary;
using PreadmissionDTOs.com.vaps.IssueManager.Rating;
using PreadmissionDTOs.com.vaps.IssueManager.Tenants;

namespace IVRMUX.Delegates.com.vapstech.IssueManager.Tenants
{
    public class TenantAgreementDelegate
    {
        CommonDelegate<TenantAgreementDTO, TenantAgreementDTO> _comm = new CommonDelegate<TenantAgreementDTO, TenantAgreementDTO>();

        //Master Type
        public TenantAgreementDTO OnLoadRatingMasterType(TenantAgreementDTO data)
        {
            return _comm.POSTIssueManager(data, "TenantAgreementFacade/OnLoadRatingMasterType");
        }
        public TenantAgreementDTO SaveMasterType(TenantAgreementDTO data)
        {
            return _comm.POSTIssueManager(data, "TenantAgreementFacade/SaveMasterType");
        }
        public TenantAgreementDTO EditMasterType(TenantAgreementDTO data)
        {
            return _comm.POSTIssueManager(data, "TenantAgreementFacade/EditMasterType");
        }
        public TenantAgreementDTO MastetTypeActiveDeactive(TenantAgreementDTO data)
        {
            return _comm.POSTIssueManager(data, "TenantAgreementFacade/MastetTypeActiveDeactive");
        }

        //Master Question
        public TenantAgreementDTO OnClickMasterQuestions(TenantAgreementDTO data)
        {
            return _comm.POSTIssueManager(data, "TenantAgreementFacade/OnClickMasterQuestions");
        }
        public TenantAgreementDTO SaveMasterQuestion(TenantAgreementDTO data)
        {
            return _comm.POSTIssueManager(data, "TenantAgreementFacade/SaveMasterQuestion");
        }
        public TenantAgreementDTO EditMasterQuestions(TenantAgreementDTO data)
        {
            return _comm.POSTIssueManager(data, "TenantAgreementFacade/EditMasterQuestions");
        }
        public TenantAgreementDTO MasteQuestionActiveDeactive(TenantAgreementDTO data)
        {
            return _comm.POSTIssueManager(data, "TenantAgreementFacade/MasteQuestionActiveDeactive");
        }
        public TenantAgreementDTO UpdateMasterQuestionOrder(TenantAgreementDTO data)
        {
            return _comm.POSTIssueManager(data, "TenantAgreementFacade/UpdateMasterQuestionOrder");
        }

        //Master Option
        public TenantAgreementDTO OnClickMasterOption(TenantAgreementDTO data)
        {
            return _comm.POSTIssueManager(data, "TenantAgreementFacade/OnClickMasterOption");
        }
        public TenantAgreementDTO SaveMasterOption(TenantAgreementDTO data)
        {
            return _comm.POSTIssueManager(data, "TenantAgreementFacade/SaveMasterOption");
        }
        public TenantAgreementDTO EditMasterOption(TenantAgreementDTO data)
        {
            return _comm.POSTIssueManager(data, "TenantAgreementFacade/EditMasterOption");
        }
        public TenantAgreementDTO MasteOptionActiveDeactive(TenantAgreementDTO data)
        {
            return _comm.POSTIssueManager(data, "TenantAgreementFacade/MasteOptionActiveDeactive");
        }
        public TenantAgreementDTO UpdateMasterOptionOrder(TenantAgreementDTO data)
        {
            return _comm.POSTIssueManager(data, "TenantAgreementFacade/UpdateMasterOptionOrder");
        }
        //Type Question Mapping
        public TenantAgreementDTO OnClickTypeQuestionMapping(TenantAgreementDTO data)
        {
            return _comm.POSTIssueManager(data, "TenantAgreementFacade/OnClickTypeQuestionMapping");
        }
        public TenantAgreementDTO OnChangeMasteTypeQuesInsitution(TenantAgreementDTO data)
        {
            return _comm.POSTIssueManager(data, "TenantAgreementFacade/OnChangeMasteTypeQuesInsitution");
        }
        public TenantAgreementDTO OnChangeQuestion(TenantAgreementDTO data)
        {
            return _comm.POSTIssueManager(data, "TenantAgreementFacade/OnChangeQuestion");
        }
        public TenantAgreementDTO OnChangeDepartment(TenantAgreementDTO data)
        {
            return _comm.POSTIssueManager(data, "TenantAgreementFacade/OnChangeDepartment");
        }
        public TenantAgreementDTO Editrenewal(TenantAgreementDTO data)
        {
            return _comm.POSTIssueManager(data, "TenantAgreementFacade/Editrenewal");
        }
        public TenantAgreementDTO deleterenewal(TenantAgreementDTO data)
        {
            return _comm.POSTIssueManager(data, "TenantAgreementFacade/deleterenewal");
        }
        public TenantAgreementDTO tenantchange(TenantAgreementDTO data)
        {
            return _comm.POSTIssueManager(data, "TenantAgreementFacade/tenantchange");
        }
        public TenantAgreementDTO saverenewal(TenantAgreementDTO data)
        {
            return _comm.POSTIssueManager(data, "TenantAgreementFacade/saverenewal");
        }
        public TenantAgreementDTO delete(TenantAgreementDTO data)
        {
            return _comm.POSTIssueManager(data, "TenantAgreementFacade/delete");
        }
        public TenantAgreementDTO Edit(TenantAgreementDTO data)
        {
            return _comm.POSTIssueManager(data, "TenantAgreementFacade/Edit");
        }

        public TenantAgreementDTO TenantDetails(TenantAgreementDTO data)
        {
            return _comm.POSTIssueManager(data, "TenantAgreementFacade/TenantDetails");
        }
        public TenantAgreementDTO ViewFiles(TenantAgreementDTO data)
        {
            return _comm.POSTIssueManager(data, "TenantAgreementFacade/ViewFiles");
        }
        public TenantAgreementDTO roomchange(TenantAgreementDTO data)
        {
            return _comm.POSTIssueManager(data, "TenantAgreementFacade/roomchange");
        }
        public TenantAgreementDTO buildingchange(TenantAgreementDTO data)
        {
            return _comm.POSTIssueManager(data, "TenantAgreementFacade/buildingchange");
        }
        public TenantAgreementDTO Floorchange(TenantAgreementDTO data)
        {
            return _comm.POSTIssueManager(data, "TenantAgreementFacade/Floorchange");
        }
        public TenantAgreementDTO CreateUser(TenantAgreementDTO data)
        {
            return _comm.POSTIssueManager(data, "TenantAgreementFacade/CreateUser");
        }
        public TenantAgreementDTO getrefundableadvance(TenantAgreementDTO data)
        {
            return _comm.POSTIssueManager(data, "TenantAgreementFacade/getrefundableadvance");
        }
        public TenantAgreementDTO saverefundadvance(TenantAgreementDTO data)
        {
            return _comm.POSTIssueManager(data, "TenantAgreementFacade/saverefundadvance");
        }
    }
}
