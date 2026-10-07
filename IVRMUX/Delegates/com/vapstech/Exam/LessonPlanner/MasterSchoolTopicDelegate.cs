using CommonLibrary;
using PreadmissionDTOs.com.vaps.Exam.LessonPlanner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.Exam.LessonPlanner
{
    public class MasterSchoolTopicDelegate
    {
        CommonDelegate<MasterSchoolTopicDTO, MasterSchoolTopicDTO> _comm = new CommonDelegate<MasterSchoolTopicDTO, MasterSchoolTopicDTO>();

        public MasterSchoolTopicDTO Getdetails(MasterSchoolTopicDTO data)
        {
            return _comm.POSTDataExam(data, "MasterSchoolTopicFacade/Getdetails");
        }
        public MasterSchoolTopicDTO savedetails(MasterSchoolTopicDTO data)
        {
            return _comm.POSTDataExam(data, "MasterSchoolTopicFacade/savedetails");
        }
        public MasterSchoolTopicDTO editdeatils(MasterSchoolTopicDTO data)
        {
            return _comm.POSTDataExam(data, "MasterSchoolTopicFacade/editdeatils");
        }
        public MasterSchoolTopicDTO deactivate(MasterSchoolTopicDTO data)
        {
            return _comm.POSTDataExam(data, "MasterSchoolTopicFacade/deactivate");
        }
        public MasterSchoolTopicDTO validateordernumber(MasterSchoolTopicDTO data)
        {
            return _comm.POSTDataExam(data, "MasterSchoolTopicFacade/validateordernumber");
        }

    }
}
