using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.HRMS
{
    [Table("Minutes_Of_Meeting_Participants")]
    public class MOMParticipantsDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long MOMPRT_Id { get; set; }
        public long MOM_Id { get; set; }
        public long HRME_Id { get; set; }
        public string MOMPRT_ParticipantsName { get; set; }
        public string MOMPRT_Designation { get; set; }
        public string MOMPRT_EXTINTFlag { get; set; }
        public bool? MOMPRT_ActiveFlag { get; set; }
        public DateTime? MOMPRT_CreatedDate { get; set; }
        public DateTime? MOMPRT_UpdatedDate { get; set; }
        public long? MOMPRT_CreatedBy { get; set; }
        public long? MOMPRT_UpdatedBy { get; set; }
        public long? MOMPRT_MobileNo { get; set; }
        public string MOMPRT_Email { get; set; }
        public long INVMS_Id { get; set; }

        //public List<MOMeetingMinutesDMO> MOMeetingMinutesDMO { get; set; }
        public virtual ICollection<MOMeetingMinutesDMO> MOMeetingMinutesDMO { get; set; } = new List<MOMeetingMinutesDMO>();



    }
}
