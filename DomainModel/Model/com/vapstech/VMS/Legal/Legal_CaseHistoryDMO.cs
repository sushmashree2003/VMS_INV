using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.VMS.Legal
{
    [Table("Legal_CaseHistory")]
    public class Legal_CaseHistoryDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long LCH_Id { get; set; }
        public long LCD_Id { get; set; }
        public string LCH_RegistrationNumber { get; set; }
        public string LCH_Judge { get; set; }
        public DateTime? LCH_BusinessonDate { get; set; }
        public string LCH_PurposeofHearing { get; set; }
        public DateTime? LCH_HearingDate { get; set; }
        public bool LCH_ActiveFlag { get; set; }
        public DateTime? LCH_CreatedDate { get; set; }
        public DateTime? LCH_UpdatedDate { get; set; }
        public long LCH_CreatedBy { get; set; }
        public long LCH_UpdatedBy { get; set; }
    }
}
