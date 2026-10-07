using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace DomainModel.Model.com.vapstech.VisitorsManagement
{
    [Table("Visitor_Management_Visitor_AuthorisedBy", Schema = "VM")]
    public class Visitor_Management_Visitor_AuthorisedByDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long VMVAUTH_Id { get; set; }
        public long MI_Id { get; set; }
        public long VMMV_Id { get; set; }       
        public long HRME_Id { get; set; }
        public string VMVAUTH_Remarks { get; set; }
        public DateTime? VMVAUTH_DateTime { get; set; }
    }
}
