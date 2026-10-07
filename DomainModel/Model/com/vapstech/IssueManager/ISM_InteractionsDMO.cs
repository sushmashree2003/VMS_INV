using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.com.vapstech.IssueManager
{
    [Table("ISM_Interactions")]
    public class ISM_InteractionsDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ISMINTR_Id { get; set; }
        public long MI_Id { get; set; }
        public string ISMINTR_InteractionId { get; set; }
        public long ISMINTR_ComposedByHRME_Id { get; set; }
        public string ISMINTR_GroupOrIndFlg { get; set; }
        public string ISMINTR_Subject { get; set; }
        public DateTime ISMINTR_DateTime { get; set; }
        public string ISMINTR_Interaction { get; set; }
        public bool ISMINTR_ActiveFlag { get; set; }       
        public long ISMINTR_CreatedBy { get; set; }
        public long ISMINTR_UpdatedBy { get; set; }

        public List<ISM_Interactions_DetailsDMO> ISM_Interactions_DetailsDMO { get; set; }
    }
}
