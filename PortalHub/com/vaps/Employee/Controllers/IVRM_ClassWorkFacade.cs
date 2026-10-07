using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Portals.Employee;
using PortalHub.com.vaps.Employee.Interfaces;
using PreadmissionDTOs.com.vaps.Portals.Employee;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace PortalHub.com.vaps.Employee.Controllers
{
    [Route("api/[controller]")]
    public class IVRM_ClassWorkFacade : Controller
    {
        // GET: api/values
        public IVRM_ClassWorkInterface _work;
        public IVRM_ClassWorkFacade(IVRM_ClassWorkInterface work)
        {
            _work = work;
        }
        // GET: api/values
        [Route("savedetail")]
        public IVRM_ClassWorkDTO savedetail([FromBody]IVRM_ClassWorkDTO data)
        {
            return _work.savedetail(data);
        }
        [Route("Getdetails")]
        public IVRM_ClassWorkDTO Getdetails([FromBody]IVRM_ClassWorkDTO data)
        {
            return _work.Getdetails(data);
        }
        [Route("deactivate")]
        public IVRM_ClassWorkDTO deactivate([FromBody]IVRM_ClassWorkDTO data)
        {
            return _work.deactivate(data);
        }
        [Route("get_classes")]
        public Task<IVRM_ClassWorkDTO> get_classes([FromBody]IVRM_ClassWorkDTO data)
        {
            return _work.get_classes(data);
        }
        [Route("getsectiondata")]
        public IVRM_ClassWorkDTO getsectiondata([FromBody]IVRM_ClassWorkDTO data)
        {
            return _work.getsectiondata(data);
        }
        [Route("getsubject")]
        public IVRM_ClassWorkDTO getsubject([FromBody]IVRM_ClassWorkDTO data)
        {
            return _work.getsubject(data);
        }
        [Route("editData")]
        public IVRM_ClassWorkDTO editData([FromBody]IVRM_ClassWorkDTO data)
        {
            return _work.editData(data);
        }
    }
}
