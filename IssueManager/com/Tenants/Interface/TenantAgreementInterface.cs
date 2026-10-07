using PreadmissionDTOs.com.vaps.IssueManager.Tenants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IssueManager.com.Tenants.Interface
{
    public interface TenantAgreementInterface
    {
        //Master Type
        TenantAgreementDTO OnLoadRatingMasterType(TenantAgreementDTO data);
        TenantAgreementDTO SaveMasterType(TenantAgreementDTO data);
        TenantAgreementDTO EditMasterType(TenantAgreementDTO data);
        TenantAgreementDTO MastetTypeActiveDeactive(TenantAgreementDTO data);

        //Master Question
        TenantAgreementDTO OnClickMasterQuestions(TenantAgreementDTO data);
        TenantAgreementDTO SaveMasterQuestion(TenantAgreementDTO data);
        TenantAgreementDTO EditMasterQuestions(TenantAgreementDTO data);
        TenantAgreementDTO MasteQuestionActiveDeactive(TenantAgreementDTO data);
        TenantAgreementDTO UpdateMasterQuestionOrder(TenantAgreementDTO data);

        //Master Option
        TenantAgreementDTO OnClickMasterOption(TenantAgreementDTO data);
        TenantAgreementDTO SaveMasterOption(TenantAgreementDTO data);
        TenantAgreementDTO EditMasterOption(TenantAgreementDTO data);
        TenantAgreementDTO MasteOptionActiveDeactive(TenantAgreementDTO data);
        TenantAgreementDTO UpdateMasterOptionOrder(TenantAgreementDTO data);

        //Type Question Mapping
        TenantAgreementDTO OnClickTypeQuestionMapping(TenantAgreementDTO data);
        TenantAgreementDTO OnChangeMasteTypeQuesInsitution(TenantAgreementDTO data);
        TenantAgreementDTO PaymentRemainder(TenantAgreementDTO data);
        TenantAgreementDTO RenewalRemainder(TenantAgreementDTO data);
        TenantAgreementDTO OverDueRemainder(TenantAgreementDTO data);
        TenantAgreementDTO Editrenewal(TenantAgreementDTO data);
        TenantAgreementDTO Deleterenewal(TenantAgreementDTO data);
        TenantAgreementDTO tenantchange(TenantAgreementDTO data);
        TenantAgreementDTO saverenewal(TenantAgreementDTO data);
        TenantAgreementDTO delete(TenantAgreementDTO data);
        TenantAgreementDTO Edit(TenantAgreementDTO data);
        TenantAgreementDTO TenantDetails(TenantAgreementDTO data);
        TenantAgreementDTO ViewFiles(TenantAgreementDTO data);
        TenantAgreementDTO roomchange(TenantAgreementDTO data);
        TenantAgreementDTO buildingchange(TenantAgreementDTO data);
        TenantAgreementDTO Floorchange(TenantAgreementDTO data);
        TenantAgreementDTO CreateUser(TenantAgreementDTO data);
        TenantAgreementDTO getrefundableadvance(TenantAgreementDTO data);
        TenantAgreementDTO saverefundadvance(TenantAgreementDTO data);
    }
}
