using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.com.vapstech.IssueManager
{
    [Table("VMS_Laptop_Application_Authorisation")]
    public class VMS_Laptop_Application_Authorisation_DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long VLAPAPPLAUT_Id { get; set; }
        public long VLAPAPPL_Id { get; set; }
        public long IVRMUL_Id { get; set; }
        public long VLAPAPPLAUT_SanctioningLevel { get; set; }
        public String VLAPAPPLAUT_Remarks { get; set; }
        public DateTime? VLAPAPPLAUT_Date { get; set; }
        public bool VLAPAPPLAUT_ActiveFlg { get; set; }
        public long VLAPAPPLAUT_CreatedBy { get; set; }
        public DateTime? VLAPAPPLAUT_CreatedDate { get; set; }
        public long VLAPAPPLAUT_UpdatedBy { get; set; }
        public DateTime? VLAPAPPLAUT_UpdatedDate { get; set; }
    }
}
