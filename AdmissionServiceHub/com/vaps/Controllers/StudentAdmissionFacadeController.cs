using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AdmissionServiceHub.com.vaps.Interfaces;
using AdmissionServiceHub.com.vaps.Services;
using PreadmissionDTOs;
using Microsoft.AspNetCore.Http;
using PreadmissionDTOs.com.vaps.admission;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace AdmissionServiceHub.com.vapstech.Controllers
{
    [Route("api/[controller]")]

    public class StudentAdmissionFacadeController : Controller
    {
        public StudentAdmissionInterface _Adm_M_StudentDTO;



        public StudentAdmissionFacadeController(StudentAdmissionInterface StudentAdmissionInterface)
        {
            _Adm_M_StudentDTO = StudentAdmissionInterface;
        }

        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
       // [HttpGet]


        [Route("Getdetails")]
        public async Task<Adm_M_StudentDTO> Getdetails([FromBody] Adm_M_StudentDTO dto)//int IVRMM_Id
        {
           return await _Adm_M_StudentDTO.GetData(dto);    
        }

        [Route("getdpstate/{id:int}")]
        public Adm_M_StudentDTO getdpstate(int id)//int IVRMM_Id
        {
            return _Adm_M_StudentDTO.getdpstate(id);
        }

        [Route("getdpcities/{id:int}")]
        public Adm_M_StudentDTO getdpcities(int id)//int IVRMM_Id
        {
            return _Adm_M_StudentDTO.getdpcities(id);
        }

        [Route("GetSelectedRowDetails")]
        public Adm_M_StudentDTO GetSelectedRowDetails([FromBody]Adm_M_StudentDTO ID)
        {
            // return _reg.getregdata(reg);      
            return _Adm_M_StudentDTO.GetSelectedRowDetails(ID);
        }

        [HttpPost]
        public Adm_M_StudentDTO Post([FromBody] Adm_M_StudentDTO masterMDT)
        {
            // return _reg.getregdata(reg);
            return  _Adm_M_StudentDTO.SaveData(masterMDT);
        }
        [Route("checkDuplicate")]
        public Adm_M_StudentDTO checkDuplicate([FromBody] Adm_M_StudentDTO masterMDT)
        {
            // return _reg.getregdata(reg);
            return _Adm_M_StudentDTO.checkDuplicate(masterMDT);
        }
        [Route("getcaste")]
        public Adm_M_StudentDTO getcaste([FromBody] Adm_M_StudentDTO masterMDT)
        {
            // return _reg.getregdata(reg);
            return _Adm_M_StudentDTO.getcaste(masterMDT);
        }
        
        //[Route("doc")]
        //public StudentDocumentDTO stud_doc_upload([FromBody] StudentDocumentDTO studeta)
        //{
        //    return _Adm_M_StudentDTO.stud_doc_upload(studeta);
        //}

        [HttpDelete]
        [Route("DeleteBondEntry/{id:int}")]
        public Adm_M_StudentDTO DeleteBondEntry(int ID)
        {
            return _Adm_M_StudentDTO.DeleteBondEntry(ID);
        }

        [HttpDelete]
        [Route("DeleteEntry/{id:int}")]
        public async Task<Adm_M_StudentDTO> DeleteEntry(int ID)
        {
            return await _Adm_M_StudentDTO.DeleteEntry(ID);
        }

        [Route("yearwisetcstd")]
        public Adm_M_StudentDTO yearwisetcstd([FromBody]Adm_M_StudentDTO data)
        {
            return _Adm_M_StudentDTO.yearwisetcstd(data);
        }

        [Route("addtocart")]
        public Adm_M_StudentDTO addtocart([FromBody]Adm_M_StudentDTO data)
        {
            return _Adm_M_StudentDTO.addtocart(data);
        }
        [Route("searchbycolumn")]
        public Adm_M_StudentDTO SearchByColumn([FromBody] Adm_M_StudentDTO dto)
        {
            return _Adm_M_StudentDTO.searchByColumn(dto);
        }
        [Route("StateByCountryName")]
        public Adm_M_StudentDTO StateByCountryName([FromBody] Adm_M_StudentDTO dt)
        {
            return _Adm_M_StudentDTO.StateByCountryName(dt);
        }

        [Route("classchangemaxminage")]
        public Adm_M_StudentDTO getmaxminage([FromBody] Adm_M_StudentDTO maxmin)
        {
            return _Adm_M_StudentDTO.getmaxminage(maxmin);
        }

        [Route("savefirsttab")]
        public Task<Adm_M_StudentDTO> savefirsttab([FromBody] Adm_M_StudentDTO maxmin)
        {
            return  _Adm_M_StudentDTO.savefirsttab(maxmin);
        }
        [Route("savesecondtab")]
        public Adm_M_StudentDTO savesecondtab([FromBody] Adm_M_StudentDTO maxmin)
        {
            return _Adm_M_StudentDTO.savesecondtab(maxmin);
        }
        [Route("savethirdtab")]
        public Task<Adm_M_StudentDTO> savethirdtab([FromBody] Adm_M_StudentDTO data)
        {
            return _Adm_M_StudentDTO.savethirdtab(data);
        }
        [Route("savefourthtab")]
        public Adm_M_StudentDTO savefourthtab([FromBody] Adm_M_StudentDTO data)
        {
            return _Adm_M_StudentDTO.savefourthtab(data);
        }
        [Route("savefinaltab")]
        public Adm_M_StudentDTO savefinaltab([FromBody] Adm_M_StudentDTO data)
        {
            return _Adm_M_StudentDTO.savefinaltab(data);
        }
        

    }
}
