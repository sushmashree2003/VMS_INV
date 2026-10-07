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
    public class FMS_Master_PhysicalFileNameFacadeController : Controller
    {

        public FMS_Master_PhysicalFileNameInterface _org;
        public FMS_Master_PhysicalFileNameFacadeController(FMS_Master_PhysicalFileNameInterface maspag)
        {
            _org = maspag;
        }

        [HttpPost]
        [Route("loadPhysicalFiledata")]
        public FMS_Master_PhysicalFileNameDTO loadPhysicalFiledata([FromBody] FMS_Master_PhysicalFileNameDTO data)
        {
            return _org.loadPhysicalFiledata(data);
        }

        [HttpPost]
        [Route("loadFileCategorydata")]
        public FMS_Master_PhysicalFileNameDTO loadFileCategorydata([FromBody] FMS_Master_PhysicalFileNameDTO data)
        {
            return _org.loadFileCategorydata(data);
        }

        [HttpPost]
        [Route("loadDispatchModedata")]
        public FMS_Master_PhysicalFileNameDTO loadDispatchModedata([FromBody] FMS_Master_PhysicalFileNameDTO data)
        {
            return _org.loadDispatchModedata(data);
        }

        [HttpPost]
        [Route("loadFileCabinetdata")]
        public FMS_Master_PhysicalFileNameDTO loadFileCabinetdata([FromBody] FMS_Master_PhysicalFileNameDTO data)
        {
            return _org.loadFileCabinetdata(data);
        }

        [HttpPost]
        [Route("savedata")]
        public FMS_Master_PhysicalFileNameDTO savedata([FromBody] FMS_Master_PhysicalFileNameDTO data)
        {
            return _org.savedata(data);
        }

        [HttpPost]
        [Route("saveFileCategory")]
        public FMS_Master_PhysicalFileNameDTO saveFileCategory([FromBody] FMS_Master_PhysicalFileNameDTO data)
        {
            return _org.saveFileCategory(data);
        }

        [HttpPost]
        [Route("ViewDeptList")]
        public FMS_Master_PhysicalFileNameDTO ViewDeptList([FromBody] FMS_Master_PhysicalFileNameDTO data)
        {
            return _org.ViewDeptList(data);
        }

        [HttpPost]
        [Route("ActiveDeactiveDept")]
        public FMS_Master_PhysicalFileNameDTO ActiveDeactiveDept([FromBody] FMS_Master_PhysicalFileNameDTO data)
        {
            return _org.ActiveDeactiveDept(data);
        }

        [HttpPost]
        [Route("saveDispatchMode")]
        public FMS_Master_PhysicalFileNameDTO saveDispatchMode([FromBody] FMS_Master_PhysicalFileNameDTO data)
        {
            return _org.saveDispatchMode(data);
        }

        [HttpPost]
        [Route("saveFileCabinet")]
        public FMS_Master_PhysicalFileNameDTO saveFileCabinet([FromBody] FMS_Master_PhysicalFileNameDTO data)
        {
            return _org.saveFileCabinet(data);
        }

        [HttpPost]
        [Route("editRecord")]
        public FMS_Master_PhysicalFileNameDTO editRecord([FromBody] FMS_Master_PhysicalFileNameDTO data)
        {
            return _org.editRecord(data);
        }

        [HttpPost]
        [Route("editRecord2")]
        public FMS_Master_PhysicalFileNameDTO editRecord2([FromBody] FMS_Master_PhysicalFileNameDTO data)
        {
            return _org.editRecord2(data);
        }

        [HttpPost]
        [Route("editRecord3")]
        public FMS_Master_PhysicalFileNameDTO editRecord3([FromBody] FMS_Master_PhysicalFileNameDTO data)
        {
            return _org.editRecord3(data);
        }

        [HttpPost]
        [Route("editRecord4")]
        public FMS_Master_PhysicalFileNameDTO editRecord4([FromBody] FMS_Master_PhysicalFileNameDTO data)
        {
            return _org.editRecord4(data);
        }

        [HttpPost]
        [Route("ActiveDeactiveRecord")]
        public FMS_Master_PhysicalFileNameDTO ActiveDeactiveRecord([FromBody] FMS_Master_PhysicalFileNameDTO data)
        {
            return _org.ActiveDeactiveRecord(data);
        }

        [HttpPost]
        [Route("ActiveDeactiveRecord2")]
        public FMS_Master_PhysicalFileNameDTO ActiveDeactiveRecord2([FromBody] FMS_Master_PhysicalFileNameDTO data)
        {
            return _org.ActiveDeactiveRecord2(data);
        }

        [HttpPost]
        [Route("ActiveDeactiveRecord3")]
        public FMS_Master_PhysicalFileNameDTO ActiveDeactiveRecord3([FromBody] FMS_Master_PhysicalFileNameDTO data)
        {
            return _org.ActiveDeactiveRecord3(data);
        }

        [HttpPost]
        [Route("ActiveDeactiveRecord4")]
        public FMS_Master_PhysicalFileNameDTO ActiveDeactiveRecord4([FromBody] FMS_Master_PhysicalFileNameDTO data)
        {
            return _org.ActiveDeactiveRecord4(data);
        }

        [HttpPost]
        [Route("loadreferenceconfg")]
        public FMS_Master_PhysicalFileNameDTO loadreferenceconfg([FromBody] FMS_Master_PhysicalFileNameDTO data)
        {
            return _org.loadreferenceconfg(data);
        }
        
        [HttpPost]
        [Route("savereferenceconfg")]
        public FMS_Master_PhysicalFileNameDTO savereferenceconfg([FromBody]FMS_Master_PhysicalFileNameDTO data)
        {
            return _org.savereferenceconfg(data);
        }

        [HttpPost]
        [Route("editreferenceconfg")]
        public FMS_Master_PhysicalFileNameDTO editreferenceconfg([FromBody]FMS_Master_PhysicalFileNameDTO data)
        {
            return _org.editreferenceconfg(data);
        }

        [HttpPost]
        [Route("ActiveDeactivereferenceconfg")]
        public FMS_Master_PhysicalFileNameDTO ActiveDeactivereferenceconfg([FromBody]FMS_Master_PhysicalFileNameDTO data)
        {
            return _org.ActiveDeactivereferenceconfg(data);
        }

        /* File Name & Cabinet Mapping */

        [Route("LoadFilenameAndCabinet")]
        public FMS_Master_PhysicalFileNameDTO LoadFilenameAndCabinet([FromBody]FMS_Master_PhysicalFileNameDTO data)
        {
            return _org.LoadFilenameAndCabinet(data);
        }

        [Route("OnChangephysicalname")]
        public FMS_Master_PhysicalFileNameDTO OnChangephysicalname([FromBody]FMS_Master_PhysicalFileNameDTO data)
        {
            return _org.OnChangephysicalname(data);
        }

        [Route("OnChangeFileCabinet")]
        public FMS_Master_PhysicalFileNameDTO OnChangeFileCabinet([FromBody]FMS_Master_PhysicalFileNameDTO data)
        { 
            return _org.OnChangeFileCabinet(data);
        }

        [Route("SaveFileNameAndCabinet")]
        public FMS_Master_PhysicalFileNameDTO SaveFileNameAndCabinet([FromBody]FMS_Master_PhysicalFileNameDTO data)
        {
            return _org.SaveFileNameAndCabinet(data);
        }

        [Route("Editfilenamecabinet")]
        public FMS_Master_PhysicalFileNameDTO Editfilenamecabinet([FromBody]FMS_Master_PhysicalFileNameDTO data)
        {
            return _org.Editfilenamecabinet(data);
        }

        [Route("ActivateRecord5")]
        public FMS_Master_PhysicalFileNameDTO ActivateRecord5([FromBody]FMS_Master_PhysicalFileNameDTO data)
        {
            return _org.ActivateRecord5(data);
        }

        [Route("OnChangeInstitution")]
        public FMS_Master_PhysicalFileNameDTO OnChangeInstitution([FromBody]FMS_Master_PhysicalFileNameDTO data)
        {
            return _org.OnChangeInstitution(data);
        }

        [Route("ViewPhysicalFileDeptList")]
        public FMS_Master_PhysicalFileNameDTO ViewPhysicalFileDeptList([FromBody]FMS_Master_PhysicalFileNameDTO data)
        {
            return _org.ViewPhysicalFileDeptList(data);
        }

        [Route("ActiveDeactiveDeptPhyscialfile")]
        public FMS_Master_PhysicalFileNameDTO ActiveDeactiveDeptPhyscialfile([FromBody]FMS_Master_PhysicalFileNameDTO data)
        {
            return _org.ActiveDeactiveDeptPhyscialfile(data);
        }
    }
}
