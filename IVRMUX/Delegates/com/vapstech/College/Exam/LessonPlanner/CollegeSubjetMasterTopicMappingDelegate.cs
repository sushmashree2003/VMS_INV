using CommonLibrary;
using PreadmissionDTOs.com.vaps.College.Exam.LessonPlanner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.College.Exam.LessonPlanner
{
    public class CollegeSubjetMasterTopicMappingDelegate
    {
        CommonDelegate<CollegeSubjetMasterTopicMappingDTO, CollegeSubjetMasterTopicMappingDTO> _comm = new CommonDelegate<CollegeSubjetMasterTopicMappingDTO, CollegeSubjetMasterTopicMappingDTO>();

        public CollegeSubjetMasterTopicMappingDTO Getdetails(CollegeSubjetMasterTopicMappingDTO data)
        {
            return _comm.POSTcolExam(data, "CollegeSubjetMasterTopicMappingFacade/Getdetails");
        }
        public CollegeSubjetMasterTopicMappingDTO onchangesubject(CollegeSubjetMasterTopicMappingDTO data)
        {
            return _comm.POSTcolExam(data, "CollegeSubjetMasterTopicMappingFacade/onchangesubject");
        }
        public CollegeSubjetMasterTopicMappingDTO savedetails(CollegeSubjetMasterTopicMappingDTO data)
        {
            return _comm.POSTcolExam(data, "CollegeSubjetMasterTopicMappingFacade/savedetails");
        }
        public CollegeSubjetMasterTopicMappingDTO deactivate(CollegeSubjetMasterTopicMappingDTO data)
        {
            return _comm.POSTcolExam(data, "CollegeSubjetMasterTopicMappingFacade/deactivate");
        }
        public CollegeSubjetMasterTopicMappingDTO editdeatils(CollegeSubjetMasterTopicMappingDTO data)
        {
            return _comm.POSTcolExam(data, "CollegeSubjetMasterTopicMappingFacade/editdeatils");
        }
        public CollegeSubjetMasterTopicMappingDTO validateordernumber(CollegeSubjetMasterTopicMappingDTO data)
        {
            return _comm.POSTcolExam(data, "CollegeSubjetMasterTopicMappingFacade/validateordernumber");
        }
    }
}
