using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.VMS.Sales
{
    [Table("ISM_Sales_Lead_Rawdata")]
    public class ISM_Sales_Lead_RawdataDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ISMSLERaw_Id { get; set; }
        public string ISMSLERaw_LeadName { get; set; }
        public string ISMSLERaw_ContactNo { get; set; }
        public string ISMSLERaw_EmailId { get; set; }
        public string ISMSLERaw_LeadCode { get; set; }
        public string ISMSLERaw_LeadAddress1 { get; set; }
        public DateTime? ISMSLERaw_CreateDate { get; set; }
        public DateTime? ISMSLERaw_UpdatedDate { get; set; }
        public long  ISMSLERaw_CreatedBy { get; set; }
        public long ISMSLERaw_UpdatedBy { get; set; }
        public string Lead_Created_By { get; set; }
        public string Source { get; set; }
        public string Category { get; set; }
        public string Contact_Person { get; set; }
        public string Designation { get; set; }
        public string Health { get; set; }
        public string State { get; set; }
        public string Remarks { get; set; }
        public string ISMSLERaw_status { get; set; }
        public string ISMSLERaw_ApprovedBy { get; set; }
        public string ISMSLERaw_ApprovedRemark { get; set; }
        public string ISMSLERaw_ApprovedByTel { get; set; }

        public string ISMSLERaw_ApprovedRemarkTel { get; set; }
        public string ISMSLERaw_statusTel { get; set; }
    }
}
