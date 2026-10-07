using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.IssueManager.Rating
{
    [Table("Ratings_Master_Questions_Classification")]
    public class Rating_Master_Question_ClassificationDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long RATMQECL_Id { get; set; }
        public long MI_Id { get; set; }
        public long RATMT_Id { get; set; }
        public string RATMQECL_QuestionsClassification { get; set; }
        public long RATMQECL_RQOrder { get; set; }
        public bool RATMQECL_ActiveFlag { get; set; }
        public long? RATMQECL_CreatedBy { get; set; }
        public DateTime? RATMQECL_CreatedDate { get; set; }
        public long? RATMQECL_UpdatedBy { get; set; }
        public DateTime? RATMQECL_UpdatedDate { get; set; }
    }
}
