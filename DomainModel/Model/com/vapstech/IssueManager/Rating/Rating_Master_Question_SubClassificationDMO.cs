using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.IssueManager.Rating
{
    [Table("Ratings_Master_Questions_SubClassification")]
    public class Rating_Master_Question_SubClassificationDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long RATMQECLSUB_Id { get; set; }
        public long RATMQECL_Id { get; set; }
        public string RATMQECLSUB_QuestionsSubclassification { get; set; }
        public long RATMQECLSUB_RQOrder { get; set; }
        public bool RATMQECLSUB_ActiveFlag { get; set; }
        public long? RATMQECLSUB_CreatedBy { get; set; }
        public DateTime? RATMQECLSUB_CreatedDate { get; set; }
        public long? RATMQECLSUB_UpdatedBy { get; set; }
        public DateTime? RATMQECLSUB_UpdatedDate { get; set; }
    }
}
