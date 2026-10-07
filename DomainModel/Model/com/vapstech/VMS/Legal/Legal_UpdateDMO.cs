using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.VMS.Legal
{

    [Table("Legal_Update")]
    public class Legal_UpdateDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long LU_Id { get; set; }
        public string LU_CaseType { get; set; }
        public string LU_FillingNumber { get; set; }
        public DateTime? LU_FillingDate { get; set; }
        public string LU_RegistrationNumber { get; set; }
        public DateTime? LU_RegistrationDate { get; set; }
        public string LU_CNRNumber { get; set; }
        public DateTime? LU_FirstHearingDate { get; set; }
        public DateTime? LU_NextHearingDate { get; set; }
        public string LU_CaseStatus { get; set; }
        public string LU_StageofCase { get; set; }
        public string LU_CourtNumber { get; set; }
        public string LU_UnderActs { get; set; }
        public string LU_UnderSection { get; set; }
        public string LU_Judge { get; set; }
        public DateTime? LU_BusinessOnDate { get; set; }
        public DateTime? LU_HearingDate { get; set; }
        public string LU_PurposeofHearing { get; set; }
        public string LU_ProcessId { get; set; }
        public DateTime? LU_ProcessDate { get; set; }
        public string LU_ProcessTittle { get; set; }
        public string LU_PartyName { get; set; }
        public string LU_IssuedProcess { get; set; }
        public string LU_PetitionerandAdvocate { get; set; }
        public string LU_RespondentandAdvocate { get; set; }
        public bool LU_ActiveFlag { get; set; }
        public DateTime? LU_CreatedDate { get; set; }
        public DateTime? LU_UpdatedDate { get; set; }
        public long LU_CreatedBy { get; set; }
        public long LU_UpdatedBy { get; set; }

    }
}
