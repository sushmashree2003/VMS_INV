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
    public class Rating_TransactionController : Controller
    {
        Rating_TransactionDelegate _delg = new Rating_TransactionDelegate();
        // GET: api/<controller>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [Route("OnLoadRatingTransaction")]
        public Rating_TransactionDTO OnLoadRatingTransaction([FromBody] Rating_TransactionDTO data)
        {
            if (data.FlagOnLoadOrOnChangeInst == "OnLoad")
            {
                data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            }
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.Role_flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            data.IVRMRT_Id = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));

            return _delg.OnLoadRatingTransaction(data);
        }

        [Route("OnChangeEmployee")]
        public Rating_TransactionDTO OnChangeEmployee([FromBody] Rating_TransactionDTO data)
        {
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.Role_flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            data.IVRMRT_Id = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return _delg.OnChangeEmployee(data);
        }

        [Route("GetSearchDetails")]
        public Rating_TransactionDTO GetSearchDetails([FromBody] Rating_TransactionDTO data)
        {
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.Role_flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            data.IVRMRT_Id = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return _delg.GetSearchDetails(data);
        }

        [Route("SaveRating")]
        public Rating_TransactionDTO SaveRating([FromBody] Rating_TransactionDTO data)
        {
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.Role_flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            data.IVRMRT_Id = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return _delg.SaveRating(data);
        }

        //consolidated_report

        [Route("get_Consolidated_details/{id:int}")]
        public Rating_TransactionDTO get_Consolidated_details(int id)
        {
            Rating_TransactionDTO data = new Rating_TransactionDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));            
            data.Role_flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            data.IVRMRT_Id = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return _delg.get_Consolidated_details(data);
        }

        [Route("getconsolidatedreport")]
        public Rating_TransactionDTO getconsolidatedreport([FromBody]Rating_TransactionDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.Role_flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            data.IVRMRT_Id = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return _delg.getconsolidatedreport(data);
        }
        [Route("getemployeereport")]
        public Rating_TransactionDTO getemployeereport([FromBody]Rating_TransactionDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.Role_flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            data.IVRMRT_Id = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return _delg.getemployeereport(data);
        }

        //Akash
        [Route("SaveOverAllRating")]
        public Rating_TransactionDTO SaveOverAllRating([FromBody] Rating_TransactionDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delg.SaveOverAllRating(data);
        }
        //OverAllEdit
        [Route("OverAllEdit")]
        public Rating_TransactionDTO OverAllEdit([FromBody] Rating_TransactionDTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delg.OverAllEdit(data);
        }

        //OverAllRatingActiveDeactive
        [Route("OverAllRatingActiveDeactive")]
        public Rating_TransactionDTO OverAllRatingActiveDeactive([FromBody] Rating_TransactionDTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delg.OverAllRatingActiveDeactive(data);
        }

        //getQuestions
        [Route("getQuestions")]
        public Rating_TransactionDTO getQuestions([FromBody] Rating_TransactionDTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delg.getQuestions(data);
        }
        //getTypeQuestions
        [Route("getTypeQuestions")]
        public Rating_TransactionDTO getTypeQuestions([FromBody] Rating_TransactionDTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delg.getTypeQuestions(data);
        }
        //viw given Rating
        [Route("viewratinglist")]
        public Rating_TransactionDTO viewratinglist([FromBody] Rating_TransactionDTO data)
        {
            return _delg.viewratinglist(data);
        }
        [Route("checkrating")]
        public Rating_TransactionDTO checkrating([FromBody] Rating_TransactionDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.Role_flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            data.IVRMRT_Id = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return _delg.checkrating(data);
            
        }
        [Route("getdesignation")]
        public Rating_TransactionDTO getdesignation([FromBody] Rating_TransactionDTO data)
        {
            return _delg.getdesignation(data);
        }
        [Route("viewautomaticrating")]
        public Rating_TransactionDTO viewautomaticrating([FromBody] Rating_TransactionDTO data)
        {
            return _delg.viewautomaticrating(data);
        }
        // Rating DressCode
        [Route("OnLoadRatingDressCode")]
        public Rating_TransactionDTO OnLoadRatingDressCode([FromBody] Rating_TransactionDTO data)
        {
            if (data.FlagOnLoadOrOnChangeInst == "OnLoad")
            {
                data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            }
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.Role_flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            data.IVRMRT_Id = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));

            return _delg.OnLoadRatingDressCode(data);
        }

        [Route("checkratingDresscode")]
        public Rating_TransactionDTO checkratingDresscode([FromBody] Rating_TransactionDTO data)
        {
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.Role_flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            data.IVRMRT_Id = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return _delg.checkratingDresscode(data);

        }

        [Route("SaveRatingDresscode")]
        public Rating_TransactionDTO SaveRatingDresscode([FromBody] Rating_TransactionDTO data)
        {
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.Role_flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            data.IVRMRT_Id = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return _delg.SaveRatingDresscode(data);
        }

        // Rating Appearance
        [Route("OnClickRatingAppearance")]
        public Rating_TransactionDTO OnClickRatingAppearance([FromBody] Rating_TransactionDTO data)
        {
            if (data.FlagOnLoadOrOnChangeInst == "OnLoad")
            {
                data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            }
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.Role_flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            data.IVRMRT_Id = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));

            return _delg.OnClickRatingAppearance(data);
        }

        [Route("checkratingAppearance")]
        public Rating_TransactionDTO checkratingAppearance([FromBody] Rating_TransactionDTO data)
        {
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.Role_flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            data.IVRMRT_Id = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return _delg.checkratingAppearance(data);

        }



        // Rating Behaviour
        [Route("OnClickRatingBehaviour")]
        public Rating_TransactionDTO OnClickRatingBehaviour([FromBody] Rating_TransactionDTO data)
        {
            if (data.FlagOnLoadOrOnChangeInst == "OnLoad")
            {
                data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            }
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.Role_flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            data.IVRMRT_Id = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));

            return _delg.OnClickRatingBehaviour(data);
        }

        [Route("checkratingbehaviour")]
        public Rating_TransactionDTO checkratingbehaviour([FromBody] Rating_TransactionDTO data)
        {
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.Role_flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            data.IVRMRT_Id = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return _delg.checkratingbehaviour(data);

        }

        [Route("OnChangeBehaviourEmployee")]
        public Rating_TransactionDTO OnChangeBehaviourEmployee([FromBody] Rating_TransactionDTO data)
        {
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.Role_flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            data.IVRMRT_Id = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return _delg.OnChangeBehaviourEmployee(data);
        }

        [Route("SaveBehaviourRating")]
        public Rating_TransactionDTO SaveBehaviourRating([FromBody] Rating_TransactionDTO data)
        {
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.Role_flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            data.IVRMRT_Id = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return _delg.SaveBehaviourRating(data);
        }

        // Rating TaskRole Category Fetch EmpoloyeeTask Category Wise

        [Route("viewemployeetaskcategory")]
        public Rating_TransactionDTO viewemployeetaskcategory([FromBody] Rating_TransactionDTO data)
        {
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.Role_flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            data.IVRMRT_Id = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return _delg.viewemployeetaskcategory(data);

        }
        [Route("SaveRatingTaskCategory")]
        public Rating_TransactionDTO SaveRatingTaskCategory([FromBody] Rating_TransactionDTO data)
        {
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.Role_flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            data.IVRMRT_Id = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return _delg.SaveRatingTaskCategory(data);
        }

        [Route("subclassificationwisereport")]
        public Rating_TransactionDTO subclassificationwisereport([FromBody]Rating_TransactionDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.Role_flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            data.IVRMRT_Id = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return _delg.subclassificationwisereport(data);
        }
    }
}