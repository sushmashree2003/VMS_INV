using CommonLibrary;
using PreadmissionDTOs.com.vaps.College.Exam.LessonPlanner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.College.Exam.LessonPlanner
{
    public class CollegeMasterTopicDelegate
    {

        CommonDelegate<CollegeMasterTopicDTO, CollegeMasterTopicDTO> _comm = new CommonDelegate<CollegeMasterTopicDTO, CollegeMasterTopicDTO>();

        public CollegeMasterTopicDTO Getdetails(CollegeMasterTopicDTO data)
        {
            return _comm.POSTcolExam(data, "CollegeMasterTopicFacade/Getdetails");
        }
        public CollegeMasterTopicDTO savedetails(CollegeMasterTopicDTO data)
        {
            return _comm.POSTcolExam(data, "CollegeMasterTopicFacade/savedetails");
        }
        public CollegeMasterTopicDTO editdeatils(CollegeMasterTopicDTO data)
        {
            return _comm.POSTcolExam(data, "CollegeMasterTopicFacade/editdeatils");
        }
        public CollegeMasterTopicDTO deactivate(CollegeMasterTopicDTO data)
        {
            return _comm.POSTcolExam(data, "CollegeMasterTopicFacade/deactivate");
        }
        public CollegeMasterTopicDTO validateordernumber(CollegeMasterTopicDTO data)
        {
            return _comm.POSTcolExam(data, "CollegeMasterTopicFacade/validateordernumber");
        }
    }
}
