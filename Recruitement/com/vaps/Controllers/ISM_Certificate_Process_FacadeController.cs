using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccessMsSqlServerProvider;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.VMS.Exit;
using Recruitment.com.vaps.Interfaces;

namespace Recruitment.com.vaps.Controllers
{
    [Produces("application/json")]
    [Route("api/ISM_Certificate_Process_Facade")]
    public class ISM_Certificate_Process_FacadeController : Controller
    {
        public ISM_Certificate_Process_Interface _eei;

        public VMSContext _vmsconte;

        public ISM_Certificate_Process_FacadeController(ISM_Certificate_Process_Interface ee)
        {
            _eei = ee;
        }

        //---------------------------------ISM_Certificate_Request-------------------------------------


        [Route("getloaddata")]
        public ISM_Certificate_RequestDTO getloaddata([FromBody] ISM_Certificate_RequestDTO data)
        {
            return _eei.getloaddata(data);
        }



        [Route("savedata")]
        public ISM_Certificate_RequestDTO savedetails([FromBody]ISM_Certificate_RequestDTO data)
        {
            return _eei.savedetails(data);
        }

        [Route("deactive")]        public ISM_Certificate_RequestDTO deactive([FromBody] ISM_Certificate_RequestDTO data)        {            return _eei.deactive(data);        }
        [Route("Edit")]        public ISM_Certificate_RequestDTO Edit([FromBody] ISM_Certificate_RequestDTO data)        {            return _eei.Edit(data);        }

        [Route("Viewdata")]
        public ISM_Certificate_RequestDTO Viewdata([FromBody] ISM_Certificate_RequestDTO data)
        {
            return _eei.Viewdata(data);
        }


        //--------------------certificate approval------------



        [Route("loaddata")]
        public ISM_Certificate_RequestDTO loaddata([FromBody] ISM_Certificate_RequestDTO data)
        {
            return _eei.loaddata(data);
        }





        [Route("viewuploadflies")]
        public ISM_Certificate_RequestDTO viewuploadflies([FromBody]ISM_Certificate_RequestDTO data)
        {
            return _eei.Viewsdata(data);
        }


        [Route("approvedrecord")]
        public ISM_Certificate_RequestDTO approvedrecord([FromBody] ISM_Certificate_RequestDTO data)
        {
            return _eei.approvedrecord(data);
        }

        //------------------ ISM Certificate Approval Report------------------------------


        [Route("Onload")]
        public ISM_Certificate_RequestDTO Onload([FromBody]ISM_Certificate_RequestDTO data)
        {
            return _eei.Onload(data);
        }

        [Route("getapprovalreport")]
        public ISM_Certificate_RequestDTO getapprovalreport([FromBody]ISM_Certificate_RequestDTO data)
        {
            return _eei.getapprovalreport(data);
        }
        [Route("DocView")]
        public ISM_Certificate_RequestDTO DocView([FromBody]ISM_Certificate_RequestDTO data)
        {
            return _eei.DocView(data);
        }
        [Route("getIEDetails")]
        public ISM_Certificate_RequestDTO getIEDetails([FromBody]ISM_Certificate_RequestDTO data)
        {
            return _eei.getIEDetails(data);
        }
        [Route("getcertificate")]
        public ISM_Certificate_RequestDTO getcertificate([FromBody]ISM_Certificate_RequestDTO data)
        {
            return _eei.getcertificate(data);
        }
        [Route("getadminloaddata")]
        public ISM_Certificate_RequestDTO getadminloaddata([FromBody]ISM_Certificate_RequestDTO data)
        {
            return _eei.getadminloaddata(data);
        }
        [Route("saveadmindetails")]
        public ISM_Certificate_RequestDTO saveadmindetails([FromBody]ISM_Certificate_RequestDTO data)
        {
            return _eei.saveadmindetails(data);
        }



    }
}