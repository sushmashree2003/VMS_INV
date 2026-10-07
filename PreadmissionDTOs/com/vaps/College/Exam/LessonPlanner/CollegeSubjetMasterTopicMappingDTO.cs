using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.College.Exam.LessonPlanner
{
    public class CollegeSubjetMasterTopicMappingDTO
    {
        public long LPSMTM_Id { get; set; }
        public long MI_Id { get; set; }
        public long ISMS_Id { get; set; }
        public long LPMT_Id { get; set; }
        public int LPSMTM_Order { get; set; }
        public bool LPSMTM_ActiveFlag { get; set; }
        public long Userid { get; set; }
        public bool returnval { get; set; }
        public string message { get; set; }
        public string ISMS_SubjectName { get; set; }
        public string LPMT_TopicName { get; set; }
        public long ISMS_OrderFlag { get; set; }
        public long LPMT_Topicorder { get; set; }
        public Array getdetails { get; set; }
        public Array activedetails { get; set; }
        public Array topicdetails { get; set; }
        public Array getsubjectlist { get; set; }
        public Array getorderdetails { get; set; }
        public CollegeSubjectWithMasterTopicMappingTempDTO[] CollegeSubjectWithMasterTopicMappingTempDTO { get; set; }
        public CollegeSubjectWithMasterTopicMappingTemporderDTO[] CollegeSubjectWithMasterTopicMappingTemporderDTO { get; set; }
    }
    public class CollegeSubjectWithMasterTopicMappingTempDTO
    {
        public long LPMT_Id { get; set; }
        public string LPMT_TopicName { get; set; }
    }

    public class CollegeSubjectWithMasterTopicMappingTemporderDTO
    {
        public long LPSMTM_Id { get; set; }
        public long LPMT_Id { get; set; }
        public long ISMS_Id { get; set; }
        public int LPSMTM_Order { get; set; }

    }
}

