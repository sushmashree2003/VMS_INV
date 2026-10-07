using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.BirthDay;
using BirthdayServiceHub.com.vaps.Interfaces;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace BirthdayServiceHub.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class BirthDayFacadeController : Controller
    {

        public BirthDayInterface _inter;
        public BirthDayFacadeController(BirthDayInterface brth)
        {
            _inter = brth;
        }

        [HttpGet("{id:int}")]
        //[Route("getdata/{id:int}")]
        public BirthDayDTO getdata(int id)
        {
            return _inter.getdata(id);
        }

        [HttpPost]
        [Route("getS")]
        public BirthDayDTO getclasssectionstudentlist([FromBody]BirthDayDTO student)
        {
            return _inter.getlistthree(student);
        }
        [Route("BindStaff")]
        public BirthDayDTO getstafflist([FromBody]BirthDayDTO staff)
        {
            return _inter.staflist(staff);
        }
        [Route("Sendsms")]
        public BirthDayDTO getstafflist1([FromBody]BirthDayDTO staff)
        {
            return _inter.staflist1(staff);
        }
        [Route("Check_SMS_Mail_Status/{id:int}")]

        public void Check_SMS_Mail_Status(int id)
        {
             _inter.Check_SMS_Mail_Status(id);
        }
        [Route("Sendmsg")]
        public BirthDayDTO Sendmsg([FromBody] BirthDayDTO msg)
        {
           return _inter.Sendmsg(msg);
        }
        [Route("getReport")]
        public BirthDayDTO getReport([FromBody] BirthDayDTO data)
        {
            return _inter.getReport(data);
        }
        [Route("getEmailSMSCount")]
        public BirthDayDTO getEmailSMSCount([FromBody] BirthDayDTO data)
        {
            return _inter.getEmailSMSCount(data);
        }
        [Route("SearchByColumn")]
        public BirthDayDTO SearchByColumn([FromBody] BirthDayDTO obj)
        {
            return _inter.SearchByColumn(obj);
        }

        [Route("getmonthreport")]
        public BirthDayDTO getmonthreport([FromBody] BirthDayDTO obj)
        {
            return _inter.getmonthreport(obj);
        }

        [Route("getstaffdetails")]
        public Task<BirthDayDTO> getstaffdetails([FromBody] BirthDayDTO obj)
        {
            return _inter.getstaffdetails(obj);
        }


    }
}
