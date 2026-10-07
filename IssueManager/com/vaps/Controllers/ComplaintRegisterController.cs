using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IssueManager.com.vaps.Interfaces;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.IssueManager;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IssueManager.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class ComplaintRegisterController : Controller
    {
        public ComplaintRegisterInterface _objinter;

        public ComplaintRegisterController(ComplaintRegisterInterface parameter)
        {
            _objinter = parameter;
        }

        [Route("save")]
        public ComplaintRegister_DTO save([FromBody] ComplaintRegister_DTO data)
        {
            return _objinter.save(data);
        }
    }
}
