using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommonLibrary;
using PreadmissionDTOs.com.vaps.IssueManager.Rating;

namespace IVRMUX.Delegates.com.vapstech.IssueManager.Rating
{
    public class Rating_TransactionDelegate
    {
        CommonDelegate<Rating_TransactionDTO, Rating_TransactionDTO> _comm = new CommonDelegate<Rating_TransactionDTO, Rating_TransactionDTO>();

        public Rating_TransactionDTO OnLoadRatingTransaction(Rating_TransactionDTO data)
        {
            return _comm.POSTIssueManager(data, "Rating_TransactionFacade/OnLoadRatingTransaction");
        }
        public Rating_TransactionDTO GetSearchDetails(Rating_TransactionDTO data)
        {
            return _comm.POSTIssueManager(data, "Rating_TransactionFacade/GetSearchDetails");
        }
        public Rating_TransactionDTO OnChangeEmployee(Rating_TransactionDTO data)
        {
            return _comm.POSTIssueManager(data, "Rating_TransactionFacade/OnChangeEmployee");
        }
        public Rating_TransactionDTO SaveRating(Rating_TransactionDTO data)
        {
            return _comm.POSTIssueManager(data, "Rating_TransactionFacade/SaveRating");
        }


        //consolidated_report

        public Rating_TransactionDTO get_Consolidated_details(Rating_TransactionDTO data)
        {
            return _comm.POSTIssueManager(data, "Rating_TransactionFacade/get_Consolidated_details/");
        }

        public Rating_TransactionDTO getconsolidatedreport(Rating_TransactionDTO data)
        {
            return _comm.POSTIssueManager(data, "Rating_TransactionFacade/getconsolidatedreport");
        }
        public Rating_TransactionDTO getemployeereport(Rating_TransactionDTO data)
        {
            return _comm.POSTIssueManager(data, "Rating_TransactionFacade/getemployeereport");
        }


        //Akash
        //SaveOverAllRating
        public Rating_TransactionDTO SaveOverAllRating(Rating_TransactionDTO data)
        {
            return _comm.POSTIssueManager(data, "Rating_TransactionFacade/SaveOverAllRating");
        }
        //OverAllEdit
        public Rating_TransactionDTO OverAllEdit(Rating_TransactionDTO data)
        {
            return _comm.POSTIssueManager(data, "Rating_TransactionFacade/OverAllEdit");
        }
        //OverAllRatingActiveDeactive
        public Rating_TransactionDTO OverAllRatingActiveDeactive(Rating_TransactionDTO data)
        {
            return _comm.POSTIssueManager(data, "Rating_TransactionFacade/OverAllRatingActiveDeactive");
        }
        //getQuestions
        public Rating_TransactionDTO getQuestions(Rating_TransactionDTO data)
        {
            return _comm.POSTIssueManager(data, "Rating_TransactionFacade/getQuestions");
        }
        //getTypeQuestions
        public Rating_TransactionDTO getTypeQuestions(Rating_TransactionDTO data)
        {
            return _comm.POSTIssueManager(data, "Rating_TransactionFacade/getTypeQuestions");
        }
        public Rating_TransactionDTO viewratinglist(Rating_TransactionDTO data)
        {
            return _comm.POSTIssueManager(data, "Rating_TransactionFacade/viewratinglist");
        }
        public Rating_TransactionDTO checkrating(Rating_TransactionDTO data)
        {
            return _comm.POSTIssueManager(data, "Rating_TransactionFacade/checkrating");
        }
        public Rating_TransactionDTO getdesignation(Rating_TransactionDTO data)
        {
            return _comm.POSTIssueManager(data, "Rating_TransactionFacade/getdesignation");
        }
        public Rating_TransactionDTO viewautomaticrating(Rating_TransactionDTO data)
        {
            return _comm.POSTIssueManager(data, "Rating_TransactionFacade/viewautomaticrating");
        }

        // Rating DressCode
        public Rating_TransactionDTO OnLoadRatingDressCode(Rating_TransactionDTO data)
        {
            return _comm.POSTIssueManager(data, "Rating_TransactionFacade/OnLoadRatingDressCode");
        }
        public Rating_TransactionDTO checkratingDresscode(Rating_TransactionDTO data)
        {
            return _comm.POSTIssueManager(data, "Rating_TransactionFacade/checkratingDresscode");
        }
        // Rating Appearance
        public Rating_TransactionDTO OnClickRatingAppearance(Rating_TransactionDTO data)
        {
            return _comm.POSTIssueManager(data, "Rating_TransactionFacade/OnClickRatingAppearance");
        }
        public Rating_TransactionDTO checkratingAppearance(Rating_TransactionDTO data)
        {
            return _comm.POSTIssueManager(data, "Rating_TransactionFacade/checkratingAppearance");
        }
        public Rating_TransactionDTO SaveRatingDresscode(Rating_TransactionDTO data)
        {
            return _comm.POSTIssueManager(data, "Rating_TransactionFacade/SaveRatingDresscode");
        }

        //Rating Behaviour
        public Rating_TransactionDTO OnClickRatingBehaviour(Rating_TransactionDTO data)
        {
            return _comm.POSTIssueManager(data, "Rating_TransactionFacade/OnClickRatingBehaviour");
        }
        public Rating_TransactionDTO checkratingbehaviour(Rating_TransactionDTO data)
        {
            return _comm.POSTIssueManager(data, "Rating_TransactionFacade/checkratingbehaviour");
        }
        public Rating_TransactionDTO OnChangeBehaviourEmployee(Rating_TransactionDTO data)
        {
            return _comm.POSTIssueManager(data, "Rating_TransactionFacade/OnChangeBehaviourEmployee");
        }
        public Rating_TransactionDTO SaveBehaviourRating(Rating_TransactionDTO data)
        {
            return _comm.POSTIssueManager(data, "Rating_TransactionFacade/SaveBehaviourRating");
        }

        // Rating TaskRole Category Fetch EmpoloyeeTask Category Wise
        public Rating_TransactionDTO viewemployeetaskcategory(Rating_TransactionDTO data)
        {
            return _comm.POSTIssueManager(data, "Rating_TransactionFacade/viewemployeetaskcategory");
        }
        public Rating_TransactionDTO SaveRatingTaskCategory(Rating_TransactionDTO data)
        {
            return _comm.POSTIssueManager(data, "Rating_TransactionFacade/SaveRatingTaskCategory");
        }
        public Rating_TransactionDTO subclassificationwisereport(Rating_TransactionDTO data)
        {
            return _comm.POSTIssueManager(data, "Rating_TransactionFacade/subclassificationwisereport");
        }
    }
}
