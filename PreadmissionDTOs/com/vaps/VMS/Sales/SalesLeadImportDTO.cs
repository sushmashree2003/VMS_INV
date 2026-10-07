using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.VMS.Sales
{
    public class SalesLeadImportDTO : CommonParamDTO
    {
        public Master_NumberingDTO transnumbconfigurationsettingsss { get; set; }

        public long ISMTCRASTO_Id { get; set; }
        public long MI_Id { get; set; }
        public long UserId { get; set; }
        public long suscnt { get; set; }
        public long failcnt { get; set; }
        public long ASMAY_Id { get; set; }
        public string ISMTPLTA_EffortInHrsnew { get; set; }
        public string message { get; set; }
        public bool returnval { get; set; }
        public int duplicatecount { get; set; }
        public long ISMTPLTA_Id { get; set; }
        public long ISMTPL_Id { get; set; }
        public string ISMTPLTA_Remarks { get; set; }
        public DateTime? ISMTPLTA_StartDate { get; set; }
        public DateTime? ISMTPLTA_EndDate { get; set; }
        public long ISMTPLTA_EffortInHrs { get; set; }
        public salesdto[] advimppln { get; set; }

        public string ISMSLE_EmailId { get; set; }
        public long ISMSLE_ContactNo { get; set; }

        public Array DuplicateData { get; set; }
        public Array InvalidData { get; set; }
        public int InvalidDatacount { get; set; }




        public string ISMSLERaw_LeadName { get; set; }
        public string ISMSLERaw_ContactNo { get; set; }
        public string ISMSLERaw_EmailId { get; set; }
        public string ISMSLERaw_LeadCode { get; set; }
        public string ISMSLERaw_LeadAddress1 { get; set; }
        public DateTime? ISMSLERaw_CreateDate { get; set; }
        public DateTime? ISMSLERaw_UpdatedDate { get; set; }
        public long? ISMSLERaw_CreatedBy { get; set; }
        public long? ISMSLERaw_UpdatedBy { get; set; }
        public string Lead_Created_By { get; set; }
        public string Source { get; set; }
        public string Category { get; set; }
        public string Contact_Person { get; set; }
        public string Designation { get; set; }
        public string Health { get; set; }
        public string State { get; set; }
        public string Remarks { get; set; }
        public string ISMSLERaw_ApprovedBy { get; set; }
        public string ISMSLERaw_status { get; set; }
        public int ISMSLERaw_ApprovedRemark { get; set; }
        public string ISMSLERaw_ApprovedByTel { get; set; }
        public string ISMSLERaw_ApprovedRemarkTel { get; set; }
        public string ISMSLERaw_statusTel { get; set; }
        public string ISMSLERaw_Exibition { get; set; }
        public DateTime? ISMSLERaw_ExibitionDate { get; set; }

        public leadrawdata[] LeadDatasNew { get; set; }




      









    }

    public class salesdto
    {
        public string LeadName { get; set; }
        public string LeadCode { get; set; }
        public string ContactPerson { get; set; }
        public string Designation { get; set; }
        public string ContactNo { get; set; }
        public string Email { get; set; }
        public string Category { get; set; }
        public string Source { get; set; }
        public string Reference { get; set; }
        public string Health { get; set; }
        public string Product { get; set; }
        public string StaffStrength { get; set; }
        public string StudentStrength { get; set; }
        public string Pincode { get; set; }
        public string TotalInstitutions { get; set; }
        public string Remark { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public string District { get; set; }
        public string City { get; set; }
        public string VisitedDay { get; set; }
        public string VisitedMonth { get; set; }
        public string VisitedYear { get; set; }

        public string TelecallerEmpCode { get; set; }

        public string Lead_Created_By { get; set; }
        public string Contact_Person { get; set; }
        public string Remarks { get; set; }
    }
    public class Duplicate
    {
        public string LeadName { get; set; }
        public string LeadCode { get; set; }
        public string ContactPerson { get; set; }
        public string Designation { get; set; }
        public string ContactNo { get; set; }
        public string Email { get; set; }
        public string Category { get; set; }
        public string Source { get; set; }
        public string Reference { get; set; }
        public string Health { get; set; }
        public string Product { get; set; }
        public string StaffStrength { get; set; }
        public string StudentStrength { get; set; }
        public string Pincode { get; set; }
        public string TotalInstitutions { get; set; }
        public string Remark { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public string District { get; set; }
        public string City { get; set; }
        public string VisitedDay { get; set; }
        public string VisitedMonth { get; set; }
        public string VisitedYear { get; set; }

        public string TelecallerEmpCode { get; set; }

        public string Lead_Created_By { get; set; }
        public string Contact_Person { get; set; }
        public string Remarks { get; set; }
        public string message { get; set; }
    }

    public class leadrawdata
    {
        public string ISMSLERaw_LeadName { get; set; }
        public string ISMSLERaw_ContactNo { get; set; }
        public string ISMSLERaw_EmailId { get; set; }
        public string ISMSLERaw_LeadCode { get; set; }
        public string ISMSLERaw_LeadAddress1 { get; set; }
        public DateTime? ISMSLERaw_CreateDate { get; set; }
        public DateTime? ISMSLERaw_UpdatedDate { get; set; }
        public long? ISMSLERaw_CreatedBy { get; set; }
        public long? ISMSLERaw_UpdatedBy { get; set; }
        public string Lead_Created_By { get; set; }
        public string Source { get; set; }
        public string Category { get; set; }
        public string Contact_Person { get; set; }
        public string Designation { get; set; }
        public string Health { get; set; }
        public string State { get; set; }
        public string Remarks { get; set; }
    }
}
