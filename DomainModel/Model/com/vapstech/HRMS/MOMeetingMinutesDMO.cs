using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.HRMS
{
    [Table("Minutes_Of_Meeting_Minutes")]
    public class MOMeetingMinutesDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long MOMMNT_Id { get; set; }
        public long MOMPRT_Id { get; set; }
        public long MOM_Id { get; set; }
        public string MOMMNT_ActionPlans { get; set; }
        public DateTime? MOMMNT_TargetDate { get; set; }
        public long HRME_Id { get; set; }
        public string MOMMNT_Names { get; set; }
        public bool MOMMNT_ActiveFlag { get; set; }
        public DateTime? MOMMNT_CreatedDate { get; set; }
        public DateTime? MOMMNT_UpdatedDate { get; set; }
        public long MOMMNT_CreatedBy { get; set; }
        public long MOMMNT_UpdatedBy { get; set; }
        [ForeignKey("MOMPRT_Id")]
        public virtual MOMParticipantsDMO MOMParticipant { get; set; }


    }
}
