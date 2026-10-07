using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PortalHub.com.vaps.Employee.Interfaces;
using PreadmissionDTOs.com.vaps.Portals.Employee;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PortalHub.com.vaps.Employee.Controllers
{
    [Route("api/[controller]")]
    public class EmployeeStudentHomeworkFacade : Controller
    {
        // GET: api/values
        public EmployeeStudentHomeworkInterface _work;
        public EmployeeStudentHomeworkFacade(EmployeeStudentHomeworkInterface work)
        {
            _work = work;
        }
        // GET: api/values
        [Route("savedetail")]
        public IVRM_Homework_DTO savedetail([FromBody]IVRM_Homework_DTO data)
        {
            return _work.savedetail(data);
        }

        [Route("Getdetails")]
        public IVRM_Homework_DTO Getdetails([FromBody]IVRM_Homework_DTO data)
        {
            return _work.Getdetails(data);
        }
        [Route("deactivate")]
        public IVRM_Homework_DTO deactivate([FromBody]IVRM_Homework_DTO data)
        {
            return _work.deactivate(data);
        }
        [Route("get_classes")]
        public Task<IVRM_Homework_DTO> get_classes([FromBody]IVRM_Homework_DTO data)
        {
            return _work.get_classes(data);
        }
        [Route("getsectiondata")]
        public IVRM_Homework_DTO getsectiondata([FromBody]IVRM_Homework_DTO data)
        {
            return _work.getsectiondata(data);
        }
        [Route("getsubject")]
        public IVRM_Homework_DTO getsubject([FromBody]IVRM_Homework_DTO data)
        {
            return _work.getsubject(data);
        }

        [Route("editData")]
        public IVRM_Homework_DTO editData([FromBody]IVRM_Homework_DTO data)
        {
            return _work.editData(data);
        }
    }
}
