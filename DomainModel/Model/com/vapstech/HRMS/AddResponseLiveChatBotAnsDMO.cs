using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.com.vapstech.HRMS
{

    [Table("Chat_FAQ_Master_Questions_Answers")]
    public class AddResponseLiveChatBotAnsDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long CTFAQMQANS_Id { get; set; }

        [ForeignKey("AddResponseLiveChatBotDMO")]
        public long CTFAQMQ_Id { get; set; } // Foreign key referencing AddResponseLiveChatBotDMO

        public string CTFAQMQANS_Answers { get; set; }
        public string CTFAQMQANS_Flg { get; set; }
        public bool CTFAQMQANS_ActiveFlag { get; set; }
        public DateTime CTFAQMQANS_CreatedDate { get; set; }
        public DateTime CTFAQMQANS_UpdatedDate { get; set; }
        public long CTFAQMQANS_CreatedBy { get; set; }
        public long CTFAQMQANS_UpdatedBy { get; set; }

        public virtual AddResponseLiveChatBotDMO AddResponseLiveChatBotDMO { get; set; }
    }
}
