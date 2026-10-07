using PreadmissionDTOs.com.vaps.IssueManager.Rating;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IssueManager.com.Rating.Interfacce
{
    public interface Rating_TransactionInterface
    {
        Rating_TransactionDTO OnLoadRatingTransaction(Rating_TransactionDTO data);
        Rating_TransactionDTO OnChangeEmployee(Rating_TransactionDTO data);
        Rating_TransactionDTO GetSearchDetails(Rating_TransactionDTO data);
        Rating_TransactionDTO SaveRating(Rating_TransactionDTO data);


        //consolidated_report
        Task<Rating_TransactionDTO> get_Consolidated_details(Rating_TransactionDTO data);
        Task<Rating_TransactionDTO> getconsolidatedreport(Rating_TransactionDTO data);
        Task<Rating_TransactionDTO> getemployeereport(Rating_TransactionDTO data);


        //Akash
        //SaveOverAllRating
        Rating_TransactionDTO SaveOverAllRating(Rating_TransactionDTO data);
        Rating_TransactionDTO OverAllEdit(Rating_TransactionDTO data);
        Rating_TransactionDTO OverAllRatingActiveDeactive(Rating_TransactionDTO data);
        Rating_TransactionDTO getQuestions(Rating_TransactionDTO data);
        Rating_TransactionDTO getTypeQuestions(Rating_TransactionDTO data);
        Rating_TransactionDTO viewratinglist(Rating_TransactionDTO data);
        Rating_TransactionDTO checkrating(Rating_TransactionDTO data);
        Rating_TransactionDTO getdesignation(Rating_TransactionDTO data);
        Rating_TransactionDTO viewautomaticrating(Rating_TransactionDTO data);

        // Rating DressCode
        Rating_TransactionDTO OnLoadRatingDressCode(Rating_TransactionDTO data);
        Rating_TransactionDTO checkratingDresscode(Rating_TransactionDTO data);
        Rating_TransactionDTO SaveRatingDresscode(Rating_TransactionDTO data);
        // Rating Appearance
        Rating_TransactionDTO OnClickRatingAppearance(Rating_TransactionDTO data);
        Rating_TransactionDTO checkratingAppearance(Rating_TransactionDTO data);

        // Rating behaviour
        Rating_TransactionDTO OnClickRatingBehaviour(Rating_TransactionDTO data);
        Rating_TransactionDTO checkratingbehaviour(Rating_TransactionDTO data);
        Rating_TransactionDTO OnChangeBehaviourEmployee(Rating_TransactionDTO data);
        Rating_TransactionDTO SaveBehaviourRating(Rating_TransactionDTO data);

        // Rating TaskRole Category Fetch EmpoloyeeTask Category Wise
        Rating_TransactionDTO viewemployeetaskcategory(Rating_TransactionDTO data);
        Rating_TransactionDTO SaveRatingTaskCategory(Rating_TransactionDTO data);

        //EmployeeMobileRating
        Task<Rating_TransactionDTO> mobemployeerating(Rating_TransactionDTO data);
        Task<Rating_TransactionDTO> subclassificationwisereport(Rating_TransactionDTO data);
    }
}
