using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.Legal
{
    public class LegalUpdateDTO
    {
        public long MI_Id { get; set; }
        public long roleId { get; set; }
        public long userId { get; set; }
        public string retrunMsg { get; set; }

        public string Designation { get; set; }
        public string MobileNo { get; set; }
        public string CNRNumber { get; set; }
        public string ActionPlans { get; set; }
        public string TargetDate { get; set; }
        public string FillingDate { get; set; }
        public string RegistrationDate { get; set; }
        public string FirstHearingDate { get; set; }
        public string NextHearingDate { get; set; }
        public string CaseStatus { get; set; }
        public string Stageofcase { get; set; }
        public string UnderActs { get; set; }
        public string UnderSection { get; set; }
        public string Judge { get; set; }
        public string BusinessOnDate { get; set; }
        public string HearingDate { get; set; }
        public string Business { get; set; }
        public string NextPurpose { get; set; }
        public string PurposeofHearing { get; set; }
        public string ProcessId { get; set; }
        public string ProcessDate { get; set; }
        public string ProcessTittle { get; set; }
        public string PartyName { get; set; }
        public string PetitionerAndAdvocate { get; set; }
        public string RespondentAndAdvocate { get; set; }
        public string IssuedProcess { get; set; }
        public string RegistrationNumber { get; set; }
        public string Email { get; set; }

        public string returnVal { get; set; }
        public string CaseType { get; set; }
        public string stuStatus { get; set; }
        public string FillingNumber { get; set; }
        public string CourtNumberandJudge { get; set; }
        public Array countdetails { get; set; }
        public int RecordsSavedCount { get; set; }

        public LegalUpdateDTO[] newlstget1 { get; set; }


        //Legal_Update
        public legalupdatelistdto[] LegalUpdate { get; set; }

        public class legalupdatelistdto
        {
            public long LU_Id { get; set; }
            public string LU_CaseType { get; set; }
            public string LU_FillingNumber { get; set; }
            public DateTime? LU_FillingDate { get; set; }
            public string LU_RegistrationNumber { get; set; }
            public DateTime? LU_RegistrationDate { get; set; }
            public string LU_CNRNumber { get; set; }
            public DateTime? LU_FirstHearingDate { get; set; }
            public DateTime? LU_NextHearingDate { get; set; }
            public string LU_StageofCase { get; set; }
            public string LU_CourtNumber { get; set; }
            public string LU_UnderActs { get; set; }
            public string LU_UnderSection { get; set; }
            public string LU_Judge { get; set; }
            public DateTime? LU_BusinessOnDate { get; set; }
            public DateTime? LU_HearingDate { get; set; }
            public string LU_PurposeofHearing { get; set; }
            public string LU_CaseStatus { get; set; }
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
            //public long UserId { get; set; }
            public long LU_CreatedBy { get; set; }
            public long LU_UpdatedBy { get; set; }
        }

        public string returnvales { get; set; }
        public long LU_Id { get; set; }
    }
}
