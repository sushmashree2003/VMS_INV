using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.IssueManager.Tenants;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using PreadmissionDTOs.com.vaps.IssueManager.Tenants;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.IssueManager.Tenants
{
    [Route("api/[controller]")]
    public class TenantAgreementController : Controller
    {
        TenantAgreementDelegate _delg = new TenantAgreementDelegate();
        // GET: api/<controller>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        //Master Type
        
        [Route("getdata/{id:int}")]
     
        public TenantAgreementDTO OnLoadRatingMasterType(int id)

        {
            TenantAgreementDTO data = new TenantAgreementDTO();

                data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
           
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delg.OnLoadRatingMasterType(data);
        }

        [Route("SaveMasterType")]
        public TenantAgreementDTO SaveMasterType([FromBody] TenantAgreementDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delg.SaveMasterType(data);
        }

        [Route("EditMasterType")]
        public TenantAgreementDTO EditMasterType([FromBody] TenantAgreementDTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delg.EditMasterType(data);
        }

        [Route("MastetTypeActiveDeactive")]
        public TenantAgreementDTO MastetTypeActiveDeactive([FromBody] TenantAgreementDTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delg.MastetTypeActiveDeactive(data);
        }

        //Master Question

        [Route("OnClickMasterQuestions")]
        public TenantAgreementDTO OnClickMasterQuestions([FromBody] TenantAgreementDTO data)
        {
          
                data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
          
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delg.OnClickMasterQuestions(data);
        }

        [Route("SaveMasterQuestion")]
        public TenantAgreementDTO SaveMasterQuestion([FromBody] TenantAgreementDTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delg.SaveMasterQuestion(data);
        }

        [Route("EditMasterQuestions")]
        public TenantAgreementDTO EditMasterQuestions([FromBody] TenantAgreementDTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delg.EditMasterQuestions(data);
        }

        [Route("MasteQuestionActiveDeactive")]
        public TenantAgreementDTO MasteQuestionActiveDeactive([FromBody] TenantAgreementDTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delg.MasteQuestionActiveDeactive(data);
        }

        [Route("UpdateMasterQuestionOrder")]
        public TenantAgreementDTO UpdateMasterQuestionOrder([FromBody] TenantAgreementDTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delg.UpdateMasterQuestionOrder(data);
        }

        //Master Option

        [Route("OnClickMasterOption")]
        public TenantAgreementDTO OnClickMasterOption([FromBody] TenantAgreementDTO data)
        {
        
                data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
          
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delg.OnClickMasterOption(data);
        }

        [Route("SaveMasterOption")]
        public TenantAgreementDTO SaveMasterOption([FromBody] TenantAgreementDTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delg.SaveMasterOption(data);
        }

        [Route("EditMasterOption")]
        public TenantAgreementDTO EditMasterOption([FromBody] TenantAgreementDTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delg.EditMasterOption(data);
        }

        [Route("MasteOptionActiveDeactive")]
        public TenantAgreementDTO MasteOptionActiveDeactive([FromBody] TenantAgreementDTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delg.MasteOptionActiveDeactive(data);
        }

        [Route("UpdateMasterOptionOrder")]
        public TenantAgreementDTO UpdateMasterOptionOrder([FromBody] TenantAgreementDTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delg.UpdateMasterOptionOrder(data);
        }

        //Type Question Mapping
        [Route("OnClickTypeQuestionMapping")]
        public TenantAgreementDTO OnClickTypeQuestionMapping([FromBody] TenantAgreementDTO data)
        {
            
                data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delg.OnClickTypeQuestionMapping(data);
        }

        [Route("OnChangeMasteTypeQuesInsitution")]
        public TenantAgreementDTO OnChangeMasteTypeQuesInsitution([FromBody] TenantAgreementDTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delg.OnChangeMasteTypeQuesInsitution(data);
        }

        [Route("OnChangeQuestion")]
        public TenantAgreementDTO OnChangeQuestion([FromBody] TenantAgreementDTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delg.OnChangeQuestion(data);
        }

      

        [Route("Editrenewal")]
        public TenantAgreementDTO Editrenewal([FromBody] TenantAgreementDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delg.Editrenewal(data);
        }

        [Route("deleterenewal")]
        public TenantAgreementDTO deleterenewal([FromBody] TenantAgreementDTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delg.deleterenewal(data);
        }

        [Route("CreateUser")]
        public TenantAgreementDTO CreateUser([FromBody] TenantAgreementDTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delg.CreateUser(data);
        }



        [Route("tenantchange")]
        public TenantAgreementDTO tenantchange([FromBody] TenantAgreementDTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delg.tenantchange(data);
        }

        [Route("saverenewal")]
        public TenantAgreementDTO saverenewal([FromBody] TenantAgreementDTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delg.saverenewal(data);
        }

        [Route("delete")]
        public TenantAgreementDTO delete([FromBody] TenantAgreementDTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delg.delete(data);
        }

        [Route("Edit")]
        public TenantAgreementDTO Edit([FromBody] TenantAgreementDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delg.Edit(data);
        }

        [Route("TenantDetails")]
        public TenantAgreementDTO TenantDetails([FromBody] TenantAgreementDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delg.TenantDetails(data);
        }

        [Route("ViewFiles")]
        public TenantAgreementDTO ViewFiles([FromBody] TenantAgreementDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delg.ViewFiles(data);
        }
        [Route("buildingchange")]
        public TenantAgreementDTO buildingchange([FromBody] TenantAgreementDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delg.buildingchange(data);
        }
        [Route("roomchange")]
        public TenantAgreementDTO roomchange([FromBody] TenantAgreementDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delg.roomchange(data);
        }
        [Route("Floorchange")]
        public TenantAgreementDTO Floorchange([FromBody] TenantAgreementDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delg.Floorchange(data);
        }
        [Route("getrefundableadvance")]
        public TenantAgreementDTO getrefundableadvance([FromBody] TenantAgreementDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delg.getrefundableadvance(data);
        }
        [Route("saverefundadvance")]
        public TenantAgreementDTO saverefundadvance([FromBody] TenantAgreementDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delg.saverefundadvance(data);
        }

    }
}
