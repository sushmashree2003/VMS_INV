using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.com.vapstech.IssueManager
{
    [Table("VMS_Laptop_Application")]
    public class VMS_Laptop_Application_DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long VLAPAPPL_Id { get; set; }
        public long MI_Id { get; set; }
        public long HRME_Id { get; set; }
        public string VLAPAPPL_ApplicationID { get; set; }
        public DateTime? VLAPAPPL_ApplicationDate { get; set; }
        public DateTime? VLAPAPPL_FromDate { get; set; }
        public DateTime? VLAPAPPL_ToDate { get; set; }
        public decimal VLAPAPPL_TotalDays { get; set; }
        public string VLAPAPPL_Reason { get; set; }
        public string VLAPAPPL_ApplicationStatus { get; set; }
        public string VLAPAPPL_SupportingDocument { get; set; }
        public bool VLAPAPPL_ActiveFlag { get; set; }
        public DateTime? VLAPAPPL_CreatedDate { get; set; }
        public DateTime? VLAPAPPL_UpdatedDate { get; set; }
    }
}
