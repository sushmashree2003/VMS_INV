 using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IssueManager.com.Feedback.Interface;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.FeedBack;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860
namespace IssueManager.com.Feedback.Facade
    {
        [Route("api/[controller]")]
        public class EmployeeHeadFeedBackFacade : Controller
        {
            public EmployeeHeadFeedBackInterface _interface;

            public EmployeeHeadFeedBackFacade(EmployeeHeadFeedBackInterface _inter)
            {
                _interface = _inter;
            }
            // GET: api/<controller>
            [HttpGet]
            public IEnumerable<string> Get()
            {
                return new string[] { "value1", "value2" };
            }
        
            [Route("getdetails")]
            public Task<EmployeeHeadFeedBackDTO> getdetails([FromBody] EmployeeHeadFeedBackDTO data)
            {
                return _interface.getdetails(data);
            }




        [HttpPost]
        [Route("savedetails")]
        public EmployeeHeadFeedBackDTO savedetails([FromBody] EmployeeHeadFeedBackDTO data)
        {
            return _interface.savedetails(data);
        }

        [Route("delete_data/{id:int}")]
        public EmployeeHeadFeedBackDTO delete_data(int id)
        {
            return _interface.delete_data(id);
        }


    }
    }
