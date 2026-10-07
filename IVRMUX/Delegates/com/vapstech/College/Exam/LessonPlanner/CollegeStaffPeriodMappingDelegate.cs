using CommonLibrary;
using PreadmissionDTOs.com.vaps.College.Exam.LessonPlanner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.College.Exam.LessonPlanner
{
    public class CollegeStaffPeriodMappingDelegate
    {
        CommonDelegate<CollegeStaffPeriodMappingDTO, CollegeStaffPeriodMappingDTO> _comm = new CommonDelegate<CollegeStaffPeriodMappingDTO, CollegeStaffPeriodMappingDTO>();

        public CollegeStaffPeriodMappingDTO Getdetails(CollegeStaffPeriodMappingDTO data)
        {
            return _comm.POSTcolExam(data, "CollegeStaffPeriodMappingFacade/Getdetails");
        }
        public CollegeStaffPeriodMappingDTO getemployeedetails(CollegeStaffPeriodMappingDTO data)
        {
            return _comm.POSTcolExam(data, "CollegeStaffPeriodMappingFacade/getemployeedetails");
        }
        public CollegeStaffPeriodMappingDTO onchangecourse(CollegeStaffPeriodMappingDTO data)
        {
            return _comm.POSTcolExam(data, "CollegeStaffPeriodMappingFacade/onchangecourse");
        }
        public CollegeStaffPeriodMappingDTO onchangebranch(CollegeStaffPeriodMappingDTO data)
        {
            return _comm.POSTcolExam(data, "CollegeStaffPeriodMappingFacade/onchangebranch");
        }
        public CollegeStaffPeriodMappingDTO onchangesemster(CollegeStaffPeriodMappingDTO data)
        {
            return _comm.POSTcolExam(data, "CollegeStaffPeriodMappingFacade/onchangesemster");
        }
        public CollegeStaffPeriodMappingDTO onchangesection(CollegeStaffPeriodMappingDTO data)
        {
            return _comm.POSTcolExam(data, "CollegeStaffPeriodMappingFacade/onchangesection");
        }
        public CollegeStaffPeriodMappingDTO getsearchdetails(CollegeStaffPeriodMappingDTO data)
        {
            return _comm.POSTcolExam(data, "CollegeStaffPeriodMappingFacade/getsearchdetails");
        }
        public CollegeStaffPeriodMappingDTO savedata(CollegeStaffPeriodMappingDTO data)
        {
            return _comm.POSTcolExam(data, "CollegeStaffPeriodMappingFacade/savedata");
        }

        // Staff Preiod Transaction
        public CollegeStaffPeriodMappingDTO Getdetailstransaction(CollegeStaffPeriodMappingDTO data)
        {
            return _comm.POSTcolExam(data, "CollegeStaffPeriodMappingFacade/Getdetailstransaction");
        }
        public CollegeStaffPeriodMappingDTO getsearchdetailstransaction(CollegeStaffPeriodMappingDTO data)
        {
            return _comm.POSTcolExam(data, "CollegeStaffPeriodMappingFacade/getsearchdetailstransaction");
        }
        public CollegeStaffPeriodMappingDTO savedatatransaction(CollegeStaffPeriodMappingDTO data)
        {
            return _comm.POSTcolExam(data, "CollegeStaffPeriodMappingFacade/savedatatransaction");
        }
        
    }
}
