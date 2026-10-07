using CommonLibrary;
using PreadmissionDTOs.com.vaps.Exam.LessonPlanner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.Exam.LessonPlanner
{
    public class SchoolSubjectWithMasterTopicMappingDelegate
    {
        CommonDelegate<SchoolSubjectWithMasterTopicMappingDTO, SchoolSubjectWithMasterTopicMappingDTO> _comm = new CommonDelegate<SchoolSubjectWithMasterTopicMappingDTO, SchoolSubjectWithMasterTopicMappingDTO>();

        public SchoolSubjectWithMasterTopicMappingDTO Getdetails(SchoolSubjectWithMasterTopicMappingDTO data)
        {
            return _comm.POSTDataExam(data, "SchoolSubjectWithMasterTopicMappingFacade/Getdetails");
        }
        public SchoolSubjectWithMasterTopicMappingDTO onchangesubject(SchoolSubjectWithMasterTopicMappingDTO data)
        {
            return _comm.POSTDataExam(data, "SchoolSubjectWithMasterTopicMappingFacade/onchangesubject");
        }        
        public SchoolSubjectWithMasterTopicMappingDTO savedetails(SchoolSubjectWithMasterTopicMappingDTO data)
        {
            return _comm.POSTDataExam(data, "SchoolSubjectWithMasterTopicMappingFacade/savedetails");
        }       
        public SchoolSubjectWithMasterTopicMappingDTO deactivate(SchoolSubjectWithMasterTopicMappingDTO data)
        {
            return _comm.POSTDataExam(data, "SchoolSubjectWithMasterTopicMappingFacade/deactivate");
        }
        public SchoolSubjectWithMasterTopicMappingDTO editdeatils(SchoolSubjectWithMasterTopicMappingDTO data)
        {
            return _comm.POSTDataExam(data, "SchoolSubjectWithMasterTopicMappingFacade/editdeatils");
        }
        public SchoolSubjectWithMasterTopicMappingDTO validateordernumber(SchoolSubjectWithMasterTopicMappingDTO data)
        {
            return _comm.POSTDataExam(data, "SchoolSubjectWithMasterTopicMappingFacade/validateordernumber");
        }
    }
}
