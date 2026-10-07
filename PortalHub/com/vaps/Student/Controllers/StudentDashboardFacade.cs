using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs;
using System.IO;
using System.Data;
using System.Data.SqlClient;
using DomainModel.Model;
using DataAccessMsSqlServerProvider;
using MailKit.Net.Smtp;
using MimeKit;
using System.Text;
using Microsoft.EntityFrameworkCore;
using System.Dynamic;
using PortalHub.com.vaps.Student.Interfaces;
using PreadmissionDTOs.com.vaps.Portals.Student;

namespace PortalHub.com.vaps.Student.Controllers
{
    [Route("api/[controller]")]
    public class StudentDashboardFacade :Controller
    {
        public StudentDashboardInterface _ads;

        public StudentDashboardFacade(StudentDashboardInterface adstu)
        {
            _ads = adstu;
        }

        // POST api/values

        [HttpPost]
        [Route("Getdetails")]       
        public Task<StudentDashboardDTO> Getdetails([FromBody]StudentDashboardDTO sddto)
        {
            return _ads.Getdetails(sddto);
        }
       
        [Route("saveakpkfile")]
        public StudentDashboardDTO saveakpkfile([FromBody] StudentDashboardDTO data)
        {
            return _ads.saveakpkfile(data);
        }

    }
}
