using System;
using System.Collections.Generic;
using System.Linq;
using IssueManager.com.Tenants.Interface;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.IssueManager.Tenants;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IssueManager.com.Tenants.Facade
{
    [Route("api/[controller]")]
    public class TenantAgreementFacade :ControllerBase
    {
        public TenantAgreementInterface _interface;

        public TenantAgreementFacade(TenantAgreementInterface _interf)
        {
            _interface = _interf;
        }

        // GET: api/<controller>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        //Master Type
        [Route("OnLoadRatingMasterType")]
        public TenantAgreementDTO OnLoadRatingMasterType([FromBody] TenantAgreementDTO data)
        {
            return _interface.OnLoadRatingMasterType(data);
        }

        [Route("SaveMasterType")]
        public TenantAgreementDTO SaveMasterType([FromBody] TenantAgreementDTO data)
        {
            return _interface.SaveMasterType(data);
        }

        [Route("EditMasterType")]
        public TenantAgreementDTO EditMasterType([FromBody] TenantAgreementDTO data)
        {
            return _interface.EditMasterType(data);
        }

        [Route("MastetTypeActiveDeactive")]
        public TenantAgreementDTO MastetTypeActiveDeactive([FromBody] TenantAgreementDTO data)
        {
            return _interface.MastetTypeActiveDeactive(data);
        }

        //Master Question

        [Route("OnClickMasterQuestions")]
        public TenantAgreementDTO OnClickMasterQuestions([FromBody] TenantAgreementDTO data)
        {
            return _interface.OnClickMasterQuestions(data);
        }

        [Route("SaveMasterQuestion")]
        public TenantAgreementDTO SaveMasterQuestion([FromBody] TenantAgreementDTO data)
        {
            return _interface.SaveMasterQuestion(data);
        }

        [Route("EditMasterQuestions")]
        public TenantAgreementDTO EditMasterQuestions([FromBody] TenantAgreementDTO data)
        {
            return _interface.EditMasterQuestions(data);
        }

        [Route("MasteQuestionActiveDeactive")]
        public TenantAgreementDTO MasteQuestionActiveDeactive([FromBody] TenantAgreementDTO data)
        {
            return _interface.MasteQuestionActiveDeactive(data);
        }

        [Route("UpdateMasterQuestionOrder")]
        public TenantAgreementDTO UpdateMasterQuestionOrder([FromBody] TenantAgreementDTO data)
        {
            return _interface.UpdateMasterQuestionOrder(data);
        }

        //Master Option

        [Route("OnClickMasterOption")]
        public TenantAgreementDTO OnClickMasterOption([FromBody] TenantAgreementDTO data)
        {
            return _interface.OnClickMasterOption(data);
        }

        [Route("SaveMasterOption")]
        public TenantAgreementDTO SaveMasterOption([FromBody] TenantAgreementDTO data)
        {
            return _interface.SaveMasterOption(data);
        }

        [Route("EditMasterOption")]
        public TenantAgreementDTO EditMasterOption([FromBody] TenantAgreementDTO data)
        {
            return _interface.EditMasterOption(data);
        }

        [Route("MasteOptionActiveDeactive")]
        public TenantAgreementDTO MasteOptionActiveDeactive([FromBody] TenantAgreementDTO data)
        {
            return _interface.MasteOptionActiveDeactive(data);
        }

        [Route("UpdateMasterOptionOrder")]
        public TenantAgreementDTO UpdateMasterOptionOrder([FromBody] TenantAgreementDTO data)
        {
            return _interface.UpdateMasterOptionOrder(data);
        }

        //Type Question Mapping
        [Route("OnClickTypeQuestionMapping")]
        public TenantAgreementDTO OnClickTypeQuestionMapping([FromBody] TenantAgreementDTO data)
        {
            return _interface.OnClickTypeQuestionMapping(data);
        }

        [Route("OnChangeMasteTypeQuesInsitution")]
        public TenantAgreementDTO OnChangeMasteTypeQuesInsitution([FromBody] TenantAgreementDTO data)
        {
            return _interface.OnChangeMasteTypeQuesInsitution(data);
        }

        [Route("PaymentRemainder")]
        public TenantAgreementDTO PaymentRemainder([FromBody] TenantAgreementDTO data)
        {
            return _interface.PaymentRemainder(data);
        }

        [Route("RenewalRemainder")]
        public TenantAgreementDTO RenewalRemainder([FromBody] TenantAgreementDTO data)
        {
            return _interface.RenewalRemainder(data);
        }
         [Route("OverDueRemainder")]
        public TenantAgreementDTO OverDueRemainder([FromBody] TenantAgreementDTO data)
        {
            return _interface.OverDueRemainder(data);
        }

        [Route("Editrenewal")]
        public TenantAgreementDTO Editrenewal([FromBody] TenantAgreementDTO data)
        {
            return _interface.Editrenewal(data);
        }

        [Route("Deleterenewal")]
        public TenantAgreementDTO Deleterenewal([FromBody] TenantAgreementDTO data)
        {
            return _interface.Deleterenewal(data);
        }

        [Route("tenantchange")]
        public TenantAgreementDTO tenantchange([FromBody] TenantAgreementDTO data)
        {
            return _interface.tenantchange(data);
        }

        [Route("saverenewal")]
        public TenantAgreementDTO saverenewal([FromBody] TenantAgreementDTO data)
        {
            return _interface.saverenewal(data);
        }

        [Route("delete")]
        public TenantAgreementDTO DeleteViewTypeQuestionDept([FromBody] TenantAgreementDTO data)
        {
            return _interface.delete(data);
        }

        [Route("Edit")]
        public TenantAgreementDTO Edit([FromBody] TenantAgreementDTO data)
        {
            return _interface.Edit(data);
        }


        [Route("TenantDetails")]
        public TenantAgreementDTO TenantDetails([FromBody] TenantAgreementDTO data)
        {
            return _interface.TenantDetails(data);
        }

        [Route("ViewFiles")]
        public TenantAgreementDTO ViewFiles([FromBody] TenantAgreementDTO data)
        {
            return _interface.ViewFiles(data);
        }
        [Route("buildingchange")]
        public TenantAgreementDTO buildingchange([FromBody] TenantAgreementDTO data)
        {
            return _interface.buildingchange(data);
        }
        [Route("roomchange")]
        public TenantAgreementDTO roomchange([FromBody] TenantAgreementDTO data)
        {
            return _interface.roomchange(data);
        }
        [Route("Floorchange")]
        public TenantAgreementDTO Floorchange([FromBody] TenantAgreementDTO data)
        {
            return _interface.Floorchange(data);
        }
        [Route("CreateUser")]
        public TenantAgreementDTO CreateUser([FromBody] TenantAgreementDTO data)
        {
            return _interface.CreateUser(data);
        }
        [Route("getrefundableadvance")]
        public TenantAgreementDTO getrefundableadvance([FromBody] TenantAgreementDTO data)
        {
            return _interface.getrefundableadvance(data);
        }
        [Route("saverefundadvance")]
        public TenantAgreementDTO saverefundadvance([FromBody] TenantAgreementDTO data)
        {
            return _interface.saverefundadvance(data);
        }
    }
}
