using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.VMS.Legal
{
    [Table("Legal_Advocate")]
    public class Legal_AdvocateDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long LA_Id { get; set; }   
        public long LCD_Id { get; set; }
        public string LA_PetitionerandAdvocate { get; set; }
        public string LA_RespondentandAdvocate { get; set; }
        public bool LA_ActiveFlag { get; set; }
        public DateTime? LA_CreatedDate { get; set; }
        public DateTime? LA_UpdatedDate { get; set; }
        public long LA_CreatedBy { get; set; }
        public long LA_UpdatedBy { get; set; }

    }
}
