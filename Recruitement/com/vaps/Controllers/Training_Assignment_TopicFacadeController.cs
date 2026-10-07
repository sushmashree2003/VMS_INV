using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.VMS.Training;
using Recruitment.com.vaps.Interfaces;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Recruitment.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class Training_Assignment_TopicFacadeController : Controller
    {
        public Training_Assignment_TopicInterface _flr;


        public Training_Assignment_TopicFacadeController(Training_Assignment_TopicInterface ct)
        {
            _flr = ct;
        }
        [Route("getdata_TAT")]
        public HR_Training_Assignment_Topic_DTO getdata_TAT([FromBody] HR_Training_Assignment_Topic_DTO dto)
        {
            return _flr.getdata_TAT(dto);
        }
    }
}
