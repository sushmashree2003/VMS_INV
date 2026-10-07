using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommonLibrary;
using PreadmissionDTOs.com.vaps.IssueManager.Rating;

namespace IVRMUX.Delegates.com.vapstech.IssueManager.Rating
{
    public class Rating_MasterDelegate
    {
        CommonDelegate<Rating_Master_DTO, Rating_Master_DTO> _comm = new CommonDelegate<Rating_Master_DTO, Rating_Master_DTO>();

        //Master Type
        public Rating_Master_DTO OnLoadRatingMasterType(Rating_Master_DTO data)
        {
            return _comm.POSTIssueManager(data, "Rating_MasterFacade/OnLoadRatingMasterType");
        }
        public Rating_Master_DTO SaveMasterType(Rating_Master_DTO data)
        {
            return _comm.POSTIssueManager(data, "Rating_MasterFacade/SaveMasterType");
        }
        public Rating_Master_DTO EditMasterType(Rating_Master_DTO data)
        {
            return _comm.POSTIssueManager(data, "Rating_MasterFacade/EditMasterType");
        }
        public Rating_Master_DTO MastetTypeActiveDeactive(Rating_Master_DTO data)
        {
            return _comm.POSTIssueManager(data, "Rating_MasterFacade/MastetTypeActiveDeactive");
        }

        //Master Classification
        public Rating_Master_DTO OnClickRatingClassification(Rating_Master_DTO data)
        {
            return _comm.POSTIssueManager(data, "Rating_MasterFacade/OnClickRatingClassification");
        }
        public Rating_Master_DTO SaveRatingClassification(Rating_Master_DTO data)
        {
            return _comm.POSTIssueManager(data, "Rating_MasterFacade/SaveRatingClassification");
        }
        public Rating_Master_DTO MasterClassificationQuestionActiveDeactive(Rating_Master_DTO data)
        {
            return _comm.POSTIssueManager(data, "Rating_MasterFacade/MasterClassificationQuestionActiveDeactive");
        }
        public Rating_Master_DTO EditMasterQuestionClassification(Rating_Master_DTO data)
        {
            return _comm.POSTIssueManager(data, "Rating_MasterFacade/EditMasterQuestionClassification");
        }
        //Master Sub-Classification
        public Rating_Master_DTO OnClickRatingSubClassification(Rating_Master_DTO data)
        {
            return _comm.POSTIssueManager(data, "Rating_MasterFacade/OnClickRatingSubClassification");
        }
        public Rating_Master_DTO SaveRatingsubClassification(Rating_Master_DTO data)
        {
            return _comm.POSTIssueManager(data, "Rating_MasterFacade/SaveRatingsubClassification");
        }
        public Rating_Master_DTO MasterSubClassificationQuestionActiveDeactive(Rating_Master_DTO data)
        {
            return _comm.POSTIssueManager(data, "Rating_MasterFacade/MasterSubClassificationQuestionActiveDeactive");
        }
        public Rating_Master_DTO EditMasterQuestionSubClassification(Rating_Master_DTO data)
        {
            return _comm.POSTIssueManager(data, "Rating_MasterFacade/EditMasterQuestionSubClassification");
        }
        //Master Question
        public Rating_Master_DTO OnClickMasterQuestions(Rating_Master_DTO data)
        {
            return _comm.POSTIssueManager(data, "Rating_MasterFacade/OnClickMasterQuestions");
        }
        public Rating_Master_DTO SaveMasterQuestion(Rating_Master_DTO data)
        {
            return _comm.POSTIssueManager(data, "Rating_MasterFacade/SaveMasterQuestion");
        }
        public Rating_Master_DTO EditMasterQuestions(Rating_Master_DTO data)
        {
            return _comm.POSTIssueManager(data, "Rating_MasterFacade/EditMasterQuestions");
        }
        public Rating_Master_DTO MasteQuestionActiveDeactive(Rating_Master_DTO data)
        {
            return _comm.POSTIssueManager(data, "Rating_MasterFacade/MasteQuestionActiveDeactive");
        }
        public Rating_Master_DTO UpdateMasterQuestionOrder(Rating_Master_DTO data)
        {
            return _comm.POSTIssueManager(data, "Rating_MasterFacade/UpdateMasterQuestionOrder");
        }

        //Master Option

        public Rating_Master_DTO OnClickMasterOption(Rating_Master_DTO data)
        {
            return _comm.POSTIssueManager(data, "Rating_MasterFacade/OnClickMasterOption");
        }
        public Rating_Master_DTO SaveMasterOption(Rating_Master_DTO data)
        {
            return _comm.POSTIssueManager(data, "Rating_MasterFacade/SaveMasterOption");
        }
        public Rating_Master_DTO EditMasterOption(Rating_Master_DTO data)
        {
            return _comm.POSTIssueManager(data, "Rating_MasterFacade/EditMasterOption");
        }
        public Rating_Master_DTO MasteOptionActiveDeactive(Rating_Master_DTO data)
        {
            return _comm.POSTIssueManager(data, "Rating_MasterFacade/MasteOptionActiveDeactive");
        }
        public Rating_Master_DTO UpdateMasterOptionOrder(Rating_Master_DTO data)
        {
            return _comm.POSTIssueManager(data, "Rating_MasterFacade/UpdateMasterOptionOrder");
        }
        // Rating Authority
        public Rating_Master_DTO OnClickRatingAuthority(Rating_Master_DTO data)
        {
            return _comm.POSTIssueManager(data, "Rating_MasterFacade/OnClickRatingAuthority");
        }
        public Rating_Master_DTO OnChangeRatingDepartment(Rating_Master_DTO data)
        {
            return _comm.POSTIssueManager(data, "Rating_MasterFacade/OnChangeRatingDepartment");
        }
        public Rating_Master_DTO SaveRatingAuthority(Rating_Master_DTO data)
        {
            return _comm.POSTIssueManager(data, "Rating_MasterFacade/SaveRatingAuthority");
        }
        public Rating_Master_DTO MasterAuthorityActiveDeactive(Rating_Master_DTO data)
        {
            return _comm.POSTIssueManager(data, "Rating_MasterFacade/MasterAuthorityActiveDeactive");
        }

        //Type Question Mapping
        public Rating_Master_DTO OnClickTypeQuestionMapping(Rating_Master_DTO data)
        {
            return _comm.POSTIssueManager(data, "Rating_MasterFacade/OnClickTypeQuestionMapping");
        }
        public Rating_Master_DTO OnChangeMasteTypeQuesInsitution(Rating_Master_DTO data)
        {
            return _comm.POSTIssueManager(data, "Rating_MasterFacade/OnChangeMasteTypeQuesInsitution");
        }
        public Rating_Master_DTO OnChangeQuestion(Rating_Master_DTO data)
        {
            return _comm.POSTIssueManager(data, "Rating_MasterFacade/OnChangeQuestion");
        }
        public Rating_Master_DTO OnChangeDepartment(Rating_Master_DTO data)
        {
            return _comm.POSTIssueManager(data, "Rating_MasterFacade/OnChangeDepartment");
        }
        public Rating_Master_DTO SaveTypeQuestion(Rating_Master_DTO data)
        {
            return _comm.POSTIssueManager(data, "Rating_MasterFacade/SaveTypeQuestion");
        }
        public Rating_Master_DTO ViewTypeQuestionDeptMapping(Rating_Master_DTO data)
        {
            return _comm.POSTIssueManager(data, "Rating_MasterFacade/ViewTypeQuestionDeptMapping");
        }
        public Rating_Master_DTO ViewAuthoritydata(Rating_Master_DTO data)
        {
            return _comm.POSTIssueManager(data, "Rating_MasterFacade/ViewAuthoritydata");
        }
        public Rating_Master_DTO ViewTypeQuestionOptionMapping(Rating_Master_DTO data)
        {
            return _comm.POSTIssueManager(data, "Rating_MasterFacade/ViewTypeQuestionOptionMapping");
        }
        public Rating_Master_DTO MasteTypeQuesOptionActiveDeactive(Rating_Master_DTO data)
        {
            return _comm.POSTIssueManager(data, "Rating_MasterFacade/MasteTypeQuesOptionActiveDeactive");
        }
        public Rating_Master_DTO DeleteViewTypeQuestionDept(Rating_Master_DTO data)
        {
            return _comm.POSTIssueManager(data, "Rating_MasterFacade/DeleteViewTypeQuestionDept");
        }
        public Rating_Master_DTO DeleteViewTypeQuestionDeptDes(Rating_Master_DTO data)
        {
            return _comm.POSTIssueManager(data, "Rating_MasterFacade/DeleteViewTypeQuestionDeptDes");
        }
        public Rating_Master_DTO DeleteOptionRecord(Rating_Master_DTO data)
        {
            return _comm.POSTIssueManager(data, "Rating_MasterFacade/DeleteOptionRecord");
        }

        public Rating_Master_DTO MastetTypeQuestionActiveDeactive(Rating_Master_DTO data)
        {
            return _comm.POSTIssueManager(data, "Rating_MasterFacade/MastetTypeQuestionActiveDeactive");
        }

        //Akash
        //SaveOverAllRating
        public Rating_Master_DTO SaveOverAllRating(Rating_Master_DTO data)
        {
            return _comm.POSTIssueManager(data, "Rating_MasterFacade/SaveOverAllRating");
        }
        //OverAllEdit
        public Rating_Master_DTO OverAllEdit(Rating_Master_DTO data)
        {
            return _comm.POSTIssueManager(data, "Rating_MasterFacade/OverAllEdit");
        }
        //OverAllRatingActiveDeactive
        public Rating_Master_DTO OverAllRatingActiveDeactive(Rating_Master_DTO data)
        {
            return _comm.POSTIssueManager(data, "Rating_MasterFacade/OverAllRatingActiveDeactive");
        }
        //on change company loading department 
        public Rating_Master_DTO OnChangeComapnyAuthoritys(Rating_Master_DTO data)
        {
            return _comm.POSTIssueManager(data, "Rating_MasterFacade/OnChangeComapnyAuthoritys");
        }
        //OnChangeDepartmentAuthority
        public Rating_Master_DTO OnChangeDepartmentAuthority(Rating_Master_DTO data)
        {
            return _comm.POSTIssueManager(data, "Rating_MasterFacade/OnChangeDepartmentAuthority");
        }

        //ViewTypeAuthorityData
        public Rating_Master_DTO ViewTypeAuthorityData(Rating_Master_DTO data)
        {
            return _comm.POSTIssueManager(data, "Rating_MasterFacade/ViewTypeAuthorityData");
        }

        //Rating type and question mapping

        public Rating_Master_DTO SaveRatingQuestionMapping(Rating_Master_DTO data)
        {
            return _comm.POSTIssueManager(data, "Rating_MasterFacade/SaveRatingQuestionMapping");
        }

        public Rating_Master_DTO OnClickRatingtypeQuestionMapping(Rating_Master_DTO data)
        {
            return _comm.POSTIssueManager(data, "Rating_MasterFacade/OnClickRatingtypeQuestionMapping");
        }
        public Rating_Master_DTO OnChangeRatingQuestion_Rating(Rating_Master_DTO data)
        {
            return _comm.POSTIssueManager(data, "Rating_MasterFacade/OnChangeRatingQuestion_Rating");
        }

        public Rating_Master_DTO viewquestions(Rating_Master_DTO data)
        {
            return _comm.POSTIssueManager(data, "Rating_MasterFacade/viewquestions");
        }



        public Rating_Master_DTO OnChangeRatingQuestionOption_Rating(Rating_Master_DTO data)
        {
            return _comm.POSTIssueManager(data, "Rating_MasterFacade/OnChangeRatingQuestionOption_Rating");
        }

    }
}
