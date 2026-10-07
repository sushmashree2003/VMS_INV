using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.IssueManager.Rating
{
    [Table("Ratings_Master_Authority_Questions")]
    public class Ratings_Master_Authority_QuestionsDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long RATMAUTTYQ_Id { get; set; }
        public long RATMAUT_Id { get; set; }
        public long RATMT_Id { get; set; }
        public long RATMQE_Id { get; set; }
        public bool RATMAUTTYQ_ActiveFlag { get; set; }
        public long RATMAUTTYQ_CreatedBy { get; set; }
        public DateTime RATMAUTTYQ_CreatedDate { get; set; }
        public long RATMAUTTYQ_UpdatedBy { get; set; }
        public DateTime RATMAUTTYQ_UpdatedDate { get; set; }

    }
}
