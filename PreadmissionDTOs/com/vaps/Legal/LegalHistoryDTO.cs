using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.Legal
{
    public class LegalHistoryDTO
    {
        public long MI_Id { get; set; }
        public Array updateDetails { get; set; }              
            public string Participants { get; set; }
            public string CaseType { get; set; }
            public string FillingNumber { get; set; }
            public string Empcode { get; set; }
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
            public string CourtNumberAndJudge { get; set; }
            public string UnderActs { get; set; }
            public string UnderSection { get; set; }
            public string Judge { get; set; }
            public string BusinessOnDate { get; set; }
            public string HearingDate { get; set; }
            public string NextPurpose { get; set; }
            public string Business { get; set; }
            public string PurposeofHearing { get; set; }
            public string ProcessId { get; set; }
            public string ProcessDate { get; set; }
            public string ProcessTittle { get; set; }
            public string PartyName { get; set; }
            public string PetitionerAndAdvocate { get; set; }
            public string RespondentAndAdvocate { get; set; }
            public string IssuedProcess { get; set; }
            public string RegistrationNumber { get; set; }
         
        public string returnvales { get; set; }        
            public string returnVal { get; set; }
        public string stuStatus { get; set; }
        public int userId { get; set; }
        public LegalUpdateDTO[] newlstget1 { get; set; }
        public legalhistorylistdto[] LegalHistory { get; set; }

        public class legalhistorylistdto
        {
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

        //public long LH_Id { get; set; }
       
       
        }
    }



