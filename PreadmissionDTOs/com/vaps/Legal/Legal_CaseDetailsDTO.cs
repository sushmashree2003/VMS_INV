using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.Legal
{
    public class Legal_CaseDetailsDTO
    {
        public long LCD_Id { get; set; }
        public string LCD_CaseType { get; set; }
        public string LCD_FillingNumber { get; set; }
        public DateTime? LCD_FillingDate { get; set; }
        public string LCD_RegistrationNumber { get; set; }
        public DateTime? LCD_RegistrationDate { get; set; }
        public string LCD_CNRNumber { get; set; }
        public bool LCD_ActiveFlag { get; set; }
        public DateTime? LCD_CreatedDate { get; set; }
        public DateTime? LCD_UpdatedDate { get; set; }
        public long LCD_CreatedBy { get; set; }
        public long LCD_UpdatedBy { get; set; }
        public long MI_Id { get; set; }
        public long userId { get; set; }
        public string returnvales { get; set; }
        public string ASMAY_Id { get; set; }
        public string roleId { get; set; }
        public Array category1 { get; set; }
        public Array Gridcasedetails { get; set; }
        public Array gridcasestatus { get; set; }
        public Array gridadvocate { get; set; }
        public Array gridacts { get; set; }
        public Array gridcasehistory { get; set; }
        public Array gridfinalorder { get; set; }
      

        //casestatus
        public long LCS_Id { get; set; }
        public string LCS_CaseStatus { get; set; }
        public DateTime? LCS_FirstHearingDate { get; set; }
        public DateTime? LCS_DecisionDate { get; set; }
        public string LCS_NatureofDisposal { get; set; }
        public string LCS_CourtNumberandJudge { get; set; }
        public bool LCS_ActiveFlag { get; set; }
        public DateTime? LCS_CreatedDate { get; set; }
        public DateTime? LCS_UpdatedDate { get; set; }
        public long LCS_CreatedBy { get; set; }
        public long LCS_UpdatedBy { get; set; }
        public string LCD_PartyName { get; set; }
        public DateTime? LCD_NextHearingDate { get; set; }

        //legalAdvocate
        public long LA_Id { get; set; }
        public string LA_PetitionerandAdvocate { get; set; }
        public string LA_RspondentandAdvocate { get; set; }
        public bool LA_ActiveFlag { get; set; }
        public DateTime? LA_CreatedDate { get; set; }
        public DateTime? LA_UpdatedDate { get; set; }
        public long LA_CreatedBy { get; set; }
        public long LA_UpdatedBy { get; set; }

        //LegalActs
        public long LACT_Id { get; set; }
        public string LACT_UnderActs { get; set; }
        public string LACT_UnderSection { get; set; }
        public bool LACT_ActiveFlag { get; set; }
        public DateTime? LACT_CreatedDate { get; set; }
        public DateTime? LACT_UpdatedDate { get; set; }
        public long LACT_CreatedBy { get; set; }
        public long LACT_UpdatedBy { get; set; }

        //legalcasehistory
        public long LCH_Id { get; set; }
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

        //LegalFinalOrder
        public long LFO_Id { get; set; }
        public string LFO_OrderDetails { get; set; }
        public string LFO_OrderNumber { get; set; }
        public DateTime? LFO_Date { get; set; }
        public bool LFO_ActiveFlag { get; set; }
        public DateTime? LFO_CreatedDate { get; set; }
        public DateTime? LFO_UpdatedDate { get; set; }
        public long LFO_CreatedBy { get; set; }
        public long LFO_UpdatedBy { get; set; }

        //Legal Docments

        public long LD_Id { get; set; }
        public string LD_DocumentName { get; set; }
        public string Ld_DocumentPath { get; set; }
        public string LD_Remarks { get; set; }
        public bool LD_ActiveFlag { get; set; }
        public DateTime LD_CreatedDate { get; set; }
        public DateTime LD_UpdatedDate { get; set; }
        public long LD_CreatedBy { get; set; }
        public long LD_UpdatedBy { get; set; }

        public Array Documentslist { get; set; }




    }
}
