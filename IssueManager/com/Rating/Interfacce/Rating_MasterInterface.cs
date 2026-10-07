using PreadmissionDTOs.com.vaps.IssueManager.Rating;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IssueManager.com.Rating.Interfacce
{
    public interface Rating_MasterInterface
    {
        //Master Type
        Rating_Master_DTO OnLoadRatingMasterType(Rating_Master_DTO data);
        Rating_Master_DTO SaveMasterType(Rating_Master_DTO data);
        Rating_Master_DTO EditMasterType(Rating_Master_DTO data);
        Rating_Master_DTO MastetTypeActiveDeactive(Rating_Master_DTO data);

        // Mater Classification
        Rating_Master_DTO OnClickRatingClassification(Rating_Master_DTO data);
        Rating_Master_DTO SaveRatingClassification(Rating_Master_DTO data);
        Rating_Master_DTO MasterClassificationQuestionActiveDeactive(Rating_Master_DTO data);
        Rating_Master_DTO EditMasterQuestionClassification(Rating_Master_DTO data);

        // Mater Sub Classification
        Rating_Master_DTO OnClickRatingSubClassification(Rating_Master_DTO data);
        Rating_Master_DTO SaveRatingsubClassification(Rating_Master_DTO data);
        Rating_Master_DTO MasterSubClassificationQuestionActiveDeactive(Rating_Master_DTO data);
        Rating_Master_DTO EditMasterQuestionSubClassification(Rating_Master_DTO data);

        //Master Question
        Rating_Master_DTO OnClickMasterQuestions(Rating_Master_DTO data);
        Rating_Master_DTO SaveMasterQuestion(Rating_Master_DTO data);
        Rating_Master_DTO EditMasterQuestions(Rating_Master_DTO data);
        Rating_Master_DTO MasteQuestionActiveDeactive(Rating_Master_DTO data);
        Rating_Master_DTO UpdateMasterQuestionOrder(Rating_Master_DTO data);

        //Master Option
        Rating_Master_DTO OnClickMasterOption(Rating_Master_DTO data);
        Rating_Master_DTO SaveMasterOption(Rating_Master_DTO data);
        Rating_Master_DTO EditMasterOption(Rating_Master_DTO data);
        Rating_Master_DTO MasteOptionActiveDeactive(Rating_Master_DTO data);
        Rating_Master_DTO UpdateMasterOptionOrder(Rating_Master_DTO data);

        // Rating Authority
        Rating_Master_DTO OnClickRatingAuthority(Rating_Master_DTO data);
        Rating_Master_DTO OnChangeRatingDepartment(Rating_Master_DTO data);
        Rating_Master_DTO SaveRatingAuthority(Rating_Master_DTO data);
        Rating_Master_DTO MasterAuthorityActiveDeactive(Rating_Master_DTO data);

        //Type Question Mapping
        Rating_Master_DTO OnClickTypeQuestionMapping(Rating_Master_DTO data);
        Rating_Master_DTO OnChangeMasteTypeQuesInsitution(Rating_Master_DTO data);
        Rating_Master_DTO OnChangeQuestion(Rating_Master_DTO data);
        Rating_Master_DTO OnChangeDepartment(Rating_Master_DTO data);
        Rating_Master_DTO SaveTypeQuestion(Rating_Master_DTO data);
        Rating_Master_DTO ViewTypeQuestionDeptMapping(Rating_Master_DTO data);
        Rating_Master_DTO ViewAuthoritydata(Rating_Master_DTO data);
        Rating_Master_DTO ViewTypeQuestionOptionMapping(Rating_Master_DTO data);
        Rating_Master_DTO MasteTypeQuesOptionActiveDeactive(Rating_Master_DTO data);
        Rating_Master_DTO DeleteViewTypeQuestionDept(Rating_Master_DTO data);
        Rating_Master_DTO DeleteViewTypeQuestionDeptDes(Rating_Master_DTO data);
        Rating_Master_DTO DeleteOptionRecord(Rating_Master_DTO data);
        Rating_Master_DTO MastetTypeQuestionActiveDeactive(Rating_Master_DTO data);

        Rating_Master_DTO SaveOverAllRating(Rating_Master_DTO data);
        Rating_Master_DTO OverAllEdit(Rating_Master_DTO data);
        Rating_Master_DTO OverAllRatingActiveDeactive(Rating_Master_DTO data);
        Rating_Master_DTO OnChangeComapnyAuthoritys(Rating_Master_DTO data);
        Rating_Master_DTO OnChangeDepartmentAuthority(Rating_Master_DTO data);
        Rating_Master_DTO ViewTypeAuthorityData(Rating_Master_DTO data);
        Rating_Master_DTO SaveRatingQuestionMapping(Rating_Master_DTO data);
        Rating_Master_DTO OnClickRatingtypeQuestionMapping(Rating_Master_DTO data);
        Rating_Master_DTO OnChangeRatingQuestion_Rating(Rating_Master_DTO data);
        Rating_Master_DTO viewquestions(Rating_Master_DTO data);

        Rating_Master_DTO OnChangeRatingQuestionOption_Rating(Rating_Master_DTO data);






    }
}
