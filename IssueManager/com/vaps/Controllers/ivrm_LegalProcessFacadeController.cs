using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IssueManager.com.vaps.Interfaces;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Legal;

namespace IssueManager.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class ivrm_LegalProcessFacadeController : Controller
    {
        // GET: api/values

        public ivrm_LegalProcessInterface _ads;

        public ivrm_LegalProcessFacadeController(ivrm_LegalProcessInterface adstu)
        {
            _ads = adstu;
        }


        [Route("Onloaddata")]
        public Legal_Notice_RequestDTO Onloaddata([FromBody]Legal_Notice_RequestDTO data)
        {
            return _ads.Onloaddata(data);
        }

        [Route("Onloadcourtdata")]
        public Legal_Notice_RequestDTO Onloadcourtdata([FromBody]Legal_Notice_RequestDTO data)
        {
            return _ads.Onloadcourtdata(data);
        }

        [Route("Onloadcasetype")]
        public Legal_Notice_RequestDTO Onloadcasetype([FromBody]Legal_Notice_RequestDTO data)
        {
            return _ads.Onloadcasetype(data);
        }

        [Route("Onloadcasetypemapping")]
        public Legal_Notice_RequestDTO Onloadcasetypemapping([FromBody]Legal_Notice_RequestDTO data)
        {
            return _ads.Onloadcasetypemapping(data);
        }

        //[Route("onclickloaddata1")]
        //public Legal_Notice_RequestDTO onclickloaddata1([FromBody]Legal_Notice_RequestDTO data)
        //{
        //    return _ads.onclickloaddata1(data);
        //}        

        [Route("savedetail")]
        public Legal_Notice_RequestDTO savedetail([FromBody] Legal_Notice_RequestDTO data)
        {
            return _ads.savedetail(data);
        }

        [Route("legalcourtsave")]
        public Legal_Notice_RequestDTO legalcourtsave([FromBody] Legal_Notice_RequestDTO data)
        {
            return _ads.legalcourtsave(data);
        }

        [Route("legalcasetypesave")]
        public Legal_Notice_RequestDTO legalcasetypesave([FromBody] Legal_Notice_RequestDTO data)
        {
            return _ads.legalcasetypesave(data);
        }

        [Route("Legalcasetypemappingsave")]
        public Legal_Notice_RequestDTO Legalcasetypemappingsave([FromBody] Legal_Notice_RequestDTO data)
        {
            return _ads.Legalcasetypemappingsave(data);
        }

    }
}