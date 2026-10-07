using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.FMS;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.FMS;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.FMS
{
    [Route("api/[controller]")]
    public class FMS_CorrespondenceController : Controller
    {
        FMS_CorrespondenceDelegate _delg = new FMS_CorrespondenceDelegate();
        // GET: api/<controller>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<controller>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        [Route("onfirsttabcorrespondence/{id:int}")]
        public FMS_CorrespondenceDTO onfirsttabcorrespondence(int id)
        {
            FMS_CorrespondenceDTO data = new FMS_CorrespondenceDTO();
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.userid = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delg.onfirsttabcorrespondence(data);
        }

        [Route("onchangeclient")]
        public FMS_CorrespondenceDTO onchangeclient([FromBody] FMS_CorrespondenceDTO data)
        {
            data.userid = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delg.onchangeclient(data);
        }
        

        [Route("getleaddetails")]
        public FMS_CorrespondenceDTO getleaddetails([FromBody] FMS_CorrespondenceDTO data)
        {            
            data.userid = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
          //  data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delg.getleaddetails(data);
        }

        [Route("checkrefnoduplicate")]
        public FMS_CorrespondenceDTO checkrefnoduplicate([FromBody] FMS_CorrespondenceDTO data)
        {
            data.userid = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delg.checkrefnoduplicate(data);
        }

        [Route("saveonfirsttabcorrespondence")]
        public FMS_CorrespondenceDTO saveonfirsttabcorrespondence([FromBody] FMS_CorrespondenceDTO data)
        {
            data.userid = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delg.saveonfirsttabcorrespondence(data);
        }

        [Route("activedeactivefirsttab")]
        public FMS_CorrespondenceDTO activedeactivefirsttab([FromBody] FMS_CorrespondenceDTO data)
        {
            data.userid = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delg.activedeactivefirsttab(data);
        }

        [Route("Editdatafirsttab")]
        public FMS_CorrespondenceDTO Editdatafirsttab([FromBody] FMS_CorrespondenceDTO data)
        {            
            data.userid = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delg.Editdatafirsttab(data);
        }

        [Route("viewfirsttabdetails")]
        public FMS_CorrespondenceDTO viewfirsttabdetails([FromBody] FMS_CorrespondenceDTO data)
        {
            data.userid = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delg.viewfirsttabdetails(data);
        }

        [Route("activedeactivesubdetails")]
        public FMS_CorrespondenceDTO activedeactivesubdetails([FromBody] FMS_CorrespondenceDTO data)
        {
            data.userid = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delg.activedeactivesubdetails(data);
        }

        [Route("searchrefernceno")]
        public FMS_CorrespondenceDTO searchrefernceno([FromBody] FMS_CorrespondenceDTO data)
        {            
            data.userid = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delg.searchrefernceno(data);
        }

        /*GENERAL DETILS FOR SEARCH REFERNCE NO. */

        [Route("searchreferncenosecondtab")]
        public FMS_CorrespondenceDTO searchreferncenosecondtab([FromBody] FMS_CorrespondenceDTO data)
        {            
            data.userid = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delg.searchreferncenosecondtab(data);
        }

        [Route("OnChangeDept")]
        public FMS_CorrespondenceDTO OnChangeDept([FromBody] FMS_CorrespondenceDTO data)
        { 
            data.userid = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delg.OnChangeDept(data);
        }

        /* SECOND TAB */

        [Route("onsecondtabcorrespondence/{id:int}")]
        public FMS_CorrespondenceDTO onsecondtabcorrespondence(int id)
        {
            FMS_CorrespondenceDTO data = new FMS_CorrespondenceDTO();
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.userid = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delg.onsecondtabcorrespondence(data);
        }

        [Route("onchangefilecabinet")]
        public FMS_CorrespondenceDTO onchangefilecabinet([FromBody] FMS_CorrespondenceDTO data)
        {            
            data.userid = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delg.onchangefilecabinet(data);
        }

        [Route("savesecondtabcorrespondence")]
        public FMS_CorrespondenceDTO savesecondtabcorrespondence([FromBody] FMS_CorrespondenceDTO data)
        {
            data.userid = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delg.savesecondtabcorrespondence(data);
        }

        [Route("Editdatasecondtab")]
        public FMS_CorrespondenceDTO Editdatasecondtab([FromBody] FMS_CorrespondenceDTO data)
        {
            data.userid = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delg.Editdatasecondtab(data);
        }

        [Route("onchangerefnosecondtab")]
        public FMS_CorrespondenceDTO onchangerefnosecondtab([FromBody] FMS_CorrespondenceDTO data)
        {           
            data.userid = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delg.onchangerefnosecondtab(data);
        }

        [Route("activedeactivesecondtab")]
        public FMS_CorrespondenceDTO activedeactivesecondtab([FromBody] FMS_CorrespondenceDTO data)
        {
            data.userid = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delg.activedeactivesecondtab(data);
        }

        [Route("ViewCorrespondanceFiles")]
        public FMS_CorrespondenceDTO ViewCorrespondanceFiles([FromBody] FMS_CorrespondenceDTO data)
        {
            data.userid = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delg.ViewCorrespondanceFiles(data);
        }

        [Route("DeleteCorrespondanceFile")]
        public FMS_CorrespondenceDTO DeleteCorrespondanceFile([FromBody] FMS_CorrespondenceDTO data)
        {
            data.userid = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delg.DeleteCorrespondanceFile(data);
        }

        [Route("OnChangephysicalpath")]
        public FMS_CorrespondenceDTO OnChangephysicalpath([FromBody] FMS_CorrespondenceDTO data)
        {
            data.userid = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delg.OnChangephysicalpath(data);
        }

        /*THIRD TAB*/
        [Route("onthirdtabcorrespondence/{id:int}")]
        public FMS_CorrespondenceDTO onthirdtabcorrespondenceS(int id)
        {
            FMS_CorrespondenceDTO data = new FMS_CorrespondenceDTO();
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.userid = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delg.onthirdtabcorrespondence(data);
        }

        [Route("savethirdtabcorrespondence")]
        public FMS_CorrespondenceDTO savethirdtabcorrespondence([FromBody] FMS_CorrespondenceDTO data)
        {            
            data.userid = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delg.savethirdtabcorrespondence(data);
        }

        [Route("onchangerefnothirdtab")]
        public FMS_CorrespondenceDTO onchangerefnothirdtab([FromBody] FMS_CorrespondenceDTO data)
        {
            data.userid = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delg.onchangerefnothirdtab(data);
        }

        [Route("Editdatathirdtab")]
        public FMS_CorrespondenceDTO Editdatathirdtab([FromBody] FMS_CorrespondenceDTO data)
        {
            data.userid = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delg.Editdatathirdtab(data);
        }

        [Route("activedeactivethirdtab")]
        public FMS_CorrespondenceDTO activedeactivethirdtab([FromBody] FMS_CorrespondenceDTO data)
        {
            data.userid = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delg.activedeactivethirdtab(data);
        }

        [Route("OnChangeLetterHeadNo")]
        public FMS_CorrespondenceDTO OnChangeLetterHeadNo([FromBody] FMS_CorrespondenceDTO data)
        {
            data.userid = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delg.OnChangeLetterHeadNo(data);
        }
    }
}
