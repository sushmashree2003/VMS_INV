using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.IssueManager.Rating
{
    [Table("Ratings_Master_Questions_Authority")]
    public class Ratings_Master_Questions_AuthorityDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long RATMQEAUT_Id { get; set; }
        public long RATMQE_Id { get; set; }
        public long HRMD_Id { get; set; }
        public long HRMDES_Id { get; set; }
        public bool RATMQEAUT_ActiveFlag { get; set; }
        public long RATMQEAUT_CreatedBy { get; set; }
        public DateTime RATMQEAUT_CreatedDate { get; set; }
        public long RATMQEAUT_UpdatedBy { get; set; }
        public DateTime RATMQEAUT_UpdatedDate { get; set; }
        public long? RATMT_Id { get; set; }
       

    }
}
