using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FMS.Com.Interface;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.FMS;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FMS.Com.Facade
{
    [Route("api/[controller]")]
    public class FMS_Master_FileCategoryFacadeController : Controller
    {
        public FMS_CorrespondenceInterface _interface;

        public FMS_Master_FileCategoryFacadeController(FMS_CorrespondenceInterface _inter)
        {
            _interface = _inter;
        }

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

        [Route("onfirsttabcorrespondence")]
        public FMS_CorrespondenceDTO onfirsttabcorrespondence([FromBody] FMS_CorrespondenceDTO data)
        {
            return _interface.onfirsttabcorrespondence(data);
        }
        [Route("onchangeclient")]
        public FMS_CorrespondenceDTO onchangeclient([FromBody] FMS_CorrespondenceDTO data)
        {
            return _interface.onchangeclient(data);
        }
        [Route("checkrefnoduplicate")]
        public FMS_CorrespondenceDTO checkrefnoduplicate([FromBody] FMS_CorrespondenceDTO data)
        {
            return _interface.checkrefnoduplicate(data);
        }
        [Route("saveonfirsttabcorrespondence")]
        public FMS_CorrespondenceDTO saveonfirsttabcorrespondence([FromBody] FMS_CorrespondenceDTO data)
        {
            return _interface.saveonfirsttabcorrespondence(data);
        }
        [Route("activedeactivefirsttab")]
        public FMS_CorrespondenceDTO activedeactivefirsttab([FromBody] FMS_CorrespondenceDTO data)
        {
            return _interface.activedeactivefirsttab(data);
        }
        [Route("Editdatafirsttab")]
        public FMS_CorrespondenceDTO Editdatafirsttab([FromBody] FMS_CorrespondenceDTO data)
        {
            return _interface.Editdatafirsttab(data);
        }
        [Route("viewfirsttabdetails")]
        public FMS_CorrespondenceDTO viewfirsttabdetails([FromBody] FMS_CorrespondenceDTO data)
        {
            return _interface.viewfirsttabdetails(data);
        }
        [Route("activedeactivesubdetails")]
        public FMS_CorrespondenceDTO activedeactivesubdetails([FromBody] FMS_CorrespondenceDTO data)
        {
            return _interface.activedeactivesubdetails(data);
        }
        [Route("searchrefernceno")]
        public FMS_CorrespondenceDTO searchrefernceno([FromBody] FMS_CorrespondenceDTO data)
        {
            return _interface.searchrefernceno(data);
        }

        /*GENERAL DETILS FOR SEARCH REFERNCE NO. */       
        [Route("searchreferncenosecondtab")]
        public FMS_CorrespondenceDTO searchreferncenosecondtab([FromBody] FMS_CorrespondenceDTO data)
        {
            return _interface.searchreferncenosecondtab(data);
        }
        /*SECOND TAB */
        [Route("onsecondtabcorrespondence")]
        public FMS_CorrespondenceDTO onsecondtabcorrespondence([FromBody] FMS_CorrespondenceDTO data)
        {
            return _interface.onsecondtabcorrespondence(data);
        }

        [Route("onchangefilecabinet")]
        public FMS_CorrespondenceDTO onchangefilecabinet([FromBody] FMS_CorrespondenceDTO data)
        {
            return _interface.onchangefilecabinet(data);
        }

        [Route("savesecondtabcorrespondence")]
        public FMS_CorrespondenceDTO savesecondtabcorrespondence([FromBody] FMS_CorrespondenceDTO data)
        {
            return _interface.savesecondtabcorrespondence(data);
        }

        [Route("Editdatasecondtab")]
        public FMS_CorrespondenceDTO Editdatasecondtab([FromBody] FMS_CorrespondenceDTO data)
        {
            return _interface.Editdatasecondtab(data);
        }

        [Route("onchangerefnosecondtab")]
        public FMS_CorrespondenceDTO onchangerefnosecondtab([FromBody] FMS_CorrespondenceDTO data)
        {
            return _interface.onchangerefnosecondtab(data);
        }

        [Route("activedeactivesecondtab")]
        public FMS_CorrespondenceDTO activedeactivesecondtab([FromBody] FMS_CorrespondenceDTO data)
        {
            return _interface.activedeactivesecondtab(data);
        }

        /*THIRD TAB */
        [Route("onthirdtabcorrespondence")]
        public FMS_CorrespondenceDTO onthirdtabcorrespondence([FromBody] FMS_CorrespondenceDTO data)
        {
            return _interface.onthirdtabcorrespondence(data);
        }        
        [Route("savethirdtabcorrespondence")]
        public FMS_CorrespondenceDTO savethirdtabcorrespondence([FromBody] FMS_CorrespondenceDTO data)
        {
            return _interface.savethirdtabcorrespondence(data);
        }        
        [Route("Editdatathirdtab")]
        public FMS_CorrespondenceDTO Editdatathirdtab([FromBody] FMS_CorrespondenceDTO data)
        {
            return _interface.Editdatathirdtab(data);
        }        
        [Route("activedeactivethirdtab")]
        public FMS_CorrespondenceDTO activedeactivethirdtab([FromBody] FMS_CorrespondenceDTO data)
        {
            return _interface.activedeactivethirdtab(data);
        }        
    }
}
