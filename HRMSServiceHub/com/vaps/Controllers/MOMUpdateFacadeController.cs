using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HRMSServiceHub.com.vaps.Interfaces;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.HRMS;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HRMSServiceHub.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class MOMUpdateFacadeController : Controller
    {
        public MOMUpdateInterface _Interfae;
        // GET: api/<controller>
        public MOMUpdateFacadeController(MOMUpdateInterface para)
        {
            _Interfae = para;
        }



        //===////////////////////////////////////trans/////////////////////////////////////////////////////////



        [Route("getonloaddata")]
        public MOMUpdateDTO getonloaddata([FromBody]MOMUpdateDTO data)
        {
            return _Interfae.getonloaddata(data);
        }
        [Route("savetransaction")]
        public MOMUpdateDTO savetransaction([FromBody]MOMUpdateDTO data)
        {
            return _Interfae.savetransaction(data);
        }
        [Route("viewflies")]
        public MOMUpdateDTO viewflies([FromBody]MOMUpdateDTO data)
        {
            return _Interfae.viewflies(data);
        }
        [Route("deactivate")]
        public MOMUpdateDTO deactivate([FromBody]MOMUpdateDTO data)
        {
            return _Interfae.deactivate(data);
        }
        [Route("Edittrans")]
        public MOMUpdateDTO Edittrans([FromBody]MOMUpdateDTO data)
        {
            return _Interfae.Edittrans(data);
        }
        [Route("oldmeetingdetail")]
        public MOMUpdateDTO oldmeetingdetail([FromBody]MOMUpdateDTO data)
        {
            return _Interfae.oldmeetingdetail(data);
        }

        //Update///////////////////////////////////////////////////////
        [Route("getDetails")]
        public MOMUpdateDTO getDetails([FromBody]MOMUpdateDTO data)
        {
            return _Interfae.getDetails(data);
        }
        [Route("UpdateDetails")]
        public MOMUpdateDTO UpdateDetails([FromBody]MOMUpdateDTO data)
        {
            return _Interfae.UpdateDetails(data);
        }
        [Route("Edit")]
        public MOMUpdateDTO Edit([FromBody]MOMUpdateDTO data)
        {
            return _Interfae.Edit(data);
        }
        [Route("viewuploadflies")]
        public MOMUpdateDTO viewuploadflies([FromBody]MOMUpdateDTO data)
        {
            return _Interfae.viewuploadflies(data);
        }
        [Route("saveData")]
        public MOMUpdateDTO saveData([FromBody]MOMUpdateDTO data)
        {
            return _Interfae.saveData(data);
        }

        [Route("checkvalidation")]
        public Task<MOMUpdateDTO> checkvalidation([FromBody]MOMUpdateDTO data)
        {
            return _Interfae.checkvalidation(data);
        }

        //[Route("save_excel_data")]
        //public Task<MOMUpdateDTO> save_excel_data([FromBody]MOMUpdateDTO data)
        //{
        //    return _Interfae.save_excel_data(data);
        //}

        ////report////=======================
        [Route("getreport")]
        public MOMUpdateDTO getreport([FromBody]MOMUpdateDTO data)
        {
            return _Interfae.getreport(data);
        }

        [Route("showreport")]
        public MOMUpdateDTO showreport([FromBody]MOMUpdateDTO data)
        {
            return _Interfae.showreport(data);
        }
        [Route("viewdetails")]
        public MOMUpdateDTO viewdetails([FromBody]MOMUpdateDTO data)
        {
            return _Interfae.viewdetails(data);
        }
        //Approval//////////////////////////////////////////////
        [Route("getdata")]
        public MOMUpdateDTO getdata([FromBody]MOMUpdateDTO data)
        {
            return _Interfae.getdata(data);
        }
        [Route("approvaldata")]
        public MOMUpdateDTO approvaldata([FromBody]MOMUpdateDTO data)
        {
            return _Interfae.approvaldata(data);
        }
        [Route("getstatus")]
        public MOMUpdateDTO getstatus([FromBody]MOMUpdateDTO data)
        {
            return _Interfae.getstatus(data);
        }
        [Route("oldmeeting")]
        public MOMUpdateDTO oldmeeting([FromBody]MOMUpdateDTO data)
        {
            return _Interfae.oldmeeting(data);
        }
        [Route("sendapprovalMail")]
        public MOMUpdateDTO sendapprovalMail([FromBody] MOMUpdateDTO data)
        {
            return _Interfae.sendapprovalMail(data);
        }

        [Route("SaveVendors")]
        public MOMUpdateDTO SaveVendors([FromBody] MOMUpdateDTO data)
        {
            return _Interfae.SaveVendors(data);
        }
        [Route("onloadVendors")]
        public MOMUpdateDTO onloadVendors([FromBody] MOMUpdateDTO data)
        {
            return _Interfae.onloadVendors(data);
        }
        
    }
}
