using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.com.vapstech.IssueManager
{
    [Table("VMS_Complaint_Register")]
    public class VMS_Complaint_Register_DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long VMSCR_Id { get; set; }
        public long MI_Id { get; set; }
        public string VMSCR_Name { get; set; }
        public string VMSCR_EmailId { get; set; }
        public string VMSCR_TaskTitle { get; set; }
        public string VMSCR_Desc { get; set; }
        public double VMSCR_MobileNo { get; set; }
        public DateTime? VMSCR_CreatedDate { get; set; }
        public DateTime? VMSCR_UpdatedDate { get; set; }
    }
}
