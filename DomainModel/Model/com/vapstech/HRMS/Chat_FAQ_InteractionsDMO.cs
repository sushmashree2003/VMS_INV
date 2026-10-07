using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DomainModel.Model.com.vapstech.HRMS
{
    [Table("Chat_FAQ_Interactions")]


    public class Chat_FAQ_InteractionsDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long CTFAQINT_Id { get; set; }
        public long MI_Id { get; set; }
        public DateTime CTFAQINT_Date { get; set; }
        public bool CTFAQINT_ActiveFlag { get; set; }
        public DateTime CTFAQINT_CreatedDate { get; set; }
        public DateTime CTFAQINT_UpdatedDate { get; set; }
        public long CTFAQINT_CreatedBy { get; set; }
        public long CTFAQINT_UpdatedBy { get; set; }

    }
}
