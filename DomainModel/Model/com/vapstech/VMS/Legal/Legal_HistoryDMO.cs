using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.VMS.Legal
{
    [Table("Legal_History")]
    public class Legal_HistoryDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long LH_Id { get; set; }
        public string LH_RegistrationNumber { get; set; }
        public string LH_Judge { get; set; }
        public DateTime? LH_BusinessOnDate { get; set; }
        public DateTime? LH_HearingDate { get; set; }
        public string LH_PurposeofHearing { get; set; }
        public string LH_Business { get; set; }
        public string LH_NextPurpose { get; set; }
        public string LH_CNRNumber { get; set; }
        public string LH_CaseNumber { get; set; }
        public bool LH_ActiveFlag { get; set; }
        public DateTime? LH_CreatedDate { get; set; }
        public DateTime? LH_UpdatedDate { get; set; }
        public long LH_CreatedBy { get; set; }
        public long LH_UpdatedBy { get; set; }
    }
}
