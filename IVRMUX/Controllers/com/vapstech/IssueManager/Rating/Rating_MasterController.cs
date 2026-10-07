using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.IssueManager.Rating;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.IssueManager.Rating;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.IssueManager.Rating
{
    [Route("api/[controller]")]
    public class Rating_MasterController : Controller
    {
        Rating_MasterDelegate _delg = new Rating_MasterDelegate();
        // GET: api/<controller>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        //Master Type

        [Route("OnLoadRatingMasterType")]
        public Rating_Master_DTO OnLoadRatingMasterType([FromBody] Rating_Master_DTO data)
        {
            if (data.FlagOnLoadOrOnChangeInst == "OnLoad")
            {
                data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            }
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delg.OnLoadRatingMasterType(data);
        }

        [Route("SaveMasterType")]
        public Rating_Master_DTO SaveMasterType([FromBody] Rating_Master_DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delg.SaveMasterType(data);
        }

        [Route("EditMasterType")]
        public Rating_Master_DTO EditMasterType([FromBody] Rating_Master_DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delg.EditMasterType(data);
        }

        [Route("MastetTypeActiveDeactive")]
        public Rating_Master_DTO MastetTypeActiveDeactive([FromBody] Rating_Master_DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delg.MastetTypeActiveDeactive(data);
        }

        // Master Classification 
        [Route("OnClickRatingClassification")]
        public Rating_Master_DTO OnClickRatingClassification([FromBody] Rating_Master_DTO data)
        {
            if (data.FlagOnLoadOrOnChangeInst == "OnLoad")
            {
                data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            }
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delg.OnClickRatingClassification(data);
        }

        [Route("SaveRatingClassification")]
        public Rating_Master_DTO SaveRatingClassification([FromBody] Rating_Master_DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delg.SaveRatingClassification(data);
        }

        [Route("MasterClassificationQuestionActiveDeactive")]
        public Rating_Master_DTO MasterClassificationQuestionActiveDeactive([FromBody] Rating_Master_DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delg.MasterClassificationQuestionActiveDeactive(data);
        }
        [Route("EditMasterQuestionClassification")]
        public Rating_Master_DTO EditMasterQuestionClassification([FromBody] Rating_Master_DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delg.EditMasterQuestionClassification(data);
        }

        // Master SUb-Classification 

        [Route("OnClickRatingSubClassification")]
        public Rating_Master_DTO OnClickRatingSubClassification([FromBody] Rating_Master_DTO data)
        {
            if (data.FlagOnLoadOrOnChangeInst == "OnLoad")
            {
                data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            }
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delg.OnClickRatingSubClassification(data);
        }

        [Route("SaveRatingsubClassification")]
        public Rating_Master_DTO SaveRatingsubClassification([FromBody] Rating_Master_DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delg.SaveRatingsubClassification(data);
        }

        [Route("MasterSubClassificationQuestionActiveDeactive")]
        public Rating_Master_DTO MasterSubClassificationQuestionActiveDeactive([FromBody] Rating_Master_DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delg.MasterSubClassificationQuestionActiveDeactive(data);
        }

        [Route("EditMasterQuestionSubClassification")]
        public Rating_Master_DTO EditMasterQuestionSubClassification([FromBody] Rating_Master_DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delg.EditMasterQuestionSubClassification(data);
        }

        //Master Question

        [Route("OnClickMasterQuestions")]
        public Rating_Master_DTO OnClickMasterQuestions([FromBody] Rating_Master_DTO data)
        {
            if (data.FlagOnLoadOrOnChangeInst == "OnLoad")
            {
                data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            }
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delg.OnClickMasterQuestions(data);
        }

        [Route("SaveMasterQuestion")]
        public Rating_Master_DTO SaveMasterQuestion([FromBody] Rating_Master_DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delg.SaveMasterQuestion(data);
        }

        [Route("EditMasterQuestions")]
        public Rating_Master_DTO EditMasterQuestions([FromBody] Rating_Master_DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delg.EditMasterQuestions(data);
        }

        [Route("MasteQuestionActiveDeactive")]
        public Rating_Master_DTO MasteQuestionActiveDeactive([FromBody] Rating_Master_DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delg.MasteQuestionActiveDeactive(data);
        }

        [Route("UpdateMasterQuestionOrder")]
        public Rating_Master_DTO UpdateMasterQuestionOrder([FromBody] Rating_Master_DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delg.UpdateMasterQuestionOrder(data);
        }

        //Master Option

        [Route("OnClickMasterOption")]
        public Rating_Master_DTO OnClickMasterOption([FromBody] Rating_Master_DTO data)
        {
            if (data.FlagOnLoadOrOnChangeInst == "OnLoad")
            {
                data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            }
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delg.OnClickMasterOption(data);
        }

        [Route("SaveMasterOption")]
        public Rating_Master_DTO SaveMasterOption([FromBody] Rating_Master_DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delg.SaveMasterOption(data);
        }

        [Route("EditMasterOption")]
        public Rating_Master_DTO EditMasterOption([FromBody] Rating_Master_DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delg.EditMasterOption(data);
        }

        [Route("MasteOptionActiveDeactive")]
        public Rating_Master_DTO MasteOptionActiveDeactive([FromBody] Rating_Master_DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delg.MasteOptionActiveDeactive(data);
        }

        [Route("UpdateMasterOptionOrder")]
        public Rating_Master_DTO UpdateMasterOptionOrder([FromBody] Rating_Master_DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delg.UpdateMasterOptionOrder(data);
        }
        // Rating Authority
        [Route("OnClickRatingAuthority")]
        public Rating_Master_DTO OnClickRatingAuthority([FromBody] Rating_Master_DTO data)
        {
            if (data.FlagOnLoadOrOnChangeInst == "OnLoad")
            {
                data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            }
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delg.OnClickRatingAuthority(data);
        }

        [Route("OnChangeRatingDepartment")]
        public Rating_Master_DTO OnChangeRatingDepartment([FromBody] Rating_Master_DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delg.OnChangeRatingDepartment(data);
        }

        [Route("SaveRatingAuthority")]
        public Rating_Master_DTO SaveRatingAuthority([FromBody] Rating_Master_DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delg.SaveRatingAuthority(data);
        }

        [Route("MasterAuthorityActiveDeactive")]
        public Rating_Master_DTO MasterAuthorityActiveDeactive([FromBody] Rating_Master_DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delg.MasterAuthorityActiveDeactive(data);
        }

        //Type Question Mapping
        [Route("OnClickTypeQuestionMapping")]
        public Rating_Master_DTO OnClickTypeQuestionMapping([FromBody] Rating_Master_DTO data)
        {
            if (data.FlagOnLoadOrOnChangeInst == "OnLoad")
            {
                data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            }
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delg.OnClickTypeQuestionMapping(data);
        }

        [Route("OnChangeMasteTypeQuesInsitution")]
        public Rating_Master_DTO OnChangeMasteTypeQuesInsitution([FromBody] Rating_Master_DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delg.OnChangeMasteTypeQuesInsitution(data);
        }

        [Route("OnChangeQuestion")]
        public Rating_Master_DTO OnChangeQuestion([FromBody] Rating_Master_DTO data)
        {
            // data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delg.OnChangeQuestion(data);
        }

        [Route("OnChangeDepartment")]
        public Rating_Master_DTO OnChangeDepartment([FromBody] Rating_Master_DTO data)
        {
            // data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delg.OnChangeDepartment(data);
        }

        [Route("SaveTypeQuestion")]
        public Rating_Master_DTO SaveTypeQuestion([FromBody] Rating_Master_DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delg.SaveTypeQuestion(data);
        }

        [Route("ViewTypeQuestionDeptMapping")]
        public Rating_Master_DTO ViewTypeQuestionDeptMapping([FromBody] Rating_Master_DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delg.ViewTypeQuestionDeptMapping(data);
        }


        [Route("ViewAuthoritydata")]
        public Rating_Master_DTO ViewAuthoritydata([FromBody] Rating_Master_DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delg.ViewAuthoritydata(data);
        }

        [Route("ViewTypeQuestionOptionMapping")]
        public Rating_Master_DTO ViewTypeQuestionOptionMapping([FromBody] Rating_Master_DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delg.ViewTypeQuestionOptionMapping(data);
        }

        [Route("MasteTypeQuesOptionActiveDeactive")]
        public Rating_Master_DTO MasteTypeQuesOptionActiveDeactive([FromBody] Rating_Master_DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delg.MasteTypeQuesOptionActiveDeactive(data);
        }

        [Route("DeleteViewTypeQuestionDept")]
        public Rating_Master_DTO DeleteViewTypeQuestionDept([FromBody] Rating_Master_DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delg.DeleteViewTypeQuestionDept(data);
        }

        [Route("DeleteViewTypeQuestionDeptDes")]
        public Rating_Master_DTO DeleteViewTypeQuestionDeptDes([FromBody] Rating_Master_DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delg.DeleteViewTypeQuestionDeptDes(data);
        }

        [Route("DeleteOptionRecord")]
        public Rating_Master_DTO DeleteOptionRecord([FromBody] Rating_Master_DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delg.DeleteOptionRecord(data);
        }

        [Route("MastetTypeQuestionActiveDeactive")]
        public Rating_Master_DTO MastetTypeQuestionActiveDeactive([FromBody] Rating_Master_DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delg.MastetTypeQuestionActiveDeactive(data);
        }

        //Akash
        [Route("SaveOverAllRating")]
        public Rating_Master_DTO SaveOverAllRating([FromBody] Rating_Master_DTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delg.SaveOverAllRating(data);
        }

        //OverAllEdit
        [Route("OverAllEdit")]
        public Rating_Master_DTO OverAllEdit([FromBody] Rating_Master_DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delg.OverAllEdit(data);
        }

        //OverAllRatingActiveDeactive
        [Route("OverAllRatingActiveDeactive")]
        public Rating_Master_DTO OverAllRatingActiveDeactive([FromBody] Rating_Master_DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delg.OverAllRatingActiveDeactive(data);
        }
        //on change company loading department 
        [Route("OnChangeComapnyAuthoritys")]
        public Rating_Master_DTO OnChangeComapnyAuthoritys([FromBody] Rating_Master_DTO data)
        {
            // data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delg.OnChangeComapnyAuthoritys(data);
        }

        //OnChangeDepartmentAuthority
        [Route("OnChangeDepartmentAuthority")]
        public Rating_Master_DTO OnChangeDepartmentAuthority([FromBody] Rating_Master_DTO data)
        {
            // data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delg.OnChangeDepartmentAuthority(data);
        }
        //ViewTypeAuthorityData
        [Route("ViewTypeAuthorityData")]
        public Rating_Master_DTO ViewTypeAuthorityData([FromBody] Rating_Master_DTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delg.ViewTypeAuthorityData(data);
        }


        //Rating type and question maping

        [Route("SaveRatingQuestionMapping")]
        public Rating_Master_DTO SaveRatingQuestionMapping([FromBody] Rating_Master_DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delg.SaveRatingQuestionMapping(data);
        }

        [Route("OnClickRatingtypeQuestionMapping")]
        public Rating_Master_DTO OnClickRatingtypeQuestionMapping([FromBody] Rating_Master_DTO data)
        {
            if (data.FlagOnLoadOrOnChangeInst == "OnLoad")
            {
                data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            }
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delg.OnClickRatingtypeQuestionMapping(data);
        }
        [Route("OnChangeRatingQuestion_Rating")]
        public Rating_Master_DTO OnChangeRatingQuestion_Rating([FromBody] Rating_Master_DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delg.OnChangeRatingQuestion_Rating(data);
        }

        [Route("viewquestions")]
        public Rating_Master_DTO viewquestions([FromBody] Rating_Master_DTO data)
        {

            // data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delg.viewquestions(data);
        }
        // 4th tab

        [Route("OnChangeRatingQuestionOption_Rating")]
        public Rating_Master_DTO OnChangeRatingQuestionOption_Rating([FromBody] Rating_Master_DTO data)
        {

            //data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            // data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delg.OnChangeRatingQuestionOption_Rating(data);
        }


    }
}
