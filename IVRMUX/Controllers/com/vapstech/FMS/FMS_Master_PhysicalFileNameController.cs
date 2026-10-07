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
    public class FMS_Master_PhysicalFileNameController : Controller
    {
        FMS_Master_PhysicalFileNameDelegate od = new FMS_Master_PhysicalFileNameDelegate();

        // Master Physcial File
        [HttpGet]
        [Route("loadPhysicalFiledata/{id:int}")]
        public FMS_Master_PhysicalFileNameDTO loadPhysicalFiledata(int id)
        {
            FMS_Master_PhysicalFileNameDTO data = new FMS_Master_PhysicalFileNameDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return od.loadPhysicalFiledata(data);
        }

        [HttpPost]
        [Route("savedata")]
        public FMS_Master_PhysicalFileNameDTO savedata([FromBody] FMS_Master_PhysicalFileNameDTO data)
        {           
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return od.savedata(data);
        }
                
        [Route("editRecord")]
        public FMS_Master_PhysicalFileNameDTO editRecord([FromBody] FMS_Master_PhysicalFileNameDTO data)
        {              
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return od.editRecord(data);
        }
       
        [Route("ActiveDeactiveRecord")]
        public FMS_Master_PhysicalFileNameDTO ActiveDeactiveRecord([FromBody] FMS_Master_PhysicalFileNameDTO data)
        { 
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return od.ActiveDeactiveRecord(data);
        }

        [Route("ViewPhysicalFileDeptList")]
        public FMS_Master_PhysicalFileNameDTO ViewPhysicalFileDeptList([FromBody] FMS_Master_PhysicalFileNameDTO data)
        {
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return od.ViewPhysicalFileDeptList(data);
        }

        [Route("ActiveDeactiveDeptPhyscialfile")]
        public FMS_Master_PhysicalFileNameDTO ActiveDeactiveDeptPhyscialfile([FromBody] FMS_Master_PhysicalFileNameDTO data)
        {
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return od.ActiveDeactiveDeptPhyscialfile(data);
        }

        // Master File Category

        [HttpGet]
        [Route("loadFileCategorydata/{id:int}")]
        public FMS_Master_PhysicalFileNameDTO loadFileCategorydata(int id)
        {
            FMS_Master_PhysicalFileNameDTO data = new FMS_Master_PhysicalFileNameDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return od.loadFileCategorydata(data);
        }

        [HttpPost]
        [Route("saveFileCategory")]
        public FMS_Master_PhysicalFileNameDTO saveFileCategory([FromBody] FMS_Master_PhysicalFileNameDTO data)
        {            
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return od.saveFileCategory(data);
        }
       
        [Route("editRecord2")]
        public FMS_Master_PhysicalFileNameDTO editRecord2([FromBody] FMS_Master_PhysicalFileNameDTO data)
        {                     
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return od.editRecord2(data);
        }
        
        [Route("ActiveDeactiveRecord2")]
        public FMS_Master_PhysicalFileNameDTO ActiveDeactiveRecord2([FromBody] FMS_Master_PhysicalFileNameDTO data)
        {         
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return od.ActiveDeactiveRecord2(data);
        }

        [HttpPost]
        [Route("ViewDeptList")]
        public FMS_Master_PhysicalFileNameDTO ViewDeptList([FromBody] FMS_Master_PhysicalFileNameDTO data)
        {            
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return od.ViewDeptList(data);
        }

        [HttpPost]
        [Route("ActiveDeactiveDept")]
        public FMS_Master_PhysicalFileNameDTO ActiveDeactiveDept([FromBody] FMS_Master_PhysicalFileNameDTO data)
        {           
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return od.ActiveDeactiveDept(data);
        }

        // Master Dispatch Mode
        [HttpGet]
        [Route("loadDispatchModedata/{id:int}")]
        public FMS_Master_PhysicalFileNameDTO loadDispatchModedata(int id)
        {
            FMS_Master_PhysicalFileNameDTO data = new FMS_Master_PhysicalFileNameDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return od.loadDispatchModedata(data);
        }

        [HttpPost]
        [Route("saveDispatchMode")]
        public FMS_Master_PhysicalFileNameDTO saveDispatchMode([FromBody] FMS_Master_PhysicalFileNameDTO data)
        {            
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return od.saveDispatchMode(data);
        }
        
        [Route("editRecord3")]
        public FMS_Master_PhysicalFileNameDTO editRecord3([FromBody] FMS_Master_PhysicalFileNameDTO data)
        {       
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return od.editRecord3(data);
        }
        
        [Route("ActiveDeactiveRecord3")]
        public FMS_Master_PhysicalFileNameDTO ActiveDeactiveRecord3([FromBody] FMS_Master_PhysicalFileNameDTO data)
        {      
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return od.ActiveDeactiveRecord3(data);
        }

        // Master File Cabinet
        [HttpGet]
        [Route("loadFileCabinetdata/{id:int}")]
        public FMS_Master_PhysicalFileNameDTO loadFileCabinetdata(int id)
        {
            FMS_Master_PhysicalFileNameDTO data = new FMS_Master_PhysicalFileNameDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return od.loadFileCabinetdata(data);
        }           

        [HttpPost]
        [Route("saveFileCabinet")]
        public FMS_Master_PhysicalFileNameDTO saveFileCabinet([FromBody] FMS_Master_PhysicalFileNameDTO data)
        {            
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return od.saveFileCabinet(data);
        }
         
        [Route("editRecord4")]
        public FMS_Master_PhysicalFileNameDTO editRecord4([FromBody] FMS_Master_PhysicalFileNameDTO data)
        {    
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return od.editRecord4(data);
        }
        
        [Route("ActiveDeactiveRecord4")]
        public FMS_Master_PhysicalFileNameDTO ActiveDeactiveRecord4([FromBody] FMS_Master_PhysicalFileNameDTO data)
        { 
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return od.ActiveDeactiveRecord4(data);
        }

        // Configuration

        [HttpGet]
        [Route("loadreferenceconfg/{id:int}")]
        public FMS_Master_PhysicalFileNameDTO loadreferenceconfg(int id)
        {
            FMS_Master_PhysicalFileNameDTO data = new FMS_Master_PhysicalFileNameDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return od.loadreferenceconfg(data);
        }

        [HttpPost]
        [Route("savereferenceconfg")]
        public FMS_Master_PhysicalFileNameDTO savereferenceconfg([FromBody]FMS_Master_PhysicalFileNameDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return od.savereferenceconfg(data);
        }

        [HttpGet]
        [Route("editreferenceconfg/{id:int}")]
        public FMS_Master_PhysicalFileNameDTO editreferenceconfg(int id)
        {
            FMS_Master_PhysicalFileNameDTO data = new FMS_Master_PhysicalFileNameDTO();
            data.FMSRNCON_Id = id;
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return od.editreferenceconfg(data);
        }

        [HttpGet]
        [Route("ActiveDeactivereferenceconfg/{id:int}")]
        public FMS_Master_PhysicalFileNameDTO ActiveDeactivereferenceconfg(int id)
        {
            FMS_Master_PhysicalFileNameDTO data = new FMS_Master_PhysicalFileNameDTO();
            data.FMSRNCON_Id = id;
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return od.ActiveDeactivereferenceconfg(data);
        }

        /* File Name & Cabinet Mapping */

        [Route("LoadFilenameAndCabinet/{id:int}")]
        public FMS_Master_PhysicalFileNameDTO LoadFilenameAndCabinet(int id)
        {
            FMS_Master_PhysicalFileNameDTO data = new FMS_Master_PhysicalFileNameDTO();          
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return od.LoadFilenameAndCabinet(data);
        }

        [Route("OnChangephysicalname")]
        public FMS_Master_PhysicalFileNameDTO OnChangephysicalname([FromBody]FMS_Master_PhysicalFileNameDTO data)
        {            
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return od.OnChangephysicalname(data);
        }

        [Route("OnChangeFileCabinet")]
        public FMS_Master_PhysicalFileNameDTO OnChangeFileCabinet([FromBody]FMS_Master_PhysicalFileNameDTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return od.OnChangeFileCabinet(data);
        }

        [Route("SaveFileNameAndCabinet")]
        public FMS_Master_PhysicalFileNameDTO SaveFileNameAndCabinet([FromBody]FMS_Master_PhysicalFileNameDTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return od.SaveFileNameAndCabinet(data);
        }

        [Route("Editfilenamecabinet")]
        public FMS_Master_PhysicalFileNameDTO Editfilenamecabinet([FromBody]FMS_Master_PhysicalFileNameDTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return od.Editfilenamecabinet(data);
        }

        [Route("ActivateRecord5")]
        public FMS_Master_PhysicalFileNameDTO ActivateRecord5([FromBody]FMS_Master_PhysicalFileNameDTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return od.ActivateRecord5(data);
        }

        // On Change Of Instituion 
        [Route("OnChangeInstitution")]
        public FMS_Master_PhysicalFileNameDTO OnChangeInstitution([FromBody] FMS_Master_PhysicalFileNameDTO data)
        {
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return od.OnChangeInstitution(data);
        }       
    }
}
