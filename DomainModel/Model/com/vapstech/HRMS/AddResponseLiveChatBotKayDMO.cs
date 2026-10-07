using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace DomainModel.Model.com.vapstech.HRMS
{
    [Table("Chat_FAQ_Master_Questions_Keywords")]
    public class AddResponseLiveChatBotKayDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long CTFAQMQKW_Id { get; set; }

        [ForeignKey("AddResponseLiveChatBotDMO")]
        public long CTFAQMQ_Id { get; set; } // Foreign key referencing FAQDMO

        public string CTFAQMQKW_Keywords { get; set; }
        public bool CTFAQMQKW_ActiveFlag { get; set; }
        public DateTime CTFAQMQKW_CreatedDate { get; set; }
        public DateTime CTFAQMQKW_UpdatedDate { get; set; }
        public long CTFAQMQKW_CreatedBy { get; set; }
        public long CTFAQMQKW_UpdatedBy { get; set; }

        public virtual AddResponseLiveChatBotDMO AddResponseLiveChatBotDMO { get; set; }
    }
}
