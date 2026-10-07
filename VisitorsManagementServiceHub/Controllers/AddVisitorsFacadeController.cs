using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.VisitorsManagement;
using VisitorsManagementServiceHub.Interfaces;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace VisitorsManagementServiceHub.Controllers
{
    [Route("api/[controller]")]
    public class AddVisitorsFacadeController : Controller
    {
        public AddVisitorsInterface interobj;
        public AddVisitorsFacadeController(AddVisitorsInterface obj)
        {
            interobj = obj;
        }

        // POST api/<controller>
        [HttpPost]
        [Route("getDetails")]
        public AddVisitorsDTO getDetails([FromBody]AddVisitorsDTO data)
        {
            return interobj.getDetails(data);
        }
        [Route("EditDetails")]
        public AddVisitorsDTO EditDetails([FromBody] AddVisitorsDTO id)
        {
            return interobj.EditDetails(id);
        }
        [Route("saveData")]
        public AddVisitorsDTO saveData([FromBody]AddVisitorsDTO data)
        {
            return interobj.saveData(data);
        }
        [Route("deactivate")]
        public AddVisitorsDTO deactivate([FromBody]AddVisitorsDTO data)
        {
            return interobj.deactivate(data);
        }

        [Route("changeofradio")]
        public Task<AddVisitorsDTO> changeofradio([FromBody]AddVisitorsDTO data)
        {
            return interobj.changeofradio(data);
        }

        [Route("GetVisitorDetails")]
        public AddVisitorsDTO GetVisitorDetails([FromBody]AddVisitorsDTO data)
        {
            return interobj.GetVisitorDetails(data);
        }

        [Route("Getmultivisitordetails")]
        public AddVisitorsDTO Getmultivisitordetails([FromBody]AddVisitorsDTO data)
        {
            return interobj.Getmultivisitordetails(data);
        }       

        [Route("UpdateStatus")]
        public AddVisitorsDTO UpdateStatus([FromBody]AddVisitorsDTO data)
        {
            return interobj.UpdateStatus(data);
        }

        [Route("getAssignDetails")]
        public AddVisitorsDTO getAssignDetails([FromBody]AddVisitorsDTO data)
        {
            return interobj.getAssignDetails(data);
        }

        [Route("getVisitorAssignDetails")]
        public AddVisitorsDTO getVisitorAssignDetails([FromBody]AddVisitorsDTO data)
        {
            return interobj.getVisitorAssignDetails(data);
        }

        [Route("saveAssignedData")]
        public AddVisitorsDTO saveAssignedData([FromBody]AddVisitorsDTO data)
        {
            return interobj.saveAssignedData(data);
        }

        [Route("SearchAppVisitors")]
        public AddVisitorsDTO SearchAppVisitors([FromBody]AddVisitorsDTO data)
        {
            return interobj.SearchAppVisitors(data);
        }
        [Route("searchfilter")]
        public AddVisitorsDTO searchfilter([FromBody]AddVisitorsDTO data)
        {
            return interobj.searchfilter(data);
        }

        [Route("visitornameschange")]
        public AddVisitorsDTO visitornameschange([FromBody]AddVisitorsDTO id)
        {
            return interobj.visitornameschange(id);
        }

        [Route("GetVisitorAssginDetails")]
        public AddVisitorsDTO GetVisitorAssginDetails([FromBody]AddVisitorsDTO id)
        {
            return interobj.GetVisitorAssginDetails(id);
        }

        [Route("addtocart")]
        public AddVisitorsDTO addtocart([FromBody]AddVisitorsDTO id)
        {
            return interobj.addtocart(id);
        }

        //Tenent Visitor Managment
        [HttpPost]
        [Route("getTenentVisitorDetails")]
        public AddVisitorsDTO getTenentVisitorDetails([FromBody]AddVisitorsDTO data)
        {
            return interobj.getTenentVisitorDetails(data);
        }
   
        [Route("QRCODECertificates")]

        public async Task<AddVisitorsDTO> QRCODECertificates([FromBody]AddVisitorsDTO dto)
        {
            return await interobj.QRCODECertificates(dto);
        }
        [Route("FetchDetails")]
        public AddVisitorsDTO FetchDetails([FromBody]AddVisitorsDTO data)
        {
            return interobj.FetchDetails(data);
        }



    }
}
