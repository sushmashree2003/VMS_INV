using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.VMS.HRMS
{
    public class AddResponseLiveChatBotDTO
    {
        public long userid { get; set; }
        public long MI_Id { get; set; }
        public long CTFAQMQ_Id { get; set; }
        public string CTFAQMQ_FAQ { get; set; }
        public string CTFAQMQ_Remarks { get; set; }
        public bool CTFAQMQ_ActiveFlag { get; set; }
        public DateTime CTFAQMQ_CreatedDate { get; set; }
        public DateTime CTFAQMQ_UpdatedDate { get; set; }
        public long CTFAQMQ_CreatedBy { get; set; }
        public long CTFAQMQ_UpdatedBy { get; set; }
        public string userMessage { get; set; }

        public string returndata { get; set; }
        public bool returnval { get; set; }

        public List<KeywordDTO> Keywords { get; set; } = new List<KeywordDTO>();
        public List<answersDTO> answers { get; set; } = new List<answersDTO>();

        // public string qetanswer { get; set; }
        public Array qetanswer { get; set; }
        public object FAQList { get; set; }
        //public KeywordDTO[] Keywords { get; set; }
        //public answersDTO[] answers { get; set; }
    }

    public class KeywordDTO
    {
        public int CTFAQMQKW_Id { get; set; }
        public long CTFAQMQ_Id { get; set; }
        public string CTFAQMQKW_Keywords { get; set; }
        public bool CTFAQMQKW_ActiveFlag { get; set; }
        public DateTime CTFAQMQKW_CreatedDate { get; set; }
        public DateTime CTFAQMQKW_UpdatedDate { get; set; }
        public long CTFAQMQKW_CreatedBy { get; set; }
        public long CTFAQMQKW_UpdatedBy { get; set; }
        public string Keyword { get; set; }
        public List<string> KeywordSuggestions { get; set; }

    }

    public class answersDTO
    {
        public long CTFAQMQANS_Id { get; set; }


        public long CTFAQMQ_Id { get; set; } // Foreign key referencing AddResponseLiveChatBotDMO

        public string CTFAQMQANS_Answers { get; set; }
        public string CTFAQMQANS_Flg { get; set; }
        public bool CTFAQMQANS_ActiveFlag { get; set; }
        public DateTime CTFAQMQANS_CreatedDate { get; set; }
        public DateTime CTFAQMQANS_UpdatedDate { get; set; }
        public long CTFAQMQANS_CreatedBy { get; set; }
        public long CTFAQMQANS_UpdatedBy { get; set; }

    }
}
