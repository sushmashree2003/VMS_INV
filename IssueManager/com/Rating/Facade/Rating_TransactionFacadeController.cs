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
    public class Rating_TransactionFacadeController : Controller
    {
        public Rating_TransactionInterface _interface;

        public Rating_TransactionFacadeController (Rating_TransactionInterface _inter)
        {
            _interface = _inter;
        }
        // GET: api/<controller>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [Route("OnLoadRatingTransaction")]
        public Rating_TransactionDTO OnLoadRatingTransaction([FromBody] Rating_TransactionDTO data)
        {
            return _interface.OnLoadRatingTransaction(data);
        }

        [Route("OnChangeEmployee")]
        public Rating_TransactionDTO OnChangeEmployee([FromBody] Rating_TransactionDTO data)
        {
            return _interface.OnChangeEmployee(data);
        }

        //[Route("OnChangeEmployeeevaluation")]
        //public Rating_TransactionDTO OnChangeEmployeeevaluation([FromBody] Rating_TransactionDTO data)
        //{
        //    return _interface.OnChangeEmployeeevaluation(data);
        //}

        //[Route("GetSearchDetailsevaluation")]
        //public Rating_TransactionDTO GetSearchDetailsevaluation([FromBody] Rating_TransactionDTO data)
        //{
        //    return _interface.GetSearchDetailsevaluation(data);
        //}

        [Route("GetSearchDetails")]
        public Rating_TransactionDTO GetSearchDetails([FromBody] Rating_TransactionDTO data)
        {
            return _interface.GetSearchDetails(data);
        }

        [Route("SaveRating")]
        public Rating_TransactionDTO SaveRating([FromBody] Rating_TransactionDTO data)
        {
            return _interface.SaveRating(data);
        }

        //[Route("Saveevaluation")]
        //public Rating_TransactionDTO Saveevaluation([FromBody] Rating_TransactionDTO data)
        //{
        //    return _interface.Saveevaluation(data);
        //}


        //consolidated_report

        [Route("get_Consolidated_details")]
        public Task<Rating_TransactionDTO> get_Consolidated_details([FromBody] Rating_TransactionDTO data)
        {
            return _interface.get_Consolidated_details(data);
        }

        [Route("getconsolidatedreport")]
        public Task<Rating_TransactionDTO> getconsolidatedreport([FromBody]Rating_TransactionDTO data)
        {
            return _interface.getconsolidatedreport(data);
        }

        [Route("getemployeereport")]
        public Task<Rating_TransactionDTO> getemployeereport([FromBody]Rating_TransactionDTO data)
        {
            return _interface.getemployeereport(data);
        }
        //Akash
        //SaveOverAllRating
        [Route("SaveOverAllRating")]
        public Rating_TransactionDTO SaveOverAllRating([FromBody] Rating_TransactionDTO data)
        {
            return _interface.SaveOverAllRating(data);
        }
        //OverAllEdit
        [Route("OverAllEdit")]
        public Rating_TransactionDTO OverAllEdit([FromBody] Rating_TransactionDTO data)
        {
            return _interface.OverAllEdit(data);
        }
        //OverAllRatingActiveDeactive
        [Route("OverAllRatingActiveDeactive")]
        public Rating_TransactionDTO OverAllRatingActiveDeactive([FromBody] Rating_TransactionDTO data)
        {
            return _interface.OverAllRatingActiveDeactive(data);
        }
        //getQuestions
        [Route("getQuestions")]
        public Rating_TransactionDTO getQuestions([FromBody] Rating_TransactionDTO data)
        {
            return _interface.getQuestions(data);
        }
        //getTypeQuestions
        [Route("getTypeQuestions")]
        public Rating_TransactionDTO getTypeQuestions([FromBody] Rating_TransactionDTO data)
        {
            return _interface.getTypeQuestions(data);
        }
        //viw given Rating
        [Route("viewratinglist")]
        public Rating_TransactionDTO viewratinglist([FromBody] Rating_TransactionDTO data)
        {
            return _interface.viewratinglist(data);
        }
        [Route("checkrating")]
        public Rating_TransactionDTO checkrating([FromBody] Rating_TransactionDTO data)
        {
            return _interface.checkrating(data);
        }
        [Route("getdesignation")]
        public Rating_TransactionDTO getdesignation([FromBody] Rating_TransactionDTO data)
        {
            return _interface.getdesignation(data);
        }
        [Route("viewautomaticrating")]
        public Rating_TransactionDTO viewautomaticrating([FromBody] Rating_TransactionDTO data)
        {
            return _interface.viewautomaticrating(data);
        }

        // Rating DressCode
        [Route("OnLoadRatingDressCode")]
        public Rating_TransactionDTO OnLoadRatingDressCode([FromBody] Rating_TransactionDTO data)
        {
            return _interface.OnLoadRatingDressCode(data);
        }

        [Route("checkratingDresscode")]
        public Rating_TransactionDTO checkratingDresscode([FromBody] Rating_TransactionDTO data)
        {
            return _interface.checkratingDresscode(data);
        }

        [Route("SaveRatingDresscode")]
        public Rating_TransactionDTO SaveRatingDresscode([FromBody] Rating_TransactionDTO data)
        {
            return _interface.SaveRatingDresscode(data);
        }

        // Rating Appearance
        [Route("OnClickRatingAppearance")]
        public Rating_TransactionDTO OnClickRatingAppearance([FromBody] Rating_TransactionDTO data)
        {
            return _interface.OnClickRatingAppearance(data);
        }

        [Route("checkratingAppearance")]
        public Rating_TransactionDTO checkratingAppearance([FromBody] Rating_TransactionDTO data)
        {
            return _interface.checkratingAppearance(data);
        }

        // Rating Behaviour
        [Route("OnClickRatingBehaviour")]
        public Rating_TransactionDTO OnClickRatingBehaviour([FromBody] Rating_TransactionDTO data)
        {
            return _interface.OnClickRatingBehaviour(data);
        }

        [Route("checkratingbehaviour")]
        public Rating_TransactionDTO checkratingbehaviour([FromBody] Rating_TransactionDTO data)
        {
            return _interface.checkratingbehaviour(data);
        }

        [Route("OnChangeBehaviourEmployee")]
        public Rating_TransactionDTO OnChangeBehaviourEmployee([FromBody] Rating_TransactionDTO data)
        {
            return _interface.OnChangeBehaviourEmployee(data);
        }

        [Route("SaveBehaviourRating")]
        public Rating_TransactionDTO SaveBehaviourRating([FromBody] Rating_TransactionDTO data)
        {
            return _interface.SaveBehaviourRating(data);
        }
        // Rating TaskRole Category Fetch EmpoloyeeTask Category Wise
        [Route("viewemployeetaskcategory")]
        public Rating_TransactionDTO viewemployeetaskcategory([FromBody] Rating_TransactionDTO data)
        {
            return _interface.viewemployeetaskcategory(data);
        }
        [Route("SaveRatingTaskCategory")]
        public Rating_TransactionDTO SaveRatingTaskCategory([FromBody] Rating_TransactionDTO data)
        {
            return _interface.SaveRatingTaskCategory(data);
        }

        //EmployeeMobileRating
        [Route("mobemployeerating")]
        public Task<Rating_TransactionDTO> mobemployeerating([FromBody]Rating_TransactionDTO data)
        {
            return _interface.mobemployeerating(data);
        }

        [Route("subclassificationwisereport")]
        public Task<Rating_TransactionDTO> subclassificationwisereport([FromBody]Rating_TransactionDTO data)
        {
            return _interface.subclassificationwisereport(data);
        }
    }
}
