using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace DomainModel.Model.com.vapstech.HRMS
{
    [Table("Chat_FAQ_Master_Questions")]
    public class AddResponseLiveChatBotDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long CTFAQMQ_Id { get; set; }

        public string CTFAQMQ_FAQ { get; set; }
        public string CTFAQMQ_Remarks { get; set; }
        public bool CTFAQMQ_ActiveFlag { get; set; }
        public DateTime CTFAQMQ_CreatedDate { get; set; }
        public DateTime CTFAQMQ_UpdatedDate { get; set; }
        public long CTFAQMQ_CreatedBy { get; set; }
        public long CTFAQMQ_UpdatedBy { get; set; }
    }
}
