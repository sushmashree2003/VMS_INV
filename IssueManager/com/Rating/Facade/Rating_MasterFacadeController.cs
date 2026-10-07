using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IssueManager.com.Rating.Interfacce;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.IssueManager.Rating;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IssueManager.com.Rating.Facade
{
    [Route("api/[controller]")]
    public class Rating_MasterFacadeController : Controller
    {
        public Rating_MasterInterface _interface;

        public Rating_MasterFacadeController(Rating_MasterInterface _interf)
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
        public Rating_Master_DTO OnLoadRatingMasterType([FromBody] Rating_Master_DTO data)
        {
            return _interface.OnLoadRatingMasterType(data);
        }

        [Route("SaveMasterType")]
        public Rating_Master_DTO SaveMasterType([FromBody] Rating_Master_DTO data)
        {
            return _interface.SaveMasterType(data);
        }

        [Route("EditMasterType")]
        public Rating_Master_DTO EditMasterType([FromBody] Rating_Master_DTO data)
        {
            return _interface.EditMasterType(data);
        }

        [Route("MastetTypeActiveDeactive")]
        public Rating_Master_DTO MastetTypeActiveDeactive([FromBody] Rating_Master_DTO data)
        {
            return _interface.MastetTypeActiveDeactive(data);
        }

        // Master Classification 

        [Route("OnClickRatingClassification")]
        public Rating_Master_DTO OnClickRatingClassification([FromBody] Rating_Master_DTO data)
        {
            return _interface.OnClickRatingClassification(data);
        }

        [Route("SaveRatingClassification")]
        public Rating_Master_DTO SaveRatingClassification([FromBody] Rating_Master_DTO data)
        {
            return _interface.SaveRatingClassification(data);
        }

        [Route("MasterClassificationQuestionActiveDeactive")]
        public Rating_Master_DTO MasterClassificationQuestionActiveDeactive([FromBody] Rating_Master_DTO data)
        {
            return _interface.MasterClassificationQuestionActiveDeactive(data);
        }
        [Route("EditMasterQuestionClassification")]
        public Rating_Master_DTO EditMasterQuestionClassification([FromBody] Rating_Master_DTO data)
        {
            return _interface.EditMasterQuestionClassification(data);
        }

        // Master Sub-Classification 

        [Route("OnClickRatingSubClassification")]
        public Rating_Master_DTO OnClickRatingSubClassification([FromBody] Rating_Master_DTO data)
        {
            return _interface.OnClickRatingSubClassification(data);
        }
        [Route("SaveRatingsubClassification")]
        public Rating_Master_DTO SaveRatingsubClassification([FromBody] Rating_Master_DTO data)
        {
            return _interface.SaveRatingsubClassification(data);
        }
        [Route("MasterSubClassificationQuestionActiveDeactive")]
        public Rating_Master_DTO MasterSubClassificationQuestionActiveDeactive([FromBody] Rating_Master_DTO data)
        {
            return _interface.MasterSubClassificationQuestionActiveDeactive(data);
        }
        [Route("EditMasterQuestionSubClassification")]
        public Rating_Master_DTO EditMasterQuestionSubClassification([FromBody] Rating_Master_DTO data)
        {
            return _interface.EditMasterQuestionSubClassification(data);
        }
        //Master Question

        [Route("OnClickMasterQuestions")]
        public Rating_Master_DTO OnClickMasterQuestions([FromBody] Rating_Master_DTO data)
        {
            return _interface.OnClickMasterQuestions(data);
        }

        [Route("SaveMasterQuestion")]
        public Rating_Master_DTO SaveMasterQuestion([FromBody] Rating_Master_DTO data)
        {
            return _interface.SaveMasterQuestion(data);
        }

        [Route("EditMasterQuestions")]
        public Rating_Master_DTO EditMasterQuestions([FromBody] Rating_Master_DTO data)
        {
            return _interface.EditMasterQuestions(data);
        }

        [Route("MasteQuestionActiveDeactive")]
        public Rating_Master_DTO MasteQuestionActiveDeactive([FromBody] Rating_Master_DTO data)
        {
            return _interface.MasteQuestionActiveDeactive(data);
        }

        [Route("UpdateMasterQuestionOrder")]
        public Rating_Master_DTO UpdateMasterQuestionOrder([FromBody] Rating_Master_DTO data)
        {
            return _interface.UpdateMasterQuestionOrder(data);
        }

        //Master Option

        [Route("OnClickMasterOption")]
        public Rating_Master_DTO OnClickMasterOption([FromBody] Rating_Master_DTO data)
        {
            return _interface.OnClickMasterOption(data);
        }

        [Route("SaveMasterOption")]
        public Rating_Master_DTO SaveMasterOption([FromBody] Rating_Master_DTO data)
        {
            return _interface.SaveMasterOption(data);
        }

        [Route("EditMasterOption")]
        public Rating_Master_DTO EditMasterOption([FromBody] Rating_Master_DTO data)
        {
            return _interface.EditMasterOption(data);
        }

        [Route("MasteOptionActiveDeactive")]
        public Rating_Master_DTO MasteOptionActiveDeactive([FromBody] Rating_Master_DTO data)
        {
            return _interface.MasteOptionActiveDeactive(data);
        }

        [Route("UpdateMasterOptionOrder")]
        public Rating_Master_DTO UpdateMasterOptionOrder([FromBody] Rating_Master_DTO data)
        {
            return _interface.UpdateMasterOptionOrder(data);
        }
        //Rating Authority

        [Route("OnClickRatingAuthority")]
        public Rating_Master_DTO OnClickRatingAuthority([FromBody] Rating_Master_DTO data)
        {
            return _interface.OnClickRatingAuthority(data);
        }

        [Route("OnChangeRatingDepartment")]
        public Rating_Master_DTO OnChangeRatingDepartment([FromBody] Rating_Master_DTO data)
        {
            return _interface.OnChangeRatingDepartment(data);
        }

        [Route("SaveRatingAuthority")]
        public Rating_Master_DTO SaveRatingAuthority([FromBody] Rating_Master_DTO data)
        {
            return _interface.SaveRatingAuthority(data);
        }
        [Route("MasterAuthorityActiveDeactive")]
        public Rating_Master_DTO MasterAuthorityActiveDeactive([FromBody] Rating_Master_DTO data)
        {
            return _interface.MasterAuthorityActiveDeactive(data);
        }
        //Type Question Mapping
        [Route("OnClickTypeQuestionMapping")]
        public Rating_Master_DTO OnClickTypeQuestionMapping([FromBody] Rating_Master_DTO data)
        {
            return _interface.OnClickTypeQuestionMapping(data);
        }

        [Route("OnChangeMasteTypeQuesInsitution")]
        public Rating_Master_DTO OnChangeMasteTypeQuesInsitution([FromBody] Rating_Master_DTO data)
        {
            return _interface.OnChangeMasteTypeQuesInsitution(data);
        }

        [Route("OnChangeQuestion")]
        public Rating_Master_DTO OnChangeQuestion([FromBody] Rating_Master_DTO data)
        {
            return _interface.OnChangeQuestion(data);
        }

        [Route("OnChangeDepartment")]
        public Rating_Master_DTO OnChangeDepartment([FromBody] Rating_Master_DTO data)
        {
            return _interface.OnChangeDepartment(data);
        }

        [Route("SaveTypeQuestion")]
        public Rating_Master_DTO SaveTypeQuestion([FromBody] Rating_Master_DTO data)
        {
            return _interface.SaveTypeQuestion(data);
        }

        [Route("ViewTypeQuestionDeptMapping")]
        public Rating_Master_DTO ViewTypeQuestionDeptMapping([FromBody] Rating_Master_DTO data)
        {
            return _interface.ViewTypeQuestionDeptMapping(data);
        }

        [Route("ViewAuthoritydata")]
        public Rating_Master_DTO ViewAuthoritydata([FromBody] Rating_Master_DTO data)
        {
            return _interface.ViewAuthoritydata(data);
        }

        [Route("ViewTypeQuestionOptionMapping")]
        public Rating_Master_DTO ViewTypeQuestionOptionMapping([FromBody] Rating_Master_DTO data)
        {
            return _interface.ViewTypeQuestionOptionMapping(data);
        }

        [Route("MasteTypeQuesOptionActiveDeactive")]
        public Rating_Master_DTO MasteTypeQuesOptionActiveDeactive([FromBody] Rating_Master_DTO data)
        {
            return _interface.MasteTypeQuesOptionActiveDeactive(data);
        }

        [Route("DeleteViewTypeQuestionDept")]
        public Rating_Master_DTO DeleteViewTypeQuestionDept([FromBody] Rating_Master_DTO data)
        {
            return _interface.DeleteViewTypeQuestionDept(data);
        }

        [Route("DeleteViewTypeQuestionDeptDes")]
        public Rating_Master_DTO DeleteViewTypeQuestionDeptDes([FromBody] Rating_Master_DTO data)
        {
            return _interface.DeleteViewTypeQuestionDeptDes(data);
        }

        [Route("DeleteOptionRecord")]
        public Rating_Master_DTO DeleteOptionRecord([FromBody] Rating_Master_DTO data)
        {
            return _interface.DeleteOptionRecord(data);
        }

        [Route("MastetTypeQuestionActiveDeactive")]
        public Rating_Master_DTO MastetTypeQuestionActiveDeactive([FromBody] Rating_Master_DTO data)
        {
            return _interface.MastetTypeQuestionActiveDeactive(data);
        }

        //Akash
        //SaveOverAllRating
        [Route("SaveOverAllRating")]
        public Rating_Master_DTO SaveOverAllRating([FromBody] Rating_Master_DTO data)
        {
            return _interface.SaveOverAllRating(data);
        }
        //OverAllEdit
        [Route("OverAllEdit")]
        public Rating_Master_DTO OverAllEdit([FromBody] Rating_Master_DTO data)
        {
            return _interface.OverAllEdit(data);
        }
        //OverAllRatingActiveDeactive
        [Route("OverAllRatingActiveDeactive")]
        public Rating_Master_DTO OverAllRatingActiveDeactive([FromBody] Rating_Master_DTO data)
        {
            return _interface.OverAllRatingActiveDeactive(data);
        }
        //on change company loading department 
        [Route("OnChangeComapnyAuthoritys")]
        public Rating_Master_DTO OnChangeComapnyAuthoritys([FromBody] Rating_Master_DTO data)
        {
            return _interface.OnChangeComapnyAuthoritys(data);
        }
        //OnChangeDepartmentAuthority
        [Route("OnChangeDepartmentAuthority")]
        public Rating_Master_DTO OnChangeDepartmentAuthority([FromBody] Rating_Master_DTO data)
        {
            return _interface.OnChangeDepartmentAuthority(data);
        }
        //ViewTypeAuthorityData
        [Route("ViewTypeAuthorityData")]
        public Rating_Master_DTO ViewTypeAuthorityData([FromBody] Rating_Master_DTO data)
        {
            return _interface.ViewTypeAuthorityData(data);
        }

        //Rating type and question mapping
        [Route("SaveRatingQuestionMapping")]
        public Rating_Master_DTO SaveRatingQuestionMapping([FromBody] Rating_Master_DTO data)
        {
            return _interface.SaveRatingQuestionMapping(data);
        }

        [Route("OnClickRatingtypeQuestionMapping")]
        public Rating_Master_DTO OnClickRatingtypeQuestionMapping([FromBody] Rating_Master_DTO data)
        {
            return _interface.OnClickRatingtypeQuestionMapping(data);
        }


        [Route("OnChangeRatingQuestion_Rating")]
        public Rating_Master_DTO OnChangeRatingQuestion_Rating([FromBody] Rating_Master_DTO data)
        {
            return _interface.OnChangeRatingQuestion_Rating(data);
        }

        [Route("viewquestions")]
        public Rating_Master_DTO viewquestions([FromBody] Rating_Master_DTO data)
        {
            return _interface.viewquestions(data);
        }

        [Route("OnChangeRatingQuestionOption_Rating")]
        public Rating_Master_DTO OnChangeRatingQuestionOption_Rating([FromBody] Rating_Master_DTO data)
        {
            return _interface.OnChangeRatingQuestionOption_Rating(data);
        }


    }
}
