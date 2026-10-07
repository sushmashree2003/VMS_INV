using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DomainModel.Model.com.vapstech.HRMS
{
    [Table("Chat_FAQ_Interactions_Details")]
    public class Chat_FAQ_Interactions_DetailsDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long CTFAQINT_Id { get; set; }


        public string CTFAQINTDET_Interaction { get; set; }

        public string CTFAQINTDET_Response { get; set; }

        public bool CTFAQINTDET_ActiveFlag { get; set; }

        public DateTime CTFAQINTDET_CreatedDate { get; set; }

        public DateTime CTFAQINTDET_UpdatedDate { get; set; }

        public long CTFAQINTDET_CreatedBy { get; set; }

        public long CTFAQINTDET_UpdatedBy { get; set; }


    }
}
