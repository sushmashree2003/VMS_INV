using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.Exam.LessonPlanner
{
    public class MasterSchoolTopicDTO
    {
        public long MI_Id { get; set; }
        public long Userid { get; set; }
        public Array getdetails { get; set; }
        public Array editdetails { get; set; }
        public Array activedetactive { get; set; }
        public bool returnval { get; set;}
        public long LPMT_Id { get; set; }      
        public string LPMT_TopicName { get; set; }
        public string LPMT_TopicDescription { get; set; }
        public int LPMT_TopicPeriods { get; set; }
        public int LPMT_Topicorder { get; set; }
        public string message { get; set; }
        public MasterSchoolTopicOrderDTO[] MasterSchoolTopicOrderDTO { get; set; }
    }
    public class MasterSchoolTopicOrderDTO
    {
        public long LPMT_Id { get; set; }
        public int LPMT_Topicorder { get; set; }

    }
}
