using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.com.vapstech.IssueManager
{
    [Table("ISM_Interactions_Details")]
    public class ISM_Interactions_DetailsDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ISMINTRD_Id { get; set; }
        public long ISMINTR_Id { get; set; }
        public long ISMINTRD_ToId { get; set; }
        public long ISMINTRD_ComposedById { get; set; }
        public string ISMINTRD_Interaction { get; set; }
        public DateTime ISMINTRD_DateTime { get; set; }
        public int ISMINTRD_InteractionOrder { get; set; }
        public bool ISMINTRD_ActiveFlag { get; set; }       
        public long ISMINTRD_CreatedBy { get; set; }
        public long ISMINTRD_UpdatedBy { get; set; }
    }
}
