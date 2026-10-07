using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.FMS
{
    [Table("FMS_Correspondence")]
    public class FMS_CorrespondenceDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long FMSCOR_Id { get; set; }
        public long MI_Id { get; set; }
        public long FMSMFC_Id { get; set; }
        public long IMFY_Id { get; set; }
        public DateTime? FMSCOR_Date { get; set; }
        public string FMSCOR_ClientSupplierFlg { get; set; }
        public string FMSCOR_Subject { get; set; }
        public string FMSCOR_RefernceNo { get; set; }
        public string FMSCOR_LetterHeadNo { get; set; }
        public string FMSCOR_ToName { get; set; }
        public string FMSCOR_Description { get; set; }
        public bool FMSCOR_ActiveFlg { get; set; }
        public DateTime? FMSCOR_CreatedDate { get; set; }
        public DateTime? FMSCOR_UpdatedDate { get; set; }
        public long? FMSCOR_CreatedBy { get; set; }
        public long? FMSCOR_UpdatedBy { get; set; }
        public List<FMS_Correspondence_ClientDMO> FMS_Correspondence_ClientDMO { get; set; }
        public List<FMS_Correspondence_SalesLeadDMO> FMS_Correspondence_SalesLeadDMO { get; set; }
        public List<FMS_Correspondence_SupplierDMO> FMS_Correspondence_SupplierDMO { get; set; }
        public List<FMS_Correspondence_EmployeeDMO> FMS_Correspondence_EmployeeDMO { get; set; }
    }
}
