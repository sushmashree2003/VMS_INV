using CommonLibrary;
using PreadmissionDTOs.com.vaps.College.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace corewebapi18072016.Delegates.com.vapstech.College.Admission
{
    public class CollegeStudentAdmissionDelegate
    {
        CommonDelegate<AdmMasterCollegeStudentDTO, AdmMasterCollegeStudentDTO> common = new CommonDelegate<AdmMasterCollegeStudentDTO, AdmMasterCollegeStudentDTO>();

        public AdmMasterCollegeStudentDTO Getdetails(AdmMasterCollegeStudentDTO obj)
        {
            return common.clgadmissionbypost(obj, "CollegeStudentAdmissionFacade/Getdetails/");
        }
        public AdmMasterCollegeStudentDTO getCourse(AdmMasterCollegeStudentDTO data)
        {
            return common.clgadmissionbypost(data, "CollegeStudentAdmissionFacade/getCourse/");
        }
        public AdmMasterCollegeStudentDTO getBranch(AdmMasterCollegeStudentDTO dt)
        {
            return common.clgadmissionbypost(dt, "CollegeStudentAdmissionFacade/getBranch/");
        }
        public AdmMasterCollegeStudentDTO getSemester(AdmMasterCollegeStudentDTO dto)
        {
            return common.clgadmissionbypost(dto, "CollegeStudentAdmissionFacade/getSemester/");
        }
        public AdmMasterCollegeStudentDTO getCaste(AdmMasterCollegeStudentDTO dto)
        {
            return common.clgadmissionbypost(dto, "CollegeStudentAdmissionFacade/getCaste/");
        }
        public AdmMasterCollegeStudentDTO getQuotaCategory(AdmMasterCollegeStudentDTO dto)
        {
            return common.clgadmissionbypost(dto, "CollegeStudentAdmissionFacade/getQuotaCategory/");
        }
        public AdmMasterCollegeStudentDTO saveStudentDetails(AdmMasterCollegeStudentDTO dto)
        {
            return common.clgadmissionbypost(dto, "CollegeStudentAdmissionFacade/saveStudentDetails/");
        }
        public AdmMasterCollegeStudentDTO Edit(AdmMasterCollegeStudentDTO data)
        {
            return common.clgadmissionbypost(data, "CollegeStudentAdmissionFacade/Edit/");
        }
        public AdmMasterCollegeStudentDTO checkDuplicate(AdmMasterCollegeStudentDTO data)
        {
            return common.clgadmissionbypost(data, "CollegeStudentAdmissionFacade/checkDuplicate/");
        }
        public AdmMasterCollegeStudentDTO getdpstate(AdmMasterCollegeStudentDTO data)
        {
            return common.clgadmissionbypost(data, "CollegeStudentAdmissionFacade/getdpstate/");
        }
        public AdmMasterCollegeStudentDTO saveAddress(AdmMasterCollegeStudentDTO add)
        {
            return common.clgadmissionbypost(add, "CollegeStudentAdmissionFacade/saveAddress/");
        }
        public AdmMasterCollegeStudentDTO saveParentsDetails(AdmMasterCollegeStudentDTO ParentsData)
        {
            return common.clgadmissionbypost(ParentsData, "CollegeStudentAdmissionFacade/saveParentsDetails/");
        }
        public AdmMasterCollegeStudentDTO StateByCountryName(AdmMasterCollegeStudentDTO country)
        {
            return common.clgadmissionbypost(country, "CollegeStudentAdmissionFacade/StateByCountryName/");
        }
        public AdmMasterCollegeStudentDTO saveOthersDetails(AdmMasterCollegeStudentDTO others)
        {
            return common.clgadmissionbypost(others, "CollegeStudentAdmissionFacade/saveOthersDetails/");
        }
        public AdmMasterCollegeStudentDTO saveDocuments(AdmMasterCollegeStudentDTO docs)
        {
            return common.clgadmissionbypost(docs, "CollegeStudentAdmissionFacade/saveDocuments/");
        }
        public AdmMasterCollegeStudentDTO SearchByColumn(AdmMasterCollegeStudentDTO docs)
        {
            return common.clgadmissionbypost(docs, "CollegeStudentAdmissionFacade/SearchByColumn/");
        }
        public AdmMasterCollegeStudentDTO DeleteEntry(AdmMasterCollegeStudentDTO docs)
        {
            return common.clgadmissionbypost(docs, "CollegeStudentAdmissionFacade/DeleteEntry/");
        }
        

    }
}
