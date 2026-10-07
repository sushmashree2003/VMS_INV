using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.College.Admission;
using corewebapi18072016.Delegates.com.vapstech.College.Admission;
using Microsoft.AspNetCore.Http;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers.com.vapstech.College.Admission
{
    [Route("api/[controller]")]
    public class CollegeStudentAdmissionController : Controller
    {
        CollegeStudentAdmissionDelegate delegates = new CollegeStudentAdmissionDelegate();
        [Route("Getdetails")]
        public AdmMasterCollegeStudentDTO Getdetails()
        {
            AdmMasterCollegeStudentDTO dto = new AdmMasterCollegeStudentDTO();
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.ASMAY_Id= Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return delegates.Getdetails(dto);
        }
        [Route("getCourse/{YearId:int}")]
        public AdmMasterCollegeStudentDTO getCourse(int YearId)
        {
            AdmMasterCollegeStudentDTO obj = new AdmMasterCollegeStudentDTO();
            obj.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            obj.ASMAY_Id = YearId;
            return delegates.getCourse(obj);
        }
        [Route("getBranch")]
        public AdmMasterCollegeStudentDTO getBranch([FromBody]AdmMasterCollegeStudentDTO dt)
        {
            dt.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return delegates.getBranch(dt);
        }
        [Route("getSemester")]
        public AdmMasterCollegeStudentDTO getSemester([FromBody]AdmMasterCollegeStudentDTO dto)
        {
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return delegates.getSemester(dto);
        }
        [Route("getCaste")]
        public AdmMasterCollegeStudentDTO getCaste([FromBody] AdmMasterCollegeStudentDTO caste)
        {
            caste.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return delegates.getCaste(caste);
        }
        [Route("getQuotaCategory/{quotaId:int}")]
        public AdmMasterCollegeStudentDTO getQuotaCategory(int quotaId)
        {
            AdmMasterCollegeStudentDTO dto = new AdmMasterCollegeStudentDTO();
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.ACQ_Id = quotaId;
            return delegates.getQuotaCategory(dto);
        }
        [Route("saveStudentDetails")]
        public AdmMasterCollegeStudentDTO saveStudentDetails([FromBody]AdmMasterCollegeStudentDTO dto)
        {
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return delegates.saveStudentDetails(dto);
        }
        [Route("Edit/{Id:int}")]
        public AdmMasterCollegeStudentDTO Edit(int Id)
        {
            AdmMasterCollegeStudentDTO edit = new AdmMasterCollegeStudentDTO();
            edit.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            edit.AMCST_Id = Id;
            return delegates.Edit(edit);
        }
        [Route("checkDuplicate")]
        public AdmMasterCollegeStudentDTO checkDuplicate([FromBody]AdmMasterCollegeStudentDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return delegates.checkDuplicate(data);
        }
        [Route("getdpstate/{countryId:int}")]
        public AdmMasterCollegeStudentDTO getdpstate(int countryId)
        {
            AdmMasterCollegeStudentDTO st = new AdmMasterCollegeStudentDTO();
            st.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            st.IVRMMC_Id = countryId;
            return delegates.getdpstate(st);
        }
        [Route("saveAddress")]
        public AdmMasterCollegeStudentDTO saveAddress([FromBody]AdmMasterCollegeStudentDTO add)
        {
            add.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return delegates.saveAddress(add);
        }
        [Route("saveParentsDetails")]
        public AdmMasterCollegeStudentDTO saveParentsDetails([FromBody]AdmMasterCollegeStudentDTO parentsData)
        {
            parentsData.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return delegates.saveParentsDetails(parentsData);
        }
        [Route("StateByCountryName")]
        public AdmMasterCollegeStudentDTO StateByCountryName([FromBody]AdmMasterCollegeStudentDTO country)
        {
            country.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return delegates.StateByCountryName(country);
        }
        [Route("saveOthersDetails")]
        public AdmMasterCollegeStudentDTO saveOthersDetails([FromBody]AdmMasterCollegeStudentDTO others)
        {
            others.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return delegates.saveOthersDetails(others);
        }
        [Route("saveDocuments")]
        public AdmMasterCollegeStudentDTO saveDocuments([FromBody]AdmMasterCollegeStudentDTO documents)
        {
            documents.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return delegates.saveDocuments(documents);
        }

        [Route("SearchByColumn")]
        public AdmMasterCollegeStudentDTO SearchByColumn([FromBody]AdmMasterCollegeStudentDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return delegates.SearchByColumn(data);
        }
        [Route("DeleteEntry")]
        public AdmMasterCollegeStudentDTO DeleteEntry([FromBody]AdmMasterCollegeStudentDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return delegates.DeleteEntry(data);
        }
    }
}
